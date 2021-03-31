using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;

namespace TP2.Finishers
{
    public class ContentFinisher : IFinisher
    {
        private int maxGenerations;
        private int currentRepeatGenerations;
        private int maxFitness;
        
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
            if(NotChangedPopulation(population)
                this.currentRepeatGenerations +=1;
            return this.currentRepeatGenerations >= this.maxGenerations;
        }

        private bool NotChangedPopulation(List<Character> newPopulation)
        {
            int currMaxFitness = 0;
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
