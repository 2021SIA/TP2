using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TP2.Models;

namespace TP2.Selections
{
    public class EliteSelection : ISelection
    {
        public IEnumerable<Character> Select(IEnumerable<Character> population, int n, int k)
        {
            int N = n;
            var filter = population
                .OrderByDescending(c => c.Fitness)
                .Take(k);
            if (k > N)
                filter = filter
                    .SelectMany((c, i) => Enumerable.Repeat(c, (int)Math.Ceiling((double)(k - i) / N)));

            return filter;
        }
    }
}
