using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;
using System.Linq;
using TP2.Genes;

namespace TP2.Crossovers
{
    public class OnePointCrossover : ICrossover
    {
        private static Random rnd = new Random();
        public ICollection<Character> Crossover(Character c1, Character c2)
        {
            int s = c1.Genes.Length;
            int p = rnd.Next(s + 1);

            var genes1 = new IGene[s];
            var genes2 = new IGene[s];

            c1.Genes[..p].CopyTo(genes1);
            c2.Genes[..p].CopyTo(genes2);
            c1.Genes[p..].CopyTo(genes1.AsSpan(p));
            c2.Genes[p..].CopyTo(genes2.AsSpan(p));

            return new Character[] { new Character(c1.CharacterType,genes1), new Character(c2.CharacterType, genes2) };
        }
    }
}
