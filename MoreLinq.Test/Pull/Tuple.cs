using System;
using System.Collections.Generic;

namespace MoreLinq.Test.Pull
{
    internal sealed class Tuple<TFirst, TSecond>
    {
        public Tuple(TFirst first, TSecond second) { First = first; Second = second; }

        public TFirst First { get; private set; }
        public TSecond Second { get; private set; }

        public override bool Equals(object value)
        {
            return value != null && Equals(value as Tuple<TFirst, TSecond>);
        }

        private bool Equals(Tuple<TFirst, TSecond> type)
        {
            return type != null
                   && EqualityComparer<TFirst>.Default.Equals(type.First, First) 
                   && EqualityComparer<TSecond>.Default.Equals(type.Second, Second);
        }

        public override int GetHashCode() { throw new NotImplementedException(); }
        
        public override string ToString()
        {
            return "{ First = " + First + ", Second = " + Second + " }";
        }
    }
}