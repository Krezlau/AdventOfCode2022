using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day6
    {
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day6.txt");

            string data = File.ReadAllText(path);

            //string data = "2-4,6-8\r\n2-3,4-5\r\n5-7,7-9\r\n2-8,3-7\r\n6-6,4-6\r\n2-6,4-8";


            for (int i = 0; i + 4< data.Length; i++)
            {
                List<char> chars= new List<char>();

                chars.Add(data[i]);
                for (int j = 1; j < 4; j++)
                {
                    if (chars.Contains(data[i + j]))
                    {
                        break;
                    }
                    chars.Add(data[i+j]);
                }
                if (chars.Count == 4)
                {
                    Console.WriteLine(i + 4);
                    break;
                }
            }

            for (int i = 0; i + 14 < data.Length; i++)
            {
                List<char> chars = new List<char>();

                chars.Add(data[i]);
                for (int j = 1; j < 14; j++)
                {
                    if (chars.Contains(data[i + j]))
                    {
                        break;
                    }
                    chars.Add(data[i + j]);
                }
                if (chars.Count == 14)
                {
                    Console.WriteLine(i + 14);
                    break;
                }
            }
        }
    }
}
