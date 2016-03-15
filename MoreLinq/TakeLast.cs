#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2009 Atif Aziz. All rights reserved.
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
    using System.Diagnostics;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a specified number of contiguous elements from the end of 
        /// a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to return the last element of.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that contains the specified number of 
        /// elements from the end of the input sequence.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 12, 34, 56, 78 };
        /// IEnumerable&lt;int&gt; result = numbers.TakeLast(2);
        /// </code>
        /// The <c>result</c> variable, when iterated over, will yield 
        /// 56 and 78 in turn.
        /// </example>

        public static IEnumerable<TSource> TakeLast<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null) throw new ArgumentNullException("source");
            return TakeLastImpl(source, count);
        }

        private static IEnumerable<T> TakeLastImpl<T>(IEnumerable<T> source, int count)
        {
            Debug.Assert(source != null);

            if (count <= 0)
                yield break;

            var q = new Queue<T>(count);

            foreach (var item in source)
            {
                if (q.Count == count)
                    q.Dequeue();
                q.Enqueue(item);
            }

            foreach (var item in q)
                yield return item;
       }
    }
}