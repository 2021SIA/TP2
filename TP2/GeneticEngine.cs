using System;
using System.Collections.Generic;
using TP2.Crossovers;
using TP2.Finishers;
using TP2.Models;
using TP2.Mutations;
using TP2.Replacements;
using TP2.Selections;
using System.Linq;
using System.Diagnostics;

namespace TP2
{
    class GeneticEngine
    {
        public GeneticEngine(ICrossover crossover, IMutation mutation, ISelection parentSelection, IReplacement replacement, ISelection replacementSelection, IFinisher finish, int n, int k, double pc)
        {
            Crossover = crossover;
            Mutation = mutation;
            ParentSelection = parentSelection;
            Replacement = replacement;
            ReplacementSelection = replacementSelection;
            Finish = finish;
            N = n;
            K = k;
            Pc = pc;
        }

        public ICrossover Crossover { get; }
        public IMutation Mutation { get; }
        public ISelection ParentSelection { get; }
        public ISelection ReplacementSelection { get; }
        public IReplacement Replacement { get; }
        public IFinisher Finish { get; }
        public int N { get; set; }
        public int K { get; set; }
        public double Pc { get; set; }

        IEnumerable<(Character c1, Character c2)> ParentPairings(IEnumerable<Character> parents)
        {
            var iter = parents.GetEnumerator();
            iter.MoveNext();
            do
            {
                Character c1 = iter.Current;
                iter.MoveNext();
                Character c2 = iter.Current;
                yield return (c1, c2);
            }
            while (iter.MoveNext());
        }

        IEnumerable<Character> AdvanceGeneration(IEnumerable<Character> population)
        {
            var parents = ParentSelection.Select(population, N, K);
            var children = ParentPairings(parents).SelectMany(pair => Crossover.Crossover(pair.c1, pair.c2));
            var mutatedChildren = children.Select(c => Mutation.Mutate(c, Pc));
            var next = Replacement.GetReplacement(population, mutatedChildren, ReplacementSelection);
            return next;
        }

        IEnumerable<Character> UntilFinish(IEnumerable<Character> population)
        {
            int gen = 0;
            Stopwatch sw = new Stopwatch();
            var currentPopulation = population;
            sw.Start();
            while (!Finish.IsFinished(currentPopulation, gen, sw.ElapsedMilliseconds))
            {
                currentPopulation = AdvanceGeneration(currentPopulation).ToList();
                gen++;
                Console.Write("\r" + new string(' ', Console.WindowWidth - 1) + "\r");
                Console.Write($"Current Generation: {gen}");
            }
            sw.Stop();
            Console.WriteLine();
            Console.WriteLine($"Advanced {gen} generations in {sw.ElapsedMilliseconds}ms");


            return currentPopulation;
        }
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Ingrese el nombre del archivo de configuración.");
                return;
            }
            Configuration config;
            try
            {
                config = Configuration.LoadConfiguration(args[0]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            int n = config.N;
            int k = config.K;
            double pc = config.MutationProbablity;

            var crossover = config.CrossoverMethod;
            var mutation = config.MutationMethod;
            var parentSelection = config.Method1;
            var replacementSelection = config.Method3;
            var replacement = config.ReplacementMethod;
            var finish = config.FinishCondition;

            var engine = new GeneticEngine(crossover, mutation, parentSelection, replacement, replacementSelection, finish, n, k, pc);
            var population = Enumerable.Range(0, n).Select(_ => new Character());
            var initialScore = population.Average(c => c.Fitness);
            Console.WriteLine($"Initial score: {initialScore}");
            var evolved = engine.UntilFinish(population);
            var finalScore = evolved.Average(c => c.Fitness);
            Console.WriteLine($"Final score: {finalScore}");

        }
    }
}
