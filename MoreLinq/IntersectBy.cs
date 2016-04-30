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

        /// <summary>
        /// Returns the set of distinct elements from the first sequence which are also 
        /// in the second sequence, according to the given key selector.
        /// </summary>
        /// <remarks>
        /// This is a set operation; if multiple elements in <paramref name="first"/> have
        /// equal keys, only the first such element is returned.
        /// This operator uses deferred execution and streams results from <paramref name="first"/>,
        /// but the entire set of keys from <paramref name="second"/> is cached as soon as execution begins.
        /// Duplicate keys from <paramref name="second"/> are not relevant and are discarded when <paramref name="second"/> is cached.
        /// </remarks>
        /// <typeparam name="TSource">The type of the source and result elements.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>, and used for equality comparison.</typeparam>
        /// <param name="first">The set of potentially included elements.</param>
        /// <param name="second">The set of elements whose keys may allow elements in <paramref name="first"/> to be returned.</param>
        /// <param name="keySelector">The mapping from source element to key.</param>
        /// <returns>The set of distinct elements from <paramref name="first"/> whose key is also the key
        /// of an element in <paramref name="second" />, according to the given key selector.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="first"/>, <paramref name="second"/>, or 
        /// <paramref name="keySelector"/> is <c>null</c>.</exception>
        public static IEnumerable<TSource> IntersectBy<TSource, TKey>(this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            Func<TSource, TKey> keySelector) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return IntersectKeysImpl(first, second.Select(keySelector), keySelector, null);
        }

        /// <summary>
        /// Returns the set of distinct elements from the first sequence which are also 
        /// in the second sequence, according to the given key selector and equality comparer.
        /// </summary>
        /// <remarks>
        /// This is a set operation; if multiple elements in <paramref name="first"/> have
        /// equal keys, only the first such element is returned.
        /// This operator uses deferred execution and streams results from <paramref name="first"/>,
        /// but the entire set of keys from <paramref name="second"/> is cached as soon as execution begins.
        /// Duplicate keys from <paramref name="second"/> are not relevant and are discarded when <paramref name="second"/> is cached.
        /// </remarks>
        /// <typeparam name="TSource">The type of the source and result elements.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>, and used for equality comparison.</typeparam>
        /// <param name="first">The set of potentially included elements.</param>
        /// <param name="second">The set of elements whose keys may allow elements in <paramref name="first"/> to be returned.</param>
        /// <param name="keySelector">The mapping from source element to key.</param>
        /// <param name="keyComparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <c>TSource</c> is used.</param>
        /// <returns>The set of distinct elements from <paramref name="first"/> whose key is also the key
        /// of an element in <paramref name="second" />, according to the given key selector.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="first"/>, <paramref name="second"/>, or 
        /// <paramref name="keySelector"/> is <c>null</c>.
        /// If <paramref name="keyComparer"/> is <c>null</c>, the default equality comparer for <typeparamref name="TSource"/> is used.</exception>
        public static IEnumerable<TSource> IntersectBy<TSource, TKey>(this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> keyComparer) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return IntersectKeysImpl(first, second.Select(keySelector), keySelector, keyComparer);
        }

        /// <summary>
        /// Returns the set of distinct elements in the first sequence,
        /// whose keys are in the second sequence, according to a given key selector.
        /// </summary>
        /// <remarks>
        /// This is a set operation; if multiple elements in <paramref name="first"/> have
        /// equal keys, only the first such element is returned.
        /// This operator uses deferred execution and streams results from <paramref name="first"/>,
        /// but the entire set of keys from <paramref name="second"/> is cached as soon as execution begins.
        /// Duplicate keys from <paramref name="second"/> are not relevant and are discarded when <paramref name="second"/> is cached.
        /// </remarks>
        /// <typeparam name="TSource">The type of source and result elements.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>, used for equality comparison.</typeparam>
        /// <param name="first">The set of potentially included elements.</param>
        /// <param name="second">The set of keys which may allow elements in <paramref name="first"/> to be returned.</param>
        /// <param name="keySelector">The mapping from source element to key.</param>
        /// <returns>The set of distinct elements from <paramref name="first"/> whose key is in <paramref name="second"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="first"/>, <paramref name="second"/>, or 
        /// <paramref name="keySelector"/> is <c>null</c>.</exception>
        public static IEnumerable<TSource> IntersectKeys<TSource, TKey>(this IEnumerable<TSource> first,
            IEnumerable<TKey> second,
            Func<TSource, TKey> keySelector) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return IntersectKeysImpl(first, second, keySelector, null);
        }

        /// <summary>
        /// Returns the set of distinct elements in the first sequence,
        /// whose keys are in the second sequence, according to a given key selector and equality comparer.
        /// </summary>
        /// <remarks>
        /// This is a set operation; if multiple elements in <paramref name="first"/> have
        /// equal keys, only the first such element is returned.
        /// This operator uses deferred execution and streams results from <paramref name="first"/>,
        /// but the entire set of keys from <paramref name="second"/> is cached as soon as execution begins.
        /// Duplicate keys from <paramref name="second"/> are not relevant and are discarded when <paramref name="second"/> is cached.
        /// </remarks>
        /// <typeparam name="TSource">The type of source and result elements.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/>, used for equality comparison.</typeparam>
        /// <param name="first">The set of potentially included elements.</param>
        /// <param name="second">The set of keys which may allow elements in <paramref name="first"/> to be returned.</param>
        /// <param name="keySelector">The mapping from source element to key.</param>
        /// <param name="keyComparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <c>TSource</c> is used.</param>
        /// <returns>The set of distinct elements from <paramref name="first"/> whose key is in <paramref name="second"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="first"/>, <paramref name="second"/>, or 
        /// <paramref name="keySelector"/> is <c>null</c>.
        /// If <paramref name="keyComparer"/> is <c>null</c>, the default equality comparer for <typeparamref name="TSource"/> is used.</exception>
        public static IEnumerable<TSource> IntersectKeys<TSource, TKey>(this IEnumerable<TSource> first,
            IEnumerable<TKey> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> keyComparer) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return IntersectKeysImpl(first, second, keySelector, keyComparer);
        }

        private static IEnumerable<TSource> IntersectKeysImpl<TSource, TKey>(IEnumerable<TSource> first,
            IEnumerable<TKey> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> keyComparer) {

            var keys = new HashSet<TKey>(second, keyComparer);
            return first.Where(x => keys.Remove(keySelector(x)));
        }
    }
}
