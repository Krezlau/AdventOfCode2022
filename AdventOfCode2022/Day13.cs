using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day13
    {
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day13.txt");

            string[] data = File.ReadAllText(path).Split("\r\n");

            //string[] data = "[1,1,3,1,1]\r\n[1,1,5,1,1]\r\n\r\n[[1],[2,3,4]]\r\n[[1],4]\r\n\r\n[9]\r\n[[8,7,6]]\r\n\r\n[[4,4],4,4]\r\n[[4,4],4,4,4]\r\n\r\n[7,7,7,7]\r\n[7,7,7]\r\n\r\n[]\r\n[3]\r\n\r\n[[[]]]\r\n[[]]\r\n\r\n[1,[2,[3,[4,[5,6,7]]]],8,9]\r\n[1,[2,[3,[4,[5,6,0]]]],8,9]".Split("\r\n");

            int sum = 0;
            int pair = 1;
            for (int i = 0; i < data.Length; i += 3)
            {
                if (Compare(data[i], data[i + 1]))
                {
                    sum += pair;
                }
                pair++;
            }

            Console.WriteLine(sum);

            //PART TWO
            List<string> packets = new List<string>();
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] != "")
                {
                    packets.Add(data[i]);
                }
            }
            packets.Add("[[2]]");
            packets.Add("[[6]]");

            packets.Sort((a, b) => { if (Compare(a, b)) return -1; return 1; });

            Console.WriteLine((packets.IndexOf("[[2]]") + 1) * (packets.IndexOf("[[6]]") + 1));
        }

        private static bool Compare(string left, string right) 
        {
            int leftI = 0;
            int rightI = 0;
            while (leftI < left.Length)
            {
                leftI++;
                rightI++;
                if (rightI == right.Length) return false;
                if (left[leftI] == right[rightI] && !Char.IsDigit(left[leftI]) && !Char.IsDigit(right[rightI])) continue;
                if (left[leftI] != right[rightI] && left[leftI] == '[')
                {
                    right = right.Insert(rightI, "[");
                    int i = rightI + 1;
                    while (i < right.Length && (Char.IsDigit(right[i]) || right[i] == ','))
                    {
                        i++;
                    }
                    right = right.Insert(i, "]");
                    continue;
                }
                if (left[leftI] != right[rightI] && right[rightI] == '[')
                {
                    left = left.Insert(leftI, "[");
                    int i = leftI + 1;
                    while (i < left.Length && (Char.IsDigit(left[i]) || left[i] == ','))
                    {
                        i++;
                    }
                    left = left.Insert(i, "]");
                    continue;
                } 
                if (left[leftI] == ']') return true;
                if (right[rightI] == ']') return false;

                string rightVal = right.Substring(rightI, 1);
                string leftVal = left.Substring(leftI, 1);

                if (Char.IsDigit(left[leftI]) && !Char.IsDigit(right[rightI])) return true;
                if (!Char.IsDigit(left[leftI]) && Char.IsDigit(right[rightI])) return false;

                if (Char.IsDigit(left[leftI + 1]))
                {
                    leftVal = left.Substring(leftI, 2);
                    leftI++;
                }
                if (Char.IsDigit(right[rightI + 1]))
                {
                    rightVal = right.Substring(rightI, 2);
                    rightI++;
                }
                if (int.Parse(leftVal) < int.Parse(rightVal)) return true;
                if (int.Parse(leftVal) > int.Parse(rightVal)) return false;
            }
            return false;
        }
    }
}
