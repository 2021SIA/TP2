﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TP2.Crossovers;
using TP2.Finishers;
using TP2.Models;
using TP2.Mutations;
using TP2.Replacements;
using TP2.Selections;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TP2
{
    public class Configuration
    {
        private class ConfigurationFile
        {
            public string Method1 { get; set; }
            public string Method2 { get; set; }
            public string Method3 { get; set; }
            public string Method4 { get; set; }
            public double A { get; set; }
            public double B { get; set; }
            public string CrossoverMethod { get; set; }
            public string MutationMethod { get; set; }
            public double MutationProbability { get; set; }
            public string ReplacementMethod { get; set; }
            public string Finish { get; set; }
            public string CharacterType { get; set; }
            public int N { get; set; }
            public int K { get; set; }
            public string Weapons { get; set; }
            public string Helmets { get; set; }
            public string Chests { get; set; }
            public string Gloves { get; set; }
            public string Boots { get; set; }
            public int TimeLimit { get; set; } = -1;
            public int GenerationsLimit { get; set; } = -1;
            public double TargetFitness { get; set; } = -1;
            public double StructurePercentage { get; set; } = -1;
        }
        public ISelection Method1 { get; }
        public ISelection Method2 { get; }
        public ISelection Method3 { get; }
        public ISelection Method4 { get; }
        public double A { get; }
        public double B { get; }
        public ICrossover CrossoverMethod { get; }
        public IMutation MutationMethod { get; }
        public double MutationProbablity { get; }
        public IReplacement ReplacementMethod { get; }
        public IFinisher FinishCondition { get; }
        public Character.Type CharacterType { get; }
        public int N { get; }
        public int K { get; }
        private static ISelection GetSelection(string selectionName)
        {
            switch (selectionName)
            {
                case "elite": return new EliteSelection();
                case "roulette": return new RouletteSelection();
                case "ranking": return new RankingSelection();
                case "boltzmann": return new BoltzmannSelection();
                case "tournament det": return new TournamentDetSelection();
                case "tournament prob": return new TournamentProbSelection();
                case "universal": return new UniversalSelection();
                default: throw new Exception("Método de selección inválido.");
            }
        }
        private static ICrossover GetCrossover(string crossoverName)
        {
            switch (crossoverName)
            {
                case "one point": return new OnePointCrossover();
                case "two point": return new TwoPointCrossover();
                case "anular": return new AnularCrossover();
                case "uniform": return new UniformCrossover();
                default: throw new Exception("Método de cruce inválido.");
            }
        }
        private static IMutation GetMutation(string mutationName)
        {
            switch (mutationName)
            {
                case "gene": return new GeneMutation();
                case "complete": return new CompleteMutation();
                case "multigene limited": return new MultiGeneLimitedMutation();
                case "multigene uniform": return new MultiGeneUniformMutation();
                default: throw new Exception("Método de mutación inválido.");
            }
        }
        private static IReplacement GetReplacement(string replacementName, int n, int k)
        {
            switch (replacementName)
            {
                case "fill all": return new FillAll(n);
                case "fill parent": return new FillParent(n, k);
                default: throw new Exception("Implementación de reemplazo inválida.");
            }
        }
        private static IFinisher GetFinisher(string finisherName, ConfigurationFile configFile)
        {
            switch (finisherName)
            {
                case "time": return new TimeFinisher(configFile.TimeLimit);
                case "generations": return new GenerationFinisher(configFile.GenerationsLimit);
                case "acceptable solution": return new SolutionFinisher(configFile.TargetFitness);
                case "structure": return new StructureFinisher(configFile.StructurePercentage);
                case "content": return new ContentFinisher();
                default: throw new Exception("Condicion de corte inválida.");
            }
        }
        private static Character.Type GetCharacterType(string characterType)
        {
            switch (characterType)
            {
                case "warrior":return Character.Type.WARRIOR;
                case "archer": return Character.Type.ARCHER;
                case "defender": return Character.Type.DEFENDER;
                case "rogue": return Character.Type.ROGUE;
                default: throw new Exception("Tipo de personaje inválido.");
            }
        }
        public static Configuration LoadConfiguration(string path)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();
            ConfigurationFile configFile = deserializer.Deserialize<ConfigurationFile>(File.OpenText(path));
            ISelection method1 = GetSelection(configFile.Method1),
                method2 = GetSelection(configFile.Method2),
                method3 = GetSelection(configFile.Method3),
                method4 = GetSelection(configFile.Method4);
            ICrossover crossover = GetCrossover(configFile.CrossoverMethod);
            IMutation mutation = GetMutation(configFile.MutationMethod);
            IReplacement replacement = GetReplacement(configFile.ReplacementMethod,configFile.N,configFile.K);
            Character.Type type = GetCharacterType(configFile.CharacterType);
            IFinisher finisher = GetFinisher(configFile.Finish, configFile);
            Console.WriteLine("Loading dataset...");
            var weapons = ItemTSVLoader.Load(configFile.Weapons, Item.Type.WEAPON);
            var boots = ItemTSVLoader.Load(configFile.Boots, Item.Type.BOOTS);
            var helmets = ItemTSVLoader.Load(configFile.Helmets, Item.Type.HELMET);
            var gloves = ItemTSVLoader.Load(configFile.Gloves, Item.Type.GLOVES);
            var chests = ItemTSVLoader.Load(configFile.Chests, Item.Type.CHEST);
            Item.LoadItems(weapons.Concat(boots).Concat(helmets).Concat(gloves).Concat(chests));
            Console.WriteLine("Dataset Loaded.");
            return new Configuration(configFile.N,configFile.K,configFile.A,configFile.B,method1,method2,method3,method4,
                crossover,mutation,configFile.MutationProbability,replacement, finisher, type);
        }
        private Configuration() { }
        private Configuration(int n, int k, double a, double b,
            ISelection method1, 
            ISelection method2, 
            ISelection method3, 
            ISelection method4, 
            ICrossover crossover,
            IMutation mutation,
            double mutationProbability,
            IReplacement replacement,
            IFinisher finisher,
            Character.Type characterType)
        {
            this.N = n;
            this.K = k;
            this.A = a;
            this.B = b;
            this.Method1 = method1;
            this.Method2 = method2;
            this.Method3 = method3;
            this.Method4 = method4;
            this.CrossoverMethod = crossover;
            this.MutationMethod = mutation;
            this.ReplacementMethod = replacement;
            this.MutationProbablity = mutationProbability;
            this.FinishCondition = finisher;
            this.CharacterType = characterType;
        }


    }
}
