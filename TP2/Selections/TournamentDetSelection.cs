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
    public class TournamentDetSelection : ISelection
    {
        private static Random rnd = new Random();
        private int M;

        public TournamentDetSelection(int M)
        {
            this.M = M;
        }
        public IEnumerable<Character> Select(IEnumerable<Character> population, int n, int selectionSize)
        { 
    
            int populationSize = population.Count();
            List<Character> populationList = population.ToList();
            List<Character> selected = new List<Character>();
            int K = selectionSize;
            IEnumerable<int> randNum;
            Character topCharacter;
            for(int i = 0; i< K; i++)
            {
                randNum = GenerateRandom(M,0,populationSize);
                topCharacter = populationList[randNum.First()];
                for(int j=1; j<M; j++)
                {
                    if(populationList[randNum.ElementAt(j)].Fitness > topCharacter.Fitness)
                        topCharacter = populationList[randNum.ElementAt(j)];
                }
                selected.Add(topCharacter);
            }
            return selected;
        }

        static Random random = new Random();

        // Note, max is exclusive here!
        private static List<int> GenerateRandom(int count, int min, int max)
        {

            //  initialize set S to empty
            //  for J := N-M + 1 to N do
            //    T := RandInt(1, J)
            //    if T is not in S then
            //      insert T in S
            //    else
            //      insert J in S
            //
            // adapted for C# which does not have an inclusive Next(..)
            // and to make it from configurable range not just 1.

            if (max <= min || count < 0 || 
                    // max - min > 0 required to avoid overflow
                    (count > max - min && max - min > 0))
            {
                // need to use 64-bit to support big ranges (negative min, positive max)
                throw new ArgumentOutOfRangeException("Range " + min + " to " + max + 
                        " (" + ((Int64)max - (Int64)min) + " values), or count " + count + " is illegal");
            }

            // generate count random values.
            HashSet<int> candidates = new HashSet<int>();

            // start count values before max, and end at max
            for (int top = max - count; top < max; top++)
            {
                // May strike a duplicate.
                // Need to add +1 to make inclusive generator
                // +1 is safe even for MaxVal max value because top < max
                if (!candidates.Add(random.Next(min, top + 1))) {
                    // collision, add inclusive max.
                    // which could not possibly have been added before.
                    candidates.Add(top);
                }
            }

            // load them in to a list, to sort
            List<int> result = candidates.ToList();

            // shuffle the results because HashSet has messed
            // with the order, and the algorithm does not produce
            // random-ordered results (e.g. max-1 will never be the first value)
            for (int i = result.Count - 1; i > 0; i--)
            {  
                int k = random.Next(i + 1);  
                int tmp = result[k];  
                result[k] = result[i];  
                result[i] = tmp;  
            }  
            return result;
        }
            

    }
}







            
            
            

            