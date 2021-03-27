﻿using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;

namespace TP2.Selections
{
    public interface ISelection
    {
        ICollection<Character> Select(ICollection<Character> population, int selectionSize);
    }
}
