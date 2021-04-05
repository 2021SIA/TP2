using System;
using System.Collections.Generic;
using System.Text;

namespace TP2.Genes
{
    public class HeightGene : IGene<double>
    {
        public static double Delta { get; set; } = 0.05;
        public static double MaxHeight { get; set; } = 2;
        public static double MinHeight { get; set; } = 1.3;
        public double Value { get; }

        private static readonly Random rnd = new Random();

        public IGene Mutate()
        {
            double d = rnd.NextDouble() * Delta;
            double height = (rnd.NextDouble() >= 0.5 ? Value + d : Value - d);
            height = Math.Clamp(height, MinHeight, MaxHeight);
            return new HeightGene(height);
        }

        public HeightGene(double height) => Value = height;
        public static HeightGene Random()
        {
            double height = rnd.NextDouble() * (MaxHeight - MinHeight) + MinHeight;
            return new HeightGene(height);
        }
    }
}
