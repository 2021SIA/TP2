﻿using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;

namespace TP2.Replacements
{
    public interface IReplacement
    {
        IEnumerable<Character> GetReplacement(IEnumerable<Character> population, IEnumerable<Character> children);
    }
}
