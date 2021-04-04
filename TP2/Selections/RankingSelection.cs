using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TP2.Models;

namespace TP2.Selections
{
    public class RankingSelection : RouletteSelection
    {
        public override IEnumerable<Character> Select(IEnumerable<Character> p, int n, int selectionSize)
        {
            var population = p.OrderByDescending(c => c.Fitness);
            double sum = (n - 1) / 2;
            return Roulette(population, n, selectionSize, (i, c) => ((double)(n - i) / n), sum);
        }
    }
}
