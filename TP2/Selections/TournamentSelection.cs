using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TP2.Models;

namespace TP2.Selections
{
    public abstract class TournamentSelection : ISelection
    {
        protected static Random random = new Random();
        private int[] indexes = null;
        private void UpdateGenerator(int n)
        {
            if (indexes == null || indexes.Length != n)
                indexes = Enumerable.Range(0, n).ToArray();
        }

        protected int[] GenerateRandom(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int j = random.Next(i, count);
                int temp = indexes[i];
                indexes[i] = indexes[j];
                indexes[j] = temp;
            }
            return indexes[..count];
        }

        public IEnumerable<Character> Select(IEnumerable<Character> population, int n, int k)
        {
            UpdateGenerator(n);
            var collection = population.ToArray();
            for (int i = 0; i < k; i++)
            {
                yield return Generator(collection);
            }
        }


        protected abstract Character Generator(IList<Character> population);
    }
}
