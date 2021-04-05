using System;
using System.Collections.Generic;
using System.Text;
using TP2.Genes;
using TP2.Models;

namespace TP2.Mutations
{
    public class GeneMutation : IMutation
    {
        private static Random rnd = new Random();
        public Character Mutate(Character c, double probability)
        {
            int s = c.Genes.Length;
            var genes = c.Genes.ToArray();

            if(rnd.NextDouble() < probability)
            {
                int p = rnd.Next(s);
                genes[p] = genes[p].Mutate();
            }

            return new Character(c.CharacterType, genes);
        }
    }
}
