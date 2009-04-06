using System;
using System.Collections;
using System.Collections.Generic;

namespace MoreLinq.Test
{
    /// <summary>
    /// Enumerable sequence which throws InvalidOperationException as soon as its
    /// enumerator is requested. Used to check lazy evaluation.
    /// </summary>
    internal sealed class BreakingSequence<T> : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator()
        {
            throw new InvalidOperationException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
