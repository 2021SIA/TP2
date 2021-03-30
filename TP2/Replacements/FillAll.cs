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
        public FillAll(int n, ISelection selection)
        {
            N = n;
            Selection = selection;
        }

        public ISelection Selection { get; }
        public int N { get; }

        public IEnumerable<Character> GetReplacement(IEnumerable<Character> population, IEnumerable<Character> children)
        {
            return Selection.Select(children.Concat(population), N);
        }
    }
}
