using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day20
    {
        private class Code
        {
            public Code(long value) 
            {
                Value= value;
            }
            public long Value { get; set; }
        }
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day20.txt");

            string[] data = File.ReadAllText(path).Split("\r\n");

            //string[] data = "1\r\n2\r\n-3\r\n3\r\n-2\r\n0\r\n4".Split("\r\n");

            var numbers = ParseData(data);

            var decoded = Decrypt(numbers, 1);

            BigInteger sum = 0;
            int index = decoded.IndexOf(decoded.Where(c => c.Value == 0).First());
            for (int i = 1; i <= 3000; i++)
            {
                index = (index + 1) % decoded.Count;
                if (i == 1000 || i == 2000 || i == 3000)
                {
                    sum += decoded[index].Value;
                }
            }

            Console.WriteLine(sum);

            // PART TWO
            long key = 811589153;

            for (int i = 0; i < numbers.Count; i++)
            {
                numbers[i].Value = numbers[i].Value * key;
            }

            decoded = Decrypt(numbers, 10);

            sum = 0;
            index = decoded.IndexOf(decoded.Where(c => c.Value == 0).First());
            for (int i = 1; i <= 3000; i++)
            {
                index = (index + 1) % decoded.Count;
                if (i == 1000 || i == 2000 || i == 3000)
                {
                    sum += decoded[index].Value;
                }
            }
            Console.WriteLine(sum);
        }

        private static List<Code> ParseData(string[] data)
        {
            List<Code> numbers = new List<Code>();
            foreach (string line in data)
            {
                numbers.Add(new Code(int.Parse(line)));
            }
            return numbers;
        }

        private static List<Code> Decrypt(List<Code> numbers, int times)
        {
            List<Code> ret = numbers.ToList();
            for (int count = 0; count < times; count++)
            {
                for (int i = 0; i < numbers.Count; i++)
                {
                    MoveNumber(numbers[i], ret);
                }
            }

            return ret;
        }

        private static void MoveNumber(Code moveVal, List<Code> numbers)
        {
            int retIndex = numbers.IndexOf(moveVal);
            if (moveVal.Value == 0) return;

            numbers.RemoveAt(retIndex);

            long index = retIndex + moveVal.Value;

            index = (Math.Abs(index * numbers.Count) + index) % numbers.Count;

            numbers.Insert((int)index, moveVal);
        }
    }
}
