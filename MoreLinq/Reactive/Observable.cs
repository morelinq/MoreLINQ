#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Atif Aziz. All rights reserved.
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

namespace MoreLinq.Reactive
{
    using System;
    using System.Collections.Generic;
    using Delegate = Delegating.Delegate;
    using static Assignment;

    /// <summary>
    /// Provides a set of static methods for writing in-memory queries over observable sequences.
    /// </summary>

    public static partial class Observable
    {
        /// <summary>
        /// Subscribes an element handler and a completion handler to an
        /// observable sequence.
        /// </summary>
        /// <typeparam name="T">Type of elements in <paramref name="source"/>.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">
        /// Action to invoke for each element in <paramref name="source"/>.</param>
        /// <param name="onError">
        /// Action to invoke upon exceptional termination of the
        /// <paramref name="source"/>.</param>
        /// <param name="onCompleted">
        /// Action to invoke upon graceful termination of <paramref name="source"/>.</param>
        /// <returns>The subscription, which when disposed, will unsubscribe
        /// from <paramref name="source"/>.</returns>

        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null) =>
            source == null
            ? throw new ArgumentNullException(nameof(source))
            : source.Subscribe(Delegate.Observer(onNext, onError, onCompleted));

        /// <summary>
        /// Projects each element of an observable sequence into a new form with
        /// the specified source and selector.
        /// </summary>
        /// <typeparam name="T">Type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">Type of projected elements.</typeparam>
        /// <param name="source">The observable sequence that is the source of
        /// projection.</param>
        /// <param name="selector">The function that projects elements in
        /// <paramref name="source"/> to results.</param>
        /// <returns>An observable sequence containing results projected from
        /// elements of <paramref name="source"/>.
        /// </returns>

        public static IObservable<TResult> Select<T, TResult>(this IObservable<T> source, Func<T, TResult> selector) =>
            Delegate.Observable<TResult>(o =>
                source.Subscribe(Delegate.Observer<T>(e => o.OnNext(selector(e)),
                                                      o.OnError, o.OnCompleted)));

        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>

        public static IObservable<T> Where<T>(this IObservable<T> source, Func<T, bool> predicate) =>
            Delegate.Observable<T>(o =>
                source.Subscribe(Delegate.Observer<T>(e => { if (predicate(e)) o.OnNext(e); },
                                 o.OnError, o.OnCompleted)));

        /// <summary>
        /// Applies an accumulator function over a sequence. The specified seed
        /// value is used as the initial accumulator value, and the specified
        /// function is used to select the result value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TAccumulate"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="seed"></param>
        /// <param name="func"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>

        public static IObservable<TResult> Aggregate<T, TAccumulate, TResult>(this IObservable<T> source, TAccumulate seed, Func<TAccumulate, T, TAccumulate> func, Func<TAccumulate, TResult> resultSelector) =>
            Delegate.Observable<TResult>(o =>
            {
                var accumulator = seed;
                IDisposable subscription = null;
                return subscription =
                    source.Subscribe(
                        Delegate.Observer<T>(
                            e =>
                            {
                                try
                                {
                                    accumulator = func(accumulator, e);
                                }
                                catch (Exception error)
                                {
                                    if (Set(ref subscription, null) is IDisposable s)
                                        s.Dispose();
                                    o.OnError(error);
                                }
                            },
                            e =>
                            {
                                accumulator = default;
                                o.OnError(e);
                            },
                            () =>
                            {
                                try
                                {
                                    o.OnNext(resultSelector(accumulator));
                                    accumulator = default;
                                    o.OnCompleted();
                                }
                                catch (Exception e)
                                {
                                    o.OnError(e);
                                }
                            }));
            });

        /// <summary>
        /// TODO
        /// </summary>

        public static IObservable<T> Distinct<T>(this IObservable<T> source) =>
            Distinct(source, null);

        /// <summary>
        /// TODO
        /// </summary>

        public static IObservable<T> Distinct<T>(this IObservable<T> source, IEqualityComparer<T> comparer) =>
            Delegate.Observable<T>(o =>
            {
                var seen = new HashSet<T>(comparer);
                return
                    source.Subscribe(
                        Delegate.Observer<T>(
                            e =>
                            {
                                if (seen.Add(e))
                                    o.OnNext(e);
                            },
                            e =>
                            {
                                seen = null;
                                o.OnError(e);
                            },
                            () =>
                            {
                                seen = null;
                                o.OnCompleted();
                            }));
            });

        /// <summary>
        /// TODO
        /// </summary>

        public static IObservable<int> Sum(this IObservable<int> source) =>
            source.Aggregate(0, (sum, x) => sum + x, sum => sum);

        /// <summary>
        /// TODO
        /// </summary>

        public static IObservable<int> Count<T>(this IObservable<T> source) =>
            source.Aggregate(0, (count, _) => count + 1, count => count);

        /// <summary>
        /// TODO
        /// </summary>

        public static IObservable<T[]> ToArray<T>(this IObservable<T> source) =>
            source.Aggregate(new List<T>(), (list, e) => { list.Add(e); return list; }, list => list.ToArray());

       /// <summary>
        /// TODO
        /// </summary>

        public static IObservable<T> Min<T>(this IObservable<T> source) where T : IComparable<T> =>
            source.Extremum(-1);

        /// <summary>
        /// TODO
        /// </summary>

        public static IObservable<T> Max<T>(this IObservable<T> source) where T : IComparable<T> =>
            source.Extremum(+1);

        static IObservable<T> Extremum<T>(this IObservable<T> source, int sign) where T : IComparable<T> =>
            source.Aggregate((Defined: false, Value: default(T)),
                             (x, e) => x.Defined ? (true, Math.Sign(e.CompareTo(x.Value)) == sign ? e : x.Value) : (true, e),
                             x => x.Defined ? x.Value : throw new InvalidOperationException());
    }
}
