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
        public FillParent(int n, int k)
        {
            N = n;
            K = k;
        }

        public int N { get; }
        public int K { get; }

        public IEnumerable<Character> GetReplacement(IEnumerable<Character> population, IEnumerable<Character> children, ISelection selection)
        {
            if (K > N)
                return selection.Select(children, K, N);
            else
            {
                return children.Concat(selection.Select(population, N, N - K));
            }
        }
    }
}
