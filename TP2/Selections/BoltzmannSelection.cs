using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;
using System.Linq;

namespace TP2.Selections
{
    public class BoltzmannSelection : ISelection
    {
        public BoltzmannSelection()
        {
            this.cantGen = 0;
        }
        private static Random rnd = new Random();
        private int cantGen;
        public IEnumerable<Character> Select(IEnumerable<Character> population, int n, int selectionSize)
        {
            int populationSize = population.Count();
            List<double> ExpValList = new List<double>();
            List<double> RelFitList = new List<double>();
            List<double> AcumFitList = new List<double>();
            List<Character> populationList = population.ToList();
            foreach(Character character in population)
            {
                ExpValList.Add(ExpectedValue(character, populationList));
            }
            int i=0;
            foreach(double pseudoFitness in ExpValList)
            {
                RelFitList.Add(relativeFitness(ExpValList[i], population));
                i+=1;
            }
            double acumelated = 0;
            foreach(double relFitness in RelFitList)
            {
                acumelated += relFitness;
                AcumFitList.Add(acumelated);
            }
            
            List<Character> newPopulation = new List<Character>();
            for (var j = 0; j < selectionSize; j++)
            {
                newPopulation.Add(getCharacter(AcumFitList, rnd.NextDouble(), populationList));
            }
            this.cantGen += 1;
            return newPopulation;
            
        }
        private Character getCharacter(List<double> accumulatedFitness, double num, List<Character> population)
        {
            var previous = 0.0;
            int populationSize = population.Count();
            for(int i = 0; i<populationSize ; i++)
            {
                if (num > previous && num < accumulatedFitness[i])
                {
                    return population[i];
                }
                previous = accumulatedFitness[i];
            }
            return population.Last();
        }
        private double ExpectedValue(Character character,  List<Character> populationList)
        {
            double average = 0;
            int populationSize = populationList.Count();
            double numerator = Math.Exp(character.Fitness/functionTemp());
            
            foreach(Character ch in populationList)
            {
                average  += Math.Exp(ch.Fitness/functionTemp());
            }
            average = average/populationSize;
            return numerator/average;
        }
        private double functionTemp()
        {
            double k = 1;
            double Tc = 0;
            double T0 = 10;
            return Tc + (T0 - Tc)*Math.Exp(-k*this.cantGen);

        }
        private double relativeFitness(double fitness, IEnumerable<Character> population)
        {
            double charFitness = fitness;
            double average = 0;
            foreach(Character ch in population)
            {
                average  += fitness;
            }
            return charFitness/average;
        }
    }
}
