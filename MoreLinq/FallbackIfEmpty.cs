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
        /// <code><![CDATA[
        /// var numbers = new[] { 123, 456, 789 };
        /// var result = numbers.Where(x => x == 100).FallbackIfEmpty(-1).Single();
        /// ]]></code>
        /// The <c>result</c> variable will contain <c>-1</c>.
        /// </example>

        public static IEnumerable<T> FallbackIfEmpty<T>(this IEnumerable<T> source, T fallback)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return FallbackIfEmptyImpl(source, 1, fallback, default, default, default, null);
        }

        /// <summary>
        /// Returns the elements of a sequence, but if it is empty then
        /// returns an alternate sequence of values.
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
            return FallbackIfEmptyImpl(source, 2, fallback1, fallback2, default, default, null);
        }

        /// <summary>
        /// Returns the elements of a sequence, but if it is empty then
        /// returns an alternate sequence of values.
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
            return FallbackIfEmptyImpl(source, 3, fallback1, fallback2, fallback3, default, null);
        }

        /// <summary>
        /// Returns the elements of a sequence, but if it is empty then
        /// returns an alternate sequence of values.
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
        /// returns an alternate sequence from an array of values.
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
        /// returns an alternate sequence of values.
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
            return FallbackIfEmptyImpl(source, null, default, default, default, default, fallback);
        }

        static IEnumerable<T> FallbackIfEmptyImpl<T>(IEnumerable<T> source,
            int? count, T? fallback1, T? fallback2, T? fallback3, T? fallback4,
            IEnumerable<T>? fallback)
        {
            return _(); IEnumerable<T> _()
            {
                if (source.TryAsCollectionLike() is null or { Count: > 0 })
                {
                    using var e = source.GetEnumerator();
                    if (e.MoveNext())
                    {
                        do { yield return e.Current; }
                        while (e.MoveNext());
                        yield break;
                    }
                }

                if (fallback is { } someFallback)
                {
                    Debug.Assert(count is null);

                    foreach (var item in someFallback)
                        yield return item;
                }
                else
                {
                    Debug.Assert(count is >= 1 and <= 4);

                    yield return fallback1!;
                    if (count > 1) yield return fallback2!;
                    if (count > 2) yield return fallback3!;
                    if (count > 3) yield return fallback4!;
                }
            }
        }
    }
}
