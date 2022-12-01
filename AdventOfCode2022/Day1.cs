using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;

namespace AdventOfCode2022
{
    internal class Day1
    {
        static void Main(string[] args)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day1.txt");

            string data = File.ReadAllText(path);

            List<string> data_split = new List<string>(data.Split('\n'));

            List<int> caloriesPerElf = new List<int>();

            for (int i = 0; i < data_split.Count; i++)
            {
                int sum = 0;
                while (data_split[i].Trim() != "")
                {
                    sum += int.Parse(data_split[i].Trim());
                    i++;
                }
                caloriesPerElf.Add(sum);
            }

            Console.WriteLine(caloriesPerElf.Max());

            int topThreeSum = 0;
            for (int i = 0; i < 3; i++)
            {
                int currentMax = caloriesPerElf.Max();
                topThreeSum+= currentMax;
                caloriesPerElf.Remove(currentMax);
            }

            Console.WriteLine(topThreeSum);
        }
    }
}