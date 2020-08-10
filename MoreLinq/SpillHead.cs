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
        /// TODO
        /// </summary>

        public static IEnumerable<(T Head, T Item)>
            SpillHead<T>(this IEnumerable<T> source) =>
            source.SpillHead(h => h, ValueTuple.Create);

        /// <summary>
        /// TODO
        /// </summary>

        public static IEnumerable<TResult>
            SpillHead<T, TResult>(
                this IEnumerable<T> source,
                Func<T, T, TResult> resultSelector) =>
            source.SpillHead(h => h, resultSelector);

        /// <summary>
        /// TODO
        /// </summary>

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
        /// TODO
        /// </summary>

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
        /// TODO
        /// </summary>

        public static IEnumerable<TResult>
            SpillHead<T, THead, TResult>(
                this IEnumerable<T> source,
                int count,
                Func<List<T>, THead> headerSelector,
                Func<THead, T, int, TResult> resultSelector) =>
            source.SpillHead((_, i) => i < count,
                             headerSelector, resultSelector);

        /// <summary>
        /// TODO
        /// </summary>

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
        /// TODO
        /// </summary>

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
        /// TODO
        /// </summary>

        public static IEnumerable<TResult>
            SpillHead<T, TMatch, THead, TResult>(
                this IEnumerable<T> source,
                Func<T, (bool, TMatch)> chooser,
                Func<List<TMatch>, THead> headerSelector,
                Func<THead, T, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (chooser == null) throw new ArgumentNullException(nameof(chooser));
            if (headerSelector == null) throw new ArgumentNullException(nameof(headerSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SpillHead((e, _) => chooser(e),
                                    headerSelector,
                                    (h, e, _) => resultSelector(h, e));
        }

        /// <summary>
        /// TODO
        /// </summary>

        public static IEnumerable<TResult>
            SpillHead<T, TMatch, THead, TResult>(
                this IEnumerable<T> source,
                Func<T, int, (bool, TMatch)> chooser,
                Func<List<TMatch>, THead> headerSelector,
                Func<THead, T, int, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (chooser == null) throw new ArgumentNullException(nameof(chooser));
            if (headerSelector == null) throw new ArgumentNullException(nameof(headerSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SpillHead(chooser,
                                    null,
                                    h => new List<TMatch>(),
                                    (a, h) => { a.Add(h); return a; },
                                    hs => headerSelector(hs ?? new List<TMatch>()),
                                    resultSelector);
        }

        /// <summary>
        /// TODO
        /// </summary>

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
        /// TODO
        /// </summary>

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
        /// TODO
        /// </summary>

        public static IEnumerable<TResult>
            SpillHead<T, TMatch, TState, THead, TResult>(
                this IEnumerable<T> source,
                Func<T, (bool, TMatch)> chooser,
                TState empty,
                Func<TMatch, TState> seeder,
                Func<TState, TMatch, TState> accumulator,
                Func<TState, THead> headerSelector,
                Func<THead, T, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (chooser == null) throw new ArgumentNullException(nameof(chooser));
            if (seeder == null) throw new ArgumentNullException(nameof(seeder));
            if (accumulator == null) throw new ArgumentNullException(nameof(accumulator));
            if (headerSelector == null) throw new ArgumentNullException(nameof(headerSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return source.SpillHead((e, _) => chooser(e),
                                    empty, seeder, accumulator, headerSelector,
                                    (h, e, _) => resultSelector(h, e));
        }

        /// <summary>
        /// TODO
        /// </summary>

        public static IEnumerable<TResult>
            SpillHead<T, TMatch, TState, THead, TResult>(
                this IEnumerable<T> source,
                Func<T, int, (bool, TMatch)> chooser,
                TState empty,
                Func<TMatch, TState> seeder,
                Func<TState, TMatch, TState> accumulator,
                Func<TState, THead> headerSelector,
                Func<THead, T, int, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (chooser == null) throw new ArgumentNullException(nameof(chooser));
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
                var (span, fm) = chooser(e.Current, i);
                var state = span ? seeder(fm) : empty;
                if (span)
                {
                    if (!e.MoveNext())
                        yield break;
                    for (; chooser(e.Current, ++i) is (true, var m); e.MoveNext())
                        state = accumulator(state, m);
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