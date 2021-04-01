using System;
using System.Collections.Generic;
using System.Text;
using TP2.Genes;
using TP2.Models;

namespace TP2.Crossovers
{
    public class AnularCrossover : ICrossover
    {
        private static Random rnd = new Random();
        public ICollection<Character> Crossover(Character c1, Character c2)
        {
            int s = c1.Genes.Length;
            int p = rnd.Next(s);
            int l = rnd.Next((s+1) / 2 + 1);

            var genes1 = new IGene[s];
            var genes2 = new IGene[s];

            for(var i = 0; i < s; i++)
            {
                var position = (p + i) % s;
                genes1[position] = i < l ? c1.Genes[position] : c2.Genes[position];
                genes2[position] = i < l ? c2.Genes[position] : c1.Genes[position];
            }
            return new Character[] { new Character(genes1), new Character(genes2) };
        }
    }
}
