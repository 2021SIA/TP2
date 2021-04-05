using System;
using TP2.Genes;
using TP2.Models;

namespace TP2.Mutations
{
    public class MultiGeneLimitedMutation : IMutation
    {
        private static Random rnd = new Random();
        private int[] GetRandomNumbers(int n)
        {
            int[] numbers = new int[n];
            for (var i = 0; i < n; i++)
                numbers[i] = i;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                int value = numbers[k];
                numbers[k] = numbers[n];
                numbers[n] = value;
            }
            return numbers;
        }
        public Character Mutate(Character c, double probability)
        {
            int m = rnd.Next(1, c.Genes.Length + 1);
            IGene[] genes = c.Genes.ToArray();
            int[] randomIndexes = GetRandomNumbers(c.Genes.Length);
            for (var i = 0; i < m; i++)
            {
                int index = randomIndexes[i];
                if (rnd.NextDouble() < probability)
                    genes[index] = c.Genes[index].Mutate();
            }
            return new Character(c.CharacterType, genes);
        }
    }
}
