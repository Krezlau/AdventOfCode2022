using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day9
    {
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day9.txt");

            string[] data = File.ReadAllText(path).Split("\r\n");

            //string[] data = "R 4\r\nU 4\r\nL 3\r\nD 1\r\nR 4\r\nD 1\r\nL 5\r\nR 2".Split("\r\n");

            //string[] data = "R 5\r\nU 8\r\nL 8\r\nD 3\r\nR 17\r\nD 10\r\nL 25\r\nU 20".Split("\r\n");

            List<string> visitedByTail = new List<string>
            {
                "0,0"
            };

            int headX = 0;
            int headY = 0;
            int tailX = 0;
            int tailY = 0;

            foreach (string line in data)
            {
                //Console.WriteLine($"{tailX},{tailY} | {headX},{headY}");
                string[] command = line.Split(" ");
                for (int i = 0; i < int.Parse(command[1]); i++)
                {
                    //move head
                    if (command[0] == "U") headY++;
                    if (command[0] == "D") headY--;
                    if (command[0] == "L") headX--;
                    if (command[0] == "R") headX++;
                    //move tail
                    if (Math.Abs(headX - tailX) <= 1 && Math.Abs(headY - tailY) <= 1) continue;
                    if (Math.Abs(headX - tailX) == 0 && Math.Abs(headY - tailY) == 2)
                    {
                        if (headY - tailY > 0) tailY++;
                        if (headY - tailY < 0) tailY--;
                        if (!visitedByTail.Contains($"{tailX},{tailY}")) visitedByTail.Add($"{tailX},{tailY}");
                        continue;
                    }
                    if (Math.Abs(headX - tailX) == 2 && Math.Abs(headY - tailY) == 0)
                    {
                        if (headX - tailX > 0) tailX++;
                        if (headX - tailX < 0) tailX--;
                        if (!visitedByTail.Contains($"{tailX},{tailY}")) visitedByTail.Add($"{tailX},{tailY}");
                        continue;
                    }
                    if (Math.Abs(headX - tailX) == 2 && Math.Abs(headY - tailY) == 1)
                    {
                        if (headY - tailY > 0) tailY++;
                        if (headY - tailY < 0) tailY--;
                        if (headX - tailX > 0) tailX++;
                        if (headX - tailX < 0) tailX--;
                        if (!visitedByTail.Contains($"{tailX},{tailY}")) visitedByTail.Add($"{tailX},{tailY}");
                        continue;
                    }
                    if (Math.Abs(headX - tailX) == 1 && Math.Abs(headY - tailY) == 2)
                    {
                        if (headY - tailY > 0) tailY++;
                        if (headY - tailY < 0) tailY--;
                        if (headX - tailX > 0) tailX++;
                        if (headX - tailX < 0) tailX--;
                        if (!visitedByTail.Contains($"{tailX},{tailY}")) visitedByTail.Add($"{tailX},{tailY}");
                        continue;
                    }
                }

            }

            Console.WriteLine(visitedByTail.Count);

            //second part

            visitedByTail = new List<string>
            {
                "0,0"
            };

            List<(int x, int y)> cords = new List<(int, int)>(10);
            for (int i = 0; i < 10; i++)
            {
                cords.Add((0, 0));
            }

            foreach (string line in data)
            {
                //Console.WriteLine($"{tailX},{tailY} | {headX},{headY}");
                string[] command = line.Split(" ");
                for (int i = 0; i < int.Parse(command[1]); i++)
                {
                    //move head
                    if (command[0] == "U") cords[0] = (cords[0].x, cords[0].y + 1);
                    if (command[0] == "D") cords[0] = (cords[0].x, cords[0].y - 1);
                    if (command[0] == "L") cords[0] = (cords[0].x - 1, cords[0].y);
                    if (command[0] == "R") cords[0] = (cords[0].x + 1, cords[0].y);
                    //move the rest
                    for (int j = 1; j < 10; j++)
                    {
                        if (Math.Abs(cords[j - 1].x - cords[j].x) <= 1 && Math.Abs(cords[j - 1].y - cords[j].y) <= 1) continue;
                        if (Math.Abs(cords[j - 1].x - cords[j].x) == 0 && Math.Abs(cords[j - 1].y - cords[j].y) == 2)
                        {
                            if (cords[j - 1].y - cords[j].y > 0) cords[j] = (cords[j].x, cords[j].y + 1);
                            if (cords[j - 1].y - cords[j].y < 0) cords[j] = (cords[j].x, cords[j].y - 1);
                            continue;
                        }
                        if (Math.Abs(cords[j - 1].x - cords[j].x) == 2 && Math.Abs(cords[j - 1].y - cords[j].y) == 0)
                        {
                            if (cords[j - 1].x - cords[j].x > 0) cords[j] = (cords[j].x + 1, cords[j].y);
                            if (cords[j - 1].x - cords[j].x < 0) cords[j] = (cords[j].x - 1, cords[j].y);
                            continue;
                        }
                        if (Math.Abs(cords[j - 1].x - cords[j].x) == 2 && Math.Abs(cords[j - 1].y - cords[j].y) == 1)
                        {
                            if (cords[j - 1].y - cords[j].y > 0) cords[j] = (cords[j].x, cords[j].y + 1);
                            if (cords[j - 1].y - cords[j].y < 0) cords[j] = (cords[j].x, cords[j].y - 1);
                            if (cords[j - 1].x - cords[j].x > 0) cords[j] = (cords[j].x + 1, cords[j].y);
                            if (cords[j - 1].x - cords[j].x < 0) cords[j] = (cords[j].x - 1, cords[j].y);
                            continue;
                        }
                        if (Math.Abs(cords[j - 1].x - cords[j].x) == 1 && Math.Abs(cords[j - 1].y - cords[j].y) == 2)
                        {
                            if (cords[j - 1].y - cords[j].y > 0) cords[j] = (cords[j].x, cords[j].y + 1);
                            if (cords[j - 1].y - cords[j].y < 0) cords[j] = (cords[j].x, cords[j].y - 1);
                            if (cords[j - 1].x - cords[j].x > 0) cords[j] = (cords[j].x + 1, cords[j].y);
                            if (cords[j - 1].x - cords[j].x < 0) cords[j] = (cords[j].x - 1, cords[j].y);
                            continue;
                        }
                        if (Math.Abs(cords[j - 1].x - cords[j].x) == 2 && Math.Abs(cords[j - 1].y - cords[j].y) == 2)
                        {
                            if (cords[j - 1].y - cords[j].y > 0) cords[j] = (cords[j].x, cords[j].y + 1);
                            if (cords[j - 1].y - cords[j].y < 0) cords[j] = (cords[j].x, cords[j].y - 1);
                            if (cords[j - 1].x - cords[j].x > 0) cords[j] = (cords[j].x + 1, cords[j].y);
                            if (cords[j - 1].x - cords[j].x < 0) cords[j] = (cords[j].x - 1, cords[j].y);
                            continue;
                        }
                    }
                    if (!visitedByTail.Contains($"{cords[9].x},{cords[9].y}")) visitedByTail.Add($"{cords[9].x},{cords[9].y}");
                    //Console.WriteLine($"{cords[1].x},{cords[1].y}");
                    //Console.WriteLine($"{cords[0].x},{cords[0].y}");
                }

            }
            Console.WriteLine(visitedByTail.Count);
        }
    }
}
