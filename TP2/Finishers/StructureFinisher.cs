using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TP2.Models;

namespace TP2.Finishers
{
    public class StructureFinisher : IFinisher
    {
        private IEnumerable<Character> lastGeneration = null;
        private int consecutiveGenerations;
        private int generationLimit;
        private double percentageLimit;
        public StructureFinisher(double percentageLimit, int generationLimit)
        {
            this.consecutiveGenerations = 0;
            this.percentageLimit = percentageLimit;
            this.generationLimit = generationLimit;
        }
        public bool IsFinished(IEnumerable<Character> population, long generations, long time)
        {
            if (lastGeneration == null)
            {
                lastGeneration = population;
                return false;
            }
            IEnumerable<Character> equalCharacters = population.Intersect(lastGeneration);
            lastGeneration = population;
            double equalPercentage = (double)equalCharacters.Count() / population.Distinct().Count();
            consecutiveGenerations = equalPercentage > percentageLimit ? consecutiveGenerations + 1 : 0;
            return consecutiveGenerations >= generationLimit;
        }
    }
}
