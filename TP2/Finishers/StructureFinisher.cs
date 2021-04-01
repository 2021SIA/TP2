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
        private double percentageLimit;
        public StructureFinisher(double percentageLimit)
        {
            this.percentageLimit = percentageLimit;
        }
        public bool IsFinished(IEnumerable<Character> population, long generations, long time)
        {
            if (lastGeneration == null)
            {
                return false;
            }
            IEnumerable<Character> equalCharacters = population.Intersect(lastGeneration);
            lastGeneration = population;
            return equalCharacters.Count() / population.Count() > percentageLimit;
        }
    }
}
