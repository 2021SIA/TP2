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
        public FillAll(int n, int k)
        {
            N = n;
            K = k;
        }

        public int N { get; }
        public int K { get; }

        public IEnumerable<Character> GetReplacement(IEnumerable<Character> population, IEnumerable<Character> children, ISelection selection)
        {
            return selection.Select(children.Concat(population), N+K, N);
        }
    }
}
