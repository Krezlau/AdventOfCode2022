using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day7
    {
        private class Directory
        {
            public Directory(string name) 
            {
                Name = name;
            }
            public string Name { get; set; }
            public List<Directory> directories { get; set; } = new List<Directory>();
            public int directSize { get; set; }
                //not including subfolders 

            public int totalSize { get; set; }

            public Directory? parentDir { get; set; }

            public int countSum()
            {
                int sum = directSize;
                foreach (Directory d in directories)
                {
                    sum += d.countSum();
                }
                return sum;
            }
        }

        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day7.txt");

            string data = File.ReadAllText(path);

            //string data = "$ cd /\r\n$ ls\r\ndir a\r\n14848514 b.txt\r\n8504156 c.dat\r\ndir d\r\n$ cd a\r\n$ ls\r\ndir e\r\n29116 f\r\n2557 g\r\n62596 h.lst\r\n$ cd e\r\n$ ls\r\n584 i\r\n$ cd ..\r\n$ cd ..\r\n$ cd d\r\n$ ls\r\n4060174 j\r\n8033020 d.log\r\n5626152 d.ext\r\n7214296 k";

            string[] data_split = data.Split("\n");

            List<Directory> directories= new List<Directory>();

            Directory root = new Directory("/");
            directories.Add(root);
            Directory current = root;
            for (int i = 1; i < data_split.Length; i++)
            {
                if (data_split[i].Trim() == "$ ls")
                {
                    while (i < data_split.Length - 1 && data_split[i + 1][0] != '$')
                    {
                        i++;
                        if (data_split[i][0] == 'd')
                        {
                            Directory newDir = new Directory(data_split[i].Trim().Split(" ")[1]);
                            newDir.parentDir = current;
                            directories.Add(newDir);
                            current.directories.Add(newDir);
                        }
                        if (data_split[i][0] != 'd')
                        {
                            current.directSize += int.Parse(data_split[i].Split(" ")[0]);
                        }
                    }
                }
                if (data_split[i].Split(" ")[1] == "cd")
                {
                    if (data_split[i].Trim().Split(" ")[2] == "..")
                    {
                        current = current.parentDir;
                        continue;
                    }
                    current = current.directories.Where(d => d.Name == data_split[i].Trim().Split(" ")[2]).First();
                }
            }


            int sum = 0;
            foreach (Directory dir in directories)
            {
                int size = dir.countSum();
                if (size <= 100_000)
                {
                    sum += size;
                    //Console.WriteLine($"{dir.Name}: {size}");
                }
                dir.totalSize = size;
            }
            Console.WriteLine(sum);

            int neededSpace = 30000000 - (70000000 - root.totalSize);

            Directory itemToDelete = directories.OrderBy(d => d.totalSize).Where(d => d.totalSize >= neededSpace).First();

            Console.WriteLine(itemToDelete.totalSize);
        }
    }
}
