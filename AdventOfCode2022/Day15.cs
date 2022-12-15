using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day15
    {
        private class Sensor
        {

            public Sensor(int x, int y, int bx, int by)
            {
                X = x;
                Y = y;
                BeaconX = bx;
                BeaconY = by;
                BeaconDist = Math.Abs(bx - x) + Math.Abs(by - y);
            }

            public int X { get; }
            public int Y { get; }
            public int BeaconX { get; }
            public int BeaconY { get; }
            public int BeaconDist { get; }

            public int CalculateDist(int x, int y)
            {
                return Math.Abs(X - x) + Math.Abs(Y - y);
            }

            public void CollectPointsToCheck(List<(int x, int y)> points)
            {
                (int x, int y) point = (X - BeaconDist - 1, Y);
                points.Add(point);
                while (point.x != X)
                {
                    point = (point.x + 1, point.y + 1);
                    points.Add(point);
                }
                while (point.y != Y)
                {
                    point = (point.x + 1, point.y - 1);
                    points.Add(point);
                }
                while (point.x != X)
                {
                    point = (point.x - 1, point.y - 1);
                    points.Add(point);
                }
                while (point.y != Y)
                {
                    point = (point.x - 1, point.y + 1);
                    points.Add(point);
                }
            }
    }

        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day15.txt");

            string[] data = File.ReadAllText(path).Split("\r\n");

            //string[] data = "Sensor at x=2, y=18: closest beacon is at x=-2, y=15\r\nSensor at x=9, y=16: closest beacon is at x=10, y=16\r\nSensor at x=13, y=2: closest beacon is at x=15, y=3\r\nSensor at x=12, y=14: closest beacon is at x=10, y=16\r\nSensor at x=10, y=20: closest beacon is at x=10, y=16\r\nSensor at x=14, y=17: closest beacon is at x=10, y=16\r\nSensor at x=8, y=7: closest beacon is at x=2, y=10\r\nSensor at x=2, y=0: closest beacon is at x=2, y=10\r\nSensor at x=0, y=11: closest beacon is at x=2, y=10\r\nSensor at x=20, y=14: closest beacon is at x=25, y=17\r\nSensor at x=17, y=20: closest beacon is at x=21, y=22\r\nSensor at x=16, y=7: closest beacon is at x=15, y=3\r\nSensor at x=14, y=3: closest beacon is at x=15, y=3\r\nSensor at x=20, y=1: closest beacon is at x=15, y=3".Split("\r\n");

            List<Sensor> sensors = new List<Sensor>();

            foreach (string line in data)
            {
                string[] line_split = line.Split("=");
                int x = int.Parse(line_split[1].Split(",")[0]);
                int y = int.Parse(line_split[2].Split(":")[0]);
                int bx = int.Parse(line_split[3].Split(",")[0]);
                int by = int.Parse(line_split[4]);

                sensors.Add(new Sensor(x, y, bx, by));
            }

            int sum = 0;
            int row = 2000000;
            HashSet<int> beaconXCords = new HashSet<int>();

            foreach (Sensor sensor in sensors)
            {
                if (sensor.BeaconY == row)
                {
                    beaconXCords.Add(sensor.BeaconX);
                }
            }

            HashSet<int> set = new HashSet<int>();
            foreach (Sensor sensor in sensors)
            {
                if (sensor.CalculateDist(sensor.X, row) > sensor.BeaconDist) continue;

                if (!set.Contains(sensor.X) && !beaconXCords.Contains(sensor.X))
                {
                    set.Add(sensor.X);
                    sum++;
                }
                int x = sensor.X + 1;
                while (sensor.CalculateDist(x, row) <= sensor.BeaconDist)
                {
                    if (beaconXCords.Contains(x) || set.Contains(x))
                    {
                        x++;
                        continue;
                    }
                    else
                    {
                        set.Add(x);
                        x++;
                        sum++;
                    }
                }
                x = sensor.X - 1;
                while (sensor.CalculateDist(x, row) <= sensor.BeaconDist)
                {
                    if (beaconXCords.Contains(x) || set.Contains(x))
                    {
                        x--;
                        continue;
                    }
                    else
                    {
                        set.Add(x);
                        x--;
                        sum++;
                    }
                }
            }

            Console.WriteLine(sum);
            //for (int i = -5; i < 30; i++)
            //{
            //    if (set.Contains(i))
            //    {
            //        Console.Write("#");
            //    }
            //    else
            //    {
            //        Console.Write(".");
            //    }
            //}

            //PART 2

            // original solution

            //for (int i = 0; i <= 4000000; i++)
            //{
            //    int ret = FindAllowedPointInLine(sensors, i, 0, 4000000);
            //    if (ret != -1)
            //    {
            //        Console.WriteLine(ret);
            //        BigInteger outcome = ret * 4;
            //        BigInteger mod = outcome % 10;
            //        outcome = outcome / 10;
            //        i += (int)mod * 1000000;
            //        Console.WriteLine($"{outcome}{i}");
            //        break;
            //    }
            //    Console.WriteLine(i);
            //}

            // improved solution
            List<(int x, int y)> pointsToCheck = new List<(int x, int y)>();
            foreach (Sensor sensor in sensors)
            {
                sensor.CollectPointsToCheck(pointsToCheck);
            }
            pointsToCheck = pointsToCheck.Where(point => point.x <= 4000000 && point.y <= 4000000 && point.x >= 0 && point.y >=0).ToList();

            foreach ((int x, int y) point in pointsToCheck)
            {
                bool ifGood = true;
                foreach (Sensor sensor in sensors)
                {
                    if (sensor.CalculateDist(point.x, point.y) <= sensor.BeaconDist)
                    {
                        ifGood = false;
                        break;
                    }
                }
                if (ifGood)
                {
                    BigInteger outcome = point.x * 4;
                    BigInteger mod = outcome % 10;
                    outcome = outcome / 10;
                    int y = point.y + (int)mod * 1000000;
                    Console.WriteLine($"{outcome}{y}");
                    break;
                }
            }
        }

        //private static int FindAllowedPointInLine(List<Sensor> sensors, int row, int upperBoundX, int lowerBoundX)
        //{
        //    List<(int x1, int x2)> illegalRanges = new List<(int x1, int x2)>();
        //    foreach (Sensor sensor in sensors)
        //    {
        //        int distance = sensor.CalculateDist(sensor.X, row);
        //        if (distance > sensor.BeaconDist) continue;

        //        illegalRanges.Add((sensor.X - (sensor.BeaconDist - distance), sensor.X + (sensor.BeaconDist - distance)));
        //    }

        //    while (illegalRanges.Count > 2)
        //    {
        //        var rangeOne = illegalRanges[0];
        //        illegalRanges.RemoveAt(0);
        //        (int x1, int x2) rangeTwo;
        //        for (int i = 0; i < illegalRanges.Count; i++)
        //        {
        //            rangeTwo = illegalRanges[i];
        //            if (!(rangeOne.x2 < rangeTwo.x1 - 1 || rangeTwo.x2 < rangeOne.x1 - 1))
        //            {
        //                illegalRanges.RemoveAt(i);
        //                int x1;
        //                int x2;
        //                if (rangeOne.x1 < rangeTwo.x1) x1 = rangeOne.x1;
        //                else x1 = rangeTwo.x1;

        //                if (rangeOne.x2 > rangeTwo.x2) x2 = rangeOne.x2;
        //                else x2 = rangeTwo.x2;

        //                illegalRanges.Add((x1, x2));
        //                break;
        //            }
        //        }
        //    }
        //    var rangeUno = illegalRanges[0];
        //    var rangeDos = illegalRanges[1];

        //    if (!(rangeUno.x2 < rangeDos.x1 - 1 || rangeDos.x2 < rangeUno.x1 - 1))
        //    {
        //        int x1;
        //        int x2;
        //        if (rangeUno.x1 < rangeDos.x1) x1 = rangeUno.x1;
        //        else x1 = rangeDos.x1;

        //        if (rangeUno.x2 > rangeDos.x2) x2 = rangeUno.x2;
        //        else x2 = rangeDos.x2;

        //        var range = (x1, x2);

        //        if (range.x1 > lowerBoundX) return lowerBoundX;
        //        if (range.x2 < upperBoundX) return upperBoundX;
        //        return -1;
        //    }

        //    if (rangeUno.x2 < rangeDos.x1) return rangeUno.x2 + 1;
        //    return rangeDos.x2 + 1;
        //}
    }
}
