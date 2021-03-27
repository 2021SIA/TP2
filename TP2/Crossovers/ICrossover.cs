using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;

namespace TP2.Crossovers
{
    public interface ICrossover
    {
        public ICollection<Character> Crossover(Character c1, Character c2); 
    }
}
