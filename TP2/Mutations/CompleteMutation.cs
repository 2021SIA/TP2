using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;
using TP2.Genes;

namespace TP2.Mutations
{
    public class CompleteMutation : IMutation
    {
        private static Random rnd = new Random();
        public Character Mutate(Character c, double probability)
        {
            int i = 0;
            var genes = c.Genes.ToArray();
            
            if(rnd.NextDouble() < probability)
            {
                foreach(IGene gene in c.Genes)
                {
                    genes[i] = gene.Mutate();
                    i+=1;
                }

            }

            return new Character(c.CharacterType,genes);
        }
    }
}
