#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Atif Aziz. All rights reserved.
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
        /// Provides a countdown counter for a given count of elements at the
        /// tail of the sequence where zero always represents the last element,
        /// one represents the second-last element, two represents the
        /// third-last element and so on.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements of <paramref name="source"/></typeparam>
        /// <typeparam name="TResult">
        /// The type of elements of the resulting sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="count">Count of tail elements of
        /// <paramref name="source"/> to count down.</param>
        /// <param name="resultSelector">
        /// A function that receives the element and the current countdown
        /// value for the element and which returns those mapped to a
        /// result returned in the resulting sequence. For elements before
        /// the last <paramref name="count"/>, the coundown value is
        /// <c>null</c>.</param>
        /// <returns>
        /// A sequence of results returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This method uses deferred execution semantics and streams its
        /// results. At most, <paramref name="count"/> elements of the source
        /// sequence may be buffered at any one time unless
        /// <paramref name="source"/> is a collection or a list.
        /// </remarks>

        public static IEnumerable<TResult> CountDown<T, TResult>(this IEnumerable<T> source,
            int count, Func<T, int?, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            switch (source)
            {
                case IList<T> list:
                    return IterateList(list.AsListLike());
                case IReadOnlyList<T> list:
                    return IterateList(list.AsListLike());
                case ICollection<T> collection:
                    return IterateCollection(collection.Count);
                case IReadOnlyCollection<T> collection:
                    return IterateCollection(collection.Count);
                default:
                    return IterateSequence();
            }

            IEnumerable<TResult> IterateList(IListLike<T> list)
            {
                var countdown = Math.Min(count, list.Count);

                for (var i = 0; i < list.Count; i++)
                {
                    var cd = list.Count - i <= count
                           ? --countdown
                           : (int?) null;
                    yield return resultSelector(list[i], cd);
                }
            }

            IEnumerable<TResult> IterateCollection(int i)
            {
                foreach (var item in source)
                    yield return resultSelector(item, i-- <= count ? i : (int?) null);
            }

            IEnumerable<TResult> IterateSequence()
            {
                var queue = new Queue<T>();

                foreach (var item in source)
                {
                    queue.Enqueue(item);
                    if (queue.Count > count)
                        yield return resultSelector(queue.Dequeue(), null);
                }

                while (queue.Count > 0)
                    yield return resultSelector(queue.Dequeue(), queue.Count);
            }
        }
    }
}
