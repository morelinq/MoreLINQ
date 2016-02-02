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

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.
        /// </summary>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3, 4 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.EquiZip(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", "4D" in turn.
        /// </example>
        /// <typeparam name="TFirst">Type of elements in first sequence</typeparam>
        /// <typeparam name="TSecond">Type of elements in second sequence</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence</typeparam>
        /// <param name="first">First sequence</param>
        /// <param name="second">Second sequence</param>
        /// <param name="resultSelector">Function to apply to each pair of elements</param>
        /// <returns>
        /// A sequence that contains elements of the two input sequences,
        /// combined by <paramref name="resultSelector"/>.
        /// </returns>
        /// <remarks>
        /// If the two input sequences are of different lengths then
        /// <see cref="InvalidOperationException"/> is thrown.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult> EquiZip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first,
             IEnumerable<TSecond> second,
             Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            return EquiZipImpl(first, second, resultSelector);
        }

        static IEnumerable<TResult> EquiZipImpl<TFirst, TSecond, TResult>(
            IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            using (var e1 = first.GetEnumerator())
            using (var e2 = second.GetEnumerator())
            {
                while (e1.MoveNext())
                {
                    if (e2.MoveNext())
                        yield return resultSelector(e1.Current, e2.Current);
                    else
                        throw new InvalidOperationException("Second sequence ran out before first");
                }
                if (e2.MoveNext())
                    throw new InvalidOperationException("First sequence ran out before second");
            }
        }
    }
}
