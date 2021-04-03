using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;
using System.Linq;
using System.Collections.Immutable;

namespace TP2.Selections
{
    public class UniversalSelection : RouletteSelection
    {
        protected override IEnumerable<double> TargetProb(int k)
        {
            for(int j = 0; j < k; j++)
            {
                double r = random.NextDouble();
                yield return (r + j) / k;
            }
        }
    }
}
