using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day2
    {
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day2.txt");

            string data = File.ReadAllText(path);

            //string data = "A Y\nB X\nC Z\n";

            string[] data_split = data.Split('\n');

            int pointsTotal = 0;
            for (int i = 0; i < data_split.Length; i++)
            {
                string[] thisRound = data_split[i].Trim().Split(' ');

                if (thisRound[1] == "X")
                {
                    pointsTotal += 1;
                    if (thisRound[0] == "A")
                    {
                        pointsTotal += 3;
                        continue;
                    }
                    if (thisRound[0] == "B")
                    {
                        continue;
                    }
                    if (thisRound[0] == "C")
                    {
                        pointsTotal += 6;
                        continue;
                    }
                }
                if (thisRound[1] == "Y")
                {
                    pointsTotal += 2;
                    if (thisRound[0] == "A")
                    {
                        pointsTotal += 6;
                        continue;
                    }
                    if (thisRound[0] == "B")
                    {
                        pointsTotal += 3;
                        continue;
                    }
                    if (thisRound[0] == "C")
                    {
                        continue;
                    }
                }
                if (thisRound[1] == "Z")
                {
                    pointsTotal += 3;
                    if (thisRound[0] == "A")
                    {
                        continue;
                    }
                    if (thisRound[0] == "B")
                    {
                        pointsTotal += 6;
                        continue;
                    }
                    if (thisRound[0] == "C")
                    {
                        pointsTotal += 3;
                        continue;
                    }
                }
            }
            Console.WriteLine(pointsTotal);

            int correctPointsTotal = 0;
            for (int i = 0; i < data_split.Length; i++)
            {
                string[] thisRound = data_split[i].Trim().Split(' ');

                if (thisRound[1] == "X")
                {
                    if (thisRound[0] == "A")
                    {
                        correctPointsTotal += 3;
                    }
                    if (thisRound[0] == "B")
                    {
                        correctPointsTotal += 1;
                    }
                    if (thisRound[0] == "C")
                    {
                        correctPointsTotal += 2;
                    }
                }
                if (thisRound[1] == "Y")
                {
                    correctPointsTotal += 3;
                    if (thisRound[0] == "A")
                    {
                        correctPointsTotal += 1;
                    }
                    if (thisRound[0] == "B")
                    {
                        correctPointsTotal += 2;
                    }
                    if (thisRound[0] == "C")
                    {
                        correctPointsTotal += 3;
                    }
                }
                if (thisRound[1] == "Z")
                {
                    correctPointsTotal += 6;
                    if (thisRound[0] == "A")
                    {
                        correctPointsTotal += 2;
                    }
                    if (thisRound[0] == "B")
                    {
                        correctPointsTotal += 3;
                    }
                    if (thisRound[0] == "C")
                    {
                        correctPointsTotal += 1;
                    }
                }
            }
            Console.WriteLine(correctPointsTotal);
        }
    }
}

