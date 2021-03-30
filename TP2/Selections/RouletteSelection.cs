using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TP2.Models;

namespace TP2.Selections
{
    public class RouletteSelection : ISelection
    {
        private static Random rnd = new Random();

        private Character getCharacter(IEnumerable<(Character c, double value)> accumulatedFitness, double num)
        {
            var previous = 0.0;
            foreach ((Character c, double value) in accumulatedFitness)
            {
                if (num > previous && num < value)
                {
                    return c;
                }
                previous = value;
            }
            return accumulatedFitness.Last().c;
        }
        public IEnumerable<Character> Select(IEnumerable<Character> population, int selectionSize)
        {

            double totalFitness = population.Aggregate(0.0, (total, c) => total + c.Fitness);

            List<(Character c, double value)> accumulatedRelativeFitness = new List<(Character c, double value)>();
            population.Aggregate(0.0, (accum, c) =>
            {
                double value = accum + (c.Fitness / totalFitness);
                accumulatedRelativeFitness.Add((c,value));
                return value;
            });

            List<Character> newPopulation = new List<Character>();
            for (var i = 0; i < selectionSize; i++)
            {
                newPopulation.Add(getCharacter(accumulatedRelativeFitness, rnd.NextDouble()));
            }
            return newPopulation;
        }
    }
}
