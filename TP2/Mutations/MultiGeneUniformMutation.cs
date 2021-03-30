using System;
using System.Collections.Generic;
using System.Text;
using TP2.Genes;
using TP2.Models;

namespace TP2.Mutations
{
    class MultiGeneUniformMutation : IMutation
    {
        private static Random rnd = new Random();
        public Character Mutate(Character c, double probability)
        {
            IGene[] genes = new IGene[c.Genes.Length];
            for(var i = 0; i < c.Genes.Length; i++)
            {
                genes[i] = rnd.NextDouble() < probability ? c.Genes[i].Mutate() : c.Genes[i];
            }
            return new Character(genes);
        }
    }
}
