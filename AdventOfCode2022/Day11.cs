using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day11
    {
        private class Monkey
        {
            public Monkey(Func<long, long> operation, long testValue, int ifTrue, int ifFalse)
            {
                Operation = operation;
                TestValue = testValue;
                this.ifTrue = ifTrue;
                this.ifFalse = ifFalse;
            }

            public List<long> Items { get; set; } = new List<long>();
            public Func<long, long> Operation { get; set; }
            public long TestValue { get; set; }
            public int ifTrue { get; set; }
            public int ifFalse { get; set; }
            public long InspectionCount { get; set; }
            public bool ifAddition { get; set; } = false;
        }

        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day11.txt");

            string[] data = File.ReadAllText(path).Split("\r\n");

            //string[] data = "Monkey 0:\r\n  Starting items: 79, 98\r\n  Operation: new = old * 19\r\n  Test: divisible by 23\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 3\r\n\r\nMonkey 1:\r\n  Starting items: 54, 65, 75, 74\r\n  Operation: new = old + 6\r\n  Test: divisible by 19\r\n    If true: throw to monkey 2\r\n    If false: throw to monkey 0\r\n\r\nMonkey 2:\r\n  Starting items: 79, 60, 97\r\n  Operation: new = old * old\r\n  Test: divisible by 13\r\n    If true: throw to monkey 1\r\n    If false: throw to monkey 3\r\n\r\nMonkey 3:\r\n  Starting items: 74\r\n  Operation: new = old + 3\r\n  Test: divisible by 17\r\n    If true: throw to monkey 0\r\n    If false: throw to monkey 1".Split("\r\n");


            List<Monkey> monkeys = new List<Monkey>();
            List<Monkey> monkeys2 = new List<Monkey>();

            for (int i = 0; i < data.Length; i += 7)
            {
                int testval = int.Parse(data[i + 3].Split(" ")[5]);
                int iftrue = int.Parse(data[i + 4].Split(" ")[9]);
                int iffalse = int.Parse(data[i + 5].Split(" ")[9]);

                List<long> items = new List<long>();
                List<long> items2 = new List<long>();
                string[] itemsLine = data[i + 1].Replace(",", "").Split(" ");
                for (int j = 4; j < itemsLine.Length; j++)
                {
                    items.Add(long.Parse(itemsLine[j]));
                    items2.Add(long.Parse(itemsLine[j]));
                }

                string[] operation = data[i + 2].Split(" ");
                if (operation[6] == "*")
                {
                    if (operation[7] == "old")
                    {
                        monkeys.Add(new Monkey((old) => old * old, testval, iftrue, iffalse));
                        monkeys2.Add(new Monkey((old) => old * old, testval, iftrue, iffalse));
                    }
                    else
                    {
                        monkeys.Add(new Monkey((old) => old * long.Parse(operation[7]), testval, iftrue, iffalse));
                        monkeys2.Add(new Monkey((old) => old * long.Parse(operation[7]), testval, iftrue, iffalse));
                    }
                }
                if (operation[6] == "+")
                {
                    if (operation[7] == "old")
                    {
                        monkeys.Add(new Monkey((old) => old + old, testval, iftrue, iffalse));
                        monkeys2.Add(new Monkey((old) => old + old, testval, iftrue, iffalse));
                        monkeys2[^1].ifAddition = true;
                    }
                    else
                    {
                        monkeys.Add(new Monkey((old) => old + long.Parse(operation[7]), testval, iftrue, iffalse));
                        monkeys2.Add(new Monkey((old) => old + long.Parse(operation[7]), testval, iftrue, iffalse));
                        monkeys2[^1].ifAddition = true;
                    }
                }
                monkeys[^1].Items = items;
                monkeys2[^1].Items = items2;
            }


            for (int i = 0; i < 20; i++)
            {
                foreach (Monkey monkey in monkeys)
                {
                    while (monkey.Items.Count != 0)
                    {
                        long item = monkey.Items[0];
                        long newVal = monkey.Operation(item) / 3;
                        if (newVal % monkey.TestValue == 0)
                        {
                            monkey.Items.Remove(item);
                            monkeys[monkey.ifTrue].Items.Add(newVal);
                        }
                        else
                        {
                            monkey.Items.Remove(item);
                            monkeys[monkey.ifFalse].Items.Add(newVal);
                        }
                        monkey.InspectionCount++;
                    }
                }
            }

            List<long> inspections = new List<long>();
            foreach (Monkey monkey in monkeys)
            {
                inspections.Add(monkey.InspectionCount);
            }
            long divider = 1;
            foreach (Monkey monkey in monkeys2)
            {
                divider *= monkey.TestValue;
            }
            inspections.Sort();
            Console.WriteLine(inspections[^1] * inspections[^2]);

            for (int i = 0; i < 10000; i++)
            {
                foreach (Monkey monkey in monkeys2)
                {
                    while (monkey.Items.Count != 0)
                    {
                        long item = monkey.Items[0];
                        long newVal = monkey.Operation(item);
                        if (!monkey.ifAddition)
                        {
                            newVal = newVal % divider;
                        }
                        
                        if (newVal % monkey.TestValue == 0)
                        {
                            monkey.Items.Remove(item);
                            monkeys2[monkey.ifTrue].Items.Add(newVal);
                        }
                        else
                        {
                            monkey.Items.Remove(item);
                            monkeys2[monkey.ifFalse].Items.Add(newVal);
                        }
                        monkey.InspectionCount++;
                    }
                }
            }

            inspections = new List<long>();
            foreach (Monkey monkey in monkeys2)
            {
                inspections.Add(monkey.InspectionCount);
            }

            inspections.Sort();
            Console.WriteLine(inspections[^1] * inspections[^2]);
        }
    }
}
