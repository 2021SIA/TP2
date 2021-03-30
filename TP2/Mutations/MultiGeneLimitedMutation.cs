using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TP2.Genes;
using TP2.Models;

namespace TP2.Mutations
{
    public class MultiGeneLimitedMutation : IMutation
    {
        private static Random rnd = new Random();
        public static IEnumerable<int> GetRandomNumbers(int n, int m)
        {
            int[] numbers = new int[n];
            for(var i = 0; i < n; i++) { numbers[i] = i; }
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                int value = numbers[k];
                numbers[k] = numbers[n];
                numbers[n] = value;
            }
            return numbers.Take(m);
        }
        public Character Mutate(Character c, double probability)
        {
            int m = rnd.Next(1,c.Genes.Length + 1);
            IGene[] genes = new IGene[c.Genes.Length];
            IEnumerable<int> randomIndexes = GetRandomNumbers(c.Genes.Length, m);
            for(var i = 0; i < c.Genes.Length; i++)
            {
                genes[i] = randomIndexes.Contains(i) && rnd.NextDouble() < probability ? c.Genes[i].Mutate() : c.Genes[i];
            }
            return new Character(genes);
        }
    }
}
