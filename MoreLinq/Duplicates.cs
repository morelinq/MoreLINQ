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

namespace MoreLinq {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    partial class MoreEnumerable {

        /// <summary>Get a sequence of values that occur in the source sequence more than once.</summary>
        /// <typeparam name="T">The type of the sequence.</typeparam>
        /// <param name="sequence">Source sequence.</param>
        /// <returns>Sequence containing duplicate values from the source sequence.</returns> 
        /// <remarks>
        /// This operator uses deferred execution and streams the results.
        /// </remarks>
        public static IEnumerable<T> DuplicateValues<T>(this IEnumerable<T> sequence) {
            if (sequence == null) throw new ArgumentNullException("sequence");

            return DuplicateKeysImpl(sequence, t => t, null);
        }

        /// <summary>Get a sequence of keys that belong to more than one element of the source sequence,
        /// according to the given key selector.</summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="sequence">Source sequence.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns>Sequence of duplicate keys from source sequence.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams the results.
        /// </remarks>
        public static IEnumerable<TKey> DuplicateKeys<TSource, TKey>(this IEnumerable<TSource> sequence,
            Func<TSource, TKey> keySelector) {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return DuplicateKeysImpl(sequence, keySelector, null);
        }

        /// <summary>Get a sequence of keys that belong to more than one element of the source sequence,
        /// according to the given key selector.</summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="sequence">Source sequence.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="keyComparer">The key comparer. If <c>null</c>, uses default TKey equality comparer.</param>
        /// <returns>Sequence of duplicate keys from source sequence.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams the results.
        /// </remarks>
        public static IEnumerable<TKey> DuplicateKeys<TSource, TKey>(this IEnumerable<TSource> sequence,
            Func<TSource, TKey> keySelector, IEqualityComparer<TKey> keyComparer) {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return DuplicateKeysImpl(sequence, keySelector, keyComparer);
        }

        private static IEnumerable<TKey> DuplicateKeysImpl<TSource, TKey>(IEnumerable<TSource> sequence,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> keyComparer) {

            return sequence.GroupBy(keySelector, keyComparer)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);
        }
        
        /// <summary>Get a sequence of elements that occur in this sequence more than once.</summary>
        /// <typeparam name="T">The type of this sequence.</typeparam>
        /// <param name="sequence">This sequence.</param>
        /// <returns>Collection containing duplicate elements from this sequence.</returns>
        public static IEnumerable<T> DuplicateElements<T>(this IEnumerable<T> sequence) {
            if (sequence == null) throw new ArgumentNullException("sequence");

            return DuplicateElementsImpl(sequence, t => t, null);
        }

        /// <summary>Get a sequence of elements that occur in this sequence more than once.</summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="sequence">This sequence.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns>Collection containing duplicate elements from this sequence.</returns>
        public static IEnumerable<TSource> DuplicateElements<TSource, TKey>(this IEnumerable<TSource> sequence,
            Func<TSource, TKey> keySelector) {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return DuplicateElementsImpl(sequence, keySelector, null);
        }

        /// <summary>Get a sequence of elements that belong to more than one element of the source sequence,
        /// according to the given key selector.</summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="sequence">Source sequence.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="keyComparer">The key comparer. If <c>null</c>, uses default TKey equality comparer.</param>
        /// <returns>Sequence of duplicate keys from source sequence.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams the results.
        /// </remarks>
        public static IEnumerable<TSource> DuplicateElements<TSource, TKey>(this IEnumerable<TSource> sequence,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> keyComparer) {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return DuplicateElementsImpl(sequence, keySelector, keyComparer);
        }

        private static IEnumerable<TSource> DuplicateElementsImpl<TSource, TKey>(IEnumerable<TSource> sequence,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> keyComparer) {

            return sequence.ExceptKeys<TSource, TKey>(
                sequence.DuplicateKeys(keySelector, keyComparer),
                keySelector,
                keyComparer);
        }
    }
}
