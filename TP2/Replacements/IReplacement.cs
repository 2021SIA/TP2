using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;

namespace TP2.Replacements
{
    public interface IReplacement
    {
        ICollection<Character> GetReplacement(ICollection<Character> population);
    }
}
