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

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Merges two or more sequences that are in a common order (either ascending or descending) into
        /// a single sequence that preserves that order.
        /// </summary>
        /// <remarks>
        /// Using SortedMerge on sequences that are not ordered or are not in the same order produces
        /// undefined results.<br/>
        /// <c>SortedMerge</c> uses performs the merge in a deferred, streaming manner. <br/>
        /// 
        /// Here is an example of a merge, as well as the produced result:
        /// <code>
        ///   var s1 = new[] { 3, 7, 11 };
        ///   var s2 = new[] { 2, 4, 20 };
        ///   var s3 = new[] { 17, 19, 25 };
        ///   var merged = s1.SortedMerge( OrderByDirection.Ascending, s2, s3 );
        ///   var result = merged.ToArray();
        ///   // result will be:
        ///   // { 2, 3, 4, 7, 11, 17, 19, 20, 25 }
        /// </code>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of the sequence</typeparam>
        /// <param name="source">The primary sequence with which to merge</param>
        /// <param name="direction">The ordering that all sequences must already exhibit</param>
        /// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
        /// <returns>A merged, order-preserving sequence containing all of the elements of the original sequences</returns>

        public static IEnumerable<TSource> SortedMerge<TSource>(this IEnumerable<TSource> source, OrderByDirection direction, params IEnumerable<TSource>[] otherSequences)
        {
            return SortedMerge(source, direction, null, otherSequences);
        }

        /// <summary>
        /// Merges two or more sequences that are in a common order (either ascending or descending) into
        /// a single sequence that preserves that order.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the sequence</typeparam>
        /// <param name="source">The primary sequence with which to merge</param>
        /// <param name="direction">The ordering that all sequences must already exhibit</param>
        /// <param name="comparer">The comparer used to evaluate the relative order between elements</param>
        /// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
        /// <returns>A merged, order-preserving sequence containing al of the elements of the original sequences</returns>

        public static IEnumerable<TSource> SortedMerge<TSource>(this IEnumerable<TSource> source, OrderByDirection direction, IComparer<TSource> comparer, params IEnumerable<TSource>[] otherSequences)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (otherSequences == null) throw new ArgumentNullException("otherSequences");

            if (otherSequences.Length == 0)
                return source; // optimization for when otherSequences is empty

            comparer = comparer ?? Comparer<TSource>.Default;

            // define an precedence function based on the comparer and direction
            // this is a function that will return True if (b) should precede (a)
            var precedenceFunc =
                direction == OrderByDirection.Ascending
                    ? (Func<TSource, TSource, bool>)((a, b) => comparer.Compare(b, a) < 0)
                    : (a, b) => comparer.Compare(b, a) > 0;

            // return the sorted merge result
            return SortedMergeImpl(precedenceFunc, new[] { source }.Concat(otherSequences));
        }

        /// <summary>
        /// Private implementation method that performs a merge of multiple, ordered sequences using
        /// a precedence function which encodes order-sensitive comparison logic based on the caller's arguments.
        /// </summary>
        /// <remarks>
        /// The algorithm employed in this implementation is not necessarily the most optimal way to merge
        /// two sequences. A swap-compare version would probably be somewhat more efficient - but at the
        /// expense of considerably more complexity. One possible optimization would be to detect that only
        /// a single sequence remains (all other being consumed) and break out of the main while-loop and
        /// simply yield the items that are part of the final sequence.
        /// 
        /// The algorithm used here will perform N*(K1+K2+...Kn-1) comparisons, where <c>N => otherSequences.Count()+1.</c>
        /// </remarks>

        private static IEnumerable<T> SortedMergeImpl<T>(Func<T, T, bool> precedenceFunc, IEnumerable<IEnumerable<T>> otherSequences)
        {
            using (var disposables = new DisposableGroup<T>(otherSequences.Select(e => e.GetEnumerator()).Acquire()))
            {
                var iterators = disposables.Iterators;

                // prime all of the iterators by advancing them to their first element (if any)
                // NOTE: We start with the last index to simplify the removal of an iterator if
                //       it happens to be terminal (no items) before we start merging
                for (var i = iterators.Count - 1; i >= 0; i--)
                {
                    if (!iterators[i].MoveNext())
                        disposables.Exclude(i);
                }

                // while all iterators have not yet been consumed...
                while (iterators.Count > 0)
                {
                    var nextIndex = 0;
                    var nextValue = disposables[0].Current;

                    // find the next least element to return
                    for (var i = 1; i < iterators.Count; i++)
                    {
                        var anotherElement = disposables[i].Current;
                        // determine which element follows based on ordering function
                        if (precedenceFunc(nextValue, anotherElement))
                        {
                            nextIndex = i;
                            nextValue = anotherElement;
                        }
                    }

                    yield return nextValue; // next value in precedence order

                    // advance iterator that yielded element, excluding it when consumed
                    if (!iterators[nextIndex].MoveNext())
                        disposables.Exclude(nextIndex);
                }
            }
        }


        /// <summary>
        /// Merges sequences that are in a common order (either ascending or descending by TKey) into
        /// a single sequence that preserves that order. Part of SortMerge algorithm.
        /// </summary>
        /// <param name="source">Sequence of sequences</param>
        /// <param name="keySelector">Function to extract a key given an element from a sequence.</param>
        /// <param name="direction">The ordering that all sequences must already exhibit.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
        /// <typeparam name="T">Type of elements.</typeparam>
        /// <typeparam name="TKey">Type of keys used for merging.</typeparam>
        /// <returns>A merged, order-preserving sequence containing all of the elements of the original sequences merged according to a key.</returns>
        /// <remarks>
        /// Using SortedMerge on sequences that are not ordered or are not in the same order produces
        /// undefined results.<br/>
        /// <c>SortedMerge</c> performs the merge in a deferred, streaming manner.<br/>
        /// 
        /// Each item from each sequence is copied into result sequence.
        /// {1, 2} + {2, 3} = {1, 2, 2, 3}
        /// </remarks>
        public static IEnumerable<T> SortedMerge<T, TKey>(this IEnumerable<IEnumerable<T>> source, Func<T, TKey> keySelector, OrderByDirection direction, IComparer<TKey> comparer)
        {
            List<IEnumerator<T>> enumerators = source
                .Select(v =>
                {
                    IEnumerator<T> enumerator = v.GetEnumerator();
                    if (enumerator.MoveNext())
                    {
                        return enumerator;
                    }
                    else
                    {
                        enumerator.Dispose();
                        return null;
                    }
                })
                .Where(v => v != null)
                .ToList();

            if (enumerators.Any())
            {
                try
                {
                    List<SortedMergeHelperItem<T, TKey>> values = enumerators
                        .Select((v, i) =>
                        {
                            T value = v.Current;
                            TKey key = keySelector(value);
                            return new SortedMergeHelperItem<T, TKey>(i, key, value);
                        })
                        .OrderBy(v => v.Key, comparer, direction)
                        .ToList();

                    int count = enumerators.Count;
                    while (count > 0)
                    {
                        var first = values.First();
                        yield return first.Value;

                        values.RemoveAt(0);

                        int enumeratorId = first.Index;
                        IEnumerator<T> enumerator = enumerators[enumeratorId];
                        bool hasValue = enumerator.MoveNext();
                        if (!hasValue)
                        {
                            --count;
                            enumerators[enumeratorId] = null;
                            enumerator.Dispose();
                        }
                        else
                        {
                            T value = enumerator.Current;
                            TKey key = keySelector(value);
                            values.AddSorted(new SortedMergeHelperItem<T, TKey>(enumeratorId, key, value), v => v.Key, comparer, direction);
                        }
                    }
                }
                finally
                {
                    enumerators
                        .Where(v => v != null)
                        .ForEach(v => v.Dispose());
                }
            }
        }


        private struct SortedMergeHelperItem<T, TKey>
        {
            public int Index { get; }
            public TKey Key { get; }
            public T Value { get; }

            public SortedMergeHelperItem(int index, TKey key, T value)
            {
                Index = index;
                Key = key;
                Value = value;
            }
        }


        /// <summary>
        /// Class used to assist in ensuring that groups of disposable iterators
        /// are disposed - either when Excluded or when the DisposableGroup is disposed.
        /// </summary>

        private sealed class DisposableGroup<T> : IDisposable
        {
            public DisposableGroup(IEnumerable<IEnumerator<T>> iterators)
            {
                Iterators = new List<IEnumerator<T>>(iterators);
            }

            public List<IEnumerator<T>> Iterators { get; private set; }

            public IEnumerator<T> this[int index] { get { return Iterators[index]; } }

            public void Exclude(int index)
            {
                Iterators[index].Dispose();
                Iterators.RemoveAt(index);
            }

            public void Dispose()
            {
                Iterators.ForEach(iter => iter.Dispose());
            }
        }
    }
}
