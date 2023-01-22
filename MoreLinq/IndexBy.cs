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
    using System.Linq;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Applies a key-generating function to each element of a sequence and
        /// returns a sequence that contains the elements of the original
        /// sequence as well its key and index inside the group of its key.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence elements.</typeparam>
        /// <typeparam name="TKey">Type of the projected key.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">
        /// Function that projects the key given an element in the source sequence.</param>
        /// <returns>
        /// A sequence of elements paired with their index within the key-group.
        /// The index is the key and the element is the value of the pair.
        /// </returns>

        public static IEnumerable<KeyValuePair<int, TSource>>
            IndexBy<TSource, TKey>(
                this IEnumerable<TSource> source,
                Func<TSource, TKey> keySelector) =>
            source.IndexBy(keySelector, null);

        /// <summary>
        /// Applies a key-generating function to each element of a sequence and
        /// returns a sequence that contains the elements of the original
        /// sequence as well its key and index inside the group of its key.
        /// An additional parameter specifies a comparer to use for testing the
        /// equivalence of keys.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence elements.</typeparam>
        /// <typeparam name="TKey">Type of the projected key.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">
        /// Function that projects the key given an element in the source sequence.</param>
        /// <param name="comparer">
        /// The equality comparer to use to determine whether or not keys are
        /// equal. If <c>null</c>, the default equality comparer for
        /// <typeparamref name="TSource"/> is used.</param>
        /// <returns>
        /// A sequence of elements paired with their index within the key-group.
        /// The index is the key and the element is the value of the pair.
        /// </returns>

        public static IEnumerable<KeyValuePair<int, TSource>>
            IndexBy<TSource, TKey>(
                this IEnumerable<TSource> source,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey>? comparer) =>
            from e in source.ScanBy(keySelector, _ => (Index: -1, Item: default(TSource)), (s, _, e) => (s.Index + 1, e), comparer)
            select new KeyValuePair<int, TSource>(e.Value.Index, e.Value.Item);
    }
}
