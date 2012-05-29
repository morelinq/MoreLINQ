using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreLinq
{
    public static partial class MoreEnumerable
    {
        // This extension methods were developed (primarily) to support the
        // implementation of the Permutations() extension methods. However,
        // it is of sufficient generality and usefulness to be elevated to
        // public extension methods in its own right.

        #region Nested Loop Generators
        /// <summary>
        /// Produces a sequence from an action based on the dynamic generation of N nested loops
        /// who iteration counts are defined by <paramref name="loopCounts"/>.
        /// </summary>
        /// <param name="action">Action delegate for which to produce a nested loop sequence</param>
        /// <param name="loopCounts">A sequence of loop repetition counts</param>
        /// <returns>A sequence of Action representing the expansion of a set of nested loops</returns>
        public static IEnumerable<Action> NestedLoops(this Action action, IEnumerable<int> loopCounts)
        {
            loopCounts.ThrowIfNull("loopCounts");
            if( loopCounts.Any( lc => lc < 0 ) )
                throw new ArgumentException("All loop counts must be >= 0", "loopCounts");
            
            return NestedLoopsImpl(action, loopCounts);
        }
        
        private static IEnumerable<Action> NestedLoopsImpl(Action action, IEnumerable<int> loopCounts)
        {
            using (var iter = loopCounts.GetEnumerator())
            {
                if (!iter.MoveNext())
                    return Enumerable.Repeat(action, 0); // null loop

                var loop = Enumerable.Repeat(action, iter.Current);
                while (iter.MoveNext())
                    loop = loop.Repeat(iter.Current);
                return loop;
            }
        }
        #endregion
    }
}