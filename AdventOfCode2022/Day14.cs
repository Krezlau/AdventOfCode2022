using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AdventOfCode2022
{
    public class Day14
    {
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day14.txt");

            string[] data = File.ReadAllText(path).Split("\r\n");

            //string[] data = "498,4 -> 498,6 -> 496,6\r\n503,4 -> 502,4 -> 502,9 -> 494,9".Split("\r\n");

            int maxY = 0;
            int[,] map = ParseMap(data, ref maxY);

            Console.WriteLine(SimulateSand(map));

            map = ParseMap(data, ref maxY);

            Console.WriteLine(SimulateSandWithFloor(map, maxY + 2));

            //for (int i = 0; i < 200; i++)
            //{
            //    for (int j = 300; j < 700; j++)
            //    {
            //        if (map[i, j] == 0) Console.Write('.');
            //        if (map[i, j] == 1) Console.Write('#');
            //        if (map[i, j] == 2) Console.Write('o');
            //    }
            //    Console.Write("\n");
            //}
        }

        private static int[,] ParseMap(string[] data, ref int maxY)
        {
            maxY = 0;
            int[,] map = new int[200, 1000];
            foreach (string line in data)
            {
                string[] line_split = line.Split(" -> ");
                for (int i = 0; i < line_split.Length; i++)
                {
                    int x = int.Parse(line_split[i].Split(",")[0]);
                    int y = int.Parse(line_split[i].Split(",")[1]);

                    if (y > maxY) maxY = y;

                    map[y, x] = 1;
                    if (i + 1 < line_split.Length)
                    {
                        int x2 = int.Parse(line_split[i + 1].Split(",")[0]);
                        int y2 = int.Parse(line_split[i + 1].Split(",")[1]);
                        if (x2 == x && y2 < y)
                        {
                            for (int j = y; j > y2; j--)
                            {
                                map[j, x] = 1;
                            }
                        }
                        if (x2 == x && y2 > y)
                        {
                            for (int j = y; j < y2; j++)
                            {
                                map[j, x] = 1;
                            }
                        }
                        if (y2 == y && x2 < x)
                        {
                            for (int j = x; j > x2; j--)
                            {
                                map[y, j] = 1;
                            }
                        }
                        if (y2 == y && x2 > x)
                        {
                            for (int j = x; j < x2; j++)
                            {
                                map[y, j] = 1;
                            }
                        }
                    }
                }
            }
            return map;
        }

        private static int SimulateSand(int[,] map)
        {
            int i = 0;
            while (true)
            {
                i++;
                int x = 500;
                int y = 0;
                while (true)
                {
                    if (y + 1 == 200) return i - 1;
                    if (map[y + 1, x] == 0)
                    {
                        y++;
                        continue;
                    }
                    if (map[y + 1, x - 1] == 0)
                    {
                        y++;
                        x--;
                        continue;
                    }
                    if (map[y + 1, x + 1] == 0)
                    {
                        y++;
                        x++;
                        continue;
                    }
                    map[y, x] = 2;
                    break;
                }
            }
        }

        private static int SimulateSandWithFloor(int[,] map, int floorLevel)
        {
            int i = 0;
            while (map[0, 500] == 0)
            {
                i++;
                int x = 500;
                int y = 0;
                while (true)
                {
                    if (y + 1 == floorLevel)
                    {
                        map[y, x] = 2;
                        break;
                    }
                    if (map[y + 1, x] == 0)
                    {
                        y++;
                        continue;
                    }
                    if (map[y + 1, x - 1] == 0)
                    {
                        y++;
                        x--;
                        continue;
                    }
                    if (map[y + 1, x + 1] == 0)
                    {
                        y++;
                        x++;
                        continue;
                    }
                    map[y, x] = 2;
                    break;
                }
            }
            return i;
        }
    }
}
