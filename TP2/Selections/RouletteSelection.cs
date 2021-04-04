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
            return Roulette(collection, n, selectionSize, (i, c) => c.Fitness, sum);
        }

        protected IEnumerable<Character> Roulette(IEnumerable<Character> population, int n, int selectionSize, Func<int,Character,double> Fitness, double fitnessSum)
        {
            var enumerator = population.GetEnumerator();
            if (!enumerator.MoveNext())
                yield break;

            var targets = TargetProb(selectionSize).ToArray();
            Array.Sort(targets);

            double accum = 0;
            int j = 0;
            Character c = enumerator.Current;
            for(int i = 1; i <= n && j < selectionSize;)
            {
                accum += Fitness(i, c);
                if (accum >= targets[j] * fitnessSum)
                {
                    j++;
                    yield return c;
                }
                else
                {
                    i++;
                    enumerator.MoveNext();
                    c = enumerator.Current;
                }
            }
        }
    }
}
