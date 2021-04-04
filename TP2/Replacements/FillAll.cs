using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;
using System.Linq;
using TP2.Selections;

namespace TP2.Replacements
{
    public class FillAll : IReplacement
    {
        public IEnumerable<Character> GetReplacement(IEnumerable<Character> population, IEnumerable<Character> children, ISelection selection, int n, int k)
        {
            return selection.Select(children.Concat(population), n+k, n);
        }
    }
}
