#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2010 Leopold Bushkin. All rights reserved.
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
        // implementation of the Permutations() extension methods.

        /// <summary>
        /// Produces a sequence from an action based on the dynamic generation of N nested loops
        /// whose iteration counts are defined by a sequence of loop counts.
        /// </summary>
        /// <param name="action">Action delegate for which to produce a nested loop sequence</param>
        /// <param name="loopCounts">A sequence of loop repetition counts</param>
        /// <returns>A sequence of Action representing the expansion of a set of nested loops</returns>

        static IEnumerable<Action> NestedLoops(this Action action, IEnumerable<ulong> loopCounts)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (loopCounts == null) throw new ArgumentNullException(nameof(loopCounts));

            return _(); IEnumerable<Action> _()
            {
                var count = loopCounts.DefaultIfEmpty()
                                      .Aggregate((acc, x) => checked(acc * x));

                for (var i = 0UL; i < count; i++)
                    yield return action;
            }
        }
    }
}
