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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Exposes the enumerator, which supports iteration over a sequence of
    /// some extremum property (maximum or minimum) of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>

    public interface IExtremaEnumerable<out T> : IEnumerable<T>
    {
        /// <summary>
        /// Returns a specified number of contiguous elements from the start of
        /// the sequence.
        /// </summary>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that contains the specified number
        /// of elements from the start of the input sequence.
        /// </returns>

        IEnumerable<T> Take(int count);

        /// <summary>
        /// Returns a specified number of contiguous elements at the end of the
        /// sequence.
        /// </summary>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that contains the specified number
        /// of elements at the end of the input sequence.
        /// </returns>

        IEnumerable<T> TakeLast(int count);
    }

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns the first element of a sequence.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The input sequence.</param>
        /// <exception cref="InvalidOperationException">
        /// The input sequence is empty.</exception>
        /// <returns>
        /// The first element of the input sequence.
        /// </returns>

        public static T First<T>(this IExtremaEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.Take(1).AsEnumerable().First();
        }

        /// <summary>
        /// Returns the first element of a sequence, or a default value if the
        /// sequence contains no elements.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The input sequence.</param>
        /// <returns>
        /// Default value of type <typeparamref name="T"/> if source is empty;
        /// otherwise, the first element in source.
        /// </returns>

        public static T? FirstOrDefault<T>(this IExtremaEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.Take(1).AsEnumerable().FirstOrDefault();
        }

        /// <summary>
        /// Returns the last element of a sequence.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The input sequence.</param>
        /// <exception cref="InvalidOperationException">
        /// The input sequence is empty.</exception>
        /// <returns>
        /// The last element of the input sequence.
        /// </returns>

        public static T Last<T>(this IExtremaEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.TakeLast(1).AsEnumerable().Last();
        }

        /// <summary>
        /// Returns the last element of a sequence, or a default value if the
        /// sequence contains no elements.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The input sequence.</param>
        /// <returns>
        /// Default value of type <typeparamref name="T"/> if source is empty;
        /// otherwise, the last element in source.
        /// </returns>

        public static T? LastOrDefault<T>(this IExtremaEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.TakeLast(1).AsEnumerable().LastOrDefault();
        }

        /// <summary>
        /// Returns the only element of a sequence, and throws an exception if
        /// there is not exactly one element in the sequence.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The input sequence.</param>
        /// <exception cref="InvalidOperationException">
        /// The input sequence contains more than one element.</exception>
        /// <returns>
        /// The single element of the input sequence.
        /// </returns>

#pragma warning disable CA1720 // Identifier contains type name
        public static T Single<T>(this IExtremaEnumerable<T> source)
#pragma warning restore CA1720 // Identifier contains type name
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.Take(2).AsEnumerable().Single();
        }

        /// <summary>
        /// Returns the only element of a sequence, or a default value if the
        /// sequence is empty; this method throws an exception if there is more
        /// than one element in the sequence.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The input sequence.</param>
        /// <returns>
        /// The single element of the input sequence, or default value of type
        /// <typeparamref name="T"/> if the sequence contains no elements.
        /// </returns>

        public static T? SingleOrDefault<T>(this IExtremaEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.Take(2).AsEnumerable().SingleOrDefault();
        }

        /// <summary>
        /// Returns the maximal elements of the given sequence, based on
        /// the given projection.
        /// </summary>
        /// <remarks>
        /// This overload uses the default comparer  for the projected type.
        /// This operator uses deferred execution. The results are evaluated
        /// and cached on first use to returned sequence.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <returns>The sequence of maximal elements, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>

        public static IExtremaEnumerable<TSource> MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, null);
        }

        /// <summary>
        /// Returns the maximal elements of the given sequence, based on
        /// the given projection and the specified comparer for projected values.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution. The results are evaluated
        /// and cached on first use to returned sequence.
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values</param>
        /// <returns>The sequence of maximal elements, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/>
        /// or <paramref name="comparer"/> is null</exception>

        public static IExtremaEnumerable<TSource> MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey>? comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            comparer ??= Comparer<TKey>.Default;
            return new ExtremaEnumerable<TSource, TKey>(source, selector, comparer.Compare);
        }

        sealed class ExtremaEnumerable<T, TKey> : IExtremaEnumerable<T>
        {
            readonly IEnumerable<T> _source;
            readonly Func<T, TKey> _selector;
            readonly Func<TKey, TKey, int> _comparer;

            public ExtremaEnumerable(IEnumerable<T> source, Func<T, TKey> selector, Func<TKey, TKey, int> comparer)
            {
                _source = source;
                _selector = selector;
                _comparer = comparer;
            }

            public IEnumerator<T> GetEnumerator() =>
                ExtremaBy(_source, Extrema.First, null, _selector, _comparer).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() =>
                GetEnumerator();

            public IEnumerable<T> Take(int count) =>
                count switch
                {
                    0 => Enumerable.Empty<T>(),
                    1 => ExtremaBy(_source, Extremum.First, 1    , _selector, _comparer),
                    _ => ExtremaBy(_source, Extrema.First , count, _selector, _comparer)
                };

            public IEnumerable<T> TakeLast(int count) =>
                count switch
                {
                    0 => Enumerable.Empty<T>(),
                    1 => ExtremaBy(_source, Extremum.Last, 1    , _selector, _comparer),
                    _ => ExtremaBy(_source, Extrema.Last , count, _selector, _comparer)
                };

            static class Extrema
            {
                public static readonly Extrema<List<T>? , T> First = new FirstExtrema();
                public static readonly Extrema<Queue<T>?, T> Last  = new LastExtrema();

                sealed class FirstExtrema : Extrema<List<T>?, T>
                {
                    public override List<T>? New() => null;
                    public override void Restart(ref List<T>? store) => store = null;
                    public override IEnumerable<T> GetEnumerable(List<T>? store) => store ?? Enumerable.Empty<T>();

                    public override void Add(ref List<T>? store, int? limit, T item)
                    {
                        if (limit == null || store is null || store.Count < limit)
                            (store ??= new List<T>()).Add(item);
                    }
                }

                sealed class LastExtrema : Extrema<Queue<T>?, T>
                {
                    public override Queue<T>? New() => null;
                    public override void Restart(ref Queue<T>? store) => store = null;
                    public override IEnumerable<T> GetEnumerable(Queue<T>? store) => store ?? Enumerable.Empty<T>();

                    public override void Add(ref Queue<T>? store, int? limit, T item)
                    {
                        if (limit is { } n && store is { } queue && queue.Count == n)
                            _ = queue.Dequeue();
                        (store ??= new Queue<T>()).Enqueue(item);
                    }
                }
            }

            sealed class Extremum : Extrema<(bool, T), T>
            {
                public static readonly Extrema<(bool, T), T> First = new Extremum(false);
                public static readonly Extrema<(bool, T), T> Last  = new Extremum(true);

                readonly bool _poppable;
                Extremum(bool poppable) => _poppable = poppable;

                public override (bool, T) New() => default;
                public override void Restart(ref (bool, T) store) => store = default;

                public override IEnumerable<T> GetEnumerable((bool, T) store) =>
                    store is (true, var item) ? Enumerable.Repeat(item, 1) : Enumerable.Empty<T>();

                public override void Add(ref (bool, T) store, int? limit, T item)
                {
                    if (!_poppable && store is (true, _))
                        return;
                    store = (true, item);
                }
            }
        }

        // > In mathematical analysis, the maxima and minima (the respective
        // > plurals of maximum and minimum) of a function, known collectively
        // > as extrema (the plural of extremum), ...
        // >
        // > - https://en.wikipedia.org/wiki/Maxima_and_minima

        static IEnumerable<TSource> ExtremaBy<TSource, TKey, TStore>(
            this IEnumerable<TSource> source,
            Extrema<TStore, TSource> extrema, int? limit,
            Func<TSource, TKey> selector, Func<TKey, TKey, int> comparer)
        {
            foreach (var item in Extrema())
                yield return item;

            IEnumerable<TSource> Extrema()
            {
                using var e = source.GetEnumerator();

                if (!e.MoveNext())
                    return new List<TSource>();

                var store = extrema.New();
                extrema.Add(ref store, limit, e.Current);
                var extremaKey = selector(e.Current);

                while (e.MoveNext())
                {
                    var item = e.Current;
                    var key = selector(item);
                    switch (comparer(key, extremaKey))
                    {
                        case > 0:
                            extrema.Restart(ref store);
                            extrema.Add(ref store, limit, item);
                            extremaKey = key;
                            break;
                        case 0:
                            extrema.Add(ref store, limit, item);
                            break;
                        default:
                            break;
                    }
                }

                return extrema.GetEnumerable(store);
            }
        }

        abstract class Extrema<TStore, T>
        {
            public abstract TStore New();
            public abstract void Restart(ref TStore store);
            public abstract IEnumerable<T> GetEnumerable(TStore store);
            public abstract void Add(ref TStore store, int? limit, T item);
        }
    }
}
