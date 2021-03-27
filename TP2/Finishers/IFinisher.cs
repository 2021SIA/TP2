using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;

namespace TP2.Finishers
{
    public interface IFinisher
    {
        bool IsFinished(ICollection<Character> population, long generations, long time);
    }
}
