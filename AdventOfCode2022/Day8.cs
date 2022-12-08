using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day8
    {
        public static void Solve()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "data\\day8.txt");

            string data = File.ReadAllText(path);

            //string data = "30373\r\n25512\r\n65332\r\n33549\r\n35390";

            string[] data_split = data.Split("\r\n");

            int sum = data_split.Length * 2 + data_split[0].Length * 2 - 4;

            for (int i = 1; i < data_split.Length - 1; i++)
            {
                for (int j = 1; j < data_split[i].Length - 1; j++)
                {
                    if (data_split[i][j] == '0') continue;
                    if (data_split[i].Substring(0, j).Where(c => c >= data_split[i][j]).Count() == 0 )
                    {
                        sum++;
                        continue;
                    }
                    if (data_split[i].Substring(j + 1, data_split[i].Length - j - 1).Where(c => c >= data_split[i][j]).Count() == 0)
                    {
                        sum++;
                        continue;
                    }
                    string dataTemp = "";
                    foreach (string line in data_split)
                    { 
                        dataTemp += line[j];
                    }
                    if (dataTemp.Substring(0, i).Where(c => c >= data_split[i][j]).Count() == 0)
                    {
                        sum++;
                        continue;
                    }
                    if (dataTemp.Substring(i + 1, dataTemp.Length - i - 1).Where(c => c >= data_split[i][j]).Count() == 0)
                    {
                        sum++;
                        continue;
                    }
                }
            }

            Console.WriteLine(sum);

            int maxScenicScore = 0;
            for (int i = 1; i < data_split.Length - 1; i++)
            {
                for (int j = 1; j < data_split[i].Length - 1; j++)
                {
                    int scenicScore = 1;
                    int k = j - 1;
                    int viewSum = 1;
                    while (k >= 0 && data_split[i][k] < data_split[i][j])  
                    {
                        k--;
                        viewSum++;
                        if (k == -1 && data_split[i][k+1] < data_split[i][j]) viewSum--;
                    }
                    scenicScore *= viewSum;
                    viewSum = 1;
                    k = j + 1;
                    while (k < data_split[i].Length && data_split[i][k] < data_split[i][j])
                    {
                        k++;
                        viewSum++;
                        if (k == data_split[i].Length && data_split[i][k-1] < data_split[i][j]) viewSum--;
                    }
                    scenicScore *= viewSum;
                    string dataTemp = "";
                    foreach (string line in data_split)
                    {
                        dataTemp += line[j];
                    }
                    viewSum = 1;
                    k = i - 1;
                    while (k >= 0 && dataTemp[k] < data_split[i][j])
                    {
                        k--;
                        viewSum++;
                        if (k == -1 && dataTemp[k+1] < data_split[i][j]) viewSum--;
                    }
                    scenicScore *= viewSum;
                    viewSum = 1;
                    k = i + 1;
                    while (k < dataTemp.Length  && dataTemp[k] < data_split[i][j])
                    {
                        k++;
                        viewSum++;
                        if (k == dataTemp.Length && dataTemp[k-1] < data_split[i][j]) viewSum--;
                    }
                    scenicScore *= viewSum;
                    if (scenicScore > maxScenicScore)
                    {
                        maxScenicScore = scenicScore;
                    }
                }
            }
            Console.WriteLine(maxScenicScore);
        }
    }
}
