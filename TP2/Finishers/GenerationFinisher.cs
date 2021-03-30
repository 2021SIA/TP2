using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;

namespace TP2.Finishers
{
    public class GenerationFinisher : IFinisher
    {
        private int maxGenerations;
        public GenerationFinisher(int maxGenerations)
        {
            this.maxGenerations = maxGenerations;
        }
        public bool IsFinished(IEnumerable<Character> population, long generations, long time)
        {
            return generations >= maxGenerations;
        }
    }
}
