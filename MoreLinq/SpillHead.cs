#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2020 Atif Aziz. All rights reserved.
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

    partial class MoreEnumerable
    {
        /// <summary>
        /// Couples the first/head element of the sequence exclusively with
        /// the remainder elements of the sequence.
        /// </summary>
        /// <typeparam name="T"> Type of source sequence elements.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>
        /// A sequence with the head element coupled together with the
        /// remainder elements of the source sequence.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<(T Head, T Item)>
            SpillHead<T>(this IEnumerable<T> source) =>
            source.SpillHead(h => h, ValueTuple.Create);

        /// <summary>
        /// Projects the first/head element of the sequence exclusively with
        /// the remainder elements of the sequence. An additional argument
        /// specifies a function that receives an element of sequence
        /// together with the first/head element and returns a projection.
        /// </summary>
        /// <typeparam name="T"> Type of source sequence elements.</typeparam>
        /// <typeparam name="TResult">
        /// Type of elements of the returned sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="resultSelector">Function that projects a result
        /// given the head and one of the remainder elements.</param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult>
            SpillHead<T, TResult>(
                this IEnumerable<T> source,
                Func<T, T, TResult> resultSelector) =>
            source.SpillHead(h => h, resultSelector);

        /// <summary>
        /// Projects the first/head element of the sequence exclusively with
        /// the remainder elements of the sequence. Additional arguments
        /// specify functions to project the head as well as the elements
        /// of the resulting sequence.
        /// </summary>
        /// <typeparam name="T"> Type of source sequence elements.</typeparam>
        /// <typeparam name="THead">Type of head projection.</typeparam>
        /// <typeparam name="TResult">
        /// Type of elements of the returned sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="headerSelector">Function that projects an
        /// intermediate head representation.
        /// </param>
        /// <param name="resultSelector">Function that projects a result
        /// given the head projection returned by <paramref name="headerSelector"/>
        /// and one of the remainder elements.
        /// </param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult>
            SpillHead<T, THead, TResult>(
                this IEnumerable<T> source,
                Func<T, THead> headerSelector,
                Func<THead, T, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (headerSelector == null) throw new ArgumentNullException(nameof(headerSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SpillHead((_, i) => i == 0,
                                    default, h => h, (a, _) => a,
                                    headerSelector,
                                    (h, e, _) => resultSelector(h, e));
        }

        /// <summary>
        /// Projects the head elements of the sequence exclusively with the
        /// remainder elements of the sequence. Additional arguments specify
        /// functions to project the head as well as the elements of the
        /// resulting sequence.
        /// </summary>
        /// <typeparam name="T"> Type of source sequence elements.</typeparam>
        /// <typeparam name="THead">Type of head projection.</typeparam>
        /// <typeparam name="TResult">
        /// Type of elements of the returned sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="count">Count head elements to collect.</param>
        /// <param name="headerSelector">Function that projects an
        /// intermediate head representation given a list of exactly
        /// <paramref name="count"/> head elements.
        /// </param>
        /// <param name="resultSelector">Function that projects a result
        /// given the head projection returned by <paramref name="headerSelector"/>
        /// and one of the remainder elements.
        /// </param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult>
            SpillHead<T, THead, TResult>(
                this IEnumerable<T> source,
                int count,
                Func<List<T>, THead> headerSelector,
                Func<THead, T, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            // TODO validate count < 1?
            if (headerSelector == null) throw new ArgumentNullException(nameof(headerSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SpillHead(count, headerSelector,
                                    (h, e, _) => resultSelector(h, e));
        }

        /// <summary>
        /// Projects the head elements of the sequence exclusively with the
        /// remainder elements of the sequence. Additional arguments specify
        /// functions to project the head as well as the elements (along with
        /// their index) of the resulting sequence.
        /// </summary>
        /// <typeparam name="T"> Type of source sequence elements.</typeparam>
        /// <typeparam name="THead">Type of head projection.</typeparam>
        /// <typeparam name="TResult">
        /// Type of elements of the returned sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="count">Count head elements to collect.</param>
        /// <param name="headerSelector">Function that projects an
        /// intermediate head representation given a list of exactly
        /// <paramref name="count"/> head elements.
        /// </param>
        /// <param name="resultSelector">Function that projects a result given
        /// the head projection returned by <paramref name="headerSelector"/>
        /// and one of the remainder elements along with its zero-based index
        /// (where zero is the first non-head element).
        /// </param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult>
            SpillHead<T, THead, TResult>(
                this IEnumerable<T> source,
                int count,
                Func<List<T>, THead> headerSelector,
                Func<THead, T, int, TResult> resultSelector) =>
            source.SpillHead((_, i) => i < count,
                             headerSelector, resultSelector);

        /// <summary>
        /// Projects the head elements of the sequence exclusively with the
        /// remainder elements of the sequence. Additional arguments specify
        /// functions to delineate header elements, project the header
        /// elements as well as the elements of the resulting sequence.
        /// </summary>
        /// <typeparam name="T"> Type of source sequence elements.</typeparam>
        /// <typeparam name="THead">Type of head projection.</typeparam>
        /// <typeparam name="TResult">
        /// Type of elements of the returned sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="predicate">Function that determines whether an
        /// element of the source sequence is at the head.</param>
        /// <param name="headerSelector">Function that projects an
        /// intermediate head representation given a list of head elements.
        /// </param>
        /// <param name="resultSelector">Function that projects a result
        /// given the head projection returned by <paramref name="headerSelector"/>
        /// and one of the remainder elements.
        /// </param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult>
            SpillHead<T, THead, TResult>(
                this IEnumerable<T> source,
                Func<T, bool> predicate,
                Func<List<T>, THead> headerSelector,
                Func<THead, T, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (headerSelector == null) throw new ArgumentNullException(nameof(headerSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SpillHead((e, _) => predicate(e),
                                    headerSelector,
                                    (h, e, _) => resultSelector(h, e));
        }

        /// <summary>
        /// Projects the head elements of the sequence exclusively with
        /// the remainder elements of the sequence. Additional arguments
        /// specify functions to delineate header elements (possibly based
        /// on their index), project the header elements as well as the
        /// elements of the resulting sequence.
        /// </summary>
        /// <typeparam name="T"> Type of source sequence elements.</typeparam>
        /// <typeparam name="THead">Type of head projection.</typeparam>
        /// <typeparam name="TResult">
        /// Type of elements of the returned sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="predicate">Function that determines whether an
        /// element of the source sequence (along with its zero-based index)
        /// is at the head.</param>
        /// <param name="headerSelector">Function that projects an
        /// intermediate head representation given a list of head elements.
        /// </param>
        /// <param name="resultSelector">Function that projects a result
        /// given the head projection returned by <paramref name="headerSelector"/>
        /// and one of the remainder elements along with its zero-based index.
        /// </param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult>
            SpillHead<T, THead, TResult>(
                this IEnumerable<T> source,
                Func<T, int, bool> predicate,
                Func<List<T>, THead> headerSelector,
                Func<THead, T, int, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (headerSelector == null) throw new ArgumentNullException(nameof(headerSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SpillHead((e, i) => predicate(e, i) ? (true, e) : default,
                                    headerSelector, resultSelector);
        }

        /// <summary>
        /// Projects the head elements of the sequence exclusively with
        /// the remainder elements of the sequence. Additional arguments
        /// specify functions to choose and delineate header elements,
        /// project the header elements as well as the elements of the
        /// resulting sequence.
        /// </summary>
        /// <typeparam name="T"> Type of source sequence elements.</typeparam>
        /// <typeparam name="TMatch">Type of head match.</typeparam>
        /// <typeparam name="THead">Type of head projection.</typeparam>
        /// <typeparam name="TResult">
        /// Type of elements of the returned sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="matcher">Function that determines whether an
        /// element of the source sequence is a match for a head.</param>
        /// <param name="headerSelector">Function that projects an
        /// intermediate head representation given a list of head matches.
        /// </param>
        /// <param name="resultSelector">Function that projects a result
        /// given the head projection returned by <paramref name="headerSelector"/>
        /// and one of the remainder elements.
        /// </param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult>
            SpillHead<T, TMatch, THead, TResult>(
                this IEnumerable<T> source,
                Func<T, (bool, TMatch)> matcher,
                Func<List<TMatch>, THead> headerSelector,
                Func<THead, T, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (matcher == null) throw new ArgumentNullException(nameof(matcher));
            if (headerSelector == null) throw new ArgumentNullException(nameof(headerSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SpillHead((e, _) => matcher(e),
                                    headerSelector,
                                    (h, e, _) => resultSelector(h, e));
        }

        /// <summary>
        /// Projects the head elements of the sequence exclusively with
        /// the remainder elements of the sequence. Additional arguments
        /// specify functions to choose and delineate header elements
        /// (possibly based on their index), project the header elements
        /// as well as the elements of the resulting sequence.
        /// </summary>
        /// <typeparam name="T"> Type of source sequence elements.</typeparam>
        /// <typeparam name="TMatch">Type of head match.</typeparam>
        /// <typeparam name="THead">Type of head projection.</typeparam>
        /// <typeparam name="TResult">
        /// Type of elements of the returned sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="matcher">Function that determines whether an
        /// element of the source sequence (along with its zero-based index)
        /// is a match for a head.</param>
        /// <param name="headerSelector">Function that projects an
        /// intermediate head representation given a list of head matches.
        /// </param>
        /// <param name="resultSelector">Function that projects a result
        /// given the head projection returned by <paramref name="headerSelector"/>
        /// and one of the remainder elements along with its zero-based index.
        /// </param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult>
            SpillHead<T, TMatch, THead, TResult>(
                this IEnumerable<T> source,
                Func<T, int, (bool, TMatch)> matcher,
                Func<List<TMatch>, THead> headerSelector,
                Func<THead, T, int, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (matcher == null) throw new ArgumentNullException(nameof(matcher));
            if (headerSelector == null) throw new ArgumentNullException(nameof(headerSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SpillHead(matcher,
                                    null,
                                    h => new List<TMatch> { h },
                                    (a, h) => { a.Add(h); return a; },
                                    hs => headerSelector(hs ?? new List<TMatch>()),
                                    resultSelector);
        }

        /// <summary>
        /// Projects the head elements of the sequence exclusively with
        /// the remainder elements of the sequence. Additional arguments
        /// specify functions to delineate header elements, fold and
        /// project the header elements as well as project elements of
        /// the resulting sequence.
        /// </summary>
        /// <typeparam name="T"> Type of source sequence elements.</typeparam>
        /// <typeparam name="TState">Type of head accumulator state.</typeparam>
        /// <typeparam name="THead">Type of head projection.</typeparam>
        /// <typeparam name="TResult">
        /// Type of elements of the returned sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="predicate">Function that determines whether an
        /// element of the source sequence is at the head.</param>
        /// <param name="empty">The empty state when head is absent.</param>
        /// <param name="seeder">Function that seeds the accumulation state
        /// with the initial head element.</param>
        /// <param name="accumulator">Function that folds subsequent head
        /// elements into the state.</param>
        /// <param name="headerSelector">Function that projects a single
        /// head representation given the accumulated state of head elements.
        /// </param>
        /// <param name="resultSelector">Function that projects a result
        /// given the head projection returned by <paramref name="headerSelector"/>
        /// and one of the remainder elements.
        /// </param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult>
            SpillHead<T, TState, THead, TResult>(
                this IEnumerable<T> source,
                Func<T, bool> predicate,
                TState empty,
                Func<T, TState> seeder,
                Func<TState, T, TState> accumulator,
                Func<TState, THead> headerSelector,
                Func<THead, T, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (seeder == null) throw new ArgumentNullException(nameof(seeder));
            if (accumulator == null) throw new ArgumentNullException(nameof(accumulator));
            if (headerSelector == null) throw new ArgumentNullException(nameof(headerSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SpillHead((e, _) => predicate(e),
                                    empty, seeder, accumulator, headerSelector,
                                    (h, e, _) => resultSelector(h, e));
        }

        /// <summary>
        /// Projects the head elements of the sequence exclusively with
        /// the remainder elements of the sequence. Additional arguments
        /// specify functions to delineate header elements (possibly based on
        /// their index), fold and project the header elements as well as
        /// project elements of the resulting sequence.
        /// </summary>
        /// <typeparam name="T"> Type of source sequence elements.</typeparam>
        /// <typeparam name="TState">Type of head accumulator state.</typeparam>
        /// <typeparam name="THead">Type of head projection.</typeparam>
        /// <typeparam name="TResult">
        /// Type of elements of the returned sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="predicate">Function that determines whether an
        /// element of the source sequence (along with its zero-based index)
        /// is at the head.</param>
        /// <param name="empty">The empty state when head is absent.</param>
        /// <param name="seeder">Function that seeds the accumulation state
        /// with the initial head element.</param>
        /// <param name="accumulator">Function that folds subsequent head
        /// elements into the state.</param>
        /// <param name="headerSelector">Function that projects a single
        /// head representation given the accumulated state of head elements.
        /// </param>
        /// <param name="resultSelector">Function that projects a result
        /// given the head projection returned by <paramref name="headerSelector"/>
        /// and one of the remainder elements along with its zero-based index.
        /// </param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult>
            SpillHead<T, TState, THead, TResult>(
                this IEnumerable<T> source,
                Func<T, int, bool> predicate,
                TState empty,
                Func<T, TState> seeder,
                Func<TState, T, TState> accumulator,
                Func<TState, THead> headerSelector,
                Func<THead, T, int, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (seeder == null) throw new ArgumentNullException(nameof(seeder));
            if (accumulator == null) throw new ArgumentNullException(nameof(accumulator));
            if (headerSelector == null) throw new ArgumentNullException(nameof(headerSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SpillHead((e, i) => predicate(e, i) ? (true, e) : default,
                                    empty, seeder, accumulator, headerSelector, resultSelector);
        }

        /// <summary>
        /// Projects the head elements of the sequence exclusively with
        /// the remainder elements of the sequence. Additional arguments
        /// specify functions to choose and delineate header elements,
        /// fold and project the header elements as well as project
        /// elements of the resulting sequence.
        /// </summary>
        /// <typeparam name="T"> Type of source sequence elements.</typeparam>
        /// <typeparam name="TMatch">Type of head match.</typeparam>
        /// <typeparam name="TState">Type of head accumulator state.</typeparam>
        /// <typeparam name="THead">Type of head projection.</typeparam>
        /// <typeparam name="TResult">
        /// Type of elements of the returned sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="matcher">Function that determines whether an
        /// element of the source sequence is a match for a head.</param>
        /// <param name="empty">The empty state when head is absent.</param>
        /// <param name="seeder">Function that seeds the accumulation state
        /// with the initial head element.</param>
        /// <param name="accumulator">Function that folds subsequent head
        /// elements into the state.</param>
        /// <param name="headerSelector">Function that projects a single
        /// head representation given the accumulated state of head elements.
        /// </param>
        /// <param name="resultSelector">Function that projects a result
        /// given the head projection returned by <paramref name="headerSelector"/>
        /// and one of the remainder elements.
        /// </param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult>
            SpillHead<T, TMatch, TState, THead, TResult>(
                this IEnumerable<T> source,
                Func<T, (bool, TMatch)> matcher,
                TState empty,
                Func<TMatch, TState> seeder,
                Func<TState, TMatch, TState> accumulator,
                Func<TState, THead> headerSelector,
                Func<THead, T, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (matcher == null) throw new ArgumentNullException(nameof(matcher));
            if (seeder == null) throw new ArgumentNullException(nameof(seeder));
            if (accumulator == null) throw new ArgumentNullException(nameof(accumulator));
            if (headerSelector == null) throw new ArgumentNullException(nameof(headerSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SpillHead((e, _) => matcher(e),
                                    empty, seeder, accumulator, headerSelector,
                                    (h, e, _) => resultSelector(h, e));
        }

        /// <summary>
        /// Projects the head elements of the sequence exclusively with
        /// the remainder elements of the sequence. Additional arguments
        /// specify functions to choose and delineate header elements
        /// (possibly based on their index), fold and project the header
        /// elements as well as project elements of the resulting sequence.
        /// </summary>
        /// <typeparam name="T"> Type of source sequence elements.</typeparam>
        /// <typeparam name="TMatch">Type of head match.</typeparam>
        /// <typeparam name="TState">Type of head accumulator state.</typeparam>
        /// <typeparam name="THead">Type of head projection.</typeparam>
        /// <typeparam name="TResult">
        /// Type of elements of the returned sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="matcher">Function that determines whether an
        /// element of the source sequence (along with its zero-based index)
        /// is a match for a head.</param>
        /// <param name="empty">The empty state when head is absent.</param>
        /// <param name="seeder">Function that seeds the accumulation state
        /// with the initial head element.</param>
        /// <param name="accumulator">Function that folds subsequent head
        /// elements into the state.</param>
        /// <param name="headerSelector">Function that projects a single
        /// head representation given the accumulated state of head elements.
        /// </param>
        /// <param name="resultSelector">Function that projects a result
        /// given the head projection returned by <paramref name="headerSelector"/>
        /// and one of the remainder elements along with its zero-based index.
        /// </param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult>
            SpillHead<T, TMatch, TState, THead, TResult>(
                this IEnumerable<T> source,
                Func<T, int, (bool, TMatch)> matcher,
                TState empty,
                Func<TMatch, TState> seeder,
                Func<TState, TMatch, TState> accumulator,
                Func<TState, THead> headerSelector,
                Func<THead, T, int, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (matcher == null) throw new ArgumentNullException(nameof(matcher));
            if (seeder == null) throw new ArgumentNullException(nameof(seeder));
            if (accumulator == null) throw new ArgumentNullException(nameof(accumulator));
            if (headerSelector == null) throw new ArgumentNullException(nameof(headerSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e = source.GetEnumerator();
                if (!e.MoveNext())
                    yield break;
                var i = 0;
                var (span, fm) = matcher(e.Current, i);
                var state = span ? seeder(fm) : empty;
                if (span)
                {
                    for (;;)
                    {
                        if (!e.MoveNext())
                            yield break;
                        if (matcher(e.Current, ++i) is (true, var m))
                            state = accumulator(state, m);
                        else
                            break;
                    }
                }
                var header = headerSelector(state);
                state = default; // available for collection by GC
                i = 0;
                do { yield return resultSelector(header, e.Current, i++); }
                while (e.MoveNext());
            }
        }
    }
}
