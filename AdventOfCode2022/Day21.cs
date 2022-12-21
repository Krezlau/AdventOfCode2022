using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day21
    {
        private class Monkey
        {
            public Monkey(string iD)
            {
                ID = iD;
            }

            public string ID { get; }
            public Func<List<Monkey>, long> Operation { get; set; }
            public Func<List<Monkey>, bool> RootOperation { get; set; }
            public (string left, string right) Dependencies { get; set; }
            public char ReverseMathChar { get; set; }

            public List<Monkey> CollectDependencies(List<Monkey> monkeys)
            {
                List<Monkey> dependencies = new List<Monkey>();
                if (Dependencies.left == null) return dependencies;
                var left = monkeys.Where(m => m.ID == Dependencies.left).First();
                var right = monkeys.Where(m => m.ID == Dependencies.right).First();
                dependencies.Add(left);
                dependencies.Add(right);
                dependencies.AddRange(left.CollectDependencies(monkeys));
                dependencies.AddRange(right.CollectDependencies(monkeys));
                return dependencies;
            }
        }

        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day21.txt");

            string[] data = File.ReadAllText(path).Split("\r\n");

            //string[] data = "root: pppw + sjmn\r\ndbpl: 5\r\ncczh: sllz + lgvd\r\nzczc: 2\r\nptdq: humn - dvpt\r\ndvpt: 3\r\nlfqf: 4\r\nhumn: 5\r\nljgn: 2\r\nsjmn: drzm * dbpl\r\nsllz: 4\r\npppw: cczh / lfqf\r\nlgvd: ljgn * ptdq\r\ndrzm: hmdt - zczc\r\nhmdt: 32".Split("\r\n");
            
            var monkeys = ParseData(data, true);

            Console.WriteLine(monkeys.Where(m => m.ID == "root").First().Operation(monkeys));

            //PART TWO

            monkeys = ParseData(data, false);
            var human = monkeys.Where(m => m.ID == "humn").First();
            var root = monkeys.Where(m => m.ID == "root").First();
            var depL = monkeys.Where(m => m.ID == root.Dependencies.left).First().CollectDependencies(monkeys);
            var depR = monkeys.Where(m => m.ID == root.Dependencies.right).First().CollectDependencies(monkeys);
            long numberToMatch = 0;
            List<(Func<List<Monkey>, long>, char c)> operations = new List<(Func<List<Monkey>, long>, char c)>();
            if (depL.Contains(human))
            {
                var current = monkeys.Where(m => m.ID == root.Dependencies.left).First();
                numberToMatch = monkeys.Where(m => m.ID == root.Dependencies.right).First().Operation(monkeys);
                while (true)
                {
                    if (current.Dependencies.left == "humn" || current.Dependencies.right == "humn") break;
                    depL = monkeys.Where(m => m.ID == current.Dependencies.left).First().CollectDependencies(monkeys);
                    depR = monkeys.Where(m => m.ID == current.Dependencies.right).First().CollectDependencies(monkeys);
                    if (depL.Contains(human))
                    {
                        switch (current.ReverseMathChar)
                        {
                            case '+':
                                numberToMatch += monkeys.Where(m => m.ID == current.Dependencies.right).First().Operation(monkeys);
                                break;
                            case '-':
                                numberToMatch -= monkeys.Where(m => m.ID == current.Dependencies.right).First().Operation(monkeys);
                                break;
                            case '*':
                                numberToMatch *= monkeys.Where(m => m.ID == current.Dependencies.right).First().Operation(monkeys);
                                break;
                            case '/':
                                numberToMatch /= monkeys.Where(m => m.ID == current.Dependencies.right).First().Operation(monkeys);
                                break;
                            default:
                                break;
                        }
                        operations.Add((current.Operation, current.ReverseMathChar));
                        current = monkeys.Where(m => m.ID == current.Dependencies.left).First();
                        continue;
                    }
                    if (depR.Contains(human))
                    {
                        switch (current.ReverseMathChar)
                        {
                            case '+':
                                numberToMatch = monkeys.Where(m => m.ID == current.Dependencies.left).First().Operation(monkeys) - numberToMatch;
                                break;
                            case '-':
                                numberToMatch -= monkeys.Where(m => m.ID == current.Dependencies.left).First().Operation(monkeys);
                                break;
                            case '*':
                                numberToMatch = monkeys.Where(m => m.ID == current.Dependencies.left).First().Operation(monkeys) / numberToMatch;
                                break;
                            case '/':
                                numberToMatch /= monkeys.Where(m => m.ID == current.Dependencies.left).First().Operation(monkeys);
                                break;
                            default:
                                break;
                        }
                        operations.Add((current.Operation, current.ReverseMathChar));
                        current = monkeys.Where(m => m.ID == current.Dependencies.right).First();
                        continue;
                    }
                }
                if (current.Dependencies.left == "humn")
                {
                    switch (current.ReverseMathChar)
                    {
                        case '+':
                            numberToMatch += monkeys.Where(m => m.ID == current.Dependencies.right).First().Operation(monkeys);
                            break;
                        case '-':
                            numberToMatch -= monkeys.Where(m => m.ID == current.Dependencies.right).First().Operation(monkeys);
                            break;
                        case '*':
                            numberToMatch *= monkeys.Where(m => m.ID == current.Dependencies.right).First().Operation(monkeys);
                            break;
                        case '/':
                            numberToMatch /= monkeys.Where(m => m.ID == current.Dependencies.right).First().Operation(monkeys);
                            break;
                        default:
                            break;
                    }
                } else
                {
                    switch (current.ReverseMathChar)
                    {
                        case '+':
                            numberToMatch = monkeys.Where(m => m.ID == current.Dependencies.left).First().Operation(monkeys) - numberToMatch;
                            break;
                        case '-':
                            numberToMatch -= monkeys.Where(m => m.ID == current.Dependencies.left).First().Operation(monkeys);
                            break;
                        case '*':
                            numberToMatch = monkeys.Where(m => m.ID == current.Dependencies.left).First().Operation(monkeys) / numberToMatch;
                            break;
                        case '/':
                            numberToMatch /= monkeys.Where(m => m.ID == current.Dependencies.left).First().Operation(monkeys);
                            break;
                        default:
                            break;
                    }
                }
            }

            Console.WriteLine(numberToMatch);
        }

        private static List<Monkey> ParseData(string[] data, bool firstPart)
        {
            List<Monkey> monkeys = new List<Monkey>();
            foreach (string line in data)
            {
                monkeys.Add(new Monkey(line.Split(":")[0]));
                string[] operation = line.Split(" ");

                if (!firstPart && line.Split(":")[0] == "root")
                {
                    monkeys[^1].RootOperation = (monkeys) => monkeys.Where(m => m.ID == operation[1]).First().Operation(monkeys) == monkeys.Where(m => m.ID == operation[3]).First().Operation(monkeys);
                    monkeys[^1].Dependencies = (operation[1], operation[3]);
                    continue;
                }
                if (!firstPart && line.Split(":")[0] == "root")
                {
                    monkeys[^1].Operation = (monkeys) => 0;
                    continue;
                }

                if (operation.Length == 2)
                {
                    monkeys[^1].Operation = (monkeys) => int.Parse(operation[1]);
                    continue;
                }
                if (operation[2] == "+")
                {
                    monkeys[^1].Operation = (monkeys) => monkeys.Where(m => m.ID == operation[1]).First().Operation(monkeys) + monkeys.Where(m => m.ID == operation[3]).First().Operation(monkeys);
                    monkeys[^1].Dependencies = (operation[1], operation[3]);
                    monkeys[^1].ReverseMathChar = '-';
                }
                if (operation[2] == "-")
                {
                    monkeys[^1].Operation = (monkeys) => monkeys.Where(m => m.ID == operation[1]).First().Operation(monkeys) - monkeys.Where(m => m.ID == operation[3]).First().Operation(monkeys);
                    monkeys[^1].Dependencies = (operation[1], operation[3]);
                    monkeys[^1].ReverseMathChar = '+';
                }
                if (operation[2] == "*")
                {
                    monkeys[^1].Operation = (monkeys) => monkeys.Where(m => m.ID == operation[1]).First().Operation(monkeys) * monkeys.Where(m => m.ID == operation[3]).First().Operation(monkeys);
                    monkeys[^1].Dependencies = (operation[1], operation[3]);
                    monkeys[^1].ReverseMathChar = '/';
                }
                if (operation[2] == "/")
                {
                    monkeys[^1].Operation = (monkeys) => monkeys.Where(m => m.ID == operation[1]).First().Operation(monkeys) / monkeys.Where(m => m.ID == operation[3]).First().Operation(monkeys);
                    monkeys[^1].Dependencies = (operation[1], operation[3]);
                    monkeys[^1].ReverseMathChar = '*';
                }
            }
            return monkeys;
        }
    }
}
