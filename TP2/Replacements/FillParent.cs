using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;
using System.Linq;
using TP2.Selections;

namespace TP2.Replacements
{
    public class FillParent : IReplacement
    {
        public FillParent(int n, int k, ISelection selection)
        {
            N = n;
            K = k;
            Selection = selection;
        }

        public int N { get; }
        public int K { get; }
        public ISelection Selection { get; }

        public IEnumerable<Character> GetReplacement(IEnumerable<Character> population, IEnumerable<Character> children)
        {
            if (K > N)
                return children.Take(N);
            else
            {
                return children.Concat(Selection.Select(population, N - K));
            }
        }
    }
}
