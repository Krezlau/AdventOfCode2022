using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AdventOfCode2022
{
    public class Day16
    {
        private class Valve
        {
            public Valve(string iD, int pressure, List<string> connections)
            {
                ID = iD;
                Pressure = pressure;
                Connections = connections;
            }

            public string ID { get; }
            public int Pressure { get; }
            public List<string> Connections { get; }
            public Valve Preceding { get; set; }
            public int Distance { get; set; }
            public bool Visited { get; set; } = false;
            public bool Opened { get; set; } = false;
        }
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day16.txt");

            string[] data = File.ReadAllText(path).Split("\r\n");

            //string[] data = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II".Split("\r\n");

            List<Valve> valves = ParseData(data);

            Valve current = valves.Where(v => v.ID == "AA").First();
            int sum = 0;
            int plus = 0;
            for (int i = 30; i > 0; i--)
            {
                sum += plus;
                Dijkstra(valves, current);

                foreach (Valve v in valves)
                {
                    if (v.Distance == 0)
                    {
                        v.Distance = v.Pressure;
                    }
                    else
                    {
                        v.Distance = v.Pressure / v.Distance;
                    }
                    if (v.Opened)
                    {
                        v.Distance = 0;
                    }
                }

                if (current.Distance == valves.Max(v => v.Distance) && !current.Opened)
                {
                    plus += current.Pressure;
                    Console.WriteLine($"opened valve {current.ID}");
                    current.Opened = true;
                    continue;
                }
                Valve goTo = valves.Where(v => v.Distance == valves.Max(v => v.Distance)).First();
                if (goTo.Preceding != current)
                {
                    while (true)
                    {
                        goTo = goTo.Preceding;
                        if (goTo.Preceding == current) break;
                    }
                }
                current = goTo;
                Console.WriteLine($"moved to valve {current.ID}");
            }
            Console.WriteLine(sum);
        }

        private static List<Valve> ParseData(string[] data)
        {
            List<Valve> valves = new List<Valve>();
            foreach (string line in data)
            {
                string[] line_split = line.Split(" ");
                int flow_rate = int.Parse(line_split[4].Split("=")[1].Replace(";", ""));
                List<string> connections = new List<string>();
                for (int i = 9; i < line_split.Length; i++)
                {
                    connections.Add(line_split[i].Replace(",", ""));
                }

                valves.Add(new Valve(line_split[1], flow_rate, connections));
            }
            return valves;
        }

        private static void Dijkstra(List<Valve> valves, Valve current)
        {
            PriorityQueue<Valve, int> pq = new PriorityQueue<Valve, int>();
            current.Distance = 0;
            current.Visited = true;
            foreach (Valve valve in valves)
            {
                if (valve != current)
                {
                    valve.Distance = 999999;
                    valve.Visited = false;
                }
            }

            foreach (string valve in current.Connections)
            {
                Valve v = valves.Where(v => v.ID == valve).First();
                v.Distance = 1;
                v.Preceding = current;
                pq.Enqueue(v, v.Distance);
            }
            

            while (pq.Count > 0)
            {
                current = pq.Dequeue();
                current.Visited = true;
                foreach (string valve in current.Connections)
                {
                    Valve v = valves.Where(v => v.ID == valve).First();
                    if (current.Distance + 1 < v.Distance && v.Visited == false)
                    {
                        v.Distance = current.Distance + 1;
                        v.Preceding = current;
                        pq.Enqueue(v, v.Distance);
                    }
                }
            }
            
        }
    }
}
