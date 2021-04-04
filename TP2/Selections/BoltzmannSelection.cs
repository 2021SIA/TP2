using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;
using System.Linq;

namespace TP2.Selections
{
    public class BoltzmannSelection : RouletteSelection
    {
        public BoltzmannSelection()
        {
            this.cantGen = 0;
        }
        private int cantGen;

        public override IEnumerable<Character> Select(IEnumerable<Character> population, int n, int selectionSize)
        {
            var collection = population.ToArray();
            double temp = functionTemp();
            double sum = collection.Sum(character => Math.Exp(character.Fitness / temp));
            var filter = Roulette(collection, n, selectionSize, (i, character) => Math.Exp(character.Fitness / temp), sum);
            cantGen++;
            return filter;
        }

        private double functionTemp()
        {
            double k = 1;
            double Tc = 1;
            double T0 = 10;
            return Tc + (T0 - Tc)*Math.Exp(-k*this.cantGen);
        }
    }
}
