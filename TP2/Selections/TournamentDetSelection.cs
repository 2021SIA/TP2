using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;
using System.Linq;

// 1. De la población de tamaño N, se eligen M individuos al azar.
// 2. De los M individuos, se elige el mejor.
// 3. Se repite el proceso (1.) hasta conseguir los K individuos que
// se precisan.


namespace TP2.Selections
{
    public class TournamentDetSelection : TournamentSelection
    {
        public TournamentDetSelection(int m)
        {
            M = m;
        }

        public int M { get; set; }
        protected override Character Generator(IList<Character> population)
        {
            var randNum = GenerateRandom(M);
            Character top = population[randNum[0]];
            for (int j = 1; j < M; j++)
            {
                if (population[randNum[1]].Fitness > top.Fitness)
                    top = population[randNum[1]];
            }
            return top;
        }
    }
}







            
            
            

            