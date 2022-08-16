#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Pierre Lando. All rights reserved.
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
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values are projected with <paramref name="resultSelector"/>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals
        /// relatively to the <paramref name="keyComparer"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of the projected value.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// If <paramref name="keyComparer"/> is null, <c>EqualityComparer.Default</c> is used.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>,
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/>.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey firstKey,
            TKey secondKey,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TResult> resultSelector,
            IEqualityComparer<TKey> keyComparer)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                firstKey,
                secondKey
            };

            return BatchByImplementation(source, keys, keySelector, keyComparer)
                    .Select(d => resultSelector(d[0], d[1]));
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values are projected with <paramref name="resultSelector"/>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of the projected value.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>,
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey firstKey,
            TKey secondKey,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TResult> resultSelector)
        {
            return BatchBy(source,
                           firstKey,
                           secondKey,
                           keySelector, resultSelector, null);
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values returned as <c>ValueTuple</c>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals
        /// relatively to the <paramref name="keyComparer"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// If <paramref name="keyComparer"/> is null, <c>EqualityComparer.Default</c> is used.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/></exception>

        public static IEnumerable<(TSource First, TSource Second)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey firstKey,
                TKey secondKey,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> keyComparer)
        {
            var keys = new []
            {
                firstKey,
                secondKey
            };

            return BatchByImplementation(source, keys, keySelector, keyComparer)
                    .Select(d => ValueTuple.Create(d[0], d[1]));
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values returned as <c>ValueTuple</c>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<(TSource First, TSource Second)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey firstKey,
                TKey secondKey,
                Func<TSource, TKey> keySelector)
        {
            return BatchBy(source,
                           firstKey,
                           secondKey,
                           keySelector, null);
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values are projected with <paramref name="resultSelector"/>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals
        /// relatively to the <paramref name="keyComparer"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of the projected value.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// If <paramref name="keyComparer"/> is null, <c>EqualityComparer.Default</c> is used.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>,
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/>.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey firstKey,
            TKey secondKey,
            TKey thirdKey,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TResult> resultSelector,
            IEqualityComparer<TKey> keyComparer)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                firstKey,
                secondKey,
                thirdKey
            };

            return BatchByImplementation(source, keys, keySelector, keyComparer)
                    .Select(d => resultSelector(d[0], d[1], d[2]));
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values are projected with <paramref name="resultSelector"/>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of the projected value.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>,
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey firstKey,
            TKey secondKey,
            TKey thirdKey,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TResult> resultSelector)
        {
            return BatchBy(source,
                           firstKey,
                           secondKey,
                           thirdKey,
                           keySelector, resultSelector, null);
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values returned as <c>ValueTuple</c>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals
        /// relatively to the <paramref name="keyComparer"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// If <paramref name="keyComparer"/> is null, <c>EqualityComparer.Default</c> is used.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/></exception>

        public static IEnumerable<(TSource First, TSource Second, TSource Third)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey firstKey,
                TKey secondKey,
                TKey thirdKey,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> keyComparer)
        {
            var keys = new []
            {
                firstKey,
                secondKey,
                thirdKey
            };

            return BatchByImplementation(source, keys, keySelector, keyComparer)
                    .Select(d => ValueTuple.Create(d[0], d[1], d[2]));
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values returned as <c>ValueTuple</c>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<(TSource First, TSource Second, TSource Third)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey firstKey,
                TKey secondKey,
                TKey thirdKey,
                Func<TSource, TKey> keySelector)
        {
            return BatchBy(source,
                           firstKey,
                           secondKey,
                           thirdKey,
                           keySelector, null);
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values are projected with <paramref name="resultSelector"/>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals
        /// relatively to the <paramref name="keyComparer"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of the projected value.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// If <paramref name="keyComparer"/> is null, <c>EqualityComparer.Default</c> is used.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>,
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/>.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey firstKey,
            TKey secondKey,
            TKey thirdKey,
            TKey fourthKey,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TResult> resultSelector,
            IEqualityComparer<TKey> keyComparer)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                firstKey,
                secondKey,
                thirdKey,
                fourthKey
            };

            return BatchByImplementation(source, keys, keySelector, keyComparer)
                    .Select(d => resultSelector(d[0], d[1], d[2], d[3]));
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values are projected with <paramref name="resultSelector"/>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of the projected value.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>,
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey firstKey,
            TKey secondKey,
            TKey thirdKey,
            TKey fourthKey,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TResult> resultSelector)
        {
            return BatchBy(source,
                           firstKey,
                           secondKey,
                           thirdKey,
                           fourthKey,
                           keySelector, resultSelector, null);
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values returned as <c>ValueTuple</c>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals
        /// relatively to the <paramref name="keyComparer"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// If <paramref name="keyComparer"/> is null, <c>EqualityComparer.Default</c> is used.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/></exception>

        public static IEnumerable<(TSource First, TSource Second, TSource Third, TSource Fourth)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey firstKey,
                TKey secondKey,
                TKey thirdKey,
                TKey fourthKey,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> keyComparer)
        {
            var keys = new []
            {
                firstKey,
                secondKey,
                thirdKey,
                fourthKey
            };

            return BatchByImplementation(source, keys, keySelector, keyComparer)
                    .Select(d => ValueTuple.Create(d[0], d[1], d[2], d[3]));
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values returned as <c>ValueTuple</c>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<(TSource First, TSource Second, TSource Third, TSource Fourth)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey firstKey,
                TKey secondKey,
                TKey thirdKey,
                TKey fourthKey,
                Func<TSource, TKey> keySelector)
        {
            return BatchBy(source,
                           firstKey,
                           secondKey,
                           thirdKey,
                           fourthKey,
                           keySelector, null);
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values are projected with <paramref name="resultSelector"/>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals
        /// relatively to the <paramref name="keyComparer"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of the projected value.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="fifthKey">Fifth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// If <paramref name="keyComparer"/> is null, <c>EqualityComparer.Default</c> is used.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>, <paramref name="fifthKey"/>,
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/>.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey firstKey,
            TKey secondKey,
            TKey thirdKey,
            TKey fourthKey,
            TKey fifthKey,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TSource, TResult> resultSelector,
            IEqualityComparer<TKey> keyComparer)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                firstKey,
                secondKey,
                thirdKey,
                fourthKey,
                fifthKey
            };

            return BatchByImplementation(source, keys, keySelector, keyComparer)
                    .Select(d => resultSelector(d[0], d[1], d[2], d[3], d[4]));
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values are projected with <paramref name="resultSelector"/>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of the projected value.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="fifthKey">Fifth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>, <paramref name="fifthKey"/>,
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey firstKey,
            TKey secondKey,
            TKey thirdKey,
            TKey fourthKey,
            TKey fifthKey,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TSource, TResult> resultSelector)
        {
            return BatchBy(source,
                           firstKey,
                           secondKey,
                           thirdKey,
                           fourthKey,
                           fifthKey,
                           keySelector, resultSelector, null);
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values returned as <c>ValueTuple</c>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals
        /// relatively to the <paramref name="keyComparer"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="fifthKey">Fifth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// If <paramref name="keyComparer"/> is null, <c>EqualityComparer.Default</c> is used.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>, <paramref name="fifthKey"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/></exception>

        public static IEnumerable<(TSource First, TSource Second, TSource Third, TSource Fourth, TSource Fifth)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey firstKey,
                TKey secondKey,
                TKey thirdKey,
                TKey fourthKey,
                TKey fifthKey,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> keyComparer)
        {
            var keys = new []
            {
                firstKey,
                secondKey,
                thirdKey,
                fourthKey,
                fifthKey
            };

            return BatchByImplementation(source, keys, keySelector, keyComparer)
                    .Select(d => ValueTuple.Create(d[0], d[1], d[2], d[3], d[4]));
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values returned as <c>ValueTuple</c>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="fifthKey">Fifth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>, <paramref name="fifthKey"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<(TSource First, TSource Second, TSource Third, TSource Fourth, TSource Fifth)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey firstKey,
                TKey secondKey,
                TKey thirdKey,
                TKey fourthKey,
                TKey fifthKey,
                Func<TSource, TKey> keySelector)
        {
            return BatchBy(source,
                           firstKey,
                           secondKey,
                           thirdKey,
                           fourthKey,
                           fifthKey,
                           keySelector, null);
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values are projected with <paramref name="resultSelector"/>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals
        /// relatively to the <paramref name="keyComparer"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of the projected value.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="fifthKey">Fifth key.</param>
        /// <param name="sixthKey">Sixth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// If <paramref name="keyComparer"/> is null, <c>EqualityComparer.Default</c> is used.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>, <paramref name="fifthKey"/>, <paramref name="sixthKey"/>,
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/>.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey firstKey,
            TKey secondKey,
            TKey thirdKey,
            TKey fourthKey,
            TKey fifthKey,
            TKey sixthKey,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TResult> resultSelector,
            IEqualityComparer<TKey> keyComparer)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                firstKey,
                secondKey,
                thirdKey,
                fourthKey,
                fifthKey,
                sixthKey
            };

            return BatchByImplementation(source, keys, keySelector, keyComparer)
                    .Select(d => resultSelector(d[0], d[1], d[2], d[3], d[4], d[5]));
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values are projected with <paramref name="resultSelector"/>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of the projected value.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="fifthKey">Fifth key.</param>
        /// <param name="sixthKey">Sixth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>, <paramref name="fifthKey"/>, <paramref name="sixthKey"/>,
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey firstKey,
            TKey secondKey,
            TKey thirdKey,
            TKey fourthKey,
            TKey fifthKey,
            TKey sixthKey,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TResult> resultSelector)
        {
            return BatchBy(source,
                           firstKey,
                           secondKey,
                           thirdKey,
                           fourthKey,
                           fifthKey,
                           sixthKey,
                           keySelector, resultSelector, null);
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values returned as <c>ValueTuple</c>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals
        /// relatively to the <paramref name="keyComparer"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="fifthKey">Fifth key.</param>
        /// <param name="sixthKey">Sixth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// If <paramref name="keyComparer"/> is null, <c>EqualityComparer.Default</c> is used.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>, <paramref name="fifthKey"/>, <paramref name="sixthKey"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/></exception>

        public static IEnumerable<(TSource First, TSource Second, TSource Third, TSource Fourth, TSource Fifth, TSource Sixth)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey firstKey,
                TKey secondKey,
                TKey thirdKey,
                TKey fourthKey,
                TKey fifthKey,
                TKey sixthKey,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> keyComparer)
        {
            var keys = new []
            {
                firstKey,
                secondKey,
                thirdKey,
                fourthKey,
                fifthKey,
                sixthKey
            };

            return BatchByImplementation(source, keys, keySelector, keyComparer)
                    .Select(d => ValueTuple.Create(d[0], d[1], d[2], d[3], d[4], d[5]));
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values returned as <c>ValueTuple</c>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="fifthKey">Fifth key.</param>
        /// <param name="sixthKey">Sixth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>, <paramref name="fifthKey"/>, <paramref name="sixthKey"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<(TSource First, TSource Second, TSource Third, TSource Fourth, TSource Fifth, TSource Sixth)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey firstKey,
                TKey secondKey,
                TKey thirdKey,
                TKey fourthKey,
                TKey fifthKey,
                TKey sixthKey,
                Func<TSource, TKey> keySelector)
        {
            return BatchBy(source,
                           firstKey,
                           secondKey,
                           thirdKey,
                           fourthKey,
                           fifthKey,
                           sixthKey,
                           keySelector, null);
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values are projected with <paramref name="resultSelector"/>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals
        /// relatively to the <paramref name="keyComparer"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of the projected value.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="fifthKey">Fifth key.</param>
        /// <param name="sixthKey">Sixth key.</param>
        /// <param name="seventhKey">Seventh key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// If <paramref name="keyComparer"/> is null, <c>EqualityComparer.Default</c> is used.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>, <paramref name="fifthKey"/>, <paramref name="sixthKey"/>, <paramref name="seventhKey"/>,
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/>.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey firstKey,
            TKey secondKey,
            TKey thirdKey,
            TKey fourthKey,
            TKey fifthKey,
            TKey sixthKey,
            TKey seventhKey,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> resultSelector,
            IEqualityComparer<TKey> keyComparer)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                firstKey,
                secondKey,
                thirdKey,
                fourthKey,
                fifthKey,
                sixthKey,
                seventhKey
            };

            return BatchByImplementation(source, keys, keySelector, keyComparer)
                    .Select(d => resultSelector(d[0], d[1], d[2], d[3], d[4], d[5], d[6]));
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values are projected with <paramref name="resultSelector"/>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of the projected value.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="fifthKey">Fifth key.</param>
        /// <param name="sixthKey">Sixth key.</param>
        /// <param name="seventhKey">Seventh key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>, <paramref name="fifthKey"/>, <paramref name="sixthKey"/>, <paramref name="seventhKey"/>,
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey firstKey,
            TKey secondKey,
            TKey thirdKey,
            TKey fourthKey,
            TKey fifthKey,
            TKey sixthKey,
            TKey seventhKey,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> resultSelector)
        {
            return BatchBy(source,
                           firstKey,
                           secondKey,
                           thirdKey,
                           fourthKey,
                           fifthKey,
                           sixthKey,
                           seventhKey,
                           keySelector, resultSelector, null);
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values returned as <c>ValueTuple</c>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals
        /// relatively to the <paramref name="keyComparer"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="fifthKey">Fifth key.</param>
        /// <param name="sixthKey">Sixth key.</param>
        /// <param name="seventhKey">Seventh key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// If <paramref name="keyComparer"/> is null, <c>EqualityComparer.Default</c> is used.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>, <paramref name="fifthKey"/>, <paramref name="sixthKey"/>, <paramref name="seventhKey"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/></exception>

        public static IEnumerable<(TSource First, TSource Second, TSource Third, TSource Fourth, TSource Fifth, TSource Sixth, TSource Seventh)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey firstKey,
                TKey secondKey,
                TKey thirdKey,
                TKey fourthKey,
                TKey fifthKey,
                TKey sixthKey,
                TKey seventhKey,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> keyComparer)
        {
            var keys = new []
            {
                firstKey,
                secondKey,
                thirdKey,
                fourthKey,
                fifthKey,
                sixthKey,
                seventhKey
            };

            return BatchByImplementation(source, keys, keySelector, keyComparer)
                    .Select(d => ValueTuple.Create(d[0], d[1], d[2], d[3], d[4], d[5], d[6]));
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values returned as <c>ValueTuple</c>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="fifthKey">Fifth key.</param>
        /// <param name="sixthKey">Sixth key.</param>
        /// <param name="seventhKey">Seventh key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>, <paramref name="fifthKey"/>, <paramref name="sixthKey"/>, <paramref name="seventhKey"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<(TSource First, TSource Second, TSource Third, TSource Fourth, TSource Fifth, TSource Sixth, TSource Seventh)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey firstKey,
                TKey secondKey,
                TKey thirdKey,
                TKey fourthKey,
                TKey fifthKey,
                TKey sixthKey,
                TKey seventhKey,
                Func<TSource, TKey> keySelector)
        {
            return BatchBy(source,
                           firstKey,
                           secondKey,
                           thirdKey,
                           fourthKey,
                           fifthKey,
                           sixthKey,
                           seventhKey,
                           keySelector, null);
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values are projected with <paramref name="resultSelector"/>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals
        /// relatively to the <paramref name="keyComparer"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of the projected value.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="fifthKey">Fifth key.</param>
        /// <param name="sixthKey">Sixth key.</param>
        /// <param name="seventhKey">Seventh key.</param>
        /// <param name="eighthKey">Eighth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// If <paramref name="keyComparer"/> is null, <c>EqualityComparer.Default</c> is used.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>, <paramref name="fifthKey"/>, <paramref name="sixthKey"/>, <paramref name="seventhKey"/>, <paramref name="eighthKey"/>,
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/>.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey firstKey,
            TKey secondKey,
            TKey thirdKey,
            TKey fourthKey,
            TKey fifthKey,
            TKey sixthKey,
            TKey seventhKey,
            TKey eighthKey,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> resultSelector,
            IEqualityComparer<TKey> keyComparer)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                firstKey,
                secondKey,
                thirdKey,
                fourthKey,
                fifthKey,
                sixthKey,
                seventhKey,
                eighthKey
            };

            return BatchByImplementation(source, keys, keySelector, keyComparer)
                    .Select(d => resultSelector(d[0], d[1], d[2], d[3], d[4], d[5], d[6], d[7]));
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values are projected with <paramref name="resultSelector"/>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of the projected value.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="fifthKey">Fifth key.</param>
        /// <param name="sixthKey">Sixth key.</param>
        /// <param name="seventhKey">Seventh key.</param>
        /// <param name="eighthKey">Eighth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>, <paramref name="fifthKey"/>, <paramref name="sixthKey"/>, <paramref name="seventhKey"/>, <paramref name="eighthKey"/>,
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey firstKey,
            TKey secondKey,
            TKey thirdKey,
            TKey fourthKey,
            TKey fifthKey,
            TKey sixthKey,
            TKey seventhKey,
            TKey eighthKey,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> resultSelector)
        {
            return BatchBy(source,
                           firstKey,
                           secondKey,
                           thirdKey,
                           fourthKey,
                           fifthKey,
                           sixthKey,
                           seventhKey,
                           eighthKey,
                           keySelector, resultSelector, null);
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values returned as <c>ValueTuple</c>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals
        /// relatively to the <paramref name="keyComparer"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="fifthKey">Fifth key.</param>
        /// <param name="sixthKey">Sixth key.</param>
        /// <param name="seventhKey">Seventh key.</param>
        /// <param name="eighthKey">Eighth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// If <paramref name="keyComparer"/> is null, <c>EqualityComparer.Default</c> is used.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>, <paramref name="fifthKey"/>, <paramref name="sixthKey"/>, <paramref name="seventhKey"/>, <paramref name="eighthKey"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/></exception>

        public static IEnumerable<(TSource First, TSource Second, TSource Third, TSource Fourth, TSource Fifth, TSource Sixth, TSource Seventh, TSource Eighth)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey firstKey,
                TKey secondKey,
                TKey thirdKey,
                TKey fourthKey,
                TKey fifthKey,
                TKey sixthKey,
                TKey seventhKey,
                TKey eighthKey,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> keyComparer)
        {
            var keys = new []
            {
                firstKey,
                secondKey,
                thirdKey,
                fourthKey,
                fifthKey,
                sixthKey,
                seventhKey,
                eighthKey
            };

            return BatchByImplementation(source, keys, keySelector, keyComparer)
                    .Select(d => ValueTuple.Create(d[0], d[1], d[2], d[3], d[4], d[5], d[6], d[7]));
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Then the buckets values returned as <c>ValueTuple</c>.
        /// Each buckets contains all of the given keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="firstKey">First key.</param>
        /// <param name="secondKey">Second key.</param>
        /// <param name="thirdKey">Third key.</param>
        /// <param name="fourthKey">Fourth key.</param>
        /// <param name="fifthKey">Fifth key.</param>
        /// <param name="sixthKey">Sixth key.</param>
        /// <param name="seventhKey">Seventh key.</param>
        /// <param name="eighthKey">Eighth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="firstKey"/>, <paramref name="secondKey"/>, <paramref name="thirdKey"/>, <paramref name="fourthKey"/>, <paramref name="fifthKey"/>, <paramref name="sixthKey"/>, <paramref name="seventhKey"/>, <paramref name="eighthKey"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<(TSource First, TSource Second, TSource Third, TSource Fourth, TSource Fifth, TSource Sixth, TSource Seventh, TSource Eighth)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey firstKey,
                TKey secondKey,
                TKey thirdKey,
                TKey fourthKey,
                TKey fifthKey,
                TKey sixthKey,
                TKey seventhKey,
                TKey eighthKey,
                Func<TSource, TKey> keySelector)
        {
            return BatchBy(source,
                           firstKey,
                           secondKey,
                           thirdKey,
                           fourthKey,
                           fifthKey,
                           sixthKey,
                           seventhKey,
                           eighthKey,
                           keySelector, null);
        }

    }
}
