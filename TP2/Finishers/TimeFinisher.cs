using System;
using System.Collections.Generic;
using System.Text;
using TP2.Models;

namespace TP2.Finishers
{
    public class TimeFinisher : IFinisher
    {
        public TimeFinisher(long timeLimit)
        {
            TimeLimit = timeLimit;
        }

        public long TimeLimit { get; set; }

        public bool IsFinished(IEnumerable<Character> population, long generations, long time)
        {
            return time >= TimeLimit;
        }
    }
}
