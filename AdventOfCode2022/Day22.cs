using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day22
    {
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day22.txt");

            string[] data = File.ReadAllText(path).Split("\r\n");

            //string[] data = "        ...#\r\n        .#..\r\n        #...\r\n        ....\r\n...#.......#\r\n........#...\r\n..#....#....\r\n..........#.\r\n        ...#....\r\n        .....#..\r\n        .#......\r\n        ......#.\r\n\r\n10R5L5R10L4R5L5".Split("\r\n");

            int[,] map = new int[1,1];
            string moves = ParseData(data, ref map);

            //for (int i = 0; i < map.GetLength(0); i++)
            //{
            //    for (int j = 0; j < map.GetLength(1); j++)
            //    {
            //        if (map[i, j] == -1)
            //        {
            //            Console.Write(" ");
            //            continue;
            //        }
            //        Console.Write(map[i, j]);
            //    }
            //    Console.Write('\n');
            //}

            moves = moves.Replace("R", ",R,");
            moves = moves.Replace("L", ",L,");

            string[] movesList = moves.Split(",");

            // 0 -> right
            // 1 -> down
            // 2 -> left
            // 3 -> up
            int direction = 0;
            (int i, int j) currentTile = (0, 0);
            for (int i = 0; i < map.GetLength(1); i++)
            {
                if (map[0, i] != -1) currentTile = (0, i);
            }


            for (int i = 0; i < movesList.Length; i++)
            {
                if (movesList[i] == "R")
                {
                    direction = (direction + 1) % 4;
                    continue;
                }
                if (movesList[i] == "L")
                {
                    direction--;
                    if (direction == -1) direction = 3;
                    continue;
                }
                int tiles = int.Parse(movesList[i]);
                for (int j = 0; j < tiles; j++)
                {
                    (int i, int j) newTile = (0, 0);
                    switch (direction)
                    {
                        case 0:
                            newTile = (currentTile.i, currentTile.j + 1);
                            if (newTile.j >= map.GetLength(1) || map[newTile.i, newTile.j] == -1)
                            {
                                int newJ = 0;
                                while (map[currentTile.i, newJ] == -1)
                                {
                                    newJ++;
                                }
                                newTile = (currentTile.i, newJ);
                            }
                            break;
                        case 1:
                            newTile = (currentTile.i + 1, currentTile.j);
                            if (newTile.i >= map.GetLength(0) || map[newTile.i, newTile.j] == -1)
                            {
                                int newI = 0;
                                while (map[newI, currentTile.j] == -1)
                                {
                                    newI++;
                                }
                                newTile = (newI, currentTile.j);
                            }
                            break;
                        case 2:
                            newTile = (currentTile.i, currentTile.j - 1);
                            if (newTile.j < 0 || map[newTile.i, newTile.j] == -1)
                            {
                                int newJ = map.GetLength(1) - 1;
                                while (map[currentTile.i, newJ] == -1)
                                {
                                    newJ--;
                                }
                                newTile = (currentTile.i, newJ);
                            }
                            break;
                        case 3:
                            newTile = (currentTile.i - 1, currentTile.j);
                            if (newTile.i < 0 || map[newTile.i, newTile.j] == -1)
                            {
                                int newI = map.GetLength(0) - 1;
                                while (map[newI, currentTile.j] == -1)
                                {
                                    newI--;
                                }
                                newTile = (newI, currentTile.j);
                            }
                            break;
                        default:
                            break;
                    }
                    if (map[newTile.i, newTile.j] == 1) break;
                    currentTile = newTile;
                }
            }
            long answer = 1000 * (currentTile.i + 1) + 4 * (currentTile.j + 1) + direction;
            Console.WriteLine(answer);
        }

        private static string ParseData(string[] data, ref int[,] map)
        {
            int maxLen = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Length > maxLen) maxLen = data[i].Length;
            }

            map = new int[data.Length - 2, maxLen - 1];
            for (int i = 0; i < data.Length - 2; i++)
            {
                int j;
                for (j = 0; j < data[i].Length; j++)
                {
                    if (data[i][j] == ' ')
                    {
                        map[i, j] = -1;
                    }
                    if (data[i][j] == '#')
                    {
                        map[i, j] = 1;
                    }
                }
                while (j < maxLen - 1)
                {
                    map[i, j++] = -1;
                }
            }
            return data[data.Length - 1];
        }
    }
}
