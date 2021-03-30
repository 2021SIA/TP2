using System;
using System.Collections.Generic;
using System.Text;
using TP2.Genes;
using TP2.Models;

namespace TP2.Crossovers
{
    public class TwoPointCrossover : ICrossover
    {
        private static Random rnd = new Random();
        public ICollection<Character> Crossover(Character c1, Character c2)
        {
            int s = c1.Genes.Length;
            int p1 = rnd.Next(s + 1);
            int p2 = rnd.Next(s + 1);
            (p1, p2) = (Math.Min(p1, p2), Math.Max(p1, p2));

            var genes1 = new IGene[s];
            var genes2 = new IGene[s];

            c1.Genes[..p1].CopyTo(genes1);
            c2.Genes[..p1].CopyTo(genes2);
            c1.Genes[p1..p2].CopyTo(genes1.AsSpan(p1));
            c2.Genes[p1..p2].CopyTo(genes2.AsSpan(p1));
            c1.Genes[p2..].CopyTo(genes1.AsSpan(p2));
            c2.Genes[p2..].CopyTo(genes2.AsSpan(p2));

            return new Character[] { new Character(genes1), new Character(genes2) };
        }
    }
}
