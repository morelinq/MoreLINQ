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

        public static IEnumerable<T> Move<T>(this IEnumerable<T> source, int fromIndex, int count, int toIndex)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (fromIndex < 0) throw new ArgumentOutOfRangeException(nameof(fromIndex), "The source index cannot be negative.");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");
            if (toIndex < 0) throw new ArgumentOutOfRangeException(nameof(toIndex), "Target index of range to move cannot be negative.");

            if (toIndex == fromIndex || count == 0)
                return source;

            return toIndex < fromIndex
                 ? _(toIndex, fromIndex - toIndex, count)
                 : _(fromIndex, count, toIndex - fromIndex);

            IEnumerable<T> _(int bufferStartIndex, int bufferSize, int bufferYieldIndex)
            {
                var hasMore = true;
                bool MoveNext(IEnumerator<T> e) => hasMore && (hasMore = e.MoveNext());

                using var e = source.GetEnumerator();

                for (var i = 0; i < bufferStartIndex && MoveNext(e); i++)
                    yield return e.Current;

                var buffer = new T[bufferSize];
                var length = 0;

                for (; length < bufferSize && MoveNext(e); length++)
                    buffer[length] = e.Current;

                for (var i = 0; i < bufferYieldIndex && MoveNext(e); i++)
                    yield return e.Current;

                for (var i = 0; i < length; i++)
                    yield return buffer[i];

                while (MoveNext(e))
                    yield return e.Current;
            }
        }
    }
}
