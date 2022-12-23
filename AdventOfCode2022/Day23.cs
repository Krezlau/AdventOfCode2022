using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day23
    {
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day23.txt");

            string[] data = File.ReadAllText(path).Split("\r\n");

            //string[] data = "....#..\r\n..###.#\r\n#...#.#\r\n.#...##\r\n#.###..\r\n##.#.##\r\n.#..#..".Split("\r\n");

            var elves = ParseData(data);

            SimulateRounds(ref elves, 10);

            (int x1, int y1, int x2, int y2) rectangle = (elves.Min(e => e.x), elves.Min(e => e.y), elves.Max(e => e.x), elves.Max(e => e.y));
            int answer = (rectangle.x2 - rectangle.x1 + 1) * (rectangle.y2 - rectangle.y1 + 1);
            answer -= elves.Count;
            Console.WriteLine(answer);

            //PART TWO
            elves = ParseData(data);
            int ans = SimulateRounds(ref elves, 1000);
            Console.WriteLine(ans);

        }

        private static HashSet<(int x, int y)> ParseData(string[] data)
        {
            HashSet<(int x, int y)> elves = new HashSet<(int x, int y)> ();
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    if (data[i][j] == '#')
                    {
                        elves.Add((j, i));
                    }
                }
            }
            return elves;
        }

        private static int SimulateRounds(ref HashSet<(int x, int y)> elves, int numberOfRounds)
        {
            HashSet<(int x, int y)> futureElves = new HashSet<(int x, int y)> ();
            HashSet<(int x, int y, int d)> futureElvesDirection = new HashSet<(int x, int y, int d)>();
            List<(int x, int y)> toDelete = new List<(int x, int y)>();

            int direction = 0;
            for (int i = 0; i < numberOfRounds; i++)
            {
                foreach (var elve in elves)
                {
                    bool ifAdded = false;
                    if (!(elves.Contains((elve.x - 1, elve.y))
                          || elves.Contains((elve.x - 1, elve.y + 1))
                          || elves.Contains((elve.x, elve.y + 1))
                          || elves.Contains((elve.x + 1, elve.y + 1))
                          || elves.Contains((elve.x + 1, elve.y))
                          || elves.Contains((elve.x + 1, elve.y - 1))
                          || elves.Contains((elve.x, elve.y - 1))
                          || elves.Contains((elve.x - 1, elve.y - 1))))
                    {
                        futureElves.Add(elve);
                        continue;
                    }
                    for (int j = direction; j < direction + 4; j++)
                    {
                        if (j % 4 == 0 && !(elves.Contains((elve.x - 1, elve.y - 1))
                                            || elves.Contains((elve.x, elve.y - 1))
                                            || elves.Contains((elve.x + 1, elve.y - 1))))
                        {
                            if (futureElves.Contains((elve.x, elve.y - 1)))
                            {
                                toDelete.Add((elve.x, elve.y - 1));
                                break;
                            } else
                            {
                                futureElves.Add((elve.x, elve.y - 1));
                                futureElvesDirection.Add((elve.x, elve.y - 1, 0));
                            }
                            ifAdded = true;
                            break;
                        }
                        if (j % 4 == 1 && !(elves.Contains((elve.x - 1, elve.y + 1))
                                            || elves.Contains((elve.x, elve.y + 1))
                                            || elves.Contains((elve.x + 1, elve.y + 1))))
                        {
                            if (futureElves.Contains((elve.x, elve.y + 1)))
                            {
                                toDelete.Add((elve.x, elve.y + 1));
                                break;
                            }
                            else
                            {
                                futureElves.Add((elve.x, elve.y + 1));
                                futureElvesDirection.Add((elve.x, elve.y + 1, 1));
                            }
                            ifAdded = true;
                            break;
                        }
                        if (j % 4 == 2 && !(elves.Contains((elve.x - 1, elve.y - 1))
                                            || elves.Contains((elve.x - 1, elve.y))
                                            || elves.Contains((elve.x - 1, elve.y + 1))))
                        {
                            if (futureElves.Contains((elve.x - 1, elve.y)))
                            {
                                toDelete.Add((elve.x - 1, elve.y));
                                break;
                            }
                            else
                            {
                                futureElves.Add((elve.x - 1, elve.y));
                                futureElvesDirection.Add((elve.x - 1, elve.y, 2));
                            }
                            ifAdded = true;
                            break;
                        }
                        if (j % 4 == 3 && !(elves.Contains((elve.x + 1, elve.y + 1))
                                            || elves.Contains((elve.x + 1, elve.y))
                                            || elves.Contains((elve.x + 1, elve.y - 1))))
                        {
                            if (futureElves.Contains((elve.x + 1, elve.y)))
                            {
                                toDelete.Add((elve.x + 1, elve.y));
                                break;
                            }
                            else
                            {
                                futureElves.Add((elve.x + 1, elve.y));
                                futureElvesDirection.Add((elve.x + 1, elve.y, 3));
                            }
                            ifAdded = true;
                            break;
                        }
                    }
                    if (!ifAdded) futureElves.Add(elve);
                }
                foreach (var elve in toDelete)
                {
                    futureElves.Remove(elve);
                    int dir = futureElvesDirection.Where(e => e.x == elve.x && e.y == elve.y).Single().d;
                    if (dir == 0) futureElves.Add((elve.x, elve.y + 1));
                    if (dir == 1) futureElves.Add((elve.x, elve.y - 1));
                    if (dir == 2) futureElves.Add((elve.x + 1, elve.y));
                    if (dir == 3) futureElves.Add((elve.x - 1, elve.y));
                }
                if (elves.All(futureElves.Contains)) return i + 1;
                elves = futureElves.ToHashSet();
                futureElves.Clear();
                futureElvesDirection.Clear();
                toDelete.Clear();
                direction++;

                //for (int x = -5; x < 15; x++)
                //{
                //    for (int j = -5; j < 10; j++)
                //    {
                //        if (elves.Contains((j, x)))
                //        {
                //            Console.Write('#');
                //            continue;
                //        }
                //        Console.Write('.');
                //    }
                //    Console.Write('\n');
                //}
                //Console.Write('\n');
                //Console.WriteLine(direction);
            }
            return numberOfRounds;
        }
    }
}
