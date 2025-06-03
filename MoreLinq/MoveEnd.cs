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

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a sequence with a range of elements in the source sequence
        /// moved to a new offset.
        /// </summary>
        /// <typeparam name="T">Type of the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="fromIndex">
        /// The zero-based index identifying the first element in the range of
        /// elements to move.</param>
        /// <param name="count">The count of items to move.</param>
        /// <param name="toIndex">
        /// The index where the specified range will be moved.</param>
        /// <returns>
        /// A sequence with the specified range moved to the new position.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// var result = Enumerable.Range(0, 6).Move(3, 2, 0);
        /// ]]></code>
        /// The <c>result</c> variable will contain <c>{ 3, 4, 0, 1, 2, 5 }</c>.
        /// </example>

        public static IEnumerable<T> MoveEnd<T>(this IEnumerable<T> source, int fromIndex, int count, int toIndex)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (fromIndex < 0) throw new ArgumentOutOfRangeException(nameof(fromIndex), "The source index cannot be negative.");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");
            if (toIndex < 0) throw new ArgumentOutOfRangeException(nameof(toIndex), "Target index of range to move cannot be negative.");

            if (toIndex == fromIndex || count == 0)
                return source;

            return _(); IEnumerable<T> _()
            {
                var queue = new Queue<T>(toIndex);
                var i = -1;
                var endIndex = fromIndex + count - 1;
                var list = new List<T>();

                foreach (var item in source)
                {
                    i++;

                    if (fromIndex <= i && i <= endIndex)
                    {
                        list.Add(item);
                        continue;
                    }

                    if (queue.Count < toIndex)
                    {
                        queue.Enqueue(item);
                        continue;
                    }

                    yield return queue.Dequeue();
                    queue.Enqueue(item);
                }

                foreach (var item in list) yield return item;

                foreach (var item in queue) yield return item;
            }
        }
    }
}
