#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Leandro F. Vieira (leandromoh). All rights reserved.
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
        /// Applies a key-generating function to each element of a sequence and returns a sequence that
        /// contains the elements of the original sequence as well its key and index inside the group of its key.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the projected key.</typeparam>
        /// <typeparam name="TResult">Type of the projected element.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Function that transforms each item of source sequence into a key to be compared against the others.</param>
        /// <param name="resultSelector">The projection to each element, its key and index.</param>
        /// <returns>A sequence of unique keys and their number of occurrences in the original sequence.</returns>

        public static IEnumerable<TResult> IndexBy<TSource, TKey, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TKey, int, TResult> resultSelector)
        {
            return IndexBy(source, keySelector, resultSelector, null);
        }

        /// <summary>
        /// Applies a key-generating function to each element of a sequence and returns a sequence that
        /// contains the elements of the original sequence as well its key and index inside the group of its key.
        /// An additional argument specifies a comparer to use for testing equivalence of keys.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the projected key.</typeparam>
        /// <typeparam name="TResult">Type of the projected element.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Function that transforms each item of source sequence into a key to be compared against the others.</param>
        /// <param name="resultSelector">The projection to each element, its key and index.</param>
        /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <typeparamref name="TSource"/> is used.</param>
        /// <returns>A sequence of unique keys and their number of occurrences in the original sequence.</returns>

        public static IEnumerable<TResult> IndexBy<TSource, TKey, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TKey, int, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return CountByImpl(source, keySelector, (e, key, count) => resultSelector(e, key, count - 1), comparer, true);
        }
    }
}
