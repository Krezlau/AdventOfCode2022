using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day18
    {
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day18.txt");

            string[] data = File.ReadAllText(path).Split("\r\n");

            //string[] data = "2,2,2\r\n1,2,2\r\n3,2,2\r\n2,1,2\r\n2,3,2\r\n2,2,1\r\n2,2,3\r\n2,2,4\r\n2,2,6\r\n1,2,5\r\n3,2,5\r\n2,1,5\r\n2,3,5".Split("\r\n");

            HashSet<(int x, int y, int z)> cubes = new HashSet<(int x, int y, int z)>();

            foreach (string line in data)
            {
                string[] line_split = line.Split(',');
                cubes.Add((int.Parse(line_split[0]), int.Parse(line_split[1]), int.Parse(line_split[2])));
            }

            int sum = 0;
            List<(int x, int y, int z)> cubesList = cubes.ToList();

            HashSet<(int x, int y, int z)> exposed = new HashSet<(int x, int y, int z)>();

            for (int i = 0; i < cubesList.Count; i++)
            {
                int sidesExposed = 6;
                var cube = cubesList[i];
                if (cubes.Contains((cube.x - 1, cube.y, cube.z))) sidesExposed--;
                else exposed.Add((cube.x - 1, cube.y, cube.z));

                if (cubes.Contains((cube.x + 1, cube.y, cube.z))) sidesExposed--;
                else exposed.Add((cube.x + 1, cube.y, cube.z));

                if (cubes.Contains((cube.x, cube.y - 1, cube.z))) sidesExposed--;
                else exposed.Add((cube.x, cube.y - 1, cube.z));

                if (cubes.Contains((cube.x, cube.y + 1, cube.z))) sidesExposed--;
                else exposed.Add((cube.x, cube.y + 1, cube.z));

                if (cubes.Contains((cube.x, cube.y, cube.z - 1))) sidesExposed--;
                else exposed.Add((cube.x, cube.y, cube.z - 1));

                if (cubes.Contains((cube.x, cube.y, cube.z + 1))) sidesExposed--;
                else exposed.Add((cube.x, cube.y, cube.z + 1));

                sum += sidesExposed;


            }
            Console.WriteLine(sum);

            // PART TWO

            int[,,] cords = new int[30, 30, 30];
            for (int z = 5; z < 30; z++)
            {
                for (int y = 5; y < 30; y++)
                {
                    for (int x = 5; x < 30; x++)
                    {
                        if (cubes.Contains((x - 5, y - 5, z - 5)))
                        {
                            cords[x, y, z] = 1;
                        }
                    }
                }
            }
            bool[,,] isVisited = new bool[30, 30, 30];
            Queue<(int x, int y, int z)> q = new Queue<(int x, int y, int z)>();

            q.Enqueue((0, 0, 0));

            while (q.Count != 0)
            {
                var cord = q.Dequeue();
                isVisited[cord.x, cord.y, cord.z] = true;
                List<(int x, int y, int z)> sides = new List<(int x, int y, int z)>
                        {
                            (cord.x - 1, cord.y, cord.z),
                            (cord.x + 1, cord.y, cord.z),
                            (cord.x, cord.y - 1, cord.z),
                            (cord.x, cord.y + 1, cord.z),
                            (cord.x, cord.y, cord.z - 1),
                            (cord.x, cord.y, cord.z + 1)
                        };
                foreach (var side in sides)
                {
                    if (side.x >= 0 && side.x < 30 && side.y >= 0 && side.y < 30 && side.z >= 0 && side.z < 30 && !isVisited[side.x, side.y, side.z] && cords[side.x, side.y, side.z] != 1)
                    {
                        isVisited[side.x, side.y, side.z] = true;
                        q.Enqueue(side);
                    }
                }
            }

            //for (int z = 0; z < 30; z++)
            //{
            //    Console.Write('\n');
            //    Console.Write('\n');
            //    for (int y = 0; y < 30; y++)
            //    {
            //        Console.Write('\n');
            //        for (int x = 0; x < 30; x++)
            //        {
            //            if (cubes.Contains((x - 5, y - 5, z - 5)))
            //            {
            //                Console.Write("#");
            //            }
            //            else if (isVisited[x, y, z])
            //            {
            //                Console.Write("*");
            //            }
            //            else
            //            {
            //                Console.Write(".");
            //            }
            //        }
            //    }
            //}

            for (int z = 0; z < 30; z++)
            {
                for (int y = 0; y < 30; y++)
                {
                    for (int x = 0; x < 30; x++)
                    {
                        if (!isVisited[x, y, z] && exposed.Contains((x - 5, y - 5, z - 5)))
                        {
                            (int x, int y, int z) airCube = (x - 5, y - 5, z - 5);
                            List<(int x, int y, int z)> sides = new List<(int x, int y, int z)>
                            {
                                (airCube.x - 1, airCube.y, airCube.z),
                                (airCube.x + 1, airCube.y, airCube.z),
                                (airCube.x, airCube.y - 1, airCube.z),
                                (airCube.x, airCube.y + 1, airCube.z),
                                (airCube.x, airCube.y, airCube.z - 1),
                                (airCube.x, airCube.y, airCube.z + 1)
                            };
                            sum -= sides.Count(cubes.Contains);
                        }
                    }
                }
            }
            Console.WriteLine(sum);

        }
    }
}
