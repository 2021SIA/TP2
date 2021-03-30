using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;

namespace TP2.Selections
{
    public interface ISelection
    {
        IEnumerable<Character> Select(IEnumerable<Character> population, int selectionSize);
    }
}
