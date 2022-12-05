using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day5
    {
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day5.txt");

            string data = File.ReadAllText(path);

            //string data = "2-4,6-8\r\n2-3,4-5\r\n5-7,7-9\r\n2-8,3-7\r\n6-6,4-6\r\n2-6,4-8";

            string[] data_split = data.Split('\n');

            List<Stack<char>> stacks = new List<Stack<char>>(9);
            List<Stack<char>> stacksTwo = new List<Stack<char>>(9);

            for (int i = 0; i < 9; i++)
            {
                stacks.Add(new Stack<char>());
                stacksTwo.Add(new Stack<char>());
            }

            for (int i = 7; i >= 0; i--)
            {
                int stackNumber = 0;
                for (int j = 1; j < data_split[i].Length && stackNumber < 9; j += 4)
                {
                    if (data_split[i][j] != ' ')
                    {
                        stacks[stackNumber].Push(data_split[i][j]);
                        stacksTwo[stackNumber].Push(data_split[i][j]);
                    }
                    stackNumber++;
                }
            }

            for (int i = 10; i < data_split.Length; i++)
            {
                string[] line = data_split[i].Trim().Split(" ");

                List<char> elements = new List<char>();
                for (int j = 0; j < int.Parse(line[1]); j++)
                {
                    char element = stacks[int.Parse(line[3]) - 1].Pop();
                    stacks[int.Parse(line[5]) - 1].Push(element);

                    elements.Add(stacksTwo[int.Parse(line[3]) - 1].Pop());
                }

                for (int j = int.Parse(line[1]) - 1; j >= 0; j--)
                {
                    stacksTwo[int.Parse(line[5]) - 1].Push(elements[j]);
                }
            }

            string output = "";
            string outputTwo = "";
            foreach (Stack<char> stack in stacks)
            {
                output += stack.Pop();
            }
            foreach(Stack<char> stack in stacksTwo)
            {
                outputTwo += stack.Pop();
            }

            Console.WriteLine(output);
            Console.WriteLine(outputTwo);
        }
    }
}
