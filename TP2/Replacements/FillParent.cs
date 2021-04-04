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
        public IEnumerable<Character> GetReplacement(IEnumerable<Character> population, IEnumerable<Character> children, ISelection selection, int n, int k)
        {
            if (k > n)
                return selection.Select(children, k, n);
            else
            {
                return children.Concat(selection.Select(population, n, n - k));
            }
        }
    }
}
