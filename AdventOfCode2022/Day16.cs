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
        private class State
        {
            public State(Valve current, int flow, int minute, List<string> openedValves, Valve? goTo)
            {
                Current = current;
                Flow = flow;
                Minute = minute;
                OpenedValves = openedValves;
                this.goTo = goTo;
            }
            public List<string> OpenedValves { get; }
            public int Minute { get; }
            public int Flow { get; }
            public Valve Current { get; }
            public Valve? goTo { get; }
        }

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
            Random rd = new Random(2137);
            int max = 0;
            for (int iter = 0; iter >= 0; iter++)
            {
                foreach (Valve v in valves)
                {
                    v.Opened = false;
                }
                Valve current = valves.Where(v => v.ID == "AA").First();
                Valve previous = current;
                int minutes = 30;
                int pressure = 0;
                while (minutes >= 0)
                {
                    if (!current.Opened && rd.Next(0, 100) > 5 && current.Pressure > 0)
                    {
                        minutes--;
                        current.Opened = true;
                        pressure += minutes * current.Pressure;
                    }
                    List<Valve> choices = new List<Valve>();
                    foreach (string id in current.Connections)
                    {
                        choices.Add(valves.Where(v => v.ID == id).First());
                    }
                    if (rd.Next(0, 100) > 5 && choices.Contains(previous) && choices.Count > 1)
                    {
                        choices.Remove(previous);
                    }
                    previous = current;
                    current = choices[rd.Next(0, choices.Count)];
                    minutes--;
                }
                if (pressure > max)
                {
                    max = pressure;
                    Console.WriteLine(max);
                }
                if (iter % 1000 == 0) Console.WriteLine($"try: {iter} max: {max}");
            }
        }

        public static void SolvePartTwo()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day16.txt");

            string[] data = File.ReadAllText(path).Split("\r\n");

            //string[] data = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB\r\nValve BB has flow rate=13; tunnels lead to valves CC, AA\r\nValve CC has flow rate=2; tunnels lead to valves DD, BB\r\nValve DD has flow rate=20; tunnels lead to valves CC, AA, EE\r\nValve EE has flow rate=3; tunnels lead to valves FF, DD\r\nValve FF has flow rate=0; tunnels lead to valves EE, GG\r\nValve GG has flow rate=0; tunnels lead to valves FF, HH\r\nValve HH has flow rate=22; tunnel leads to valve GG\r\nValve II has flow rate=0; tunnels lead to valves AA, JJ\r\nValve JJ has flow rate=21; tunnel leads to valve II".Split("\r\n");

            List<Valve> valves = ParseData(data);
            Random rd = new Random(2138);
            int max = 0;
            for (int iter = 0; iter >= 0; iter++)
            {
                foreach (Valve v in valves)
                {
                    v.Opened = false;
                }
                Valve currentOne = valves.Where(v => v.ID == "AA").First();
                Valve previousOne = currentOne;
                Valve currentTwo = currentOne;
                Valve previousTwo = previousOne;
                int minutesOne = 26;
                int minutesTwo = 26;
                int pressure = 0;
                while (minutesOne >= 0 || minutesTwo >= 0)
                {
                    if (minutesOne >= 0)
                    {
                        if (!currentOne.Opened && rd.Next(0, 100) > 5 && currentOne.Pressure > 0)
                        {
                            minutesOne--;
                            currentOne.Opened = true;
                            pressure += minutesOne * currentOne.Pressure;
                        }
                        List<Valve> choicesOne = new List<Valve>();
                        foreach (string id in currentOne.Connections)
                        {
                            choicesOne.Add(valves.Where(v => v.ID == id).First());
                        }
                        if (rd.Next(0, 100) > 5 && choicesOne.Contains(previousOne) && choicesOne.Count > 1)
                        {
                            choicesOne.Remove(previousOne);
                        }
                        previousOne = currentOne;
                        currentOne = choicesOne[rd.Next(0, choicesOne.Count)];
                        minutesOne--;
                    }
                    if (minutesTwo >= 0)
                    {
                        if (!currentTwo.Opened && rd.Next(0, 100) > 5 && currentTwo.Pressure > 0)
                        {
                            minutesTwo--;
                            currentTwo.Opened = true;
                            pressure += minutesTwo * currentTwo.Pressure;
                        }
                        List<Valve> choicesTwo = new List<Valve>();
                        foreach (string id in currentTwo.Connections)
                        {
                            choicesTwo.Add(valves.Where(v => v.ID == id).First());
                        }
                        if (rd.Next(0, 100) > 5 && choicesTwo.Contains(previousTwo) && choicesTwo.Count > 1)
                        {
                            choicesTwo.Remove(previousTwo);
                        }
                        previousTwo = currentTwo;
                        currentTwo = choicesTwo[rd.Next(0, choicesTwo.Count)];
                        minutesTwo--;
                    }
                    
                }
                if (pressure > max)
                {
                    max = pressure;
                    Console.WriteLine(max);
                }
                if (iter % 1000 == 0) Console.WriteLine($"try: {iter} max: {max}");
            }
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
    }
}
