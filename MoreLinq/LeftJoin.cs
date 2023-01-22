#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Atif Aziz. All seconds reserved.
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
        /// Performs a left outer join on two homogeneous sequences.
        /// Additional arguments specify key selection functions and result
        /// projection functions.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the key selector function.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the result elements.</typeparam>
        /// <param name="first">
        /// The first sequence of the join operation.</param>
        /// <param name="second">
        /// The second sequence of the join operation.</param>
        /// <param name="keySelector">
        /// Function that projects the key given an element of one of the
        /// sequences to join.</param>
        /// <param name="firstSelector">
        /// Function that projects the result given just an element from
        /// <paramref name="first"/> where there is no corresponding element
        /// in <paramref name="second"/>.</param>
        /// <param name="bothSelector">
        /// Function that projects the result given an element from
        /// <paramref name="first"/> and an element from <paramref name="second"/>
        /// that match on a common key.</param>
        /// <returns>A sequence containing results projected from a left
        /// outer join of the two input sequences.</returns>

        public static IEnumerable<TResult> LeftJoin<TSource, TKey, TResult>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            Func<TSource, TResult> firstSelector,
            Func<TSource, TSource, TResult> bothSelector)
        {
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            return first.LeftJoin(second, keySelector,
                                  firstSelector, bothSelector,
                                  null);
        }

        /// <summary>
        /// Performs a left outer join on two homogeneous sequences.
        /// Additional arguments specify key selection functions, result
        /// projection functions and a key comparer.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the key selector function.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the result elements.</typeparam>
        /// <param name="first">
        /// The first sequence of the join operation.</param>
        /// <param name="second">
        /// The second sequence of the join operation.</param>
        /// <param name="keySelector">
        /// Function that projects the key given an element of one of the
        /// sequences to join.</param>
        /// <param name="firstSelector">
        /// Function that projects the result given just an element from
        /// <paramref name="first"/> where there is no corresponding element
        /// in <paramref name="second"/>.</param>
        /// <param name="bothSelector">
        /// Function that projects the result given an element from
        /// <paramref name="first"/> and an element from <paramref name="second"/>
        /// that match on a common key.</param>
        /// <param name="comparer">
        /// The <see cref="IEqualityComparer{T}"/> instance used to compare
        /// keys for equality.</param>
        /// <returns>A sequence containing results projected from a left
        /// outer join of the two input sequences.</returns>

        public static IEnumerable<TResult> LeftJoin<TSource, TKey, TResult>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            Func<TSource, TKey> keySelector,
            Func<TSource, TResult> firstSelector,
            Func<TSource, TSource, TResult> bothSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            return first.LeftJoin(second,
                                  keySelector, keySelector,
                                  firstSelector, bothSelector,
                                  comparer);
        }

        /// <summary>
        /// Performs a left outer join on two heterogeneous sequences.
        /// Additional arguments specify key selection functions and result
        /// projection functions.
        /// </summary>
        /// <typeparam name="TFirst">
        /// The type of elements in the first sequence.</typeparam>
        /// <typeparam name="TSecond">
        /// The type of elements in the second sequence.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the result elements.</typeparam>
        /// <param name="first">
        /// The first sequence of the join operation.</param>
        /// <param name="second">
        /// The second sequence of the join operation.</param>
        /// <param name="firstKeySelector">
        /// Function that projects the key given an element from <paramref name="first"/>.</param>
        /// <param name="secondKeySelector">
        /// Function that projects the key given an element from <paramref name="second"/>.</param>
        /// <param name="firstSelector">
        /// Function that projects the result given just an element from
        /// <paramref name="first"/> where there is no corresponding element
        /// in <paramref name="second"/>.</param>
        /// <param name="bothSelector">
        /// Function that projects the result given an element from
        /// <paramref name="first"/> and an element from <paramref name="second"/>
        /// that match on a common key.</param>
        /// <returns>A sequence containing results projected from a left
        /// outer join of the two input sequences.</returns>

        public static IEnumerable<TResult> LeftJoin<TFirst, TSecond, TKey, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TFirst, TResult> firstSelector,
            Func<TFirst, TSecond, TResult> bothSelector) =>
            first.LeftJoin(second,
                           firstKeySelector, secondKeySelector,
                           firstSelector, bothSelector,
                           null);

        /// <summary>
        /// Performs a left outer join on two heterogeneous sequences.
        /// Additional arguments specify key selection functions, result
        /// projection functions and a key comparer.
        /// </summary>
        /// <typeparam name="TFirst">
        /// The type of elements in the first sequence.</typeparam>
        /// <typeparam name="TSecond">
        /// The type of elements in the second sequence.</typeparam>
        /// <typeparam name="TKey">
        /// The type of the key returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the result elements.</typeparam>
        /// <param name="first">
        /// The first sequence of the join operation.</param>
        /// <param name="second">
        /// The second sequence of the join operation.</param>
        /// <param name="firstKeySelector">
        /// Function that projects the key given an element from <paramref name="first"/>.</param>
        /// <param name="secondKeySelector">
        /// Function that projects the key given an element from <paramref name="second"/>.</param>
        /// <param name="firstSelector">
        /// Function that projects the result given just an element from
        /// <paramref name="first"/> where there is no corresponding element
        /// in <paramref name="second"/>.</param>
        /// <param name="bothSelector">
        /// Function that projects the result given an element from
        /// <paramref name="first"/> and an element from <paramref name="second"/>
        /// that match on a common key.</param>
        /// <param name="comparer">
        /// The <see cref="IEqualityComparer{T}"/> instance used to compare
        /// keys for equality.</param>
        /// <returns>A sequence containing results projected from a left
        /// outer join of the two input sequences.</returns>

        public static IEnumerable<TResult> LeftJoin<TFirst, TSecond, TKey, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TFirst, TResult> firstSelector,
            Func<TFirst, TSecond, TResult> bothSelector,
            IEqualityComparer<TKey>? comparer)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (firstKeySelector == null) throw new ArgumentNullException(nameof(firstKeySelector));
            if (secondKeySelector == null) throw new ArgumentNullException(nameof(secondKeySelector));
            if (firstSelector == null) throw new ArgumentNullException(nameof(firstSelector));
            if (bothSelector == null) throw new ArgumentNullException(nameof(bothSelector));

            return
                from f in first.GroupJoin(second, firstKeySelector, secondKeySelector,
                                          (f, ss) => (Value: f, Seconds: from s in ss select (HasValue: true, Value: s)),
                                          comparer)
                from s in f.Seconds.DefaultIfEmpty()
                select s.HasValue ? bothSelector(f.Value, s.Value) : firstSelector(f.Value);
        }
    }
}
