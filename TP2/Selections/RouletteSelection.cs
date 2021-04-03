using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TP2.Models;

namespace TP2.Selections
{
    public class RouletteSelection : ISelection
    {
        protected static Random random = new Random();

        protected virtual IEnumerable<double> TargetProb(int k)
        {
            for (int j = 0; j < k; j++)
            {
                double r = random.NextDouble();
                yield return r;
            }
        }


        public virtual IEnumerable<Character> Select(IEnumerable<Character> population, int n, int selectionSize)
        {
            var collection = population.ToArray();
            double sum = collection.Sum(c => c.Fitness);
            return Roulette(collection, selectionSize, (i, c) => c.Fitness, sum);
        }

        protected IEnumerable<Character> Roulette(IEnumerable<Character> population, int selectionSize, Func<int,Character,double> Fitness, double fitnessSum)
        {
            var targets = TargetProb(selectionSize).ToArray();
            Array.Sort(targets);

            double accum = 0;
            int i = 1, j = 0;
            foreach (var c in population)
            {
                accum += Fitness(i, c) / fitnessSum;
                if (accum >= targets[j])
                {
                    j++;
                    yield return c;
                }
                if (j == selectionSize)
                    break;
                i++;
            }
        }
    }
}
