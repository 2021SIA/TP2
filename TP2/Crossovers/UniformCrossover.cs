using System;
using System.Collections.Generic;
using System.Text;
using TP2.Genes;
using TP2.Models;
using System.Linq;

namespace TP2.Crossovers
{
    public class UniformCrossover : ICrossover
    {
        private static Random rnd = new Random();
         public ICollection<Character> Crossover(Character c1, Character c2)
        {
            int flip;
            int s = c1.Genes.Length;

            var genes1 = new IGene[s];
            var genes2 = new IGene[s];

            for(var i = 0; i < s; i++)
            {
                flip = rnd.Next(2);
                genes1[i] = flip == 0 ? c1.Genes[i] : c2.Genes[i];
                genes2[i] = flip == 0 ? c2.Genes[i] : c1.Genes[i];
            }
            return new Character[] { new Character(genes1), new Character(genes2) };
        }
    }
}
