using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TP2.Models;

namespace TP2.Selections
{
    public class RankingSelection : ISelection
    {
        private static Random rnd = new Random();
        private Character getCharacter(IEnumerable<(Character c, double value)> accumulatedRelativeFitness, double num)
        {
            var previous = 0.0;
            foreach ((Character c, double value) in accumulatedRelativeFitness)
            {
                if (num > previous && num < value)
                {
                    return c;
                }
                previous = value;
            }
            return accumulatedRelativeFitness.Last().c;
        }
        public IEnumerable<Character> Select(IEnumerable<Character> population, int selectionSize)
        {
            int populationSize = population.Count();
            List<(Character c, double value)> accumulatedFitness = new List<(Character c, double value)>();

            double pseudoFitnessSum = population.ToList()
                .OrderBy(c => c.Fitness)
                .Reverse()
                .Select((c,rank) => (c,(populationSize - (rank + 1)) / (double)populationSize))
                .Aggregate(0.0,(accum,pseudoFitness) => {
                    var value = accum + pseudoFitness.Item2;
                    accumulatedFitness.Add((pseudoFitness.c, value));
                    return value;
                });

            var accumulatedRelativeFitness = accumulatedFitness.Select(fitness => (fitness.c, fitness.value / pseudoFitnessSum));

            List<Character> newPopulation = new List<Character>();
            for (var i = 0; i < selectionSize; i++)
            {
                newPopulation.Add(getCharacter(accumulatedRelativeFitness, rnd.NextDouble()));
            }
            return newPopulation;
        }
    }
}
