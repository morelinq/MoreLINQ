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
        /// Returns the sequence of elements in the first sequence which are also in the second sequence.
        /// </summary>
        /// <remarks>
        /// This is not a set operation like <see cref="System.Linq.Enumerable.Intersect{TSource}(
        /// System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEnumerable{TSource})"/>; 
        /// if multiple elements in <paramref name="first"/> are equal, all such elements are returned.
        /// This operator uses deferred execution and streams results from <paramref name="first"/>,
        /// but the entire set of elements from <paramref name="second"/> is cached as soon as execution begins.
        /// Duplicate values in <paramref name="second"/> are not relevant and are discarded when <paramref name="second"/> is cached.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">The sequence of potentially included elements.</param>
        /// <param name="second">The set of elements which may allow elements in <paramref name="first"/> to be returned.</param>
        /// <param name="comparer">The element comparer. If <c>null</c>, uses the default equality comparer for <typeparamref name="TSource"/>.</param>
        /// <returns>The sequence of elements from <paramref name="first"/> that are also in <paramref name="second"/>, 
        /// including duplicate values from <paramref name="first"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="first"/> or <paramref name="second"/> is <c>null</c>. 
        /// If <paramref name="comparer"/> is <c>null</c>, the default equality comparer for <typeparamref name="TSource"/> is used.</exception>
        public static IEnumerable<TSource> IntersectAll<TSource>(this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            return IntersectAllKeysImpl(first, second, x => x, comparer);
        }

        /// <summary>
        /// Returns the sequence of elements in the first sequence which are also
        /// in the second sequence, according to a given key selector.
        /// </summary>
        /// <remarks>
        /// This is not a set operation like <see cref="MoreLinq.MoreEnumerable.IntersectBy{TSource, TKey}(
        /// System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey})"/>;  
        /// if multiple elements in <paramref name="first"/> have equal keys, all such elements are returned.
        /// This operator uses deferred execution and streams results from <paramref name="first"/>,
        /// but the entire set of keys from <paramref name="second"/> is cached as soon as execution begins.
        /// Duplicate keys from <paramref name="second"/> are not relevant and are discarded when <paramref name="second"/> is cached.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/> and used for comparing values.</typeparam>
        /// <param name="first">The sequence of potentially included elements.</param>
        /// <param name="second">The sequence of elements whose keys may allow elements in <paramref name="first"/> to be returned.</param>
        /// <param name="keySelector">The mapping from source element to key.</param>
        /// <returns>The sequence of elements from <paramref name="first"/> whose key was also a key for
        /// any element in <paramref name="second"/>, including values with duplicate keys from <paramref name="first"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="first"/>, <paramref name="second"/>, or 
        /// <paramref name="keySelector"/> is <c>null</c>.</exception>
        public static IEnumerable<TSource> IntersectAllBy<TSource, TKey>(this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            Func<TSource, TKey> keySelector) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return IntersectAllKeysImpl(first, second.Select(keySelector), keySelector, null);
        }

        /// <summary>
        /// Returns the sequence of elements in the first sequence which are also
        /// in the second sequence, according to a given key selector and equality comparer.
        /// </summary>
        /// <remarks>
        /// This is not a set operation like <see cref="MoreLinq.MoreEnumerable.IntersectBy{TSource, TKey}(
        /// System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEnumerable{TSource}, System.Func{TSource, TKey})"/>;  
        /// This operator uses deferred execution and streams results from <paramref name="first"/>,
        /// but the entire set of keys from <paramref name="second"/> is cached as soon as execution begins.
        /// Duplicate keys from <paramref name="second"/> are not relevant and are discarded when <paramref name="second"/> is cached.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements in the input sequences.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/> and used for comparing values.</typeparam>
        /// <param name="first">The sequence of potentially included elements.</param>
        /// <param name="second">The sequence of elements whose keys may allow elements in <paramref name="first"/> to be returned.</param>
        /// <param name="keySelector">The mapping from source element to key.</param>
        /// <param name="keyComparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <c>TSource</c> is used.</param>
        /// <returns>The sequence of elements from <paramref name="first"/> whose key was also a key for
        /// any element in <paramref name="second"/>, including values with duplicate keys from <paramref name="first"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="first"/>, <paramref name="second"/>, or 
        /// <paramref name="keySelector"/> is <c>null</c>.
        /// If <paramref name="keyComparer"/> is <c>null</c>, the default equality comparer for <typeparamref name="TSource"/> is used.</exception>
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
        /// Returns the sequence of elements in the first sequence,
        /// whose keys are in the second sequence, according to a given key selector.
        /// </summary>
        /// <remarks>
        /// This is not a set operation like <see cref="MoreLinq.MoreEnumerable.IntersectKeys{TSource, TKey}(
        /// System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEnumerable{TKey}, System.Func{TSource, TKey})"/>;  
        /// if multiple elements in <paramref name="first"/> have equal keys, all such elements are returned.
        /// This operator uses deferred execution and streams results from <paramref name="first"/>,
        /// but the entire set of keys from <paramref name="second"/> is cached as soon as execution begins.
        /// Duplicate keys from <paramref name="second"/> are not relevant and are discarded when <paramref name="second"/> is cached.
        /// </remarks>
        /// <typeparam name="TSource">The type of source and result elements.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/> and used for comparing values.</typeparam>
        /// <param name="first">The sequence of potentially included elements.</param>
        /// <param name="second">The set of keys which may allow elements in <paramref name="first"/> to be returned.</param>
        /// <param name="keySelector">The mapping from source element to key.</param>
        /// <returns>The sequence of elements from <paramref name="first"/> whose key is in <paramref name="second"/>, 
        /// including values with duplicate keys from <paramref name="first"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="first"/>, <paramref name="second"/>, or 
        /// <paramref name="keySelector"/> is <c>null</c>.</exception>
        public static IEnumerable<TSource> IntersectAllKeys<TSource, TKey>(this IEnumerable<TSource> first,
            IEnumerable<TKey> second,
            Func<TSource, TKey> keySelector) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return IntersectAllKeysImpl(first, second, keySelector, null);
        }
        
        /// <summary>
        /// Returns the sequence of elements from the first collection,
        /// whose keys are in the second collection, according to a given key selector and equality comparer.
        /// </summary>
        /// <remarks>
        /// This is not a set operation like <see cref="MoreLinq.MoreEnumerable.IntersectKeys{TSource, TKey}(
        /// System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEnumerable{TKey}, System.Func{TSource, TKey})"/>;  
        /// if multiple elements in <paramref name="first"/> have equal keys, all such elements are returned.
        /// This operator uses deferred execution and streams results from <paramref name="first"/>,
        /// but the entire set of keys from <paramref name="second"/> is cached as soon as execution begins.
        /// Duplicate keys from <paramref name="second"/> are not relevant and are discarded when <paramref name="second"/> is cached.
        /// </remarks>
        /// <typeparam name="TSource">The type of source and result elements.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/> and used for comparing values.</typeparam>
        /// <param name="first">The sequence of potentially included elements.</param>
        /// <param name="second">The set of keys which may allow elements in <paramref name="first"/> to be returned.</param>
        /// <param name="keySelector">The mapping from source element to key.</param>
        /// <param name="keyComparer">The key comparer. If <c>null</c>, uses the default TKey equality comparer.</param>
        /// <returns>The sequence of elements from <paramref name="first"/> whose key is in <paramref name="second"/>, 
        /// including values with duplicate keys from <paramref name="first"/>.</returns>   
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="first"/>, <paramref name="second"/>, or 
        /// <paramref name="keySelector"/> is <c>null</c>.
        /// If <paramref name="keyComparer"/> is <c>null</c>, the default equality comparer for <typeparamref name="TSource"/> is used.</exception>
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
