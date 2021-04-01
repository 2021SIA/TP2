using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;
using System.Linq;

namespace TP2.Selections
{
    public class UniversalSelection : ISelection
    {
        private static Random rnd = new Random();

        private IEnumerable<double> TargetProb(int k)
        {
            for(int j = 0; j < k; j++)
            {
                double r = rnd.NextDouble();
                yield return (r + j) / k;
            }
        }


        public IEnumerable<Character> Select(IEnumerable<Character> population, int n, int selectionSize)
        {
            double fitSum = population.Sum(c => c.Fitness);

            var targets = TargetProb(selectionSize).ToArray();
            Array.Sort(targets);

            double accum = 0;
            int j = 0;
            foreach(var c in population)
            {
                accum += c.Fitness / fitSum;
                if(accum >= targets[j])
                {
                    j++;
                    yield return c;
                }
                if (j == selectionSize)
                    break;
            }
        }
    }
}
