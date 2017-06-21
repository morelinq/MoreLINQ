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
        /// Take a range of the source sequence and moves it to another position. 
        /// </summary>
        /// <typeparam name="T">Type of the source sequence</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="oldIndex">The zero-based and original index where the range to be moved begins.</param>
        /// <param name="count">The count of items to move.</param>
        /// <param name="newIndex">The index where the specified range will be placed.</param>
        /// <returns>The sequence with the specified range moved to the new position.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// var result = Enumerable.Range(0, 6).MoveRange(3, 2, 0);
        /// </code>
        /// The <c>result</c> variable will contain <c>{ 3, 4, 0, 1, 2, 5 }</c>.
        /// </example>

        public static IEnumerable<T> MoveRange<T>(this IEnumerable<T> source, int oldIndex, int count, int newIndex)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (oldIndex < 0) throw new ArgumentOutOfRangeException(nameof(oldIndex), "The source index cannot be negative.");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");
            if (newIndex < 0) throw new ArgumentOutOfRangeException(nameof(newIndex), "Target index of range to move cannot be negative.");

            if (newIndex == oldIndex || count == 0)
                return source;

            if (newIndex < oldIndex)
                return _(newIndex, oldIndex - newIndex, count);
            else
                return _(oldIndex, count, newIndex - oldIndex);

            IEnumerable<T> _(int bufferStartoldIndex, int bufferSize, int bufferYieldoldIndex)
            {
                using (var e = source.GetEnumerator())
                {
                    for (var i = 0; i < bufferStartoldIndex && e.MoveNext(); i++)
                        yield return e.Current;

                    var buffer = new T[bufferSize];
                    var length = 0;

                    for (; length < bufferSize && e.MoveNext(); length++)
                        buffer[length] = e.Current;

                    for (var i = 0; i < bufferYieldoldIndex && e.MoveNext(); i++)
                        yield return e.Current;

                    for (var i = 0; i < length; i++)
                        yield return buffer[i];

                    while (e.MoveNext())
                        yield return e.Current;
                }
            }
        }
    }
}
