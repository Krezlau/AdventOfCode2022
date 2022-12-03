using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day3
    {
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day3.txt");

            string data = File.ReadAllText(path);

            //string data = "vJrwpWtwJgWrhcsFMMfFFhFp\r\njqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL\r\nPmmdzqPrVvPwwTWBwg\r\nwMqvLMZHhHMvwLHjbvcjnnSBnvTQFn\r\nttgJtRGJQctTZtZT\r\nCrZsJsPPZsGzwwsLwLmpwMDw";

            string[] data_split = data.Split('\n');

            int sum = 0;
            int sumOfBadges = 0;
            for (int i = 0; i < data_split.Length; i++)
            {
                string trimmed = data_split[i].Trim();
                char c = FindMisplacedItem(trimmed);
                sum += DeterminePriority(c);
                if (i % 3 == 2)
                {
                    char badge = FindTheBadge(trimmed, data_split[i - 1].Trim(), data_split[i - 2].Trim());
                    sumOfBadges += DeterminePriority(badge);
                }
            }
            Console.WriteLine(sum);
            Console.WriteLine(sumOfBadges);
        }

        private static char FindMisplacedItem(string input)
        {
            string compartmentOne = input.Substring(0, input.Length / 2);
            string compartmentTwo = input.Substring(input.Length / 2);

            foreach (char c in compartmentOne)
            {
                if (compartmentTwo.Contains(c))
                {
                    return c;
                }
            }
            return 'x';
        }

        private static char FindTheBadge(string stringOne, string stringTwo, string stringThree)
        {
            foreach (char c in stringOne)
            {
                if (stringTwo.Contains(c) && stringThree.Contains(c))
                {
                    return c;
                }
            }
            return 'x';
        }

        private static int DeterminePriority(char c)
        {
            int asciiCode = (int)c;
            if (asciiCode < 97)
            {
                return asciiCode - 38;
            }
            if (asciiCode >= 97)
            {
                return asciiCode - 96;
            }
            return 0;
        }
    }
}
