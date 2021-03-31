using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;
using System.Linq;

namespace TP2.Finishers
{
    public class SolutionFinisher : IFinisher
    {
        public SolutionFinisher(double targetFitness)
        {
            TargetFitness = targetFitness;
        }

        public double TargetFitness { get; set; }
        private int genCount = 0;
        public bool IsFinished(IEnumerable<Character> population, long generations, long time)
        {
            var max = population.Max(c => c.Fitness);
            genCount++;
            if(genCount == 50)
            {
                Console.WriteLine($"Max fitness: {max}");
                genCount = 0;
            }
            return max >= TargetFitness;
        }
    }
}
