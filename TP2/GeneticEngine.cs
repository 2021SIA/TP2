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
using System.Threading.Tasks;
using System.IO;

namespace TP2
{
    public class GeneticEngine
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
        public GeneticEngine(Configuration config)
        {
            N = config.N;
            K = config.K;
            Pc = config.MutationProbablity;
            Crossover = config.CrossoverMethod;
            Mutation = config.MutationMethod;
            ParentSelection = config.Method1;
            ReplacementSelection = config.Method3;
            Replacement = config.ReplacementMethod;
            Finish = config.FinishCondition;
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

        public IEnumerable<Character> AdvanceGeneration(IEnumerable<Character> population)
        {
            var parents = ParentSelection.Select(population, N, K);
            var children = ParentPairings(parents).SelectMany(pair => Crossover.Crossover(pair.c1, pair.c2));
            var mutatedChildren = children.Select(c => Mutation.Mutate(c, Pc));
            var next = Replacement.GetReplacement(population, mutatedChildren, ReplacementSelection);
            return next;
        }

        public IEnumerable<Character> UntilFinish(IEnumerable<Character> population)
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
        /// <summary>
        /// Genetic Algorithms Engine
        /// </summary>
        /// <param name="config">Configuration yaml file path</param>
        /// <param name="listen">Flag to run engine as web server</param>
        static void Main(string config, string dataset, bool listen)
        {
            if(dataset.Length == 0)
            {
                Console.WriteLine("Ingrese el nombre del directorio conteniendo el dataset.");
                return;
            }
            if(config.Length == 0 && !listen)
            {
                Console.WriteLine("Ingrese el nombre del archivo de configuraci�n.");
                return;
            }
            Console.WriteLine("Loading dataset...");
            var weapons = ItemTSVLoader.Load(Path.Join(dataset,"armas.tsv"), Item.Type.WEAPON);
            var boots = ItemTSVLoader.Load(Path.Join(dataset, "botas.tsv"), Item.Type.BOOTS);
            var helmets = ItemTSVLoader.Load(Path.Join(dataset, "cascos.tsv"), Item.Type.HELMET);
            var gloves = ItemTSVLoader.Load(Path.Join(dataset, "guantes.tsv"), Item.Type.GLOVES);
            var chests = ItemTSVLoader.Load(Path.Join(dataset, "pecheras.tsv"), Item.Type.CHEST);
            Item.LoadItems(weapons.Concat(boots).Concat(helmets).Concat(gloves).Concat(chests));
            Console.WriteLine("Dataset Loaded.");
            if (listen)
            {
                GeneticEngineServer engineServer = new GeneticEngineServer("http://localhost:5000/");
                Task listenTask = engineServer.HandleIncomingConnections();
                listenTask.GetAwaiter().GetResult();
            }
            else
            {
                Configuration configuration;
                try
                {
                    configuration = Configuration.FromYamlFile(config);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
                var engine = new GeneticEngine(configuration);
                var population = Enumerable.Range(0, engine.N).Select(_ => new Character());
                var initialScore = population.Sum(c => c.Fitness);
                Console.WriteLine($"Initial score: {initialScore}");
                var evolved = engine.UntilFinish(population);
                var finalScore = evolved.Sum(c => c.Fitness);
                Console.WriteLine($"Final score: {finalScore}");
            }

        }
    }
}
