using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TP2.Models;

namespace TP2.Selections
{
    public class EliteSelection : ISelection
    {
        public IEnumerable<Character> Select(IEnumerable<Character> population, int selectionSize)
        {
            var n = population.Count();
            var filter = population
                .OrderByDescending(c => c.Fitness)
                .Take(selectionSize);
            if (selectionSize > n)
                filter = filter
                    .SelectMany((c, i) => Enumerable.Repeat(c, (int)Math.Ceiling((double)(selectionSize - i) / n)));
            
            return filter;
        }
    }
}
