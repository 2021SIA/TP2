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
        public GeneticEngine(ICrossover crossover, IMutation mutation, ISelection parentSelection, IReplacement replacement, IFinisher finish, int k, double pc)
        {
            Crossover = crossover;
            Mutation = mutation;
            ParentSelection = parentSelection;
            Replacement = replacement;
            Finish = finish;
            K = k;
            Pc = pc;
        }

        public ICrossover Crossover { get; }
        public IMutation Mutation { get; }
        public ISelection ParentSelection { get; }
        public IReplacement Replacement { get; }
        public IFinisher Finish { get; }
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
            var parents = ParentSelection.Select(population, K);
            //var scorePo = population.Average(c => c.Fitness);
            //var scorePa = parents.Average(c => c.Fitness);
            var children = ParentPairings(parents).SelectMany(pair => Crossover.Crossover(pair.c1, pair.c2));
            var mutatedChildren = children.Select(c => Mutation.Mutate(c, Pc));
            var next = Replacement.GetReplacement(population, mutatedChildren);
            //var scoreRep = next.Average(c => c.Fitness);
            return next.ToList();
        }

        IEnumerable<Character> UntilFinish(IEnumerable<Character> population)
        {
            int gen = 0;
            Stopwatch sw = new Stopwatch();
            var currentPopulation = population;
            sw.Start();
            while (!Finish.IsFinished(currentPopulation, gen, sw.ElapsedMilliseconds))
            {
                currentPopulation = AdvanceGeneration(currentPopulation);
                gen++;
            }

            return currentPopulation;
        }

        static void Main(string[] args)
        {
            int n = 1000;
            int k = 200;
            int gens = 1000;
            double pc = 0.05;

            var weapons = ItemTSVLoader.Load("Dataset/allitems/armas.tsv", Item.Type.WEAPON);
            var boots = ItemTSVLoader.Load("Dataset/allitems/botas.tsv", Item.Type.BOOTS);
            var helmets = ItemTSVLoader.Load("Dataset/allitems/cascos.tsv", Item.Type.HELMET);
            var gloves = ItemTSVLoader.Load("Dataset/allitems/guantes.tsv", Item.Type.GLOVES);
            var chests = ItemTSVLoader.Load("Dataset/allitems/pecheras.tsv", Item.Type.CHEST);
            Item.LoadItems(weapons.Concat(boots).Concat(helmets).Concat(gloves).Concat(chests));

            var crossover = new TwoPointCrossover();
            var mutation = new GeneMutation();
            var parentSelection = new EliteSelection();
            var populationSelection = new EliteSelection();
            var replacement = new FillAll(n, populationSelection);
            var finish = new GenerationFinisher(gens);

            var engine = new GeneticEngine(crossover, mutation, parentSelection, replacement, finish, k, pc);
            var population = Enumerable.Range(0, n).Select(_ => new Character());
            var initialScore = population.Sum(c => c.Fitness);
            Console.WriteLine($"Initial score: {initialScore}");
            var evolved = engine.UntilFinish(population);
            var finalScore = evolved.Sum(c => c.Fitness);
            Console.WriteLine($"Final score: {finalScore}");

        }
    }
}
