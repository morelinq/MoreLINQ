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
        /// <summary>
        /// Generates a sequence of lists that represent the permutations of the original sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="sequence">The original sequence to permute.</param>
        /// <returns>
        /// A sequence of lists representing permutations of the original sequence.</returns>
        /// <exception cref="OverflowException">
        /// Too many permutations (limited by <see cref="ulong.MaxValue"/>); thrown during iteration
        /// of the resulting sequence.</exception>
        /// <remarks>
        /// <para>
        /// A permutation is a unique re-ordering of the elements of the sequence.</para>
        /// <para>
        /// This operator returns permutations in a deferred, streaming fashion; however, each
        /// permutation is materialized into a new list. There are N! permutations of a sequence,
        /// where N &#8658; <c>sequence.Count()</c>.</para>
        /// <para>
        /// Be aware that the original sequence is considered one of the permutations and will be
        /// returned as one of the results.</para>
        /// </remarks>

        public static IEnumerable<IList<T>> Permutations<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));

            return _(sequence);

            static IEnumerable<IList<T>> _(IEnumerable<T> sequence)
            {
                // The algorithm used to generate permutations uses the fact that any set can be put
                // into 1-to-1 correspondence with the set of ordinals number (0..n). The
                // implementation here is based on the algorithm described by Kenneth H. Rosen, in
                // Discrete Mathematics and Its Applications, 2nd edition, pp. 282-284.

                var valueSet = sequence.ToArray();

                // There's always at least one permutation: a copy of original set.

                yield return (IList<T>)valueSet.Clone();

                // For empty sets and sets of cardinality 1, there exists only a single permutation.

                if (valueSet.Length is 0 or 1)
                    yield break;

                var permutation = new int[valueSet.Length];

                // Initialize lexographic ordering of the permutation indexes.

                for (var i = 0; i < permutation.Length; i++)
                    permutation[i] = i;

                // For sets larger than 1 element, the number of nested loops needed is one less
                // than the set length. Note that the factorial grows VERY rapidly such that 13!
                // overflows the range of an Int32 and 28! overflows the range of a Decimal.

                ulong factorial = valueSet.Length switch
                {
                     0 =>                         1,
                     1 =>                         1,
                     2 =>                         2,
                     3 =>                         6,
                     4 =>                        24,
                     5 =>                       120,
                     6 =>                       720,
                     7 =>                     5_040,
                     8 =>                    40_320,
                     9 =>                   362_880,
                    10 =>                 3_628_800,
                    11 =>                39_916_800,
                    12 =>               479_001_600,
                    13 =>             6_227_020_800,
                    14 =>            87_178_291_200,
                    15 =>         1_307_674_368_000,
                    16 =>        20_922_789_888_000,
                    17 =>       355_687_428_096_000,
                    18 =>     6_402_373_705_728_000,
                    19 =>   121_645_100_408_832_000,
                    20 => 2_432_902_008_176_640_000,
                    _  => throw new OverflowException("Too many permutations."),
                };

                for (var n = 1UL; n < factorial; n++)
                {
                    // Transposes elements in the cached permutation array to produce the next
                    // permutation.

                    // Find the largest index j with permutation[j] < permutation[j+1]:

                    var j = permutation.Length - 2;
                    while (permutation[j] > permutation[j + 1])
                        j--;

                    // Find index k such that permutation[k] is the smallest integer greater than
                    // permutation[j] to the right of permutation[j]:

                    var k = permutation.Length - 1;
                    while (permutation[j] > permutation[k])
                        k--;

                    (permutation[j], permutation[k]) = (permutation[k], permutation[j]);

                    // Move the tail of the permutation after the j-th position in increasing order.

                    for (int x = permutation.Length - 1, y = j + 1; x > y; x--, y++)
                        (permutation[x], permutation[y]) = (permutation[y], permutation[x]);

                    // Yield a new array containing the values from the original set in their new
                    // permuted order.

                    var permutedSet = new T[permutation.Length];
                    for (var i = 0; i < permutation.Length; i++)
                        permutedSet[i] = valueSet[permutation[i]];

                    yield return permutedSet;
                }
            }
        }
    }
}
