#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Atif Aziz. All rights reserved.
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
        /// Combines <see cref="Enumerable.OrderBy{TSource,TKey}(IEnumerable{TSource},Func{TSource,TKey})"/>,
        /// where each element is its key, and <see cref="Enumerable.Take{TSource}(IEnumerable{TSource},int)"/>
        /// in a single operation.
        /// </summary>
        /// <typeparam name="T">Type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="count">Number of (maximum) elements to return.</param>
        /// <returns>A sequence containing at most top <paramref name="count"/>
        /// elements from source, in their ascending order.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams it results.
        /// </remarks>

        public static IEnumerable<T> PartialSort<T>(this IEnumerable<T> source, int count)
        {
            return source.PartialSort(count, null);
        }

        /// <summary>
        /// Combines <see cref="MoreEnumerable.OrderBy{T, TKey}(IEnumerable{T}, Func{T, TKey}, IComparer{TKey}, OrderByDirection)"/>,
        /// where each element is its key, and <see cref="Enumerable.Take{TSource}(IEnumerable{TSource},int)"/>
        /// in a single operation.
        /// An additional parameter specifies the direction of the sort
        /// </summary>
        /// <typeparam name="T">Type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="count">Number of (maximum) elements to return.</param>
        /// <param name="direction">The direction in which to sort the elements</param>
        /// <returns>A sequence containing at most top <paramref name="count"/>
        /// elements from source, in the specified order.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams it results.
        /// </remarks>

        public static IEnumerable<T> PartialSort<T>(this IEnumerable<T> source,
            int count, OrderByDirection direction)
        {
            return source.PartialSort(count, null, direction);
        }

        /// <summary>
        /// Combines <see cref="Enumerable.OrderBy{TSource,TKey}(IEnumerable{TSource},Func{TSource,TKey},IComparer{TKey})"/>,
        /// where each element is its key, and <see cref="Enumerable.Take{TSource}(IEnumerable{TSource},int)"/>
        /// in a single operation. An additional parameter specifies how the
        /// elements compare to each other.
        /// </summary>
        /// <typeparam name="T">Type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="count">Number of (maximum) elements to return.</param>
        /// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
        /// <returns>A sequence containing at most top <paramref name="count"/>
        /// elements from source, in their ascending order.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams it results.
        /// </remarks>

        public static IEnumerable<T> PartialSort<T>(this IEnumerable<T> source,
            int count, IComparer<T>? comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return PartialSortByImpl<T, T>(source, count, null, null, comparer);
        }

        /// <summary>
        /// Combines <see cref="MoreEnumerable.OrderBy{T, TKey}(IEnumerable{T}, Func{T, TKey}, IComparer{TKey}, OrderByDirection)"/>,
        /// where each element is its key, and <see cref="Enumerable.Take{TSource}(IEnumerable{TSource},int)"/>
        /// in a single operation.
        /// Additional parameters specify how the elements compare to each other and
        /// the direction of the sort.
        /// </summary>
        /// <typeparam name="T">Type of elements in the sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="count">Number of (maximum) elements to return.</param>
        /// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
        /// <param name="direction">The direction in which to sort the elements</param>
        /// <returns>A sequence containing at most top <paramref name="count"/>
        /// elements from source, in the specified order.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams it results.
        /// </remarks>

        public static IEnumerable<T> PartialSort<T>(this IEnumerable<T> source,
            int count, IComparer<T>? comparer, OrderByDirection direction)
        {
            comparer ??= Comparer<T>.Default;
            if (direction == OrderByDirection.Descending)
                comparer = new ReverseComparer<T>(comparer);
            return source.PartialSort(count, comparer);
        }

        /// <summary>
        /// Combines <see cref="Enumerable.OrderBy{TSource,TKey}(IEnumerable{TSource},Func{TSource,TKey},IComparer{TKey})"/>,
        /// and <see cref="Enumerable.Take{TSource}(IEnumerable{TSource},int)"/> in a single operation.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
        /// <typeparam name="TKey">Type of keys.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="count">Number of (maximum) elements to return.</param>
        /// <returns>A sequence containing at most top <paramref name="count"/>
        /// elements from source, in ascending order of their keys.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams it results.
        /// </remarks>

        public static IEnumerable<TSource> PartialSortBy<TSource, TKey>(
            this IEnumerable<TSource> source, int count,
            Func<TSource, TKey> keySelector)
        {
            return source.PartialSortBy(count, keySelector, null);
        }

        /// <summary>
        /// Combines <see cref="MoreEnumerable.OrderBy{T, TKey}(IEnumerable{T}, Func{T, TKey}, OrderByDirection)"/>,
        /// and <see cref="Enumerable.Take{TSource}(IEnumerable{TSource},int)"/> in a single operation.
        /// An additional parameter specifies the direction of the sort
        /// </summary>
        /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
        /// <typeparam name="TKey">Type of keys.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="count">Number of (maximum) elements to return.</param>
        /// <param name="direction">The direction in which to sort the elements</param>
        /// <returns>A sequence containing at most top <paramref name="count"/>
        /// elements from source, in the specified order of their keys.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams it results.
        /// </remarks>

        public static IEnumerable<TSource> PartialSortBy<TSource, TKey>(
            this IEnumerable<TSource> source, int count,
            Func<TSource, TKey> keySelector, OrderByDirection direction)
        {
            return source.PartialSortBy(count, keySelector, null, direction);
        }

        /// <summary>
        /// Combines <see cref="Enumerable.OrderBy{TSource,TKey}(IEnumerable{TSource},Func{TSource,TKey},IComparer{TKey})"/>,
        /// and <see cref="Enumerable.Take{TSource}(IEnumerable{TSource},int)"/> in a single operation.
        /// An additional parameter specifies how the keys compare to each other.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
        /// <typeparam name="TKey">Type of keys.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="count">Number of (maximum) elements to return.</param>
        /// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
        /// <returns>A sequence containing at most top <paramref name="count"/>
        /// elements from source, in ascending order of their keys.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams it results.
        /// </remarks>

        public static IEnumerable<TSource> PartialSortBy<TSource, TKey>(
            this IEnumerable<TSource> source, int count,
            Func<TSource, TKey> keySelector,
            IComparer<TKey>? comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            return PartialSortByImpl(source, count, keySelector, comparer, null);
        }

        /// <summary>
        /// Combines <see cref="MoreEnumerable.OrderBy{T, TKey}(IEnumerable{T}, Func{T, TKey}, OrderByDirection)"/>,
        /// and <see cref="Enumerable.Take{TSource}(IEnumerable{TSource},int)"/> in a single operation.
        /// Additional parameters specify how the elements compare to each other and
        /// the direction of the sort.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in the sequence.</typeparam>
        /// <typeparam name="TKey">Type of keys.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <param name="count">Number of (maximum) elements to return.</param>
        /// <param name="comparer">A <see cref="IComparer{T}"/> to compare elements.</param>
        /// <param name="direction">The direction in which to sort the elements</param>
        /// <returns>A sequence containing at most top <paramref name="count"/>
        /// elements from source, in the specified order of their keys.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams it results.
        /// </remarks>

        public static IEnumerable<TSource> PartialSortBy<TSource, TKey>(
            this IEnumerable<TSource> source, int count,
            Func<TSource, TKey> keySelector,
            IComparer<TKey>? comparer,
            OrderByDirection direction)
        {
            comparer ??= Comparer<TKey>.Default;
            if (direction == OrderByDirection.Descending)
                comparer = new ReverseComparer<TKey>(comparer);
            return source.PartialSortBy(count, keySelector, comparer);
        }

        static IEnumerable<TSource> PartialSortByImpl<TSource, TKey>(
            IEnumerable<TSource> source, int count,
            Func<TSource, TKey>? keySelector,
            IComparer<TKey>? keyComparer,
            IComparer<TSource>? comparer)
        {
            var keys = keySelector != null ? new List<TKey>(count) : null;
            var top = new List<TSource>(count);

            int? Insert<T>(List<T> list, T item, IComparer<T>? comparer)
            {
                var i = list.BinarySearch(item, comparer);
                if (i < 0 && (i = ~i) >= count)
                    return null;
                if (list.Count == count)
                    list.RemoveAt(count - 1);
                list.Insert(i, item);
                return i;
            }

            foreach (var item in source)
            {
                if (keys != null)
                {
                    var key = Assume.NotNull(keySelector)(item);
                    if (Insert(keys, key, keyComparer) is { } i)
                    {
                        if (top.Count == count)
                            top.RemoveAt(count - 1);
                        top.Insert(i, item);
                    }
                }
                else
                {
                    _ = Insert(top, item, comparer);
                }

                // TODO Stable sorting
            }

            // ReSharper disable once LoopCanBeConvertedToQuery

            foreach (var item in top)
                yield return item;
        }
    }
}
