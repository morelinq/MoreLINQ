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
        /// Returns the sequence of elements in the first sequence which are also
        /// in the second sequence.
        /// </summary>
        /// <remarks>
        /// This is a sequence operation; if multiple elements in <paramref name="first"/> are equal, 
        /// all such elements are returned.
        /// This operator uses deferred execution and streams the results, although
        /// a set of elements from <paramref name="second"/> is immediately selected and retained.
        /// </remarks>
        /// <typeparam name="T">The type of the elements in the input sequences.</typeparam>
        /// <param name="first">The sequence of potentially included elements.</param>
        /// <param name="second">The sequence of elements which may allow elements in
        /// <paramref name="first"/> to be returned.</param>
        /// <param name="comparer">The element comparer. If <c>null</c>, uses the default T equality comparer.</param>
        /// <returns>A sequence of elements from <paramref name="first"/> that are also in <paramref name="second"/>.</returns>
        public static IEnumerable<T> IntersectAll<T>(this IEnumerable<T> first,
            IEnumerable<T> second,
            IEqualityComparer<T> comparer) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            return IntersectAllKeysImpl(first, second, x => x, comparer);
        }
      
        /// <summary>
        /// Returns the sequence of elements from the first sequence
        /// which are also in the second sequence, according to the given key selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the source and result elements.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="first">The sequence of potentially included elements.</param>
        /// <param name="second">The sequence of elements whose keys may allow elements in
        /// <paramref name="first"/> to be returned.</param>
        /// <param name="keySelector">The mapping from source element to key.</param>
        /// <returns>Sequence of elements from first sequence
        /// which are also in second sequence, according to the given key selector.</returns>
        /// <remarks>
        /// This is a sequence operation; if multiple elements in <paramref name="first"/> have
        /// equal keys, all such elements are returned.
        /// This operator uses deferred execution and streams the results, although
        /// a set of keys from <paramref name="second"/> is immediately selected and retained.
        /// </remarks>
        public static IEnumerable<TSource> IntersectAllBy<TSource, TKey>(this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            Func<TSource, TKey> keySelector) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return IntersectAllKeysImpl(first, second.Select(keySelector), keySelector, null);
        }

        /// <summary>
        /// Returns the sequence of elements from the first sequence
        /// which are in the second sequence, according to the given key selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the source and result elements.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="first">The sequence of potentially included elements.</param>
        /// <param name="second">The sequence of elements whose keys may allow elements in
        /// <paramref name="first"/> to be returned.</param>
        /// <param name="keySelector">The mapping from source element to key.</param>
        /// <param name="keyComparer">The key comparer. If <c>null</c>, uses the default TKey equality comparer.</param>
        /// <returns>Sequence of elements from first sequence
        /// which are in second sequence, according to the given key selector.</returns>
        /// <remarks>This is a sequence operation; if multiple elements in <paramref name="first"/> have
        /// equal keys, all such elements are returned.
        /// This operator uses deferred execution and streams the results, although
        /// a set of keys from <paramref name="second"/> is immediately selected and retained.
        /// </remarks>
        public static IEnumerable<TSource> IntersectAllBy<TSource, TKey>(this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> keyComparer) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return IntersectAllKeysImpl(first, second.Select(keySelector), keySelector, keyComparer);
        }

        /// <summary>
        /// Returns the sequence of elements from the first sequence
        /// whose keys are in the second sequence, according to the given key selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the source and result elements.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="first">The sequence of potentially included elements.</param>
        /// <param name="second">The sequence of keys which may allow elements in
        /// <paramref name="first"/> to be returned.</param>
        /// <param name="keySelector">The mapping from source element to key.</param>
        /// <returns>Sequence of elements from first sequence
        /// whose keys are in second sequence, according to the given key selector.</returns>
        /// <remarks>
        /// This is a sequence operation; if multiple elements in <paramref name="first"/> have
        /// equal keys, all such elements are returned.
        /// This operator uses deferred execution and streams the results, although
        /// a set of keys from <paramref name="second"/> is immediately selected and retained.
        /// </remarks>
        public static IEnumerable<TSource> IntersectAllKeys<TSource, TKey>(this IEnumerable<TSource> first,
            IEnumerable<TKey> second,
            Func<TSource, TKey> keySelector) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return IntersectAllKeysImpl(first, second, keySelector, null);
        }

        /// <summary>
        /// Returns the sequence of elements from the first sequence
        /// whose keys are in the second sequence, according to the given key selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the source and result elements.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="first">The sequence of potentially included elements.</param>
        /// <param name="second">The sequence of keys which may allow elements in
        /// <paramref name="first"/> to be returned.</param>
        /// <param name="keySelector">The mapping from source element to key.</param>
        /// <param name="keyComparer">The key comparer. If <c>null</c>, uses the default TKey equality comparer.</param>
        /// <returns>Sequence of elements from first sequence
        /// whose keys are in second sequence, according to the given key selector.</returns>
        /// <remarks>This is a sequence operation; if multiple elements in <paramref name="first"/> have
        /// equal keys, all such elements are returned.
        /// This operator uses deferred execution and streams the results, although
        /// a set of keys from <paramref name="second"/> is immediately selected and retained.
        /// </remarks>
        public static IEnumerable<TSource> IntersectAllKeys<TSource, TKey>(this IEnumerable<TSource> first,
            IEnumerable<TKey> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> keyComparer) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return IntersectAllKeysImpl(first, second, keySelector, keyComparer);
        }

        private static IEnumerable<TSource> IntersectAllKeysImpl<TSource, TKey>(IEnumerable<TSource> first,
            IEnumerable<TKey> second,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> keyComparer) {

            var keys = new HashSet<TKey>(second, keyComparer);
            foreach (TSource element in first) {
                var k = keySelector(element);
                if (keys.Contains(k))
                    yield return element;
            }
        }
    }
}
