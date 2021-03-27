using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;

namespace TP2.Mutations
{
    public interface IMutation
    {
        Character Mutate(Character c, double probability);
    }
}
