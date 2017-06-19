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
        /// Take a range of the source sequence and put it at other index. 
        /// </summary>
        /// <typeparam name="T">Type of the source sequence</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="index">The zero-based index where the range to be swapped begins.</param>
        /// <param name="count">The count of items to swap.</param>
        /// <param name="putAt">The index where the specified range will be placed.</param>
        /// <returns>The sequence with the specified range swapped to the specified position.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// var result = Enumerable.Range(0, 6).SwapRange(3, 2, 0);
        /// </code>
        /// The <c>result</c> variable will contain <c>{ 3, 4, 0, 1, 2, 5 }</c>.
        /// </example>

        public static IEnumerable<T> SwapRange<T>(this IEnumerable<T> source, int index, int count, int putAt)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative.");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");
            if (putAt < 0) throw new ArgumentOutOfRangeException(nameof(putAt), "PutAt cannot be negative.");

            if (putAt == index || count == 0)
                return source;

            if (putAt < index)
                return _(putAt, index - putAt, count);
            else
                return _(index, count, putAt - index);

            IEnumerable<T> _(int startBuffer, int bufferSize, int beforeYieldBuffer)
            {
                using (var ector = source.GetEnumerator())
                {
                    for (var i = 0; i < startBuffer && ector.MoveNext(); i++)
                        yield return ector.Current;

                    var buffer = new T[bufferSize];
                    var _count = 0;

                    for (; _count < bufferSize && ector.MoveNext(); _count++)
                        buffer[_count] = ector.Current;

                    for (var i = 0; i < beforeYieldBuffer && ector.MoveNext(); i++)
                        yield return ector.Current;

                    for (var i = 0; i < _count; i++)
                        yield return buffer[i];

                    while (ector.MoveNext())
                        yield return ector.Current;
                }
            }
        }
    }
}
