using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day24
    {
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day24.txt");

            //string[] data = File.ReadAllText(path).Split("\r\n");

            string[] data = "#.######\r\n#>>.<^<#\r\n#.<..<<#\r\n#>v.><>#\r\n#<^v^^>#\r\n######.#".Split("\r\n");

            (int x1, int x2, int y1, int y2) bounds = (0, data[0].Length - 1, 0, data.Length - 1);
            (int x, int y) startingPoint = (1, 0);
            (int x, int y) endPoint = (data[0].Length - 2, data.Length - 1);

            List<(int x, int y, int direction)> blizzards = new List<(int x, int y, int direction)>();

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    if (data[i][j] == '>') blizzards.Add((j, i, 0));
                    if (data[i][j] == 'v') blizzards.Add((j, i, 1));
                    if (data[i][j] == '<') blizzards.Add((j, i, 2));
                    if (data[i][j] == '^') blizzards.Add((j, i, 3));
                }
            }

            int answer = Simulate(startingPoint, endPoint, bounds, blizzards, 0);
            Console.WriteLine(answer);

            // part two

            int tripBack = Simulate(endPoint, startingPoint, bounds, blizzards, answer);
            int finalTrip = Simulate(startingPoint, endPoint, bounds, blizzards, tripBack);
            Console.WriteLine(finalTrip);
        }

        private static int Simulate((int x, int y) startingPoint, (int x, int y) endingPoint, (int x1, int x2, int y1, int y2) bounds, List<(int x, int y, int direction)> blizzards, int startingMinute)
        {
            PriorityQueue<(int x, int y, int distance), int> pq = new PriorityQueue<(int x, int y, int distance), int>();
            List<(int x, int y, int minute)> visited = new List<(int x, int y, int minute)>();
            visited.Add((startingPoint.x, startingPoint.y, startingMinute));
            if (startingPoint.y == 0)
            {
                pq.Enqueue((startingPoint.x, startingPoint.y + 1, startingMinute + 1), startingMinute + 1);
            }
            else
            {
                pq.Enqueue((startingPoint.x, startingPoint.y - 1, startingMinute + 1), startingMinute + 1);
            }
            visited.Add((startingPoint.x, startingPoint.y + 1, startingMinute + 1));
            (int x, int y, int distance) current = (visited[^1].x, visited[^1].y, visited[^1].minute);

            List<List<(int x, int y, int direction)>> blizzardsEachMinute = new List<List<(int x, int y, int direction)>>();

            blizzardsEachMinute.Add(blizzards);

            for (int i = 0; i < 3000; i++)
            {
                List<(int x, int y, int direction)> bliz = new List<(int x, int y, int direction)>();
                foreach (var b in blizzardsEachMinute[i])
                {
                    switch (b.direction)
                    {
                        case 0:
                            if (b.x + 1 == bounds.x2)
                            {
                                bliz.Add((bounds.x1 + 1, b.y, b.direction));
                            } else
                            {
                                bliz.Add((b.x + 1, b.y, b.direction));
                            }
                            break;
                        case 1:
                            if (b.y + 1 == bounds.y2)
                            {
                                bliz.Add((b.x, bounds.y1 + 1, b.direction));
                            }
                            else
                            {
                                bliz.Add((b.x, b.y + 1, b.direction));
                            }
                            break;
                        case 2:
                            if (b.x - 1 == bounds.x1)
                            {
                                bliz.Add((bounds.x2 - 1, b.y, b.direction));
                            }
                            else
                            {
                                bliz.Add((b.x - 1, b.y, b.direction));
                            }
                            break;
                        case 3:
                            if (b.y - 1 == bounds.y1)
                            {
                                bliz.Add((b.x, bounds.y2 - 1, b.direction));
                            }
                            else
                            {
                                bliz.Add((b.x, b.y - 1, b.direction));
                            }
                            break;
                        default:
                            break;
                    }
                }
                blizzardsEachMinute.Add(bliz);
            }
            int min = 99999999;
            while (pq.Count != 0)
            {
                current = pq.Dequeue();
                if (current.distance >= 3000 || current.distance > min) continue;
                //Console.WriteLine(pq.Count);
                if (visited.Count(d => d.x == endingPoint.x && d.y == endingPoint.y) > 0) return min;

                if (current.x == startingPoint.x && current.y == startingPoint.y)
                {
                    if (startingPoint.y == 0)
                    {
                        pq.Enqueue((current.x, current.y + 1, current.distance + 1), ((bounds.x2 - current.x) + (bounds.y2 - current.y)) * current.distance);
                        continue;
                    }
                    pq.Enqueue((current.x, current.y - 1, current.distance + 1), ((current.x) + (current.y)) * current.distance);
                    continue;
                }

                for (int m = 1; m < 2; m++)
                {
                    int pqcount = pq.Count;
                    if (visited.Count(d => d.x == current.x + 1 && d.y == current.y && d.minute == current.distance + 1) < 1
                    && blizzardsEachMinute[current.distance + m].Count(b => b.x == current.x + 1 && b.y == current.y) == 0
                    && current.x + 1 < bounds.x2)
                    {
                        if (current.distance + 1 < min)
                        {
                            if (startingPoint.y == 0)
                            {
                                pq.Enqueue((current.x + 1, current.y, current.distance + m), ((bounds.x2 - current.x) + (bounds.y2 - current.y)) * current.distance);

                            } else
                            {
                                pq.Enqueue((current.x + 1, current.y, current.distance + m), ((current.x) + (current.y)) * current.distance);
                            }
                            visited.Add((current.x + 1, current.y, current.distance + m));
                        }
                    }
                    if (visited.Count(d => d.x == current.x - 1 && d.y == current.y && d.minute == current.distance + 1) < 1
                    && blizzardsEachMinute[current.distance + m].Count(b => b.x == current.x - 1 && b.y == current.y) == 0
                    && current.x - 1 > bounds.x1)
                    {
                        if (current.distance + 1 < min)
                        {
                            if (startingPoint.y == 0)
                            {
                                pq.Enqueue((current.x - 1, current.y, current.distance + m), ((bounds.x2 - current.x) + (bounds.y2 - current.y)) * current.distance);
                            } else
                            {
                                pq.Enqueue((current.x - 1, current.y, current.distance + m), ((current.x) + (current.y)) * current.distance);
                            }
                            visited.Add((current.x - 1, current.y, current.distance + m));
                        }
                    }
                    if (visited.Count(d => d.x == current.x && d.y == current.y + 1 && d.minute == current.distance + 1) < 1
                    && blizzardsEachMinute[current.distance + m].Count(b => b.x == current.x && b.y == current.y + 1) == 0
                    && (current.y + 1 < bounds.y2 || (current.y + 1 == endingPoint.y && current.x == endingPoint.x) || (current.y + 1 == startingPoint.y && current.x == startingPoint.x)))
                    {
                        if (current.distance + 1 < min)
                        {
                            if (startingPoint.y == 0)
                            {
                                pq.Enqueue((current.x, current.y + 1, current.distance + m), ((bounds.x2 - current.x) + (bounds.y2 - current.y)) * current.distance);
                            } else
                            {
                                pq.Enqueue((current.x, current.y + 1, current.distance + m), ((current.x) + (current.y)) * current.distance);
                            }
                            if (current.y + 1 == endingPoint.y)
                            {
                                if (current.distance + 1 < min) min = current.distance + 1;
                            }
                            visited.Add((current.x, current.y + 1, current.distance + m));
                        }
                    }
                    if (visited.Count(d => d.x == current.x && d.y == current.y - 1 && d.minute == current.distance + 1) < 1
                    && blizzardsEachMinute[current.distance + m].Count(b => b.x == current.x && b.y == current.y - 1) == 0
                    && current.y - 1 > bounds.y1 || (current.y - 1 == endingPoint.y && current.x == endingPoint.x) || (current.y + 1 == startingPoint.y && current.x == startingPoint.x))
                    {
                        if (current.distance + 1 < min)
                        {
                            if (startingPoint.y == 0)
                            {
                                pq.Enqueue((current.x, current.y - 1, current.distance + m), ((bounds.x2 - current.x) + (bounds.y2 - current.y)) * current.distance);
                            } else
                            {
                                pq.Enqueue((current.x, current.y - 1, current.distance + m), ((current.x) + (current.y)) * current.distance);
                            }
                            if (current.y - 1 == endingPoint.y)
                            {
                                if (current.distance + 1 < min) min = current.distance + 1;
                            }
                            visited.Add((current.x, current.y - 1, current.distance + m));
                        }
                        
                    }
                    if (blizzardsEachMinute[current.distance + m].Count(b => b.x == current.x && b.y == current.y) == 0)
                    {
                        if (startingPoint.y == 0)
                        {
                            pq.Enqueue((current.x, current.y, current.distance + m), ((bounds.x2 - current.x) + (bounds.y2 - current.y)) * current.distance);
                        } else
                        {
                            pq.Enqueue((current.x, current.y, current.distance + m), ((current.x) + (current.y)) * current.distance);
                        }
                    }
                    //if (pq.Count == pqcount)
                    //{
                    //    if (blizzardsEachMinute[current.distance + m].Count(b => b.x == current.x + 1 && b.y == current.y) == 0
                    //     && current.x + 1 != bounds.x2)
                    //    {
                    //        visited.Add((current.x + 1, current.y, current.distance + m));
                    //        pq.Enqueue((current.x + 1, current.y, current.distance + m), current.distance + m);
                    //    }
                    //    if (blizzardsEachMinute[current.distance + m].Count(b => b.x == current.x - 1 && b.y == current.y) == 0
                    //    && current.x - 1 != bounds.x1)
                    //    {
                    //        visited.Add((current.x - 1, current.y, current.distance + m));
                    //        pq.Enqueue((current.x - 1, current.y, current.distance + m), current.distance + m);
                    //    }
                    //    if (visited.Count(d => d.x == current.x && d.y == current.y + 1) == 0
                    //    && blizzardsEachMinute[current.distance + m].Count(b => b.x == current.x && b.y == current.y + 1) == 0
                    //    && current.y + 1 != bounds.y2)
                    //    {
                    //        visited.Add((current.x, current.y + 1, current.distance + m));
                    //        pq.Enqueue((current.x, current.y + 1, current.distance + m), current.distance + m);
                    //    }
                    //    if (blizzardsEachMinute[current.distance + m].Count(b => b.x == current.x && b.y == current.y - 1) == 0
                    //    && current.y - 1 != bounds.y1)
                    //    {
                    //        visited.Add((current.x, current.y - 1, current.distance + m));
                    //        pq.Enqueue((current.x, current.y - 1, current.distance + m), current.distance + m);
                    //    }
                    //}
                }
                //Console.WriteLine(min);
            }
            min = visited.Where(d => d.x == endingPoint.x && d.y == endingPoint.y).Min(d => d.minute);
            return min;
        }
    }
}
