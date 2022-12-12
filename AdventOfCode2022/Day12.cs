using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day12
    {
        private class NodeDistance
        {
            public NodeDistance(int x, int y, int height)
            {
                X = x;
                Y = y;
                Height = height;
            }
            public int Distance { get; set; } = 999999;
            public int Height { get; set; }
            public int PrecedingNodeX { get; set; }
            public int PrecedingNodeY { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public bool WasVisited { get; set; } = false;
        }

        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day12.txt");

            string[] data = File.ReadAllText(path).Split("\r\n");

            //string[] data = "Sabqponm\r\nabcryxxl\r\naccszExk\r\nacctuvwj\r\nabdefghi".Split("\r\n");

            List<List<NodeDistance>> distances = new List<List<NodeDistance>>();
            int startX = 0;
            int startY = 0;
            int endX = 0;
            int endY = 0;
            for (int i = 0; i < data.Length; i++)
            {
                distances.Add(new List<NodeDistance>());
                for (int j = 0; j < data[i].Length; j++)
                {
                    if (data[i][j] == 'S')
                    {
                        distances[i].Add(new NodeDistance(i, j, 97));
                        startX = i;
                        startY = j;
                        continue;
                    }
                    if (data[i][j] == 'E')
                    {
                        distances[i].Add(new NodeDistance(i, j, 122));
                        endX = i;
                        endY = j;
                        continue;
                    }
                    distances[i].Add(new NodeDistance(i, j, (int)data[i][j]));
                }
            }

            Dijkstra(distances, startX, startY, (x) => x >= -1);

            Console.WriteLine(distances[endX][endY].Distance);
            
            //PART 2

            startX= endX;
            startY= endY;

            foreach (List<NodeDistance> list in distances)
            {
                foreach (NodeDistance node in list)
                {
                    node.Distance = 999999;
                    node.WasVisited = false;
                }
            }

            Dijkstra(distances, startX, startY, (x) => x <= 1);

            int min = 999999;
            foreach (List<NodeDistance> list in distances)
            {
                foreach (NodeDistance node in list)
                {
                    if (node.Height == (int) 'a' && node.Distance < min)
                    {
                        min = node.Distance;
                    }
                }
            }
            Console.WriteLine(min);
        }

        private static void Dijkstra(List<List<NodeDistance>> distances, int startX, int startY, Func<int, bool> heightRequirement)
        {
            PriorityQueue<NodeDistance, int> pq = new PriorityQueue<NodeDistance, int>();
            distances[startX][startY].Distance = 0;

            EnqueueNeighbours(distances, pq, startX, startY, 0, heightRequirement);
            NodeDistance current;

            while (pq.Count > 0)
            {
                current = pq.Dequeue();
                distances[current.X][current.Y].WasVisited = true;
                EnqueueNeighbours(distances, pq, current.X, current.Y, current.Distance, heightRequirement);
            }
        }

        private static void EnqueueNeighbours(List<List<NodeDistance>> distances, PriorityQueue<NodeDistance, int> pq, int x, int y, int distance, Func<int, bool> heightRequirement)
        {
            if (x > 0)
            {
                if (heightRequirement(distances[x][y].Height - distances[x - 1][y].Height))
                {
                    if (distances[x][y].Distance + 1 < distances[x - 1][y].Distance)
                    {
                        distances[x - 1][y].Distance = distance + 1;
                        distances[x - 1][y].PrecedingNodeX= x;
                        distances[x - 1][y].PrecedingNodeY= y;
                        pq.Enqueue(distances[x - 1][y], distances[x - 1][y].Distance);
                    }
                }
            }
            if (x < distances.Count - 1)
            {
                if (heightRequirement(distances[x][y].Height - distances[x + 1][y].Height))
                {
                    if (distances[x][y].Distance + 1 < distances[x + 1][y].Distance)
                    {
                        distances[x + 1][y].Distance = distance + 1;
                        distances[x + 1][y].PrecedingNodeX = x;
                        distances[x + 1][y].PrecedingNodeY = y;
                        pq.Enqueue(distances[x + 1][y], distances[x + 1][y].Distance);
                    }
                }
            }
            if (y > 0)
            {
                if (heightRequirement(distances[x][y].Height - distances[x][y - 1].Height))
                {
                    if (distances[x][y].Distance + 1 < distances[x][y - 1].Distance)
                    {
                        distances[x][y - 1].Distance = distance + 1;
                        distances[x][y - 1].PrecedingNodeX = x;
                        distances[x][y - 1].PrecedingNodeY = y;
                        pq.Enqueue(distances[x][y - 1], distances[x][y - 1].Distance);
                    }
                }
            }
            if (y < distances[0].Count - 1)
            {
                if (heightRequirement(distances[x][y].Height - distances[x][y + 1].Height))
                {
                    if (distances[x][y].Distance + 1 < distances[x][y + 1].Distance)
                    {
                        distances[x][y + 1].Distance = distance + 1;
                        distances[x][y + 1].PrecedingNodeX = x;
                        distances[x][y + 1].PrecedingNodeY = y;
                        pq.Enqueue(distances[x][y + 1], distances[x][y + 1].Distance);
                    }
                }
            }
        }
    }
}
