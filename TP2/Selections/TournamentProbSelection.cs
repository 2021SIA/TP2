using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;
using System.Linq;

// Se elige un valor de Threshold en [0.5 , 1]
// 2. De la población de tamaño N, se eligen 2 individuos al azar.
// 3. Se toma un valor r al azar uniformemente en [0,1].
// a. Si r < Threshold se selecciona el más apto.
// b. Caso contrario, se selecciona el menos apto.
// 4. Se repite el proceso (1.) hasta conseguir los K individuos que
// se precisan.
namespace TP2.Selections
{
    public class TournamentProbSelection : TournamentSelection
    {
        protected override Character Generator(IList<Character> population)
        {
            int[] randNum = GenerateRandom(2);
            double threshold = random.NextDouble() / 2.0 + 0.5;
            var character1 = population[randNum[0]];
            var character2 = population[randNum[1]];
            double r = random.NextDouble();
            if (r < threshold)
                return (character1.Fitness > character2.Fitness ? character1 : character2);
            else
                return (character1.Fitness > character2.Fitness ? character2 : character1);
        }
    }
}
