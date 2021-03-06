using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;
using System.Linq;

namespace TP2.Finishers
{
    public class ContentFinisher : IFinisher
    {
        private long maxGenerations;
        private int currentRepeatGenerations;
        private double maxFitness;
        private double fitnessEpsilon;

        public ContentFinisher(long generations, double fitnessEpsilon)
        {
            this.currentRepeatGenerations = 0;
            this.maxGenerations = generations;
            this.maxFitness = 0;
            this.fitnessEpsilon = fitnessEpsilon;
        }

        public bool IsFinished(IEnumerable<Character> population, long generations, long time)
        {
            if (NotChangedPopulation(population))
                this.currentRepeatGenerations +=1;
            else
                this.currentRepeatGenerations = 1;
            return this.currentRepeatGenerations >= this.maxGenerations;
        }

        private bool NotChangedPopulation(IEnumerable<Character> newPopulation)
        {
            double currMaxFitness = 0;
            foreach(Character character in newPopulation)
            {
                if(character.Fitness > currMaxFitness)
                    currMaxFitness = character.Fitness;
            }
            if(currMaxFitness - this.maxFitness > fitnessEpsilon)
            {
                this.maxFitness = currMaxFitness;
                return false;
            }
            return true;
        }
    }
}
