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

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections.Generic;

    partial class ExperimentalEnumerable
    {
        /// <summary>
        /// TODO
        /// </summary>

        public static IEnumerable<(T Head, T Item)>
            SpillSpan<T>(this IEnumerable<T> source) =>
            source.SpillSpan(h => h, ValueTuple.Create);

        /// <summary>
        /// TODO
        /// </summary>

        public static IEnumerable<R>
            SpillSpan<T, R>(
                this IEnumerable<T> source,
                Func<T, T, R> resultSelector) =>
            source.SpillSpan(h => h, resultSelector);

        /// <summary>
        /// TODO
        /// </summary>

        public static IEnumerable<R>
            SpillSpan<T, H, R>(
                this IEnumerable<T> source,
                Func<T, H> headerSelector,
                Func<H, T, R> resultSelector) =>
            source.SpillSpan(1, h => headerSelector(h[0]),
                             (h, e, _) => resultSelector(h, e));

        /// <summary>
        /// TODO
        /// </summary>

        public static IEnumerable<R>
            SpillSpan<T, H, R>(
                this IEnumerable<T> source,
                int count,
                Func<List<T>, H> headerSelector,
                Func<H, T, R> resultSelector) =>
            source.SpillSpan(count, headerSelector,
                             (h, e, _) => resultSelector(h, e));

        /// <summary>
        /// TODO
        /// </summary>

        public static IEnumerable<R>
            SpillSpan<T, H, R>(
                this IEnumerable<T> source,
                int count,
                Func<List<T>, H> headerSelector,
                Func<H, T, int, R> resultSelector) =>
            source.SpillSpan((e, i) => i < count,
                             headerSelector,
                             resultSelector);

        /// <summary>
        /// TODO
        /// </summary>

        public static IEnumerable<R>
            SpillSpan<T, H, R>(
                this IEnumerable<T> source,
                Func<T, bool> predicate,
                Func<List<T>, H> headerSelector,
                Func<H, T, R> resultSelector) =>
            source.SpillSpan((e, _) => predicate(e),
                             headerSelector,
                             (h, e, _) => resultSelector(h, e));

        /// <summary>
        /// TODO
        /// </summary>

        public static IEnumerable<R>
            SpillSpan<T, H, R>(
                this IEnumerable<T> source,
                Func<T, int, bool> predicate,
                Func<List<T>, H> headerSelector,
                Func<H, T, int, R> resultSelector)
        {
            return _(); IEnumerable<R> _()
            {
                using var e = source.GetEnumerator();
                if (!e.MoveNext())
                    yield break;
                var i = 0;
                var items = new List<T>();
                for (; predicate(e.Current, i); e.MoveNext(), i++)
                    items.Add(e.Current);
                var header = headerSelector(items);
                items = null; // available for collection by GC
                i = 0;
                do { yield return resultSelector(header, e.Current, i++); }
                while (e.MoveNext());
            }
        }

        /// <summary>
        /// TODO
        /// </summary>

        public static IEnumerable<R>
            SpillSpan<T, A, H, R>(
                this IEnumerable<T> source,
                Func<T, bool> predicate,
                A empty,
                Func<T, A> seeder,
                Func<A, T, A> accumulator,
                Func<A, H> headerSelector,
                Func<H, T, R> resultSelector) =>
            source.SpillSpan(e => predicate(e) ? (true, e) : default,
                             empty, seeder, accumulator, headerSelector, resultSelector);

        /// <summary>
        /// TODO
        /// </summary>

        public static IEnumerable<R>
            SpillSpan<T, M, A, H, R>(
                this IEnumerable<T> source,
                Func<T, (bool, M)> chooser,
                A empty,
                Func<M, A> seeder,
                Func<A, M, A> accumulator,
                Func<A, H> headerSelector,
                Func<H, T, R> resultSelector)
        {
            return _(); IEnumerable<R> _()
            {
                using var e = source.GetEnumerator();
                if (!e.MoveNext())
                    yield break;
                var (span, fm) = chooser(e.Current);
                var state = span ? seeder(fm) : empty;
                if (span)
                {
                    if (!e.MoveNext())
                        yield break;
                    for (; chooser(e.Current) is (true, var m); e.MoveNext())
                        state = accumulator(state, m);
                }
                var header = headerSelector(state);
                state = default; // available for collection by GC
                do { yield return resultSelector(header, e.Current); }
                while (e.MoveNext());
            }
        }
    }
}
