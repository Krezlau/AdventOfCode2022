using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day19
    {
        private class Blueprint
        {
            public Blueprint(int iD,
                             (int ore, int clay, int obsidian) oreRobotCost,
                             (int ore, int clay, int obsidian) clayRobotCost,
                             (int ore, int clay, int obsidian) obsidianRobotCost,
                             (int ore, int clay, int obsidian) geodeRobotCost)
            {
                ID = iD;
                OreRobotCost = oreRobotCost;
                ClayRobotCost = clayRobotCost;
                ObsidianRobotCost = obsidianRobotCost;
                GeodeRobotCost = geodeRobotCost;
            }
            public int ID { get; }
            public (int ore, int clay, int obsidian) OreRobotCost { get; }

            public (int ore, int clay, int obsidian) ClayRobotCost { get; }

            public (int ore, int clay, int obsidian) ObsidianRobotCost { get; }

            public (int ore, int clay, int obsidian) GeodeRobotCost { get; }

            public int MaxGeodes { get; set; } = 0;
        }

        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day19.txt");

            string[] data = File.ReadAllText(path).Split("\r\n");

            //string[] data = "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.\r\nBlueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.".Split("\r\n");

            var blueprints = ParseData(data);

            foreach (Blueprint blueprint in blueprints)
            {
                Simulate(blueprint, 24, 100000);
            }
            int sum = 0;
            foreach (Blueprint blueprint in blueprints)
            {
                sum += blueprint.ID * blueprint.MaxGeodes;
                Console.WriteLine(blueprint.MaxGeodes);
            }
            Console.WriteLine(sum);

            //PART TWO
            int outcome = 1;
            for (int i = 0; i < blueprints.Count && i < 3; i++)
            {
                Simulate(blueprints[i], 32, 1000000);
                outcome *= blueprints[i].MaxGeodes;
            }

            Console.WriteLine(outcome);
        }

        private static List<Blueprint> ParseData(string[] data)
        {
            List<Blueprint> blueprints = new List<Blueprint>();
            int i = 1;
            foreach (string line in data)
            {
                string[] line_split = line.Split(" ");
                blueprints.Add(new Blueprint(i++,
                                             (int.Parse(line_split[6]), 0, 0),
                                             (int.Parse(line_split[12]), 0, 0),
                                             (int.Parse(line_split[18]), int.Parse(line_split[21]), 0),
                                             (int.Parse(line_split[27]), 0, int.Parse(line_split[30]))));
            }
            return blueprints;
        }

        private static void Simulate(Blueprint blueprint, int minutes, int numberOfTries)
        {
            blueprint.MaxGeodes = 0;
            Random rd = new Random();
            for (int i = 0; i < numberOfTries; i++)
            {
                (int ore, int clay, int obsidian, int geode) robots = (1, 0, 0, 0);
                (int ore, int clay, int obsidian, int geode) resources = (0, 0, 0, 0);

                for (int min = 0; min < minutes; min++)
                {
                    bool continueFlag = false;

                    if (rd.Next(0, 100) < 80
                        && resources.ore >= blueprint.GeodeRobotCost.ore
                        && resources.obsidian >= blueprint.GeodeRobotCost.obsidian
                        && !continueFlag)
                    {
                        robots.geode += 1;
                        resources.geode -= 1;
                        resources.ore -= blueprint.GeodeRobotCost.ore;
                        resources.obsidian -= blueprint.GeodeRobotCost.obsidian;
                        continueFlag = true;
                    }

                    if ((resources.ore >= blueprint.ObsidianRobotCost.ore && resources.clay >= blueprint.ObsidianRobotCost.clay)
                        && !continueFlag)
                    {
                        robots.obsidian += 1;
                        resources.obsidian -= 1;
                        resources.ore -= blueprint.ObsidianRobotCost.ore;
                        resources.clay -= blueprint.ObsidianRobotCost.clay;
                        continueFlag = true;
                    }

                    if (rd.Next(0, 100) < 50
                        && resources.ore >= blueprint.ClayRobotCost.ore
                        && !continueFlag)
                    {
                        robots.clay += 1;
                        resources.clay -= 1;
                        resources.ore -= blueprint.ClayRobotCost.ore;
                        continueFlag = true;
                    }

                    if (rd.Next(0, 100) < 50
                        && resources.ore >= blueprint.OreRobotCost.ore
                        && !continueFlag)
                    {
                        robots.ore += 1;
                        resources.ore -= 1;
                        resources.ore -= blueprint.OreRobotCost.ore;
                    }

                    resources.ore += robots.ore;
                    resources.clay += robots.clay;
                    resources.obsidian += robots.obsidian;
                    resources.geode += robots.geode;

                    //Console.WriteLine($"ore: {resources.ore}, clay: {resources.clay}, obsidian: {resources.obsidian}, geode: {resources.geode}");
                }
                Console.WriteLine($"{blueprint.ID}: {blueprint.MaxGeodes}");
                if (resources.geode > blueprint.MaxGeodes)
                {
                    blueprint.MaxGeodes= resources.geode;
                    Console.WriteLine($"{blueprint.ID}: {resources.geode}");
                }
            }
        }
    }
}
