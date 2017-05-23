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
        /// Returns the elements of the specified sequence or the specified
        /// value in a singleton collection if the sequence is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequences.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="fallback">The value to return in a singleton
        /// collection if <paramref name="source"/> is empty.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that contains <paramref name="fallback"/>
        /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
        /// </returns>
        /// <example>
        /// <code>
        /// var numbers = { 123, 456, 789 };
        /// var result = numbers.Where(x => x == 100).FallbackIfEmpty(-1).Single();
        /// </code>
        /// The <c>result</c> variable will contain <c>-1</c>.
        /// </example>

        public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, T fallback)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return FallbackIfEmptyImpl(source, 1, fallback, default(T), default(T), default(T), null);
        }

        /// <summary>
        /// Returns the elements of a sequence, but if it is empty then
        /// returns an altenate sequence of values.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequences.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="fallback1">First value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <param name="fallback2">Second value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that containing fallback values
        /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
        /// </returns>

        public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, T fallback1, T fallback2)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return FallbackIfEmptyImpl(source, 2, fallback1, fallback2, default(T), default(T), null);
        }

        /// <summary>
        /// Returns the elements of a sequence, but if it is empty then
        /// returns an altenate sequence of values.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequences.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="fallback1">First value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <param name="fallback2">Second value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <param name="fallback3">Third value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that containing fallback values
        /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
        /// </returns>

        public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, T fallback1, T fallback2, T fallback3)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return FallbackIfEmptyImpl(source, 3, fallback1, fallback2, fallback3, default(T), null);
        }

        /// <summary>
        /// Returns the elements of a sequence, but if it is empty then
        /// returns an altenate sequence of values.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequences.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="fallback1">First value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <param name="fallback2">Second value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <param name="fallback3">Third value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <param name="fallback4">Fourth value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that containing fallback values
        /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
        /// </returns>

        public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, T fallback1, T fallback2, T fallback3, T fallback4)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return FallbackIfEmptyImpl(source, 4, fallback1, fallback2, fallback3, fallback4, null);
        }

        /// <summary>
        /// Returns the elements of a sequence, but if it is empty then
        /// returns an altenate sequence from an array of values.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequences.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="fallback">The array that is returned as the alternate
        /// sequence if <paramref name="source"/> is empty.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that containing fallback values
        /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
        /// </returns>

        public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, params T[] fallback)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (fallback == null) throw new ArgumentNullException(nameof(fallback));
            return source.FallbackIfEmpty((IEnumerable<T>)fallback);
        }

        /// <summary>
        /// Returns the elements of a sequence, but if it is empty then
        /// returns an altenate sequence of values.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequences.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="fallback">The alternate sequence that is returned
        /// if <paramref name="source"/> is empty.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that containing fallback values
        /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
        /// </returns>

        public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, IEnumerable<T> fallback)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (fallback == null) throw new ArgumentNullException(nameof(fallback));
            return FallbackIfEmptyImpl(source, 0, default(T), default(T), default(T), default(T), fallback);
        }

        /// <summary>
        /// Returns the elements of a sequence, but if it is empty then
        /// returns an altenate sequence of values.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequences.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="fallbackFactory">A function that generates the first value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that containing fallback values
        /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
        /// </returns>

        public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, Func<T> fallbackFactory)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (fallbackFactory == null) throw new ArgumentNullException(nameof(fallbackFactory));
            return FallbackIfEmpty(source, 1, fallbackFactory, null, null, null);
        }

        /// <summary>
        /// Returns the elements of a sequence, but if it is empty then
        /// returns an altenate sequence of values.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequences.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="fallbackFactory1">A function that generates the first value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <param name="fallbackFactory2">A function that generates the second value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that containing fallback values
        /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
        /// </returns>

        public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, Func<T> fallbackFactory1, Func<T> fallbackFactory2)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (fallbackFactory1 == null) throw new ArgumentNullException(nameof(fallbackFactory1));
            if (fallbackFactory2 == null) throw new ArgumentNullException(nameof(fallbackFactory2));
            return FallbackIfEmpty(source, 2, fallbackFactory1, fallbackFactory2, null, null);
        }

        /// <summary>
        /// Returns the elements of a sequence, but if it is empty then
        /// returns an altenate sequence of values.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequences.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="fallbackFactory1">A function that generates the first value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <param name="fallbackFactory2">A function that generates the second value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <param name="fallbackFactory3">A function that generates the third value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that containing fallback values
        /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
        /// </returns>

        public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, Func<T> fallbackFactory1, Func<T> fallbackFactory2, Func<T> fallbackFactory3)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (fallbackFactory1 == null) throw new ArgumentNullException(nameof(fallbackFactory1));
            if (fallbackFactory2 == null) throw new ArgumentNullException(nameof(fallbackFactory2));
            if (fallbackFactory3 == null) throw new ArgumentNullException(nameof(fallbackFactory3));
            return FallbackIfEmpty(source, 3, fallbackFactory1, fallbackFactory2, fallbackFactory3, null);
        }

        /// <summary>
        /// Returns the elements of a sequence, but if it is empty then
        /// returns an altenate sequence of values.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequences.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="fallbackFactory1">A function that generates the first value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <param name="fallbackFactory2">A function that generates the second value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <param name="fallbackFactory3">A function that generates the third value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <param name="fallbackFactory4">A function that generates the fourth value of the alternate sequence that
        /// is returned if <paramref name="source"/> is empty.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that containing fallback values
        /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
        /// </returns>

        public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, Func<T> fallbackFactory1, Func<T> fallbackFactory2, Func<T> fallbackFactory3, Func<T> fallbackFactory4)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (fallbackFactory1 == null) throw new ArgumentNullException(nameof(fallbackFactory1));
            if (fallbackFactory2 == null) throw new ArgumentNullException(nameof(fallbackFactory2));
            if (fallbackFactory3 == null) throw new ArgumentNullException(nameof(fallbackFactory3));
            if (fallbackFactory4 == null) throw new ArgumentNullException(nameof(fallbackFactory4));
            return FallbackIfEmpty(source, 4, fallbackFactory1, fallbackFactory2, fallbackFactory3, fallbackFactory4);
        }

        private static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, int count, Func<T> fallbackFactory1, Func<T> fallbackFactory2, Func<T> fallbackFactory3, Func<T> fallbackFactory4)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0 || count > 4) {
                throw new ArgumentException(nameof(count));
            }
            return FallbackIfEmptyImpl(source, null, default(T), default(T), default(T), default(T), fallback());
            IEnumerable<T> fallback()
            {
                yield return fallbackFactory1();
                if (count > 1) yield return fallbackFactory2();
                if (count > 2) yield return fallbackFactory3();
                if (count > 3) yield return fallbackFactory4();
            }
        }

        /// <summary>
        /// Returns the elements of a sequence, but if it is empty then
        /// returns an altenate sequence from an array of values.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequences.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="fallbackFactories">The array of functions that generate the values of the alternate
        /// sequence if <paramref name="source"/> is empty.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that containing fallback values
        /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
        /// </returns>

        public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, params Func<T>[] fallbackFactories)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (fallbackFactories == null) throw new ArgumentNullException(nameof(fallbackFactories));

            return FallbackIfEmpty(source, (IEnumerable<Func<T>>)fallbackFactories);
        }

        /// <summary>
        /// Returns the elements of a sequence, but if it is empty then
        /// returns an altenate sequence from an array of values.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequences.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="fallbackFactories">The sequence of functions that generate the values of the alternate
        /// sequence if <paramref name="source"/> is empty.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that containing fallback values
        /// if <paramref name="source"/> is empty; otherwise, <paramref name="source"/>.
        /// </returns>

        public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, IEnumerable<Func<T>> fallbackFactories)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (fallbackFactories == null) throw new ArgumentNullException(nameof(fallbackFactories));

            return FallbackIfEmptyImpl(source, null, default(T), default(T), default(T), default(T), fallbackFactories.Select(f => f()));
        }

        static IEnumerable<T> FallbackIfEmptyImpl<T>(IEnumerable<T> source,
            int? count, T fallback1, T fallback2, T fallback3, T fallback4,
            IEnumerable<T> fallback)
        {
            if (source is ICollection<T> collection) {
                if (collection.Count == 0) {
                    return fallbackChooser();
                }
                else {
                    return collection;
                }
            }

            return nonCollectionIterator();

            IEnumerable<T> nonCollectionIterator()
            {
                using (var e = source.GetEnumerator()) {
                    if (e.MoveNext()) {
                        do { yield return e.Current; }
                        while (e.MoveNext());
                        yield break;
                    }
                }

                foreach (var item in fallbackChooser())
                    yield return item;
            }
            IEnumerable<T> fallbackChooser()
            {
                if (count > 0 && count <= 4) {
                    return instancesFallback();
                }
                else {
                    return fallback;
                }
            }
            IEnumerable<T> instancesFallback()
            {
                yield return fallback1;
                if (count > 1) yield return fallback2;
                if (count > 2) yield return fallback3;
                if (count > 3) yield return fallback4;
            }
        }
    }
}
