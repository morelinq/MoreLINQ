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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, 
        /// <paramref name="keySelector"/>, <paramref name="resultSelector"/> or <paramref name="keyComparer"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/>.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey first,
            TKey second,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TResult> resultSelector,
            IEqualityComparer<TKey> keyComparer)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                first,
                second
            };

            return BatchBy(source, keys, keySelector, keyComparer)
                .Select(d => resultSelector(
                    d[first],
                    d[second]));
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, 
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey first,
            TKey second,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TResult> resultSelector)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                first,
                second
            };

            return BatchBy(source, keys, keySelector)
                .Select(d => resultSelector(
                    d[first],
                    d[second]));
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, 
        /// <paramref name="keySelector"/> or <paramref name="keyComparer"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/></exception>

        public static IEnumerable<(TSource first, TSource second)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey first,
                TKey second,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> keyComparer)
        {
            return BatchBy(source,
                           first,
                           second,
                           keySelector, ValueTuple.Create, keyComparer);
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<(TSource first, TSource second)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey first,
                TKey second,
                Func<TSource, TKey> keySelector)
        {
            return BatchBy(source,
                           first,
                           second,
                           keySelector, ValueTuple.Create);
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, 
        /// <paramref name="keySelector"/>, <paramref name="resultSelector"/> or <paramref name="keyComparer"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/>.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey first,
            TKey second,
            TKey third,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TResult> resultSelector,
            IEqualityComparer<TKey> keyComparer)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                first,
                second,
                third
            };

            return BatchBy(source, keys, keySelector, keyComparer)
                .Select(d => resultSelector(
                    d[first],
                    d[second],
                    d[third]));
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, 
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey first,
            TKey second,
            TKey third,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TResult> resultSelector)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                first,
                second,
                third
            };

            return BatchBy(source, keys, keySelector)
                .Select(d => resultSelector(
                    d[first],
                    d[second],
                    d[third]));
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, 
        /// <paramref name="keySelector"/> or <paramref name="keyComparer"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/></exception>

        public static IEnumerable<(TSource first, TSource second, TSource third)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey first,
                TKey second,
                TKey third,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> keyComparer)
        {
            return BatchBy(source,
                           first,
                           second,
                           third,
                           keySelector, ValueTuple.Create, keyComparer);
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<(TSource first, TSource second, TSource third)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey first,
                TKey second,
                TKey third,
                Func<TSource, TKey> keySelector)
        {
            return BatchBy(source,
                           first,
                           second,
                           third,
                           keySelector, ValueTuple.Create);
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, 
        /// <paramref name="keySelector"/>, <paramref name="resultSelector"/> or <paramref name="keyComparer"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/>.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey first,
            TKey second,
            TKey third,
            TKey fourth,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TResult> resultSelector,
            IEqualityComparer<TKey> keyComparer)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                first,
                second,
                third,
                fourth
            };

            return BatchBy(source, keys, keySelector, keyComparer)
                .Select(d => resultSelector(
                    d[first],
                    d[second],
                    d[third],
                    d[fourth]));
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, 
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey first,
            TKey second,
            TKey third,
            TKey fourth,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TResult> resultSelector)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                first,
                second,
                third,
                fourth
            };

            return BatchBy(source, keys, keySelector)
                .Select(d => resultSelector(
                    d[first],
                    d[second],
                    d[third],
                    d[fourth]));
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, 
        /// <paramref name="keySelector"/> or <paramref name="keyComparer"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/></exception>

        public static IEnumerable<(TSource first, TSource second, TSource third, TSource fourth)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey first,
                TKey second,
                TKey third,
                TKey fourth,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> keyComparer)
        {
            return BatchBy(source,
                           first,
                           second,
                           third,
                           fourth,
                           keySelector, ValueTuple.Create, keyComparer);
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<(TSource first, TSource second, TSource third, TSource fourth)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey first,
                TKey second,
                TKey third,
                TKey fourth,
                Func<TSource, TKey> keySelector)
        {
            return BatchBy(source,
                           first,
                           second,
                           third,
                           fourth,
                           keySelector, ValueTuple.Create);
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="fifth">fifth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, <paramref name="fifth"/>, 
        /// <paramref name="keySelector"/>, <paramref name="resultSelector"/> or <paramref name="keyComparer"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/>.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey first,
            TKey second,
            TKey third,
            TKey fourth,
            TKey fifth,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TSource, TResult> resultSelector,
            IEqualityComparer<TKey> keyComparer)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                first,
                second,
                third,
                fourth,
                fifth
            };

            return BatchBy(source, keys, keySelector, keyComparer)
                .Select(d => resultSelector(
                    d[first],
                    d[second],
                    d[third],
                    d[fourth],
                    d[fifth]));
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="fifth">fifth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, <paramref name="fifth"/>, 
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey first,
            TKey second,
            TKey third,
            TKey fourth,
            TKey fifth,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TSource, TResult> resultSelector)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                first,
                second,
                third,
                fourth,
                fifth
            };

            return BatchBy(source, keys, keySelector)
                .Select(d => resultSelector(
                    d[first],
                    d[second],
                    d[third],
                    d[fourth],
                    d[fifth]));
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="fifth">fifth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, <paramref name="fifth"/>, 
        /// <paramref name="keySelector"/> or <paramref name="keyComparer"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/></exception>

        public static IEnumerable<(TSource first, TSource second, TSource third, TSource fourth, TSource fifth)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey first,
                TKey second,
                TKey third,
                TKey fourth,
                TKey fifth,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> keyComparer)
        {
            return BatchBy(source,
                           first,
                           second,
                           third,
                           fourth,
                           fifth,
                           keySelector, ValueTuple.Create, keyComparer);
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="fifth">fifth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, <paramref name="fifth"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<(TSource first, TSource second, TSource third, TSource fourth, TSource fifth)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey first,
                TKey second,
                TKey third,
                TKey fourth,
                TKey fifth,
                Func<TSource, TKey> keySelector)
        {
            return BatchBy(source,
                           first,
                           second,
                           third,
                           fourth,
                           fifth,
                           keySelector, ValueTuple.Create);
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="fifth">fifth key.</param>
        /// <param name="sixth">sixth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, <paramref name="fifth"/>, <paramref name="sixth"/>, 
        /// <paramref name="keySelector"/>, <paramref name="resultSelector"/> or <paramref name="keyComparer"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/>.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey first,
            TKey second,
            TKey third,
            TKey fourth,
            TKey fifth,
            TKey sixth,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TResult> resultSelector,
            IEqualityComparer<TKey> keyComparer)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                first,
                second,
                third,
                fourth,
                fifth,
                sixth
            };

            return BatchBy(source, keys, keySelector, keyComparer)
                .Select(d => resultSelector(
                    d[first],
                    d[second],
                    d[third],
                    d[fourth],
                    d[fifth],
                    d[sixth]));
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="fifth">fifth key.</param>
        /// <param name="sixth">sixth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, <paramref name="fifth"/>, <paramref name="sixth"/>, 
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey first,
            TKey second,
            TKey third,
            TKey fourth,
            TKey fifth,
            TKey sixth,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TResult> resultSelector)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                first,
                second,
                third,
                fourth,
                fifth,
                sixth
            };

            return BatchBy(source, keys, keySelector)
                .Select(d => resultSelector(
                    d[first],
                    d[second],
                    d[third],
                    d[fourth],
                    d[fifth],
                    d[sixth]));
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="fifth">fifth key.</param>
        /// <param name="sixth">sixth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, <paramref name="fifth"/>, <paramref name="sixth"/>, 
        /// <paramref name="keySelector"/> or <paramref name="keyComparer"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/></exception>

        public static IEnumerable<(TSource first, TSource second, TSource third, TSource fourth, TSource fifth, TSource sixth)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey first,
                TKey second,
                TKey third,
                TKey fourth,
                TKey fifth,
                TKey sixth,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> keyComparer)
        {
            return BatchBy(source,
                           first,
                           second,
                           third,
                           fourth,
                           fifth,
                           sixth,
                           keySelector, ValueTuple.Create, keyComparer);
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="fifth">fifth key.</param>
        /// <param name="sixth">sixth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, <paramref name="fifth"/>, <paramref name="sixth"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<(TSource first, TSource second, TSource third, TSource fourth, TSource fifth, TSource sixth)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey first,
                TKey second,
                TKey third,
                TKey fourth,
                TKey fifth,
                TKey sixth,
                Func<TSource, TKey> keySelector)
        {
            return BatchBy(source,
                           first,
                           second,
                           third,
                           fourth,
                           fifth,
                           sixth,
                           keySelector, ValueTuple.Create);
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="fifth">fifth key.</param>
        /// <param name="sixth">sixth key.</param>
        /// <param name="seventh">seventh key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, <paramref name="fifth"/>, <paramref name="sixth"/>, <paramref name="seventh"/>, 
        /// <paramref name="keySelector"/>, <paramref name="resultSelector"/> or <paramref name="keyComparer"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/>.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey first,
            TKey second,
            TKey third,
            TKey fourth,
            TKey fifth,
            TKey sixth,
            TKey seventh,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> resultSelector,
            IEqualityComparer<TKey> keyComparer)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                seventh
            };

            return BatchBy(source, keys, keySelector, keyComparer)
                .Select(d => resultSelector(
                    d[first],
                    d[second],
                    d[third],
                    d[fourth],
                    d[fifth],
                    d[sixth],
                    d[seventh]));
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="fifth">fifth key.</param>
        /// <param name="sixth">sixth key.</param>
        /// <param name="seventh">seventh key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, <paramref name="fifth"/>, <paramref name="sixth"/>, <paramref name="seventh"/>, 
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey first,
            TKey second,
            TKey third,
            TKey fourth,
            TKey fifth,
            TKey sixth,
            TKey seventh,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> resultSelector)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                seventh
            };

            return BatchBy(source, keys, keySelector)
                .Select(d => resultSelector(
                    d[first],
                    d[second],
                    d[third],
                    d[fourth],
                    d[fifth],
                    d[sixth],
                    d[seventh]));
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="fifth">fifth key.</param>
        /// <param name="sixth">sixth key.</param>
        /// <param name="seventh">seventh key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, <paramref name="fifth"/>, <paramref name="sixth"/>, <paramref name="seventh"/>, 
        /// <paramref name="keySelector"/> or <paramref name="keyComparer"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/></exception>

        public static IEnumerable<(TSource first, TSource second, TSource third, TSource fourth, TSource fifth, TSource sixth, TSource seventh)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey first,
                TKey second,
                TKey third,
                TKey fourth,
                TKey fifth,
                TKey sixth,
                TKey seventh,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> keyComparer)
        {
            return BatchBy(source,
                           first,
                           second,
                           third,
                           fourth,
                           fifth,
                           sixth,
                           seventh,
                           keySelector, ValueTuple.Create, keyComparer);
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="fifth">fifth key.</param>
        /// <param name="sixth">sixth key.</param>
        /// <param name="seventh">seventh key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, <paramref name="fifth"/>, <paramref name="sixth"/>, <paramref name="seventh"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<(TSource first, TSource second, TSource third, TSource fourth, TSource fifth, TSource sixth, TSource seventh)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey first,
                TKey second,
                TKey third,
                TKey fourth,
                TKey fifth,
                TKey sixth,
                TKey seventh,
                Func<TSource, TKey> keySelector)
        {
            return BatchBy(source,
                           first,
                           second,
                           third,
                           fourth,
                           fifth,
                           sixth,
                           seventh,
                           keySelector, ValueTuple.Create);
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="fifth">fifth key.</param>
        /// <param name="sixth">sixth key.</param>
        /// <param name="seventh">seventh key.</param>
        /// <param name="eighth">eighth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, <paramref name="fifth"/>, <paramref name="sixth"/>, <paramref name="seventh"/>, <paramref name="eighth"/>, 
        /// <paramref name="keySelector"/>, <paramref name="resultSelector"/> or <paramref name="keyComparer"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/>.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey first,
            TKey second,
            TKey third,
            TKey fourth,
            TKey fifth,
            TKey sixth,
            TKey seventh,
            TKey eighth,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> resultSelector,
            IEqualityComparer<TKey> keyComparer)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                seventh,
                eighth
            };

            return BatchBy(source, keys, keySelector, keyComparer)
                .Select(d => resultSelector(
                    d[first],
                    d[second],
                    d[third],
                    d[fourth],
                    d[fifth],
                    d[sixth],
                    d[seventh],
                    d[eighth]));
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="fifth">fifth key.</param>
        /// <param name="sixth">sixth key.</param>
        /// <param name="seventh">seventh key.</param>
        /// <param name="eighth">eighth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="resultSelector">The function used to project the buckets.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, <paramref name="fifth"/>, <paramref name="sixth"/>, <paramref name="seventh"/>, <paramref name="eighth"/>, 
        /// <paramref name="keySelector"/> or <paramref name="resultSelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<TResult> BatchBy<TKey, TSource, TResult>(
            this IEnumerable<TSource> source,
            TKey first,
            TKey second,
            TKey third,
            TKey fourth,
            TKey fifth,
            TKey sixth,
            TKey seventh,
            TKey eighth,
            Func<TSource, TKey> keySelector,
            Func<TSource, TSource, TSource, TSource, TSource, TSource, TSource, TSource, TResult> resultSelector)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var keys = new []
            {
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                seventh,
                eighth
            };

            return BatchBy(source, keys, keySelector)
                .Select(d => resultSelector(
                    d[first],
                    d[second],
                    d[third],
                    d[fourth],
                    d[fifth],
                    d[sixth],
                    d[seventh],
                    d[eighth]));
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="fifth">fifth key.</param>
        /// <param name="sixth">sixth key.</param>
        /// <param name="seventh">seventh key.</param>
        /// <param name="eighth">eighth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, <paramref name="fifth"/>, <paramref name="sixth"/>, <paramref name="seventh"/>, <paramref name="eighth"/>, 
        /// <paramref name="keySelector"/> or <paramref name="keyComparer"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys relatively to <paramref name="keyComparer"/></exception>

        public static IEnumerable<(TSource first, TSource second, TSource third, TSource fourth, TSource fifth, TSource sixth, TSource seventh, TSource eighth)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey first,
                TKey second,
                TKey third,
                TKey fourth,
                TKey fifth,
                TKey sixth,
                TKey seventh,
                TKey eighth,
                Func<TSource, TKey> keySelector,
                IEqualityComparer<TKey> keyComparer)
        {
            return BatchBy(source,
                           first,
                           second,
                           third,
                           fourth,
                           fifth,
                           sixth,
                           seventh,
                           eighth,
                           keySelector, ValueTuple.Create, keyComparer);
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
        /// <param name="first">first key.</param>
        /// <param name="second">second key.</param>
        /// <param name="third">third key.</param>
        /// <param name="fourth">fourth key.</param>
        /// <param name="fifth">fifth key.</param>
        /// <param name="sixth">sixth key.</param>
        /// <param name="seventh">seventh key.</param>
        /// <param name="eighth">eighth key.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <returns>The build up sequence of projected buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>,
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="fourth"/>, <paramref name="fifth"/>, <paramref name="sixth"/>, <paramref name="seventh"/>, <paramref name="eighth"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">There is some duplicate keys.</exception>

        public static IEnumerable<(TSource first, TSource second, TSource third, TSource fourth, TSource fifth, TSource sixth, TSource seventh, TSource eighth)>
            BatchBy<TKey, TSource>(
                this IEnumerable<TSource> source,
                TKey first,
                TKey second,
                TKey third,
                TKey fourth,
                TKey fifth,
                TKey sixth,
                TKey seventh,
                TKey eighth,
                Func<TSource, TKey> keySelector)
        {
            return BatchBy(source,
                           first,
                           second,
                           third,
                           fourth,
                           fifth,
                           sixth,
                           seventh,
                           eighth,
                           keySelector, ValueTuple.Create);
        }

    }
}
