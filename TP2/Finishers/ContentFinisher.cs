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
        
        public ContentFinisher()
        {
            this.currentRepeatGenerations = 0;
        }
        public void MaxRepeatedGenerations(long generations)
        {
            this.maxGenerations = generations;
            this.maxFitness = 0;
        }

        public bool IsFinished(IEnumerable<Character> population, long generations, long time)
        {
            List<Character> populationList = population.ToList();
            if (NotChangedPopulation(populationList))
                this.currentRepeatGenerations +=1;
            return this.currentRepeatGenerations >= this.maxGenerations;
        }

        private bool NotChangedPopulation(List<Character> newPopulation)
        {
            double currMaxFitness = 0;
            foreach(Character character in newPopulation)
            {
                if(character.Fitness > currMaxFitness)
                    currMaxFitness = character.Fitness;
            }
            if(currMaxFitness != this.maxFitness)
            {
                this.maxFitness = currMaxFitness;
                return false;
            }
            return true;
        }
    }
}
