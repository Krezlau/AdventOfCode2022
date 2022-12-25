using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day25
    {
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day25.txt");

            string[] data = File.ReadAllText(path).Split("\r\n");

            //string[] data = "1=-0-2\r\n12111\r\n2=0=\r\n21\r\n2=01\r\n111\r\n20012\r\n112\r\n1=-1=\r\n1-12\r\n12\r\n1=\r\n122".Split("\r\n");

            long sum = 0;
            foreach (string line in data)
            {
                sum += ConvertToInt(line);
            }
            Console.WriteLine(sum);
            Console.WriteLine(ConvertToSnafu(sum));

            Console.WriteLine(ConvertToSnafu(1747));
            Console.WriteLine(ConvertToSnafu(906));

        }

        private static long ConvertToInt(string number)
        {
            long sum = 0;
            long power = (long)Math.Pow(5, number.Length);
            for (int i = 0; i < number.Length; i++)
            {
                power /= 5;
                char c = number[i];
                if (c == '-')
                {
                    sum -= power;
                    continue;
                }
                if (c == '=')
                {
                    sum -= power * 2;
                    continue;
                }
                long cInt = int.Parse(c.ToString());
                sum += cInt * power;
            }
            return sum;
        }

        private static string ConvertToSnafu(long number)
        {
            int power = 0;
            while (BigInteger.Pow(5, power) < number)
            {
                power++;
            }

            long remaining = number;
            string inFive = "";
            int currentDigit = 0;
            while (remaining > 0)
            {

                while (remaining - BigInteger.Pow(5, power) >= 0 && currentDigit < 4)
                {
                    remaining -= (long)BigInteger.Pow(5, power);
                    currentDigit++;
                }
                inFive += currentDigit;
                currentDigit = 0;
                power--;
                if (remaining == 0 && power != -1)
                {
                    inFive += '0';
                    break;
                }
            }
            if (inFive[0] == '0') inFive = inFive.Remove(0, 1);
            Console.WriteLine("infive = " + inFive);
            int startingNumber = inFive.Length - 1;

            while (inFive.Contains('3') || inFive.Contains('4'))
            {
                if (inFive[0] == '3') inFive = inFive.Remove(0, 1).Insert(0, "1=");
                if (inFive[0] == '4') inFive = inFive.Remove(0, 1).Insert(0, "1-");
                for (int i = 1; i < inFive.Length; i++)
                {
                    if (inFive[i] == '3')
                    {
                        inFive = inFive.Remove(i, 1).Insert(i, "=");
                        switch (inFive[i - 1])
                        {
                            case '=':
                                inFive = inFive.Remove(i - 1, 1).Insert(i - 1, "-");
                                break;
                            case '-':
                                inFive = inFive.Remove(i - 1, 1).Insert(i - 1, "0");
                                break;
                            case '0':
                                inFive = inFive.Remove(i - 1, 1).Insert(i - 1, "1");
                                break;
                            case '1':
                                inFive = inFive.Remove(i - 1, 1).Insert(i - 1, "2");
                                break;
                            case '2':
                                inFive = inFive.Remove(i - 1, 1).Insert(i - 1, "3");
                                break;
                            default:
                                break;
                        }
                        break;
                    }
                    if (inFive[i] == '4')
                    {
                        inFive = inFive.Remove(i, 1).Insert(i, "-");
                        switch (inFive[i - 1])
                        {
                            case '=':
                                inFive = inFive.Remove(i - 1, 1).Insert(i - 1, "-");
                                break;
                            case '-':
                                inFive = inFive.Remove(i - 1, 1).Insert(i - 1, "0");
                                break;
                            case '0':
                                inFive = inFive.Remove(i - 1, 1).Insert(i - 1, "1");
                                break;
                            case '1':
                                inFive = inFive.Remove(i - 1, 1).Insert(i - 1, "2");
                                break;
                            case '2':
                                inFive = inFive.Remove(i - 1, 1).Insert(i - 1, "3");
                                break;
                            default:
                                break;
                        }
                        break;
                    }
                }

                //for (int i = startingNumber; i > 0; i--)
                //{
                //    char c = inFive[i];
                //    if (c == '=' || c == '-')
                //    {
                //        switch (inFive[i - 1])
                //        {
                //            case '=':
                //                inFive = inFive.Remove(i - 1, 1).Insert(i - 1, "-");
                //                break;
                //            case '-':
                //                inFive = inFive.Remove(i - 1, 1).Insert(i - 1, "0");
                //                break;
                //            case '0':
                //                inFive = inFive.Remove(i - 1, 1).Insert(i - 1, "1");
                //                break;
                //            case '1':
                //                inFive = inFive.Remove(i - 1, 1).Insert(i - 1, "2");
                //                break;
                //            case '2':
                //                inFive = inFive.Remove(i - 1, 1).Insert(i - 1, "3");
                //                break;
                //            default:
                //                break;
                //        }
                //    }
                //    if (c == '3')
                //    {
                //        inFive = inFive.Remove(i, 1).Insert(i, "=");
                //        i++;
                //        continue;
                //    }
                //    if (c == '4')
                //    {
                //        inFive = inFive.Remove(i, 1).Insert(i, "-");
                //        i++;
                //        continue;
                //    }
                //}
                //startingNumber = 0;
                if (inFive[0] == '=' || inFive[0] == '-') inFive.Insert(0, "1");
                if (inFive[0] == '3') inFive = inFive.Remove(0, 1).Insert(0, "1=");
                if (inFive[0] == '4') inFive = inFive.Remove(0, 1).Insert(0, "1-");
            }
            return inFive;
        }
    }
}
