using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AdventOfCode2022
{
    public class Day17
    {
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day17.txt");

            string data = File.ReadAllText(path);

            //string data = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";

            HashSet<(int x, int y)> rocks = new HashSet<(int x, int y)>();

            Console.WriteLine(SimulatePartOne(rocks, data, 2022));

            //PART 2

            var parameters = FindCycle(rocks, data);

            BigInteger cycles = (1_000_000_000_000 - parameters[0]) / parameters[1] - 1;
            BigInteger answer = parameters[2] + cycles * parameters[3];
            Console.WriteLine(answer);
        }

        private static int SimulatePartOne(HashSet<(int x, int y)> rocks, string data, int iterations)
        {
            for (int i = 0; i < 7; i++)
            {
                rocks.Add((i, 0));
            }
            int height = 0;
            int dataI = 0;
            for (long i = 0; i < iterations; i++)
            {
                //Console.WriteLine($"i: {i}, height: {height}");
                int x = 2;
                int y = height + 4;
                List<(int x, int y)> rockCords = new List<(int x, int y)>();
                switch (i % 5)
                {
                    case 0:
                        rockCords.Add((x, y));
                        rockCords.Add((x + 1, y));
                        rockCords.Add((x + 2, y));
                        rockCords.Add((x + 3, y));
                        break;
                    case 1:
                        rockCords.Add((x + 1, y));
                        rockCords.Add((x, y + 1));
                        rockCords.Add((x + 1, y + 2));
                        rockCords.Add((x + 1, y + 1));
                        rockCords.Add((x + 2, y + 1));
                        break;
                    case 2:
                        rockCords.Add((x, y));
                        rockCords.Add((x + 1, y));
                        rockCords.Add((x + 2, y));
                        rockCords.Add((x + 2, y + 1));
                        rockCords.Add((x + 2, y + 2));
                        break;
                    case 3:
                        rockCords.Add((x, y));
                        rockCords.Add((x, y + 1));
                        rockCords.Add((x, y + 2));
                        rockCords.Add((x, y + 3));
                        break;
                    case 4:
                        rockCords.Add((x, y));
                        rockCords.Add((x + 1, y + 1));
                        rockCords.Add((x + 1, y));
                        rockCords.Add((x, y + 1));
                        break;
                    default:
                        break;
                }
                while (true)
                {
                    //foreach ((int x, int y) cord in rockCords)
                    //{
                    //    Console.WriteLine(cord);
                    //}

                    //wind
                    if (data[dataI % data.Length] == '<' && rockCords.Min(c => c.x) > 0)
                    {
                        if (!rockCords.Any(c => rocks.Contains((c.x - 1, c.y))))
                        {
                            rockCords = rockCords.Select((c) => (c.x - 1, c.y)).ToList();
                        }
                    }
                    if (data[dataI % data.Length] == '>' && rockCords.Max(c => c.x) < 6)
                    {
                        if (!rockCords.Any(c => rocks.Contains((c.x + 1, c.y))))
                        {
                            rockCords = rockCords.Select((c) => (c.x + 1, c.y)).ToList();
                        }
                    }
                    dataI++;

                    //fall
                    bool isStaying = false;
                    foreach ((int x, int y) cord in rockCords)
                    {
                        if (rocks.Contains((cord.x, cord.y - 1)))
                        {
                            isStaying = true;
                            if (rockCords.Max(c => c.y) > height) height = rockCords.Max(c => c.y);
                            break;
                        }
                    }
                    if (isStaying)
                    {
                        foreach ((int x, int y) cord in rockCords)
                        {
                            rocks.Add(cord);
                            //Console.WriteLine($"Rock comes to rest: {cord.x},{cord.y}");
                        }
                        break;
                    }

                    rockCords = rockCords.Select((c) => (c.x, c.y - 1)).ToList();
                }
            }
            return height;
        }

        private static List<int> FindCycle(HashSet<(int x, int y)> rocks, string data)
        {
            List<List<(int x, int y)>> cordsAfterEach = new List<List<(int x, int y)>>();
            List<int> heightAfterEach = new List<int>();
            int counter = 0;
            for (int i = 0; i < 5000; i++)
            {
                rocks = new HashSet<(int x, int y)>();
                int height = SimulatePartOne(rocks, data, i);
                cordsAfterEach.Add(rocks.ToList());
                heightAfterEach.Add(height);
            }
            Console.WriteLine("finished");
            for (int startingNumber = 1010; startingNumber < 10000; startingNumber++)
            {
                for (int length = 1; startingNumber + length < 5000; length++)
                {
                    if (heightAfterEach[startingNumber + length] * 2 - heightAfterEach[startingNumber] == heightAfterEach[startingNumber + length * 2])
                    {
                        var cycleCords = cordsAfterEach[startingNumber + length].Where(c => !cordsAfterEach[startingNumber].Contains(c)).ToList();
                        var cycleCords2 = cordsAfterEach[startingNumber + length * 2].Where(c => !cordsAfterEach[startingNumber + length].Contains(c)).Select(c => (c.x, c.y - heightAfterEach[startingNumber + length] + heightAfterEach[startingNumber])).ToList();
                        if (cycleCords.All(cycleCords2.Contains))
                        {
                            Console.WriteLine($"start: {startingNumber}, length: {length}, height: {heightAfterEach[startingNumber + length]} height/cycle: {heightAfterEach[startingNumber + length] - heightAfterEach[startingNumber]}");
                            return new List<int> { startingNumber, length, heightAfterEach[startingNumber + length], heightAfterEach[startingNumber + length] - heightAfterEach[startingNumber] };
                        }
                    }
                    Console.WriteLine($"starting point: {startingNumber}, length: {length}");
                }
            }
            return null;
        }
    }
}
