using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;
using System.Linq;

namespace TP2.Selections
{
    public class BoltzmannSelection : RouletteSelection
    {
        private int cantGen;
        private double k = 1;
        private double Tc = 1;
        private double T0 = 10;
        public BoltzmannSelection(double k, double Tc, double T0)
        {
            this.cantGen = 0;
            this.k = k;
            this.Tc = Tc;
            this.T0 = T0;
        }

        public override IEnumerable<Character> Select(IEnumerable<Character> population, int n, int selectionSize)
        {
            var collection = population.ToArray();
            double temp = functionTemp();
            double avgExpFitness = collection.Average(character => Math.Exp(character.Fitness / temp));
            double sum = collection.Sum(character => Math.Exp(character.Fitness / temp) / avgExpFitness);
            var filter = Roulette(collection, n, selectionSize, (i, character) => Math.Exp(character.Fitness / temp) / avgExpFitness, sum);
            cantGen++;
            return filter;
        }

        private double functionTemp()
        {
            return Tc + (T0 - Tc)*Math.Exp(-k*this.cantGen);
        }
    }
}
