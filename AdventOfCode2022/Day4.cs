using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day4
    {
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day4.txt");

            string data = File.ReadAllText(path);

            //string data = "2-4,6-8\r\n2-3,4-5\r\n5-7,7-9\r\n2-8,3-7\r\n6-6,4-6\r\n2-6,4-8";

            string[] data_split = data.Split('\n');

            int sum = 0;
            int overlapSum = 0;
            foreach (string line in data_split)
            {
                string[] split = line.Split(',');
                int[] rangeStart = new int[2];
                int[] rangeEnd = new int[2];

                rangeStart[0] = int.Parse(split[0].Split('-')[0]);
                rangeEnd[0] = int.Parse(split[0].Split('-')[1]);

                rangeStart[1] = int.Parse(split[1].Split('-')[0]);
                rangeEnd[1] = int.Parse(split[1].Split('-')[1]);

                if (rangeStart[1] >= rangeStart[0] && rangeEnd[1] <= rangeEnd[0])
                {
                    sum++;
                }
                else if (rangeStart[0] >= rangeStart[1] && rangeEnd[0] <= rangeEnd[1])
                {
                    sum++;
                }
                if (!((rangeEnd[0] < rangeStart[1]) || (rangeEnd[1] < rangeStart[0]))){
                    overlapSum++;
                }
            }

            Console.WriteLine(sum);
            Console.WriteLine(overlapSum);
        }
    }
}
