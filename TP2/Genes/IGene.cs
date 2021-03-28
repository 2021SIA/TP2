using System;
using System.Collections.Generic;
using System.Text;

namespace TP2.Genes
{
    public interface IGene
    {
        IGene Mutate();
    }
    public interface IGene<T> : IGene
    {
        T Value { get; }
    }
}
