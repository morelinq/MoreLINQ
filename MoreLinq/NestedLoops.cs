#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class MoreEnumerable
    {
        // This extension method was developed (primarily) to support the
        // implementation of the Permutations() extension methods. However,
        // it is of sufficient generality and usefulness to be elevated to
        // a public extension method in its own right.

        /// <summary>
        /// Produces a sequence from an action based on the dynamic generation of N nested loops
        /// who iteration counts are defined by <paramref name="loopCounts"/>.
        /// </summary>
        /// <param name="action">Action delegate for which to produce a nested loop sequence</param>
        /// <param name="loopCounts">A sequence of loop repetition counts</param>
        /// <returns>A sequence of Action representing the expansion of a set of nested loops</returns>
       
        public static IEnumerable<Action> NestedLoops(this Action action, IEnumerable<int> loopCounts)
        {
            if (loopCounts == null) throw new ArgumentNullException("loopCounts");

            using (var iter = loopCounts.GetEnumerator())
            {
                var loopCount = NextLoopCount(iter);
                if (loopCount == null)
                    return Enumerable.Empty<Action>(); // null loop
                var loop = Enumerable.Repeat(action, loopCount.Value);
                while ((loopCount = NextLoopCount(iter)) != null)
                    loop = loop.Repeat(loopCount.Value);
                return loop;
            }
        }

        private static int? NextLoopCount(IEnumerator<int> iter)
        {
            if (!iter.MoveNext())
                return null;
            if (iter.Current < 0)
                throw new ArgumentException("All loop counts must be greater than or equal to zero.", "loopCounts");
            return iter.Current;
        }
    }
}