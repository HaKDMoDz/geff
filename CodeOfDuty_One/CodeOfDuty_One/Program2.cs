// Code Of Duty 1
// Concours Criteo 11/06/2011
// Auteur: Julien PATTE - julien.patte AT gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace CodeOfDuty
{
    class CodeOfDuty
    {
        static void Main(string[] args)
        {
            string inputPath = (args.Length > 1 ? args[0] : @".\input.txt");
            string outputPath = (args.Length > 2 ? args[1] : @".\output.txt");

            using (StreamReader input = new StreamReader(inputPath))
            {
                using (StreamWriter output = new StreamWriter(outputPath))
                {
                    for (int[] data = ReadData(input); data.Length > 0; data = ReadData(input))
                    {
                        List<int[]> iterations = ProcessData(data);
                        if (iterations == null)
                        {
                            output.WriteLine(-1);
                        }
                        else
                        {
                            output.WriteLine(iterations.Count - 1);
                            int iter = 0;
                            foreach (int[] d in iterations)
                                output.WriteLine("{0} : ({1})", iter++, string.Join(", ", d));
                        }

                        output.WriteLine();
                        input.ReadLine();
                    }
                }
            }
        }

        static int[] ReadData(TextReader input)
        {
            int length = Convert.ToInt32(input.ReadLine());
            if (length == 0)
                return new int[0];
            else
                return input.ReadLine().Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(n => Convert.ToInt32(n)).ToArray();
        }

        static List<int[]> ProcessData(int[] data)
        {
            double dmean = data.Sum() / (double)data.Length;
            int mean = (int)dmean;
            if (mean != dmean)
                return null; // It's a trap!

            List<int[]> iterations = new List<int[]>();

            // Where is the money ?
            int[] excess = new int[data.Length];
            excess[0] = 0;

            int targetSum = 0;
            int currentSum = data[0];
            for (int i = 1; i < data.Length; i++)
            {
                targetSum += mean;
                excess[i] = currentSum - targetSum;
                currentSum += data[i];
            }

            // Let's share some coins
            int? firstChanged = 0;
            int? lastChanged = data.Length - 1;
            do
            {
                iterations.Add(data.ToArray());

                // Those who didn't give shall never give anything again
                int first = firstChanged.Value;
                int last = lastChanged.Value;
                firstChanged = null;
                lastChanged = null;

                for (int i = first; i <= last; i++)
                {
                    int excessLeft = excess[i];
                    int excessRight = mean - excessLeft - data[i];

                    if (excessLeft < 0 || excessRight < 0)
                    {
                        data[i]--; // Steal from the Rich
                        firstChanged = firstChanged ?? i;
                        lastChanged = i;
                        if (excessLeft < 0)
                        {
                            data[i - 1]++; // Give to the lefty Poor 
                            excess[i]++;
                        }
                        else
                        {
                            data[i + 1]++; // Give to the righty Poor
                            excess[i + 1]--;
                        }
                    }
                }

            }
            while (firstChanged.HasValue); // Stop when communism has been achieved

            return iterations;
        }
    }
}