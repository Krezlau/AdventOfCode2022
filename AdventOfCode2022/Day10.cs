using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day10
    {
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day10.txt");

            string[] data = File.ReadAllText(path).Split("\r\n");

            //string[] data = "addx 15\r\naddx -11\r\naddx 6\r\naddx -3\r\naddx 5\r\naddx -1\r\naddx -8\r\naddx 13\r\naddx 4\r\nnoop\r\naddx -1\r\naddx 5\r\naddx -1\r\naddx 5\r\naddx -1\r\naddx 5\r\naddx -1\r\naddx 5\r\naddx -1\r\naddx -35\r\naddx 1\r\naddx 24\r\naddx -19\r\naddx 1\r\naddx 16\r\naddx -11\r\nnoop\r\nnoop\r\naddx 21\r\naddx -15\r\nnoop\r\nnoop\r\naddx -3\r\naddx 9\r\naddx 1\r\naddx -3\r\naddx 8\r\naddx 1\r\naddx 5\r\nnoop\r\nnoop\r\nnoop\r\nnoop\r\nnoop\r\naddx -36\r\nnoop\r\naddx 1\r\naddx 7\r\nnoop\r\nnoop\r\nnoop\r\naddx 2\r\naddx 6\r\nnoop\r\nnoop\r\nnoop\r\nnoop\r\nnoop\r\naddx 1\r\nnoop\r\nnoop\r\naddx 7\r\naddx 1\r\nnoop\r\naddx -13\r\naddx 13\r\naddx 7\r\nnoop\r\naddx 1\r\naddx -33\r\nnoop\r\nnoop\r\nnoop\r\naddx 2\r\nnoop\r\nnoop\r\nnoop\r\naddx 8\r\nnoop\r\naddx -1\r\naddx 2\r\naddx 1\r\nnoop\r\naddx 17\r\naddx -9\r\naddx 1\r\naddx 1\r\naddx -3\r\naddx 11\r\nnoop\r\nnoop\r\naddx 1\r\nnoop\r\naddx 1\r\nnoop\r\nnoop\r\naddx -13\r\naddx -19\r\naddx 1\r\naddx 3\r\naddx 26\r\naddx -30\r\naddx 12\r\naddx -1\r\naddx 3\r\naddx 1\r\nnoop\r\nnoop\r\nnoop\r\naddx -9\r\naddx 18\r\naddx 1\r\naddx 2\r\nnoop\r\nnoop\r\naddx 9\r\nnoop\r\nnoop\r\nnoop\r\naddx -1\r\naddx 2\r\naddx -37\r\naddx 1\r\naddx 3\r\nnoop\r\naddx 15\r\naddx -21\r\naddx 22\r\naddx -6\r\naddx 1\r\nnoop\r\naddx 2\r\naddx 1\r\nnoop\r\naddx -10\r\nnoop\r\nnoop\r\naddx 20\r\naddx 1\r\naddx 2\r\naddx 2\r\naddx -6\r\naddx -11\r\nnoop\r\nnoop\r\nnoop".Split("\r\n");

            int registerValue = 1;
            int sum = 0;
            int cycle = 1;
            string image = "";

            foreach (string line in data)
            {
                int val = Math.Abs(registerValue - ((cycle - 1) % 40));
                if (cycle % 40 == 0) val = Math.Abs(registerValue - (cycle - 1));

                if (val <= 1) image += '#';
                else image += '.';

                //Console.WriteLine($"register: {registerValue}, cycle: {cycle}, char: {image[^1]}");

                if (cycle is 20 or 60 or 100 or 140 or 180 or 220)
                {
                    sum += cycle * registerValue;
                }
                if (line != "noop")
                {
                    string[] command = line.Split(" ");
                    cycle++;

                    val = Math.Abs(registerValue - ((cycle - 1) % 40));
                    if (cycle % 40 == 0) val = Math.Abs(registerValue - (cycle - 1));

                    if (val <= 1) image += '#';
                    else image += '.';

                    //Console.WriteLine($"register: {registerValue}, cycle: {cycle}, char: {image[^1]}");

                    if (cycle is 20 or 60 or 100 or 140 or 180 or 220)
                    {
                        sum += cycle * registerValue;
                    }
                    registerValue += int.Parse(command[1]);
                }
                cycle++;
            }

            Console.WriteLine(sum);
            for (int i = 200; i >= 40; i -= 40)
            {
                image = image.Insert(i, "\n");
            }

            Console.WriteLine(image);
        }
    }
}
