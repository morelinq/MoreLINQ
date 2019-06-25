#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Atif Aziz. All rights reserved.
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
        /// Applies two accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TState1">The type of the state of the first accumulator.</typeparam>
        /// <typeparam name="TState2">The type of the state of the second accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="seed1">The seed value for the first accumulator.</param>
        /// <param name="accumulator1">The first accumulator.</param>
        /// <param name="seed2">The seed value for the second accumulator.</param>
        /// <param name="accumulator2">The second accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TResult Aggregate<T, TState1, TState2, TResult>(
            this IEnumerable<T> source,
            TState1 seed1, Func<TState1, T, TState1> accumulator1,
            TState2 seed2, Func<TState2, T, TState2> accumulator2,
            Func<TState1, TState2, TResult> resultSelector) =>
            source.Aggregate(
                (seed1, seed2),
                (s, e) => (accumulator1(s.Item1, e), accumulator2(s.Item2, e)),
                s => resultSelector(s.Item1, s.Item2));
    }
}

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections.Generic;
    using Reactive;

    static partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Applies two accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult1">The type of the result of the first accumulator.</typeparam>
        /// <typeparam name="TResult2">The type of the result of the second accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="aggregatorSelector1">The first accumulator.</param>
        /// <param name="aggregatorSelector2">The second accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TResult Aggregate<T, TResult1, TResult2, TResult>(
            this IEnumerable<T> source,
            Func<IObservable<T>, IObservable<TResult1>> aggregatorSelector1,
            Func<IObservable<T>, IObservable<TResult2>> aggregatorSelector2,
            Func<TResult1, TResult2, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (aggregatorSelector1 == null) throw new ArgumentNullException(nameof(aggregatorSelector1));
            if (aggregatorSelector2 == null) throw new ArgumentNullException(nameof(aggregatorSelector2));

            var r1 = new (bool, TResult1)[1];
            var r2 = new (bool, TResult2)[1];

            var subject = new Subject<T>();

            using (SubscribeSingle(aggregatorSelector1, subject, r1, nameof(aggregatorSelector1)))
            using (SubscribeSingle(aggregatorSelector2, subject, r2, nameof(aggregatorSelector2)))
            {
                foreach (var item in source)
                    subject.OnNext(item);

                subject.OnCompleted();
            }

            return resultSelector
            (
                GetAggregateResult(r1[0], nameof(aggregatorSelector1)),
                GetAggregateResult(r2[0], nameof(aggregatorSelector2))
            );
        }
    }
}

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Applies three accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TState1">The type of the state of the first accumulator.</typeparam>
        /// <typeparam name="TState2">The type of the state of the second accumulator.</typeparam>
        /// <typeparam name="TState3">The type of the state of the third accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="seed1">The seed value for the first accumulator.</param>
        /// <param name="accumulator1">The first accumulator.</param>
        /// <param name="seed2">The seed value for the second accumulator.</param>
        /// <param name="accumulator2">The second accumulator.</param>
        /// <param name="seed3">The seed value for the third accumulator.</param>
        /// <param name="accumulator3">The third accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TResult Aggregate<T, TState1, TState2, TState3, TResult>(
            this IEnumerable<T> source,
            TState1 seed1, Func<TState1, T, TState1> accumulator1,
            TState2 seed2, Func<TState2, T, TState2> accumulator2,
            TState3 seed3, Func<TState3, T, TState3> accumulator3,
            Func<TState1, TState2, TState3, TResult> resultSelector) =>
            source.Aggregate(
                (seed1, seed2, seed3),
                (s, e) => (accumulator1(s.Item1, e), accumulator2(s.Item2, e), accumulator3(s.Item3, e)),
                s => resultSelector(s.Item1, s.Item2, s.Item3));
    }
}

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections.Generic;
    using Reactive;

    static partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Applies three accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult1">The type of the result of the first accumulator.</typeparam>
        /// <typeparam name="TResult2">The type of the result of the second accumulator.</typeparam>
        /// <typeparam name="TResult3">The type of the result of the third accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="aggregatorSelector1">The first accumulator.</param>
        /// <param name="aggregatorSelector2">The second accumulator.</param>
        /// <param name="aggregatorSelector3">The third accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TResult Aggregate<T, TResult1, TResult2, TResult3, TResult>(
            this IEnumerable<T> source,
            Func<IObservable<T>, IObservable<TResult1>> aggregatorSelector1,
            Func<IObservable<T>, IObservable<TResult2>> aggregatorSelector2,
            Func<IObservable<T>, IObservable<TResult3>> aggregatorSelector3,
            Func<TResult1, TResult2, TResult3, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (aggregatorSelector1 == null) throw new ArgumentNullException(nameof(aggregatorSelector1));
            if (aggregatorSelector2 == null) throw new ArgumentNullException(nameof(aggregatorSelector2));
            if (aggregatorSelector3 == null) throw new ArgumentNullException(nameof(aggregatorSelector3));

            var r1 = new (bool, TResult1)[1];
            var r2 = new (bool, TResult2)[1];
            var r3 = new (bool, TResult3)[1];

            var subject = new Subject<T>();

            using (SubscribeSingle(aggregatorSelector1, subject, r1, nameof(aggregatorSelector1)))
            using (SubscribeSingle(aggregatorSelector2, subject, r2, nameof(aggregatorSelector2)))
            using (SubscribeSingle(aggregatorSelector3, subject, r3, nameof(aggregatorSelector3)))
            {
                foreach (var item in source)
                    subject.OnNext(item);

                subject.OnCompleted();
            }

            return resultSelector
            (
                GetAggregateResult(r1[0], nameof(aggregatorSelector1)),
                GetAggregateResult(r2[0], nameof(aggregatorSelector2)),
                GetAggregateResult(r3[0], nameof(aggregatorSelector3))
            );
        }
    }
}

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Applies four accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TState1">The type of the state of the first accumulator.</typeparam>
        /// <typeparam name="TState2">The type of the state of the second accumulator.</typeparam>
        /// <typeparam name="TState3">The type of the state of the third accumulator.</typeparam>
        /// <typeparam name="TState4">The type of the state of the fourth accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="seed1">The seed value for the first accumulator.</param>
        /// <param name="accumulator1">The first accumulator.</param>
        /// <param name="seed2">The seed value for the second accumulator.</param>
        /// <param name="accumulator2">The second accumulator.</param>
        /// <param name="seed3">The seed value for the third accumulator.</param>
        /// <param name="accumulator3">The third accumulator.</param>
        /// <param name="seed4">The seed value for the fourth accumulator.</param>
        /// <param name="accumulator4">The fourth accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TResult Aggregate<T, TState1, TState2, TState3, TState4, TResult>(
            this IEnumerable<T> source,
            TState1 seed1, Func<TState1, T, TState1> accumulator1,
            TState2 seed2, Func<TState2, T, TState2> accumulator2,
            TState3 seed3, Func<TState3, T, TState3> accumulator3,
            TState4 seed4, Func<TState4, T, TState4> accumulator4,
            Func<TState1, TState2, TState3, TState4, TResult> resultSelector) =>
            source.Aggregate(
                (seed1, seed2, seed3, seed4),
                (s, e) => (accumulator1(s.Item1, e), accumulator2(s.Item2, e), accumulator3(s.Item3, e), accumulator4(s.Item4, e)),
                s => resultSelector(s.Item1, s.Item2, s.Item3, s.Item4));
    }
}

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections.Generic;
    using Reactive;

    static partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Applies four accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult1">The type of the result of the first accumulator.</typeparam>
        /// <typeparam name="TResult2">The type of the result of the second accumulator.</typeparam>
        /// <typeparam name="TResult3">The type of the result of the third accumulator.</typeparam>
        /// <typeparam name="TResult4">The type of the result of the fourth accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="aggregatorSelector1">The first accumulator.</param>
        /// <param name="aggregatorSelector2">The second accumulator.</param>
        /// <param name="aggregatorSelector3">The third accumulator.</param>
        /// <param name="aggregatorSelector4">The fourth accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TResult Aggregate<T, TResult1, TResult2, TResult3, TResult4, TResult>(
            this IEnumerable<T> source,
            Func<IObservable<T>, IObservable<TResult1>> aggregatorSelector1,
            Func<IObservable<T>, IObservable<TResult2>> aggregatorSelector2,
            Func<IObservable<T>, IObservable<TResult3>> aggregatorSelector3,
            Func<IObservable<T>, IObservable<TResult4>> aggregatorSelector4,
            Func<TResult1, TResult2, TResult3, TResult4, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (aggregatorSelector1 == null) throw new ArgumentNullException(nameof(aggregatorSelector1));
            if (aggregatorSelector2 == null) throw new ArgumentNullException(nameof(aggregatorSelector2));
            if (aggregatorSelector3 == null) throw new ArgumentNullException(nameof(aggregatorSelector3));
            if (aggregatorSelector4 == null) throw new ArgumentNullException(nameof(aggregatorSelector4));

            var r1 = new (bool, TResult1)[1];
            var r2 = new (bool, TResult2)[1];
            var r3 = new (bool, TResult3)[1];
            var r4 = new (bool, TResult4)[1];

            var subject = new Subject<T>();

            using (SubscribeSingle(aggregatorSelector1, subject, r1, nameof(aggregatorSelector1)))
            using (SubscribeSingle(aggregatorSelector2, subject, r2, nameof(aggregatorSelector2)))
            using (SubscribeSingle(aggregatorSelector3, subject, r3, nameof(aggregatorSelector3)))
            using (SubscribeSingle(aggregatorSelector4, subject, r4, nameof(aggregatorSelector4)))
            {
                foreach (var item in source)
                    subject.OnNext(item);

                subject.OnCompleted();
            }

            return resultSelector
            (
                GetAggregateResult(r1[0], nameof(aggregatorSelector1)),
                GetAggregateResult(r2[0], nameof(aggregatorSelector2)),
                GetAggregateResult(r3[0], nameof(aggregatorSelector3)),
                GetAggregateResult(r4[0], nameof(aggregatorSelector4))
            );
        }
    }
}

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Applies five accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TState1">The type of the state of the first accumulator.</typeparam>
        /// <typeparam name="TState2">The type of the state of the second accumulator.</typeparam>
        /// <typeparam name="TState3">The type of the state of the third accumulator.</typeparam>
        /// <typeparam name="TState4">The type of the state of the fourth accumulator.</typeparam>
        /// <typeparam name="TState5">The type of the state of the fifth accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="seed1">The seed value for the first accumulator.</param>
        /// <param name="accumulator1">The first accumulator.</param>
        /// <param name="seed2">The seed value for the second accumulator.</param>
        /// <param name="accumulator2">The second accumulator.</param>
        /// <param name="seed3">The seed value for the third accumulator.</param>
        /// <param name="accumulator3">The third accumulator.</param>
        /// <param name="seed4">The seed value for the fourth accumulator.</param>
        /// <param name="accumulator4">The fourth accumulator.</param>
        /// <param name="seed5">The seed value for the fifth accumulator.</param>
        /// <param name="accumulator5">The fifth accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TResult Aggregate<T, TState1, TState2, TState3, TState4, TState5, TResult>(
            this IEnumerable<T> source,
            TState1 seed1, Func<TState1, T, TState1> accumulator1,
            TState2 seed2, Func<TState2, T, TState2> accumulator2,
            TState3 seed3, Func<TState3, T, TState3> accumulator3,
            TState4 seed4, Func<TState4, T, TState4> accumulator4,
            TState5 seed5, Func<TState5, T, TState5> accumulator5,
            Func<TState1, TState2, TState3, TState4, TState5, TResult> resultSelector) =>
            source.Aggregate(
                (seed1, seed2, seed3, seed4, seed5),
                (s, e) => (accumulator1(s.Item1, e), accumulator2(s.Item2, e), accumulator3(s.Item3, e), accumulator4(s.Item4, e), accumulator5(s.Item5, e)),
                s => resultSelector(s.Item1, s.Item2, s.Item3, s.Item4, s.Item5));
    }
}

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections.Generic;
    using Reactive;

    static partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Applies five accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult1">The type of the result of the first accumulator.</typeparam>
        /// <typeparam name="TResult2">The type of the result of the second accumulator.</typeparam>
        /// <typeparam name="TResult3">The type of the result of the third accumulator.</typeparam>
        /// <typeparam name="TResult4">The type of the result of the fourth accumulator.</typeparam>
        /// <typeparam name="TResult5">The type of the result of the fifth accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="aggregatorSelector1">The first accumulator.</param>
        /// <param name="aggregatorSelector2">The second accumulator.</param>
        /// <param name="aggregatorSelector3">The third accumulator.</param>
        /// <param name="aggregatorSelector4">The fourth accumulator.</param>
        /// <param name="aggregatorSelector5">The fifth accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TResult Aggregate<T, TResult1, TResult2, TResult3, TResult4, TResult5, TResult>(
            this IEnumerable<T> source,
            Func<IObservable<T>, IObservable<TResult1>> aggregatorSelector1,
            Func<IObservable<T>, IObservable<TResult2>> aggregatorSelector2,
            Func<IObservable<T>, IObservable<TResult3>> aggregatorSelector3,
            Func<IObservable<T>, IObservable<TResult4>> aggregatorSelector4,
            Func<IObservable<T>, IObservable<TResult5>> aggregatorSelector5,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (aggregatorSelector1 == null) throw new ArgumentNullException(nameof(aggregatorSelector1));
            if (aggregatorSelector2 == null) throw new ArgumentNullException(nameof(aggregatorSelector2));
            if (aggregatorSelector3 == null) throw new ArgumentNullException(nameof(aggregatorSelector3));
            if (aggregatorSelector4 == null) throw new ArgumentNullException(nameof(aggregatorSelector4));
            if (aggregatorSelector5 == null) throw new ArgumentNullException(nameof(aggregatorSelector5));

            var r1 = new (bool, TResult1)[1];
            var r2 = new (bool, TResult2)[1];
            var r3 = new (bool, TResult3)[1];
            var r4 = new (bool, TResult4)[1];
            var r5 = new (bool, TResult5)[1];

            var subject = new Subject<T>();

            using (SubscribeSingle(aggregatorSelector1, subject, r1, nameof(aggregatorSelector1)))
            using (SubscribeSingle(aggregatorSelector2, subject, r2, nameof(aggregatorSelector2)))
            using (SubscribeSingle(aggregatorSelector3, subject, r3, nameof(aggregatorSelector3)))
            using (SubscribeSingle(aggregatorSelector4, subject, r4, nameof(aggregatorSelector4)))
            using (SubscribeSingle(aggregatorSelector5, subject, r5, nameof(aggregatorSelector5)))
            {
                foreach (var item in source)
                    subject.OnNext(item);

                subject.OnCompleted();
            }

            return resultSelector
            (
                GetAggregateResult(r1[0], nameof(aggregatorSelector1)),
                GetAggregateResult(r2[0], nameof(aggregatorSelector2)),
                GetAggregateResult(r3[0], nameof(aggregatorSelector3)),
                GetAggregateResult(r4[0], nameof(aggregatorSelector4)),
                GetAggregateResult(r5[0], nameof(aggregatorSelector5))
            );
        }
    }
}

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Applies six accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TState1">The type of the state of the first accumulator.</typeparam>
        /// <typeparam name="TState2">The type of the state of the second accumulator.</typeparam>
        /// <typeparam name="TState3">The type of the state of the third accumulator.</typeparam>
        /// <typeparam name="TState4">The type of the state of the fourth accumulator.</typeparam>
        /// <typeparam name="TState5">The type of the state of the fifth accumulator.</typeparam>
        /// <typeparam name="TState6">The type of the state of the sixth accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="seed1">The seed value for the first accumulator.</param>
        /// <param name="accumulator1">The first accumulator.</param>
        /// <param name="seed2">The seed value for the second accumulator.</param>
        /// <param name="accumulator2">The second accumulator.</param>
        /// <param name="seed3">The seed value for the third accumulator.</param>
        /// <param name="accumulator3">The third accumulator.</param>
        /// <param name="seed4">The seed value for the fourth accumulator.</param>
        /// <param name="accumulator4">The fourth accumulator.</param>
        /// <param name="seed5">The seed value for the fifth accumulator.</param>
        /// <param name="accumulator5">The fifth accumulator.</param>
        /// <param name="seed6">The seed value for the sixth accumulator.</param>
        /// <param name="accumulator6">The sixth accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TResult Aggregate<T, TState1, TState2, TState3, TState4, TState5, TState6, TResult>(
            this IEnumerable<T> source,
            TState1 seed1, Func<TState1, T, TState1> accumulator1,
            TState2 seed2, Func<TState2, T, TState2> accumulator2,
            TState3 seed3, Func<TState3, T, TState3> accumulator3,
            TState4 seed4, Func<TState4, T, TState4> accumulator4,
            TState5 seed5, Func<TState5, T, TState5> accumulator5,
            TState6 seed6, Func<TState6, T, TState6> accumulator6,
            Func<TState1, TState2, TState3, TState4, TState5, TState6, TResult> resultSelector) =>
            source.Aggregate(
                (seed1, seed2, seed3, seed4, seed5, seed6),
                (s, e) => (accumulator1(s.Item1, e), accumulator2(s.Item2, e), accumulator3(s.Item3, e), accumulator4(s.Item4, e), accumulator5(s.Item5, e), accumulator6(s.Item6, e)),
                s => resultSelector(s.Item1, s.Item2, s.Item3, s.Item4, s.Item5, s.Item6));
    }
}

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections.Generic;
    using Reactive;

    static partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Applies six accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult1">The type of the result of the first accumulator.</typeparam>
        /// <typeparam name="TResult2">The type of the result of the second accumulator.</typeparam>
        /// <typeparam name="TResult3">The type of the result of the third accumulator.</typeparam>
        /// <typeparam name="TResult4">The type of the result of the fourth accumulator.</typeparam>
        /// <typeparam name="TResult5">The type of the result of the fifth accumulator.</typeparam>
        /// <typeparam name="TResult6">The type of the result of the sixth accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="aggregatorSelector1">The first accumulator.</param>
        /// <param name="aggregatorSelector2">The second accumulator.</param>
        /// <param name="aggregatorSelector3">The third accumulator.</param>
        /// <param name="aggregatorSelector4">The fourth accumulator.</param>
        /// <param name="aggregatorSelector5">The fifth accumulator.</param>
        /// <param name="aggregatorSelector6">The sixth accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TResult Aggregate<T, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult>(
            this IEnumerable<T> source,
            Func<IObservable<T>, IObservable<TResult1>> aggregatorSelector1,
            Func<IObservable<T>, IObservable<TResult2>> aggregatorSelector2,
            Func<IObservable<T>, IObservable<TResult3>> aggregatorSelector3,
            Func<IObservable<T>, IObservable<TResult4>> aggregatorSelector4,
            Func<IObservable<T>, IObservable<TResult5>> aggregatorSelector5,
            Func<IObservable<T>, IObservable<TResult6>> aggregatorSelector6,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (aggregatorSelector1 == null) throw new ArgumentNullException(nameof(aggregatorSelector1));
            if (aggregatorSelector2 == null) throw new ArgumentNullException(nameof(aggregatorSelector2));
            if (aggregatorSelector3 == null) throw new ArgumentNullException(nameof(aggregatorSelector3));
            if (aggregatorSelector4 == null) throw new ArgumentNullException(nameof(aggregatorSelector4));
            if (aggregatorSelector5 == null) throw new ArgumentNullException(nameof(aggregatorSelector5));
            if (aggregatorSelector6 == null) throw new ArgumentNullException(nameof(aggregatorSelector6));

            var r1 = new (bool, TResult1)[1];
            var r2 = new (bool, TResult2)[1];
            var r3 = new (bool, TResult3)[1];
            var r4 = new (bool, TResult4)[1];
            var r5 = new (bool, TResult5)[1];
            var r6 = new (bool, TResult6)[1];

            var subject = new Subject<T>();

            using (SubscribeSingle(aggregatorSelector1, subject, r1, nameof(aggregatorSelector1)))
            using (SubscribeSingle(aggregatorSelector2, subject, r2, nameof(aggregatorSelector2)))
            using (SubscribeSingle(aggregatorSelector3, subject, r3, nameof(aggregatorSelector3)))
            using (SubscribeSingle(aggregatorSelector4, subject, r4, nameof(aggregatorSelector4)))
            using (SubscribeSingle(aggregatorSelector5, subject, r5, nameof(aggregatorSelector5)))
            using (SubscribeSingle(aggregatorSelector6, subject, r6, nameof(aggregatorSelector6)))
            {
                foreach (var item in source)
                    subject.OnNext(item);

                subject.OnCompleted();
            }

            return resultSelector
            (
                GetAggregateResult(r1[0], nameof(aggregatorSelector1)),
                GetAggregateResult(r2[0], nameof(aggregatorSelector2)),
                GetAggregateResult(r3[0], nameof(aggregatorSelector3)),
                GetAggregateResult(r4[0], nameof(aggregatorSelector4)),
                GetAggregateResult(r5[0], nameof(aggregatorSelector5)),
                GetAggregateResult(r6[0], nameof(aggregatorSelector6))
            );
        }
    }
}

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Applies seven accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TState1">The type of the state of the first accumulator.</typeparam>
        /// <typeparam name="TState2">The type of the state of the second accumulator.</typeparam>
        /// <typeparam name="TState3">The type of the state of the third accumulator.</typeparam>
        /// <typeparam name="TState4">The type of the state of the fourth accumulator.</typeparam>
        /// <typeparam name="TState5">The type of the state of the fifth accumulator.</typeparam>
        /// <typeparam name="TState6">The type of the state of the sixth accumulator.</typeparam>
        /// <typeparam name="TState7">The type of the state of the seventh accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="seed1">The seed value for the first accumulator.</param>
        /// <param name="accumulator1">The first accumulator.</param>
        /// <param name="seed2">The seed value for the second accumulator.</param>
        /// <param name="accumulator2">The second accumulator.</param>
        /// <param name="seed3">The seed value for the third accumulator.</param>
        /// <param name="accumulator3">The third accumulator.</param>
        /// <param name="seed4">The seed value for the fourth accumulator.</param>
        /// <param name="accumulator4">The fourth accumulator.</param>
        /// <param name="seed5">The seed value for the fifth accumulator.</param>
        /// <param name="accumulator5">The fifth accumulator.</param>
        /// <param name="seed6">The seed value for the sixth accumulator.</param>
        /// <param name="accumulator6">The sixth accumulator.</param>
        /// <param name="seed7">The seed value for the seventh accumulator.</param>
        /// <param name="accumulator7">The seventh accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TResult Aggregate<T, TState1, TState2, TState3, TState4, TState5, TState6, TState7, TResult>(
            this IEnumerable<T> source,
            TState1 seed1, Func<TState1, T, TState1> accumulator1,
            TState2 seed2, Func<TState2, T, TState2> accumulator2,
            TState3 seed3, Func<TState3, T, TState3> accumulator3,
            TState4 seed4, Func<TState4, T, TState4> accumulator4,
            TState5 seed5, Func<TState5, T, TState5> accumulator5,
            TState6 seed6, Func<TState6, T, TState6> accumulator6,
            TState7 seed7, Func<TState7, T, TState7> accumulator7,
            Func<TState1, TState2, TState3, TState4, TState5, TState6, TState7, TResult> resultSelector) =>
            source.Aggregate(
                (seed1, seed2, seed3, seed4, seed5, seed6, seed7),
                (s, e) => (accumulator1(s.Item1, e), accumulator2(s.Item2, e), accumulator3(s.Item3, e), accumulator4(s.Item4, e), accumulator5(s.Item5, e), accumulator6(s.Item6, e), accumulator7(s.Item7, e)),
                s => resultSelector(s.Item1, s.Item2, s.Item3, s.Item4, s.Item5, s.Item6, s.Item7));
    }
}

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections.Generic;
    using Reactive;

    static partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Applies seven accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult1">The type of the result of the first accumulator.</typeparam>
        /// <typeparam name="TResult2">The type of the result of the second accumulator.</typeparam>
        /// <typeparam name="TResult3">The type of the result of the third accumulator.</typeparam>
        /// <typeparam name="TResult4">The type of the result of the fourth accumulator.</typeparam>
        /// <typeparam name="TResult5">The type of the result of the fifth accumulator.</typeparam>
        /// <typeparam name="TResult6">The type of the result of the sixth accumulator.</typeparam>
        /// <typeparam name="TResult7">The type of the result of the seventh accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="aggregatorSelector1">The first accumulator.</param>
        /// <param name="aggregatorSelector2">The second accumulator.</param>
        /// <param name="aggregatorSelector3">The third accumulator.</param>
        /// <param name="aggregatorSelector4">The fourth accumulator.</param>
        /// <param name="aggregatorSelector5">The fifth accumulator.</param>
        /// <param name="aggregatorSelector6">The sixth accumulator.</param>
        /// <param name="aggregatorSelector7">The seventh accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TResult Aggregate<T, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult>(
            this IEnumerable<T> source,
            Func<IObservable<T>, IObservable<TResult1>> aggregatorSelector1,
            Func<IObservable<T>, IObservable<TResult2>> aggregatorSelector2,
            Func<IObservable<T>, IObservable<TResult3>> aggregatorSelector3,
            Func<IObservable<T>, IObservable<TResult4>> aggregatorSelector4,
            Func<IObservable<T>, IObservable<TResult5>> aggregatorSelector5,
            Func<IObservable<T>, IObservable<TResult6>> aggregatorSelector6,
            Func<IObservable<T>, IObservable<TResult7>> aggregatorSelector7,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (aggregatorSelector1 == null) throw new ArgumentNullException(nameof(aggregatorSelector1));
            if (aggregatorSelector2 == null) throw new ArgumentNullException(nameof(aggregatorSelector2));
            if (aggregatorSelector3 == null) throw new ArgumentNullException(nameof(aggregatorSelector3));
            if (aggregatorSelector4 == null) throw new ArgumentNullException(nameof(aggregatorSelector4));
            if (aggregatorSelector5 == null) throw new ArgumentNullException(nameof(aggregatorSelector5));
            if (aggregatorSelector6 == null) throw new ArgumentNullException(nameof(aggregatorSelector6));
            if (aggregatorSelector7 == null) throw new ArgumentNullException(nameof(aggregatorSelector7));

            var r1 = new (bool, TResult1)[1];
            var r2 = new (bool, TResult2)[1];
            var r3 = new (bool, TResult3)[1];
            var r4 = new (bool, TResult4)[1];
            var r5 = new (bool, TResult5)[1];
            var r6 = new (bool, TResult6)[1];
            var r7 = new (bool, TResult7)[1];

            var subject = new Subject<T>();

            using (SubscribeSingle(aggregatorSelector1, subject, r1, nameof(aggregatorSelector1)))
            using (SubscribeSingle(aggregatorSelector2, subject, r2, nameof(aggregatorSelector2)))
            using (SubscribeSingle(aggregatorSelector3, subject, r3, nameof(aggregatorSelector3)))
            using (SubscribeSingle(aggregatorSelector4, subject, r4, nameof(aggregatorSelector4)))
            using (SubscribeSingle(aggregatorSelector5, subject, r5, nameof(aggregatorSelector5)))
            using (SubscribeSingle(aggregatorSelector6, subject, r6, nameof(aggregatorSelector6)))
            using (SubscribeSingle(aggregatorSelector7, subject, r7, nameof(aggregatorSelector7)))
            {
                foreach (var item in source)
                    subject.OnNext(item);

                subject.OnCompleted();
            }

            return resultSelector
            (
                GetAggregateResult(r1[0], nameof(aggregatorSelector1)),
                GetAggregateResult(r2[0], nameof(aggregatorSelector2)),
                GetAggregateResult(r3[0], nameof(aggregatorSelector3)),
                GetAggregateResult(r4[0], nameof(aggregatorSelector4)),
                GetAggregateResult(r5[0], nameof(aggregatorSelector5)),
                GetAggregateResult(r6[0], nameof(aggregatorSelector6)),
                GetAggregateResult(r7[0], nameof(aggregatorSelector7))
            );
        }
    }
}

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Applies eight accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TState1">The type of the state of the first accumulator.</typeparam>
        /// <typeparam name="TState2">The type of the state of the second accumulator.</typeparam>
        /// <typeparam name="TState3">The type of the state of the third accumulator.</typeparam>
        /// <typeparam name="TState4">The type of the state of the fourth accumulator.</typeparam>
        /// <typeparam name="TState5">The type of the state of the fifth accumulator.</typeparam>
        /// <typeparam name="TState6">The type of the state of the sixth accumulator.</typeparam>
        /// <typeparam name="TState7">The type of the state of the seventh accumulator.</typeparam>
        /// <typeparam name="TState8">The type of the state of the eighth accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="seed1">The seed value for the first accumulator.</param>
        /// <param name="accumulator1">The first accumulator.</param>
        /// <param name="seed2">The seed value for the second accumulator.</param>
        /// <param name="accumulator2">The second accumulator.</param>
        /// <param name="seed3">The seed value for the third accumulator.</param>
        /// <param name="accumulator3">The third accumulator.</param>
        /// <param name="seed4">The seed value for the fourth accumulator.</param>
        /// <param name="accumulator4">The fourth accumulator.</param>
        /// <param name="seed5">The seed value for the fifth accumulator.</param>
        /// <param name="accumulator5">The fifth accumulator.</param>
        /// <param name="seed6">The seed value for the sixth accumulator.</param>
        /// <param name="accumulator6">The sixth accumulator.</param>
        /// <param name="seed7">The seed value for the seventh accumulator.</param>
        /// <param name="accumulator7">The seventh accumulator.</param>
        /// <param name="seed8">The seed value for the eighth accumulator.</param>
        /// <param name="accumulator8">The eighth accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TResult Aggregate<T, TState1, TState2, TState3, TState4, TState5, TState6, TState7, TState8, TResult>(
            this IEnumerable<T> source,
            TState1 seed1, Func<TState1, T, TState1> accumulator1,
            TState2 seed2, Func<TState2, T, TState2> accumulator2,
            TState3 seed3, Func<TState3, T, TState3> accumulator3,
            TState4 seed4, Func<TState4, T, TState4> accumulator4,
            TState5 seed5, Func<TState5, T, TState5> accumulator5,
            TState6 seed6, Func<TState6, T, TState6> accumulator6,
            TState7 seed7, Func<TState7, T, TState7> accumulator7,
            TState8 seed8, Func<TState8, T, TState8> accumulator8,
            Func<TState1, TState2, TState3, TState4, TState5, TState6, TState7, TState8, TResult> resultSelector) =>
            source.Aggregate(
                (seed1, seed2, seed3, seed4, seed5, seed6, seed7, seed8),
                (s, e) => (accumulator1(s.Item1, e), accumulator2(s.Item2, e), accumulator3(s.Item3, e), accumulator4(s.Item4, e), accumulator5(s.Item5, e), accumulator6(s.Item6, e), accumulator7(s.Item7, e), accumulator8(s.Item8, e)),
                s => resultSelector(s.Item1, s.Item2, s.Item3, s.Item4, s.Item5, s.Item6, s.Item7, s.Item8));
    }
}

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections.Generic;
    using Reactive;

    static partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Applies eight accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult1">The type of the result of the first accumulator.</typeparam>
        /// <typeparam name="TResult2">The type of the result of the second accumulator.</typeparam>
        /// <typeparam name="TResult3">The type of the result of the third accumulator.</typeparam>
        /// <typeparam name="TResult4">The type of the result of the fourth accumulator.</typeparam>
        /// <typeparam name="TResult5">The type of the result of the fifth accumulator.</typeparam>
        /// <typeparam name="TResult6">The type of the result of the sixth accumulator.</typeparam>
        /// <typeparam name="TResult7">The type of the result of the seventh accumulator.</typeparam>
        /// <typeparam name="TResult8">The type of the result of the eighth accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="aggregatorSelector1">The first accumulator.</param>
        /// <param name="aggregatorSelector2">The second accumulator.</param>
        /// <param name="aggregatorSelector3">The third accumulator.</param>
        /// <param name="aggregatorSelector4">The fourth accumulator.</param>
        /// <param name="aggregatorSelector5">The fifth accumulator.</param>
        /// <param name="aggregatorSelector6">The sixth accumulator.</param>
        /// <param name="aggregatorSelector7">The seventh accumulator.</param>
        /// <param name="aggregatorSelector8">The eighth accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// This operator executes immediately.
        /// </remarks>

        public static TResult Aggregate<T, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult>(
            this IEnumerable<T> source,
            Func<IObservable<T>, IObservable<TResult1>> aggregatorSelector1,
            Func<IObservable<T>, IObservable<TResult2>> aggregatorSelector2,
            Func<IObservable<T>, IObservable<TResult3>> aggregatorSelector3,
            Func<IObservable<T>, IObservable<TResult4>> aggregatorSelector4,
            Func<IObservable<T>, IObservable<TResult5>> aggregatorSelector5,
            Func<IObservable<T>, IObservable<TResult6>> aggregatorSelector6,
            Func<IObservable<T>, IObservable<TResult7>> aggregatorSelector7,
            Func<IObservable<T>, IObservable<TResult8>> aggregatorSelector8,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (aggregatorSelector1 == null) throw new ArgumentNullException(nameof(aggregatorSelector1));
            if (aggregatorSelector2 == null) throw new ArgumentNullException(nameof(aggregatorSelector2));
            if (aggregatorSelector3 == null) throw new ArgumentNullException(nameof(aggregatorSelector3));
            if (aggregatorSelector4 == null) throw new ArgumentNullException(nameof(aggregatorSelector4));
            if (aggregatorSelector5 == null) throw new ArgumentNullException(nameof(aggregatorSelector5));
            if (aggregatorSelector6 == null) throw new ArgumentNullException(nameof(aggregatorSelector6));
            if (aggregatorSelector7 == null) throw new ArgumentNullException(nameof(aggregatorSelector7));
            if (aggregatorSelector8 == null) throw new ArgumentNullException(nameof(aggregatorSelector8));

            var r1 = new (bool, TResult1)[1];
            var r2 = new (bool, TResult2)[1];
            var r3 = new (bool, TResult3)[1];
            var r4 = new (bool, TResult4)[1];
            var r5 = new (bool, TResult5)[1];
            var r6 = new (bool, TResult6)[1];
            var r7 = new (bool, TResult7)[1];
            var r8 = new (bool, TResult8)[1];

            var subject = new Subject<T>();

            using (SubscribeSingle(aggregatorSelector1, subject, r1, nameof(aggregatorSelector1)))
            using (SubscribeSingle(aggregatorSelector2, subject, r2, nameof(aggregatorSelector2)))
            using (SubscribeSingle(aggregatorSelector3, subject, r3, nameof(aggregatorSelector3)))
            using (SubscribeSingle(aggregatorSelector4, subject, r4, nameof(aggregatorSelector4)))
            using (SubscribeSingle(aggregatorSelector5, subject, r5, nameof(aggregatorSelector5)))
            using (SubscribeSingle(aggregatorSelector6, subject, r6, nameof(aggregatorSelector6)))
            using (SubscribeSingle(aggregatorSelector7, subject, r7, nameof(aggregatorSelector7)))
            using (SubscribeSingle(aggregatorSelector8, subject, r8, nameof(aggregatorSelector8)))
            {
                foreach (var item in source)
                    subject.OnNext(item);

                subject.OnCompleted();
            }

            return resultSelector
            (
                GetAggregateResult(r1[0], nameof(aggregatorSelector1)),
                GetAggregateResult(r2[0], nameof(aggregatorSelector2)),
                GetAggregateResult(r3[0], nameof(aggregatorSelector3)),
                GetAggregateResult(r4[0], nameof(aggregatorSelector4)),
                GetAggregateResult(r5[0], nameof(aggregatorSelector5)),
                GetAggregateResult(r6[0], nameof(aggregatorSelector6)),
                GetAggregateResult(r7[0], nameof(aggregatorSelector7)),
                GetAggregateResult(r8[0], nameof(aggregatorSelector8))
            );
        }
    }
}
