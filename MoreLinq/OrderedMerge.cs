#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2013 Atif Aziz. All rights reserved.
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
        /// Merges two ordered sequences into one. Where the elements equal
        /// in both sequences, the element from the first sequence is
        /// returned in the resulting sequence.
        /// </summary>
        /// <typeparam name="T">Type of elements in input and output sequences.</typeparam>
        /// <param name="first">The first input sequence.</param>
        /// <param name="second">The second input sequence.</param>
        /// <returns>
        /// A sequence with elements from the two input sequences merged, as
        /// in a full outer join.</returns>
        /// <remarks>
        /// This method uses deferred execution. The behavior is undefined
        /// if the sequences are unordered as inputs.
        /// </remarks>


        public static IEnumerable<T> OrderedMerge<T>(
            this IEnumerable<T> first,
            IEnumerable<T> second)
        {
            return OrderedMerge(first, second, null);
        }

        /// <summary>
        /// Merges two ordered sequences into one with an additional
        /// parameter specifying how to compare the elements of the
        /// sequences. Where the elements equal in both sequences, the
        /// element from the first sequence is returned in the resulting
        /// sequence.
        /// </summary>
        /// <typeparam name="T">Type of elements in input and output sequences.</typeparam>
        /// <param name="first">The first input sequence.</param>
        /// <param name="second">The second input sequence.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare elements.</param>
        /// <returns>
        /// A sequence with elements from the two input sequences merged, as
        /// in a full outer join.</returns>
        /// <remarks>
        /// This method uses deferred execution. The behavior is undefined
        /// if the sequences are unordered as inputs.
        /// </remarks>

        public static IEnumerable<T> OrderedMerge<T>(
            this IEnumerable<T> first,
            IEnumerable<T> second,
            IComparer<T>? comparer)
        {
            return OrderedMerge(first, second, IdFn, IdFn, IdFn, (a, _) => a, comparer);
        }

        /// <summary>
        /// Merges two ordered sequences into one with an additional
        /// parameter specifying the element key by which the sequences are
        /// ordered. Where the keys equal in both sequences, the
        /// element from the first sequence is returned in the resulting
        /// sequence.
        /// </summary>
        /// <typeparam name="T">Type of elements in input and output sequences.</typeparam>
        /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
        /// <param name="first">The first input sequence.</param>
        /// <param name="second">The second input sequence.</param>
        /// <param name="keySelector">Function to extract a key given an element.</param>
        /// <returns>
        /// A sequence with elements from the two input sequences merged
        /// according to a key, as in a full outer join.</returns>
        /// <remarks>
        /// This method uses deferred execution. The behavior is undefined
        /// if the sequences are unordered (by key) as inputs.
        /// </remarks>

        public static IEnumerable<T> OrderedMerge<T, TKey>(
            this IEnumerable<T> first,
            IEnumerable<T> second,
            Func<T, TKey> keySelector)
        {
            return OrderedMerge(first, second, keySelector, IdFn, IdFn, (a, _) => a, null);
        }

        /// <summary>
        /// Merges two ordered sequences into one. Additional parameters
        /// specify the element key by which the sequences are ordered,
        /// the result when element is found in first sequence but not in
        /// the second, the result when element is found in second sequence
        /// but not in the first and the result when elements are found in
        /// both sequences.
        /// </summary>
        /// <typeparam name="T">Type of elements in source sequences.</typeparam>
        /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
        /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
        /// <param name="first">The first input sequence.</param>
        /// <param name="second">The second input sequence.</param>
        /// <param name="keySelector">Function to extract a key given an element.</param>
        /// <param name="firstSelector">Function to project the result element
        /// when only the first sequence yields a source element.</param>
        /// <param name="secondSelector">Function to project the result element
        /// when only the second sequence yields a source element.</param>
        /// <param name="bothSelector">Function to project the result element
        /// when only both sequences yield a source element whose keys are
        /// equal.</param>
        /// <returns>
        /// A sequence with projections from the two input sequences merged
        /// according to a key, as in a full outer join.</returns>
        /// <remarks>
        /// This method uses deferred execution. The behavior is undefined
        /// if the sequences are unordered (by key) as inputs.
        /// </remarks>

        public static IEnumerable<TResult> OrderedMerge<T, TKey, TResult>(
            this IEnumerable<T> first,
            IEnumerable<T> second,
            Func<T, TKey> keySelector,
            Func<T, TResult> firstSelector,
            Func<T, TResult> secondSelector,
            Func<T, T, TResult> bothSelector)
        {
            return OrderedMerge(first, second, keySelector, firstSelector, secondSelector, bothSelector, null);
        }

        /// <summary>
        /// Merges two ordered sequences into one. Additional parameters
        /// specify the element key by which the sequences are ordered,
        /// the result when element is found in first sequence but not in
        /// the second, the result when element is found in second sequence
        /// but not in the first, the result when elements are found in
        /// both sequences and a method for comparing keys.
        /// </summary>
        /// <typeparam name="T">Type of elements in source sequences.</typeparam>
        /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
        /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
        /// <param name="first">The first input sequence.</param>
        /// <param name="second">The second input sequence.</param>
        /// <param name="keySelector">Function to extract a key given an element.</param>
        /// <param name="firstSelector">Function to project the result element
        /// when only the first sequence yields a source element.</param>
        /// <param name="secondSelector">Function to project the result element
        /// when only the second sequence yields a source element.</param>
        /// <param name="bothSelector">Function to project the result element
        /// when only both sequences yield a source element whose keys are
        /// equal.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
        /// <returns>
        /// A sequence with projections from the two input sequences merged
        /// according to a key, as in a full outer join.</returns>
        /// <remarks>
        /// This method uses deferred execution. The behavior is undefined
        /// if the sequences are unordered (by key) as inputs.
        /// </remarks>

        public static IEnumerable<TResult> OrderedMerge<T, TKey, TResult>(
            this IEnumerable<T> first,
            IEnumerable<T> second,
            Func<T, TKey> keySelector,
            Func<T, TResult> firstSelector,
            Func<T, TResult> secondSelector,
            Func<T, T, TResult> bothSelector,
            IComparer<TKey>? comparer)
        {
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector)); // Argument name changes to 'firstKeySelector'
            return OrderedMerge(first, second, keySelector, keySelector, firstSelector, secondSelector, bothSelector, comparer);
        }

        /// <summary>
        /// Merges two heterogeneous sequences ordered by a common key type
        /// into a homogeneous one. Additional parameters specify the
        /// element key by which the sequences are ordered, the result when
        /// element is found in first sequence but not in the second and
        /// the result when element is found in second sequence but not in
        /// the first, the result when elements are found in both sequences.
        /// </summary>
        /// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
        /// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
        /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
        /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
        /// <param name="first">The first input sequence.</param>
        /// <param name="second">The second input sequence.</param>
        /// <param name="firstKeySelector">Function to extract a key given an
        /// element from the first sequence.</param>
        /// <param name="secondKeySelector">Function to extract a key given an
        /// element from the second sequence.</param>
        /// <param name="firstSelector">Function to project the result element
        /// when only the first sequence yields a source element.</param>
        /// <param name="secondSelector">Function to project the result element
        /// when only the second sequence yields a source element.</param>
        /// <param name="bothSelector">Function to project the result element
        /// when only both sequences yield a source element whose keys are
        /// equal.</param>
        /// <returns>
        /// A sequence with projections from the two input sequences merged
        /// according to a key, as in a full outer join.</returns>
        /// <remarks>
        /// This method uses deferred execution. The behavior is undefined
        /// if the sequences are unordered (by key) as inputs.
        /// </remarks>

        public static IEnumerable<TResult> OrderedMerge<TFirst, TSecond, TKey, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TFirst, TResult> firstSelector,
            Func<TSecond, TResult> secondSelector,
            Func<TFirst, TSecond, TResult> bothSelector)
        {
            return OrderedMerge(first, second, firstKeySelector, secondKeySelector, firstSelector, secondSelector, bothSelector, null);
        }

        /// <summary>
        /// Merges two heterogeneous sequences ordered by a common key type
        /// into a homogeneous one. Additional parameters specify the
        /// element key by which the sequences are ordered, the result when
        /// element is found in first sequence but not in the second,
        /// the result when element is found in second sequence but not in
        /// the first, the result when elements are found in both sequences
        /// and a method for comparing keys.
        /// </summary>
        /// <typeparam name="TFirst">Type of elements in the first sequence.</typeparam>
        /// <typeparam name="TSecond">Type of elements in the second sequence.</typeparam>
        /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
        /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
        /// <param name="first">The first input sequence.</param>
        /// <param name="second">The second input sequence.</param>
        /// <param name="firstKeySelector">Function to extract a key given an
        /// element from the first sequence.</param>
        /// <param name="secondKeySelector">Function to extract a key given an
        /// element from the second sequence.</param>
        /// <param name="firstSelector">Function to project the result element
        /// when only the first sequence yields a source element.</param>
        /// <param name="secondSelector">Function to project the result element
        /// when only the second sequence yields a source element.</param>
        /// <param name="bothSelector">Function to project the result element
        /// when only both sequences yield a source element whose keys are
        /// equal.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
        /// <returns>
        /// A sequence with projections from the two input sequences merged
        /// according to a key, as in a full outer join.</returns>
        /// <remarks>
        /// This method uses deferred execution. The behavior is undefined
        /// if the sequences are unordered (by key) as inputs.
        /// </remarks>

        public static IEnumerable<TResult> OrderedMerge<TFirst, TSecond, TKey, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TKey> firstKeySelector,
            Func<TSecond, TKey> secondKeySelector,
            Func<TFirst, TResult> firstSelector,
            Func<TSecond, TResult> secondSelector,
            Func<TFirst, TSecond, TResult> bothSelector,
            IComparer<TKey>? comparer)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (firstKeySelector == null) throw new ArgumentNullException(nameof(firstKeySelector));
            if (secondKeySelector == null) throw new ArgumentNullException(nameof(secondKeySelector));
            if (firstSelector == null) throw new ArgumentNullException(nameof(firstSelector));
            if (bothSelector == null) throw new ArgumentNullException(nameof(bothSelector));
            if (secondSelector == null) throw new ArgumentNullException(nameof(secondSelector));

            return _(comparer ?? Comparer<TKey>.Default);

            IEnumerable<TResult> _(IComparer<TKey> comparer)
            {
                using var e1 = first.GetEnumerator();
                using var e2 = second.GetEnumerator();

                var gotFirst = e1.MoveNext();
                var gotSecond = e2.MoveNext();

                while (gotFirst || gotSecond)
                {
                    if (gotFirst && gotSecond)
                    {
                        var element1 = e1.Current;
                        var key1 = firstKeySelector(element1);
                        var element2 = e2.Current;
                        var key2 = secondKeySelector(element2);
                        switch (comparer.Compare(key1, key2))
                        {
                            case < 0:
                                yield return firstSelector(element1);
                                gotFirst = e1.MoveNext();
                                break;
                            case > 0:
                                yield return secondSelector(element2);
                                gotSecond = e2.MoveNext();
                                break;
                            default:
                                yield return bothSelector(element1, element2);
                                gotFirst = e1.MoveNext();
                                gotSecond = e2.MoveNext();
                                break;
                        }
                    }
                    else if (gotSecond)
                    {
                        yield return secondSelector(e2.Current);
                        gotSecond = e2.MoveNext();
                    }
                    else // (gotFirst)
                    {
                        yield return firstSelector(e1.Current);
                        gotFirst = e1.MoveNext();
                    }
                }
            }
        }
    }
}
