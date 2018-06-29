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

        public static T FirstOrDefault<T>(this IExtremaEnumerable<T> source)
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

        public static T LastOrDefault<T>(this IExtremaEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.Take(1).AsEnumerable().LastOrDefault();
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

        public static T Single<T>(this IExtremaEnumerable<T> source)
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

        public static T SingleOrDefault<T>(this IExtremaEnumerable<T> source)
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
        /// <returns>The maximal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>

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
        /// <returns>The maximal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/>
        /// or <paramref name="comparer"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>

        public static IExtremaEnumerable<TSource> MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            comparer = comparer ?? Comparer<TKey>.Default;
            return new ExtremaEnumerable<TSource, TKey>(source, selector, (x, y) => comparer.Compare(x, y));
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

            public IEnumerable<T> Take(int count)
                => count == 0 ? Enumerable.Empty<T>()
                 : count == 1 ? ExtremaBy(_source, Extremum.First, 1    , _selector, _comparer)
                              : ExtremaBy(_source, Extrema.First , count, _selector, _comparer);

            public IEnumerable<T> TakeLast(int count)
                => count == 0 ? Enumerable.Empty<T>()
                 : count == 1 ? ExtremaBy(_source, Extremum.Last, 1    , _selector, _comparer)
                              : ExtremaBy(_source, Extrema.Last , count, _selector, _comparer);

            static class Extrema
            {
                public static readonly Extrema<List<T> , T> First = new FirstExtrema();
                public static readonly Extrema<Queue<T>, T> Last  = new LastExtrema();

                sealed class FirstExtrema : Extrema<List<T>, T>
                {
                    protected override IEnumerable<T> GetSomeEnumerable(List<T> store) => store;
                    protected override int Count(List<T> store) => store?.Count ?? 0;
                    protected override void Push(ref List<T> store, T item) => (store ?? (store = new List<T>())).Add(item);
                    protected override bool TryPop(ref List<T> store) => false;
                }

                sealed class LastExtrema : Extrema<Queue<T>, T>
                {
                    protected override IEnumerable<T> GetSomeEnumerable(Queue<T> store) => store;
                    protected override int Count(Queue<T> store) => store?.Count ?? 0;
                    protected override void Push(ref Queue<T> store, T item) => (store ?? (store = new Queue<T>())).Enqueue(item);
                    protected override bool TryPop(ref Queue<T> store) { store.Dequeue(); return true; }
                }
            }

            sealed class Extremum : Extrema<(bool HasValue, T Value), T>
            {
                public static readonly Extrema<(bool, T), T> First = new Extremum(false);
                public static readonly Extrema<(bool, T), T> Last  = new Extremum(true);

                readonly bool _poppable;
                Extremum(bool poppable) => _poppable = poppable;

                protected override IEnumerable<T> GetSomeEnumerable((bool HasValue, T Value) store) =>
                    Enumerable.Repeat(store.Value, 1);

                protected override int Count((bool HasValue, T Value) store) => store.HasValue ? 1 : 0;
                protected override void Push(ref (bool, T) store, T item) => store = (true, item);

                protected override bool TryPop(ref (bool, T) store)
                {
                    if (!_poppable)
                        return false;

                    Restart(ref store);
                    return true;
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
                using (var e = source.GetEnumerator())
                {
                    if (!e.MoveNext())
                        return new List<TSource>();

                    var store = extrema.New();
                    extrema.Add(ref store, limit, e.Current);
                    var extremaKey = selector(e.Current);

                    while (e.MoveNext())
                    {
                        var item = e.Current;
                        var key = selector(item);
                        var comparison = comparer(key, extremaKey);
                        if (comparison > 0)
                        {
                            extrema.Restart(ref store);
                            extrema.Add(ref store, limit, item);
                            extremaKey = key;
                        }
                        else if (comparison == 0)
                        {
                            extrema.Add(ref store, limit, item);
                        }
                    }

                    return extrema.GetEnumerable(store);
                }
            }
        }

        abstract class Extrema<TStore, T>
        {
            public virtual TStore New() => default;
            public virtual void Restart(ref TStore store) => store = default;

            public void Add(ref TStore store, int? limit, T item)
            {
                if (limit == null || Count(store) < limit || TryPop(ref store))
                    Push(ref store, item);
            }

            protected abstract int Count(TStore store);
            protected abstract void Push(ref TStore store, T item);
            protected abstract bool TryPop(ref TStore store);

            public virtual IEnumerable<T> GetEnumerable(TStore store) =>
                Count(store) > 0
                ? GetSomeEnumerable(store)
                : Enumerable.Empty<T>();

            protected abstract IEnumerable<T> GetSomeEnumerable(TStore store);
        }
    }
}
