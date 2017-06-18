#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Leandro F. Vieira (leandromoh). All rights reserved.
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

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Generates a sequence of integral numbers within the (inclusive) specified range.
        /// If sequence is ascending the step is +1, otherwise -1.
        /// </summary>
        /// <param name="source">The value of the first integer in the sequence.</param>
        /// <param name="index">The value of the last integer in the sequence.</param>
        /// <param name="count">The value of the last integer in the sequence.</param>
        /// <param name="putAt">The value of the last integer in the sequence.</param>
        /// <returns>An <see cref="IEnumerable{Int32}"/> that contains a range of sequential integral numbers.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// var result = MoreEnumerable.Sequence(6, 0);
        /// </code>
        /// The <c>result</c> variable will contain <c>{ 6, 5, 4, 3, 2, 1, 0 }</c>.
        /// </example>

        public static IEnumerable<T> SwapRange<T>(this IEnumerable<T> source, int index, int count, int putAt)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "Index count cannot be negative.");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count count cannot be negative.");
            if (putAt < 0) throw new ArgumentOutOfRangeException(nameof(putAt), "PutAt count cannot be negative.");

            if (putAt == index)
                return source;

            if (putAt < index)
                return _(putAt, index - putAt, count);
            else
                return _(index, count, putAt - index);

            IEnumerable<T> _(int x, int y, int z)
            {
                using (var ector = source.GetEnumerator())
                {
                    for(var i = 0; i < x && ector.MoveNext(); i++)
                        yield return ector.Current;

                    var array = new T[y];
                    var quantity = 0;

                    for (; quantity < y && ector.MoveNext(); quantity++)
                        array[quantity] = ector.Current;

                    for (var i = 0; i < z && ector.MoveNext(); i++)
                        yield return ector.Current;

                    for (var i = 0; i < quantity; i++)
                        yield return array[i];

                    while (ector.MoveNext())
                        yield return ector.Current;
                }
            }
        }
    }
}
