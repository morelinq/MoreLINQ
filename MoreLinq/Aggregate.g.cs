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

    partial class MoreEnumerable
    {
        /// <summary>
        /// Applies two accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
        /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
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

        public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TResult>(
            this IEnumerable<T> source,
            TAccumulate1 seed1, Func<TAccumulate1, T, TAccumulate1> accumulator1,
            TAccumulate2 seed2, Func<TAccumulate2, T, TAccumulate2> accumulator2,
            Func<TAccumulate1, TAccumulate2, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (accumulator1 == null) throw new ArgumentNullException(nameof(accumulator1));
            if (accumulator2 == null) throw new ArgumentNullException(nameof(accumulator2));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var a1 = seed1;
            var a2 = seed2;

            foreach (var item in source)
            {
                a1 = accumulator1(a1, item);
                a2 = accumulator2(a2, item);
            }

            return resultSelector(a1, a2);
        }
    }
}

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections.Generic;
    using Reactive;

    partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Applies two accumulator queries sequentially in a single
        /// pass over a sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult1">The type of the result of the first accumulator.</typeparam>
        /// <typeparam name="TResult2">The type of the result of the second accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="accumulator1">The first accumulator.</param>
        /// <param name="accumulator2">The second accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// An <see cref="IObservable{T}"/> returned by an accumulator function
        /// produced zero or more than a single aggregate result.
        /// </exception>
        /// <remarks>
        /// <para>This operator executes immediately.</para>
        /// <para>
        /// Each accumulator argument is a function that receives an
        /// <see cref="IObservable{T}"/>, which when subscribed to, produces the
        /// items in the <paramref name="source"/> sequence and in original
        /// order; the function must then return an <see cref="IObservable{T}"/>
        /// that produces a single aggregate on completion (when
        /// <see cref="IObserver{T}.OnCompleted"/> is called. An error is raised
        /// at run-time if the <see cref="IObserver{T}"/> returned by an
        /// accumulator function produces no result or produces more than a
        /// single result.
        /// </para>
        /// </remarks>

        public static TResult Aggregate<T, TResult1, TResult2, TResult>(
            this IEnumerable<T> source,
            Func<IObservable<T>, IObservable<TResult1>> accumulator1,
            Func<IObservable<T>, IObservable<TResult2>> accumulator2,
            Func<TResult1, TResult2, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (accumulator1 == null) throw new ArgumentNullException(nameof(accumulator1));
            if (accumulator2 == null) throw new ArgumentNullException(nameof(accumulator2));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var r1 = new (bool, TResult1)[1];
            var r2 = new (bool, TResult2)[1];

            var subject = new Subject<T>();

            using (SubscribeSingle(accumulator1, subject, r1, nameof(accumulator1)))
            using (SubscribeSingle(accumulator2, subject, r2, nameof(accumulator2)))
            {
                foreach (var item in source)
                    subject.OnNext(item);

                subject.OnCompleted();
            }

            return resultSelector
            (
                GetAggregateResult(r1[0], nameof(accumulator1)),
                GetAggregateResult(r2[0], nameof(accumulator2))
            );
        }
    }
}

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;

    partial class MoreEnumerable
    {
        /// <summary>
        /// Applies three accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
        /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
        /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
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

        public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TResult>(
            this IEnumerable<T> source,
            TAccumulate1 seed1, Func<TAccumulate1, T, TAccumulate1> accumulator1,
            TAccumulate2 seed2, Func<TAccumulate2, T, TAccumulate2> accumulator2,
            TAccumulate3 seed3, Func<TAccumulate3, T, TAccumulate3> accumulator3,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (accumulator1 == null) throw new ArgumentNullException(nameof(accumulator1));
            if (accumulator2 == null) throw new ArgumentNullException(nameof(accumulator2));
            if (accumulator3 == null) throw new ArgumentNullException(nameof(accumulator3));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var a1 = seed1;
            var a2 = seed2;
            var a3 = seed3;

            foreach (var item in source)
            {
                a1 = accumulator1(a1, item);
                a2 = accumulator2(a2, item);
                a3 = accumulator3(a3, item);
            }

            return resultSelector(a1, a2, a3);
        }
    }
}

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections.Generic;
    using Reactive;

    partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Applies three accumulator queries sequentially in a single
        /// pass over a sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult1">The type of the result of the first accumulator.</typeparam>
        /// <typeparam name="TResult2">The type of the result of the second accumulator.</typeparam>
        /// <typeparam name="TResult3">The type of the result of the third accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="accumulator1">The first accumulator.</param>
        /// <param name="accumulator2">The second accumulator.</param>
        /// <param name="accumulator3">The third accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// An <see cref="IObservable{T}"/> returned by an accumulator function
        /// produced zero or more than a single aggregate result.
        /// </exception>
        /// <remarks>
        /// <para>This operator executes immediately.</para>
        /// <para>
        /// Each accumulator argument is a function that receives an
        /// <see cref="IObservable{T}"/>, which when subscribed to, produces the
        /// items in the <paramref name="source"/> sequence and in original
        /// order; the function must then return an <see cref="IObservable{T}"/>
        /// that produces a single aggregate on completion (when
        /// <see cref="IObserver{T}.OnCompleted"/> is called. An error is raised
        /// at run-time if the <see cref="IObserver{T}"/> returned by an
        /// accumulator function produces no result or produces more than a
        /// single result.
        /// </para>
        /// </remarks>

        public static TResult Aggregate<T, TResult1, TResult2, TResult3, TResult>(
            this IEnumerable<T> source,
            Func<IObservable<T>, IObservable<TResult1>> accumulator1,
            Func<IObservable<T>, IObservable<TResult2>> accumulator2,
            Func<IObservable<T>, IObservable<TResult3>> accumulator3,
            Func<TResult1, TResult2, TResult3, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (accumulator1 == null) throw new ArgumentNullException(nameof(accumulator1));
            if (accumulator2 == null) throw new ArgumentNullException(nameof(accumulator2));
            if (accumulator3 == null) throw new ArgumentNullException(nameof(accumulator3));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var r1 = new (bool, TResult1)[1];
            var r2 = new (bool, TResult2)[1];
            var r3 = new (bool, TResult3)[1];

            var subject = new Subject<T>();

            using (SubscribeSingle(accumulator1, subject, r1, nameof(accumulator1)))
            using (SubscribeSingle(accumulator2, subject, r2, nameof(accumulator2)))
            using (SubscribeSingle(accumulator3, subject, r3, nameof(accumulator3)))
            {
                foreach (var item in source)
                    subject.OnNext(item);

                subject.OnCompleted();
            }

            return resultSelector
            (
                GetAggregateResult(r1[0], nameof(accumulator1)),
                GetAggregateResult(r2[0], nameof(accumulator2)),
                GetAggregateResult(r3[0], nameof(accumulator3))
            );
        }
    }
}

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;

    partial class MoreEnumerable
    {
        /// <summary>
        /// Applies four accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
        /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
        /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
        /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
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

        public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TResult>(
            this IEnumerable<T> source,
            TAccumulate1 seed1, Func<TAccumulate1, T, TAccumulate1> accumulator1,
            TAccumulate2 seed2, Func<TAccumulate2, T, TAccumulate2> accumulator2,
            TAccumulate3 seed3, Func<TAccumulate3, T, TAccumulate3> accumulator3,
            TAccumulate4 seed4, Func<TAccumulate4, T, TAccumulate4> accumulator4,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (accumulator1 == null) throw new ArgumentNullException(nameof(accumulator1));
            if (accumulator2 == null) throw new ArgumentNullException(nameof(accumulator2));
            if (accumulator3 == null) throw new ArgumentNullException(nameof(accumulator3));
            if (accumulator4 == null) throw new ArgumentNullException(nameof(accumulator4));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var a1 = seed1;
            var a2 = seed2;
            var a3 = seed3;
            var a4 = seed4;

            foreach (var item in source)
            {
                a1 = accumulator1(a1, item);
                a2 = accumulator2(a2, item);
                a3 = accumulator3(a3, item);
                a4 = accumulator4(a4, item);
            }

            return resultSelector(a1, a2, a3, a4);
        }
    }
}

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections.Generic;
    using Reactive;

    partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Applies four accumulator queries sequentially in a single
        /// pass over a sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult1">The type of the result of the first accumulator.</typeparam>
        /// <typeparam name="TResult2">The type of the result of the second accumulator.</typeparam>
        /// <typeparam name="TResult3">The type of the result of the third accumulator.</typeparam>
        /// <typeparam name="TResult4">The type of the result of the fourth accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="accumulator1">The first accumulator.</param>
        /// <param name="accumulator2">The second accumulator.</param>
        /// <param name="accumulator3">The third accumulator.</param>
        /// <param name="accumulator4">The fourth accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// An <see cref="IObservable{T}"/> returned by an accumulator function
        /// produced zero or more than a single aggregate result.
        /// </exception>
        /// <remarks>
        /// <para>This operator executes immediately.</para>
        /// <para>
        /// Each accumulator argument is a function that receives an
        /// <see cref="IObservable{T}"/>, which when subscribed to, produces the
        /// items in the <paramref name="source"/> sequence and in original
        /// order; the function must then return an <see cref="IObservable{T}"/>
        /// that produces a single aggregate on completion (when
        /// <see cref="IObserver{T}.OnCompleted"/> is called. An error is raised
        /// at run-time if the <see cref="IObserver{T}"/> returned by an
        /// accumulator function produces no result or produces more than a
        /// single result.
        /// </para>
        /// </remarks>

        public static TResult Aggregate<T, TResult1, TResult2, TResult3, TResult4, TResult>(
            this IEnumerable<T> source,
            Func<IObservable<T>, IObservable<TResult1>> accumulator1,
            Func<IObservable<T>, IObservable<TResult2>> accumulator2,
            Func<IObservable<T>, IObservable<TResult3>> accumulator3,
            Func<IObservable<T>, IObservable<TResult4>> accumulator4,
            Func<TResult1, TResult2, TResult3, TResult4, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (accumulator1 == null) throw new ArgumentNullException(nameof(accumulator1));
            if (accumulator2 == null) throw new ArgumentNullException(nameof(accumulator2));
            if (accumulator3 == null) throw new ArgumentNullException(nameof(accumulator3));
            if (accumulator4 == null) throw new ArgumentNullException(nameof(accumulator4));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var r1 = new (bool, TResult1)[1];
            var r2 = new (bool, TResult2)[1];
            var r3 = new (bool, TResult3)[1];
            var r4 = new (bool, TResult4)[1];

            var subject = new Subject<T>();

            using (SubscribeSingle(accumulator1, subject, r1, nameof(accumulator1)))
            using (SubscribeSingle(accumulator2, subject, r2, nameof(accumulator2)))
            using (SubscribeSingle(accumulator3, subject, r3, nameof(accumulator3)))
            using (SubscribeSingle(accumulator4, subject, r4, nameof(accumulator4)))
            {
                foreach (var item in source)
                    subject.OnNext(item);

                subject.OnCompleted();
            }

            return resultSelector
            (
                GetAggregateResult(r1[0], nameof(accumulator1)),
                GetAggregateResult(r2[0], nameof(accumulator2)),
                GetAggregateResult(r3[0], nameof(accumulator3)),
                GetAggregateResult(r4[0], nameof(accumulator4))
            );
        }
    }
}

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;

    partial class MoreEnumerable
    {
        /// <summary>
        /// Applies five accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
        /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
        /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
        /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
        /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
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

        public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TResult>(
            this IEnumerable<T> source,
            TAccumulate1 seed1, Func<TAccumulate1, T, TAccumulate1> accumulator1,
            TAccumulate2 seed2, Func<TAccumulate2, T, TAccumulate2> accumulator2,
            TAccumulate3 seed3, Func<TAccumulate3, T, TAccumulate3> accumulator3,
            TAccumulate4 seed4, Func<TAccumulate4, T, TAccumulate4> accumulator4,
            TAccumulate5 seed5, Func<TAccumulate5, T, TAccumulate5> accumulator5,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (accumulator1 == null) throw new ArgumentNullException(nameof(accumulator1));
            if (accumulator2 == null) throw new ArgumentNullException(nameof(accumulator2));
            if (accumulator3 == null) throw new ArgumentNullException(nameof(accumulator3));
            if (accumulator4 == null) throw new ArgumentNullException(nameof(accumulator4));
            if (accumulator5 == null) throw new ArgumentNullException(nameof(accumulator5));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var a1 = seed1;
            var a2 = seed2;
            var a3 = seed3;
            var a4 = seed4;
            var a5 = seed5;

            foreach (var item in source)
            {
                a1 = accumulator1(a1, item);
                a2 = accumulator2(a2, item);
                a3 = accumulator3(a3, item);
                a4 = accumulator4(a4, item);
                a5 = accumulator5(a5, item);
            }

            return resultSelector(a1, a2, a3, a4, a5);
        }
    }
}

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections.Generic;
    using Reactive;

    partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Applies five accumulator queries sequentially in a single
        /// pass over a sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult1">The type of the result of the first accumulator.</typeparam>
        /// <typeparam name="TResult2">The type of the result of the second accumulator.</typeparam>
        /// <typeparam name="TResult3">The type of the result of the third accumulator.</typeparam>
        /// <typeparam name="TResult4">The type of the result of the fourth accumulator.</typeparam>
        /// <typeparam name="TResult5">The type of the result of the fifth accumulator.</typeparam>
        /// <typeparam name="TResult">The type of the accumulated result.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="accumulator1">The first accumulator.</param>
        /// <param name="accumulator2">The second accumulator.</param>
        /// <param name="accumulator3">The third accumulator.</param>
        /// <param name="accumulator4">The fourth accumulator.</param>
        /// <param name="accumulator5">The fifth accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// An <see cref="IObservable{T}"/> returned by an accumulator function
        /// produced zero or more than a single aggregate result.
        /// </exception>
        /// <remarks>
        /// <para>This operator executes immediately.</para>
        /// <para>
        /// Each accumulator argument is a function that receives an
        /// <see cref="IObservable{T}"/>, which when subscribed to, produces the
        /// items in the <paramref name="source"/> sequence and in original
        /// order; the function must then return an <see cref="IObservable{T}"/>
        /// that produces a single aggregate on completion (when
        /// <see cref="IObserver{T}.OnCompleted"/> is called. An error is raised
        /// at run-time if the <see cref="IObserver{T}"/> returned by an
        /// accumulator function produces no result or produces more than a
        /// single result.
        /// </para>
        /// </remarks>

        public static TResult Aggregate<T, TResult1, TResult2, TResult3, TResult4, TResult5, TResult>(
            this IEnumerable<T> source,
            Func<IObservable<T>, IObservable<TResult1>> accumulator1,
            Func<IObservable<T>, IObservable<TResult2>> accumulator2,
            Func<IObservable<T>, IObservable<TResult3>> accumulator3,
            Func<IObservable<T>, IObservable<TResult4>> accumulator4,
            Func<IObservable<T>, IObservable<TResult5>> accumulator5,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (accumulator1 == null) throw new ArgumentNullException(nameof(accumulator1));
            if (accumulator2 == null) throw new ArgumentNullException(nameof(accumulator2));
            if (accumulator3 == null) throw new ArgumentNullException(nameof(accumulator3));
            if (accumulator4 == null) throw new ArgumentNullException(nameof(accumulator4));
            if (accumulator5 == null) throw new ArgumentNullException(nameof(accumulator5));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var r1 = new (bool, TResult1)[1];
            var r2 = new (bool, TResult2)[1];
            var r3 = new (bool, TResult3)[1];
            var r4 = new (bool, TResult4)[1];
            var r5 = new (bool, TResult5)[1];

            var subject = new Subject<T>();

            using (SubscribeSingle(accumulator1, subject, r1, nameof(accumulator1)))
            using (SubscribeSingle(accumulator2, subject, r2, nameof(accumulator2)))
            using (SubscribeSingle(accumulator3, subject, r3, nameof(accumulator3)))
            using (SubscribeSingle(accumulator4, subject, r4, nameof(accumulator4)))
            using (SubscribeSingle(accumulator5, subject, r5, nameof(accumulator5)))
            {
                foreach (var item in source)
                    subject.OnNext(item);

                subject.OnCompleted();
            }

            return resultSelector
            (
                GetAggregateResult(r1[0], nameof(accumulator1)),
                GetAggregateResult(r2[0], nameof(accumulator2)),
                GetAggregateResult(r3[0], nameof(accumulator3)),
                GetAggregateResult(r4[0], nameof(accumulator4)),
                GetAggregateResult(r5[0], nameof(accumulator5))
            );
        }
    }
}

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;

    partial class MoreEnumerable
    {
        /// <summary>
        /// Applies six accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
        /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
        /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
        /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
        /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
        /// <typeparam name="TAccumulate6">The type of sixth accumulator value.</typeparam>
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

        public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TResult>(
            this IEnumerable<T> source,
            TAccumulate1 seed1, Func<TAccumulate1, T, TAccumulate1> accumulator1,
            TAccumulate2 seed2, Func<TAccumulate2, T, TAccumulate2> accumulator2,
            TAccumulate3 seed3, Func<TAccumulate3, T, TAccumulate3> accumulator3,
            TAccumulate4 seed4, Func<TAccumulate4, T, TAccumulate4> accumulator4,
            TAccumulate5 seed5, Func<TAccumulate5, T, TAccumulate5> accumulator5,
            TAccumulate6 seed6, Func<TAccumulate6, T, TAccumulate6> accumulator6,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (accumulator1 == null) throw new ArgumentNullException(nameof(accumulator1));
            if (accumulator2 == null) throw new ArgumentNullException(nameof(accumulator2));
            if (accumulator3 == null) throw new ArgumentNullException(nameof(accumulator3));
            if (accumulator4 == null) throw new ArgumentNullException(nameof(accumulator4));
            if (accumulator5 == null) throw new ArgumentNullException(nameof(accumulator5));
            if (accumulator6 == null) throw new ArgumentNullException(nameof(accumulator6));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var a1 = seed1;
            var a2 = seed2;
            var a3 = seed3;
            var a4 = seed4;
            var a5 = seed5;
            var a6 = seed6;

            foreach (var item in source)
            {
                a1 = accumulator1(a1, item);
                a2 = accumulator2(a2, item);
                a3 = accumulator3(a3, item);
                a4 = accumulator4(a4, item);
                a5 = accumulator5(a5, item);
                a6 = accumulator6(a6, item);
            }

            return resultSelector(a1, a2, a3, a4, a5, a6);
        }
    }
}

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections.Generic;
    using Reactive;

    partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Applies six accumulator queries sequentially in a single
        /// pass over a sequence.
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
        /// <param name="accumulator1">The first accumulator.</param>
        /// <param name="accumulator2">The second accumulator.</param>
        /// <param name="accumulator3">The third accumulator.</param>
        /// <param name="accumulator4">The fourth accumulator.</param>
        /// <param name="accumulator5">The fifth accumulator.</param>
        /// <param name="accumulator6">The sixth accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// An <see cref="IObservable{T}"/> returned by an accumulator function
        /// produced zero or more than a single aggregate result.
        /// </exception>
        /// <remarks>
        /// <para>This operator executes immediately.</para>
        /// <para>
        /// Each accumulator argument is a function that receives an
        /// <see cref="IObservable{T}"/>, which when subscribed to, produces the
        /// items in the <paramref name="source"/> sequence and in original
        /// order; the function must then return an <see cref="IObservable{T}"/>
        /// that produces a single aggregate on completion (when
        /// <see cref="IObserver{T}.OnCompleted"/> is called. An error is raised
        /// at run-time if the <see cref="IObserver{T}"/> returned by an
        /// accumulator function produces no result or produces more than a
        /// single result.
        /// </para>
        /// </remarks>

        public static TResult Aggregate<T, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult>(
            this IEnumerable<T> source,
            Func<IObservable<T>, IObservable<TResult1>> accumulator1,
            Func<IObservable<T>, IObservable<TResult2>> accumulator2,
            Func<IObservable<T>, IObservable<TResult3>> accumulator3,
            Func<IObservable<T>, IObservable<TResult4>> accumulator4,
            Func<IObservable<T>, IObservable<TResult5>> accumulator5,
            Func<IObservable<T>, IObservable<TResult6>> accumulator6,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (accumulator1 == null) throw new ArgumentNullException(nameof(accumulator1));
            if (accumulator2 == null) throw new ArgumentNullException(nameof(accumulator2));
            if (accumulator3 == null) throw new ArgumentNullException(nameof(accumulator3));
            if (accumulator4 == null) throw new ArgumentNullException(nameof(accumulator4));
            if (accumulator5 == null) throw new ArgumentNullException(nameof(accumulator5));
            if (accumulator6 == null) throw new ArgumentNullException(nameof(accumulator6));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var r1 = new (bool, TResult1)[1];
            var r2 = new (bool, TResult2)[1];
            var r3 = new (bool, TResult3)[1];
            var r4 = new (bool, TResult4)[1];
            var r5 = new (bool, TResult5)[1];
            var r6 = new (bool, TResult6)[1];

            var subject = new Subject<T>();

            using (SubscribeSingle(accumulator1, subject, r1, nameof(accumulator1)))
            using (SubscribeSingle(accumulator2, subject, r2, nameof(accumulator2)))
            using (SubscribeSingle(accumulator3, subject, r3, nameof(accumulator3)))
            using (SubscribeSingle(accumulator4, subject, r4, nameof(accumulator4)))
            using (SubscribeSingle(accumulator5, subject, r5, nameof(accumulator5)))
            using (SubscribeSingle(accumulator6, subject, r6, nameof(accumulator6)))
            {
                foreach (var item in source)
                    subject.OnNext(item);

                subject.OnCompleted();
            }

            return resultSelector
            (
                GetAggregateResult(r1[0], nameof(accumulator1)),
                GetAggregateResult(r2[0], nameof(accumulator2)),
                GetAggregateResult(r3[0], nameof(accumulator3)),
                GetAggregateResult(r4[0], nameof(accumulator4)),
                GetAggregateResult(r5[0], nameof(accumulator5)),
                GetAggregateResult(r6[0], nameof(accumulator6))
            );
        }
    }
}

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;

    partial class MoreEnumerable
    {
        /// <summary>
        /// Applies seven accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
        /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
        /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
        /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
        /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
        /// <typeparam name="TAccumulate6">The type of sixth accumulator value.</typeparam>
        /// <typeparam name="TAccumulate7">The type of seventh accumulator value.</typeparam>
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

        public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TResult>(
            this IEnumerable<T> source,
            TAccumulate1 seed1, Func<TAccumulate1, T, TAccumulate1> accumulator1,
            TAccumulate2 seed2, Func<TAccumulate2, T, TAccumulate2> accumulator2,
            TAccumulate3 seed3, Func<TAccumulate3, T, TAccumulate3> accumulator3,
            TAccumulate4 seed4, Func<TAccumulate4, T, TAccumulate4> accumulator4,
            TAccumulate5 seed5, Func<TAccumulate5, T, TAccumulate5> accumulator5,
            TAccumulate6 seed6, Func<TAccumulate6, T, TAccumulate6> accumulator6,
            TAccumulate7 seed7, Func<TAccumulate7, T, TAccumulate7> accumulator7,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (accumulator1 == null) throw new ArgumentNullException(nameof(accumulator1));
            if (accumulator2 == null) throw new ArgumentNullException(nameof(accumulator2));
            if (accumulator3 == null) throw new ArgumentNullException(nameof(accumulator3));
            if (accumulator4 == null) throw new ArgumentNullException(nameof(accumulator4));
            if (accumulator5 == null) throw new ArgumentNullException(nameof(accumulator5));
            if (accumulator6 == null) throw new ArgumentNullException(nameof(accumulator6));
            if (accumulator7 == null) throw new ArgumentNullException(nameof(accumulator7));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var a1 = seed1;
            var a2 = seed2;
            var a3 = seed3;
            var a4 = seed4;
            var a5 = seed5;
            var a6 = seed6;
            var a7 = seed7;

            foreach (var item in source)
            {
                a1 = accumulator1(a1, item);
                a2 = accumulator2(a2, item);
                a3 = accumulator3(a3, item);
                a4 = accumulator4(a4, item);
                a5 = accumulator5(a5, item);
                a6 = accumulator6(a6, item);
                a7 = accumulator7(a7, item);
            }

            return resultSelector(a1, a2, a3, a4, a5, a6, a7);
        }
    }
}

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections.Generic;
    using Reactive;

    partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Applies seven accumulator queries sequentially in a single
        /// pass over a sequence.
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
        /// <param name="accumulator1">The first accumulator.</param>
        /// <param name="accumulator2">The second accumulator.</param>
        /// <param name="accumulator3">The third accumulator.</param>
        /// <param name="accumulator4">The fourth accumulator.</param>
        /// <param name="accumulator5">The fifth accumulator.</param>
        /// <param name="accumulator6">The sixth accumulator.</param>
        /// <param name="accumulator7">The seventh accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// An <see cref="IObservable{T}"/> returned by an accumulator function
        /// produced zero or more than a single aggregate result.
        /// </exception>
        /// <remarks>
        /// <para>This operator executes immediately.</para>
        /// <para>
        /// Each accumulator argument is a function that receives an
        /// <see cref="IObservable{T}"/>, which when subscribed to, produces the
        /// items in the <paramref name="source"/> sequence and in original
        /// order; the function must then return an <see cref="IObservable{T}"/>
        /// that produces a single aggregate on completion (when
        /// <see cref="IObserver{T}.OnCompleted"/> is called. An error is raised
        /// at run-time if the <see cref="IObserver{T}"/> returned by an
        /// accumulator function produces no result or produces more than a
        /// single result.
        /// </para>
        /// </remarks>

        public static TResult Aggregate<T, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult>(
            this IEnumerable<T> source,
            Func<IObservable<T>, IObservable<TResult1>> accumulator1,
            Func<IObservable<T>, IObservable<TResult2>> accumulator2,
            Func<IObservable<T>, IObservable<TResult3>> accumulator3,
            Func<IObservable<T>, IObservable<TResult4>> accumulator4,
            Func<IObservable<T>, IObservable<TResult5>> accumulator5,
            Func<IObservable<T>, IObservable<TResult6>> accumulator6,
            Func<IObservable<T>, IObservable<TResult7>> accumulator7,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (accumulator1 == null) throw new ArgumentNullException(nameof(accumulator1));
            if (accumulator2 == null) throw new ArgumentNullException(nameof(accumulator2));
            if (accumulator3 == null) throw new ArgumentNullException(nameof(accumulator3));
            if (accumulator4 == null) throw new ArgumentNullException(nameof(accumulator4));
            if (accumulator5 == null) throw new ArgumentNullException(nameof(accumulator5));
            if (accumulator6 == null) throw new ArgumentNullException(nameof(accumulator6));
            if (accumulator7 == null) throw new ArgumentNullException(nameof(accumulator7));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var r1 = new (bool, TResult1)[1];
            var r2 = new (bool, TResult2)[1];
            var r3 = new (bool, TResult3)[1];
            var r4 = new (bool, TResult4)[1];
            var r5 = new (bool, TResult5)[1];
            var r6 = new (bool, TResult6)[1];
            var r7 = new (bool, TResult7)[1];

            var subject = new Subject<T>();

            using (SubscribeSingle(accumulator1, subject, r1, nameof(accumulator1)))
            using (SubscribeSingle(accumulator2, subject, r2, nameof(accumulator2)))
            using (SubscribeSingle(accumulator3, subject, r3, nameof(accumulator3)))
            using (SubscribeSingle(accumulator4, subject, r4, nameof(accumulator4)))
            using (SubscribeSingle(accumulator5, subject, r5, nameof(accumulator5)))
            using (SubscribeSingle(accumulator6, subject, r6, nameof(accumulator6)))
            using (SubscribeSingle(accumulator7, subject, r7, nameof(accumulator7)))
            {
                foreach (var item in source)
                    subject.OnNext(item);

                subject.OnCompleted();
            }

            return resultSelector
            (
                GetAggregateResult(r1[0], nameof(accumulator1)),
                GetAggregateResult(r2[0], nameof(accumulator2)),
                GetAggregateResult(r3[0], nameof(accumulator3)),
                GetAggregateResult(r4[0], nameof(accumulator4)),
                GetAggregateResult(r5[0], nameof(accumulator5)),
                GetAggregateResult(r6[0], nameof(accumulator6)),
                GetAggregateResult(r7[0], nameof(accumulator7))
            );
        }
    }
}

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;

    partial class MoreEnumerable
    {
        /// <summary>
        /// Applies eight accumulators sequentially in a single pass over a
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of elements in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TAccumulate1">The type of first accumulator value.</typeparam>
        /// <typeparam name="TAccumulate2">The type of second accumulator value.</typeparam>
        /// <typeparam name="TAccumulate3">The type of third accumulator value.</typeparam>
        /// <typeparam name="TAccumulate4">The type of fourth accumulator value.</typeparam>
        /// <typeparam name="TAccumulate5">The type of fifth accumulator value.</typeparam>
        /// <typeparam name="TAccumulate6">The type of sixth accumulator value.</typeparam>
        /// <typeparam name="TAccumulate7">The type of seventh accumulator value.</typeparam>
        /// <typeparam name="TAccumulate8">The type of eighth accumulator value.</typeparam>
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

        public static TResult Aggregate<T, TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TAccumulate8, TResult>(
            this IEnumerable<T> source,
            TAccumulate1 seed1, Func<TAccumulate1, T, TAccumulate1> accumulator1,
            TAccumulate2 seed2, Func<TAccumulate2, T, TAccumulate2> accumulator2,
            TAccumulate3 seed3, Func<TAccumulate3, T, TAccumulate3> accumulator3,
            TAccumulate4 seed4, Func<TAccumulate4, T, TAccumulate4> accumulator4,
            TAccumulate5 seed5, Func<TAccumulate5, T, TAccumulate5> accumulator5,
            TAccumulate6 seed6, Func<TAccumulate6, T, TAccumulate6> accumulator6,
            TAccumulate7 seed7, Func<TAccumulate7, T, TAccumulate7> accumulator7,
            TAccumulate8 seed8, Func<TAccumulate8, T, TAccumulate8> accumulator8,
            Func<TAccumulate1, TAccumulate2, TAccumulate3, TAccumulate4, TAccumulate5, TAccumulate6, TAccumulate7, TAccumulate8, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (accumulator1 == null) throw new ArgumentNullException(nameof(accumulator1));
            if (accumulator2 == null) throw new ArgumentNullException(nameof(accumulator2));
            if (accumulator3 == null) throw new ArgumentNullException(nameof(accumulator3));
            if (accumulator4 == null) throw new ArgumentNullException(nameof(accumulator4));
            if (accumulator5 == null) throw new ArgumentNullException(nameof(accumulator5));
            if (accumulator6 == null) throw new ArgumentNullException(nameof(accumulator6));
            if (accumulator7 == null) throw new ArgumentNullException(nameof(accumulator7));
            if (accumulator8 == null) throw new ArgumentNullException(nameof(accumulator8));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var a1 = seed1;
            var a2 = seed2;
            var a3 = seed3;
            var a4 = seed4;
            var a5 = seed5;
            var a6 = seed6;
            var a7 = seed7;
            var a8 = seed8;

            foreach (var item in source)
            {
                a1 = accumulator1(a1, item);
                a2 = accumulator2(a2, item);
                a3 = accumulator3(a3, item);
                a4 = accumulator4(a4, item);
                a5 = accumulator5(a5, item);
                a6 = accumulator6(a6, item);
                a7 = accumulator7(a7, item);
                a8 = accumulator8(a8, item);
            }

            return resultSelector(a1, a2, a3, a4, a5, a6, a7, a8);
        }
    }
}

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections.Generic;
    using Reactive;

    partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Applies eight accumulator queries sequentially in a single
        /// pass over a sequence.
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
        /// <param name="accumulator1">The first accumulator.</param>
        /// <param name="accumulator2">The second accumulator.</param>
        /// <param name="accumulator3">The third accumulator.</param>
        /// <param name="accumulator4">The fourth accumulator.</param>
        /// <param name="accumulator5">The fifth accumulator.</param>
        /// <param name="accumulator6">The sixth accumulator.</param>
        /// <param name="accumulator7">The seventh accumulator.</param>
        /// <param name="accumulator8">The eighth accumulator.</param>
        /// <param name="resultSelector">
        /// A function that projects a single result given the result of each
        /// accumulator.</param>
        /// <returns>The value returned by <paramref name="resultSelector"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// An <see cref="IObservable{T}"/> returned by an accumulator function
        /// produced zero or more than a single aggregate result.
        /// </exception>
        /// <remarks>
        /// <para>This operator executes immediately.</para>
        /// <para>
        /// Each accumulator argument is a function that receives an
        /// <see cref="IObservable{T}"/>, which when subscribed to, produces the
        /// items in the <paramref name="source"/> sequence and in original
        /// order; the function must then return an <see cref="IObservable{T}"/>
        /// that produces a single aggregate on completion (when
        /// <see cref="IObserver{T}.OnCompleted"/> is called. An error is raised
        /// at run-time if the <see cref="IObserver{T}"/> returned by an
        /// accumulator function produces no result or produces more than a
        /// single result.
        /// </para>
        /// </remarks>

        public static TResult Aggregate<T, TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult>(
            this IEnumerable<T> source,
            Func<IObservable<T>, IObservable<TResult1>> accumulator1,
            Func<IObservable<T>, IObservable<TResult2>> accumulator2,
            Func<IObservable<T>, IObservable<TResult3>> accumulator3,
            Func<IObservable<T>, IObservable<TResult4>> accumulator4,
            Func<IObservable<T>, IObservable<TResult5>> accumulator5,
            Func<IObservable<T>, IObservable<TResult6>> accumulator6,
            Func<IObservable<T>, IObservable<TResult7>> accumulator7,
            Func<IObservable<T>, IObservable<TResult8>> accumulator8,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (accumulator1 == null) throw new ArgumentNullException(nameof(accumulator1));
            if (accumulator2 == null) throw new ArgumentNullException(nameof(accumulator2));
            if (accumulator3 == null) throw new ArgumentNullException(nameof(accumulator3));
            if (accumulator4 == null) throw new ArgumentNullException(nameof(accumulator4));
            if (accumulator5 == null) throw new ArgumentNullException(nameof(accumulator5));
            if (accumulator6 == null) throw new ArgumentNullException(nameof(accumulator6));
            if (accumulator7 == null) throw new ArgumentNullException(nameof(accumulator7));
            if (accumulator8 == null) throw new ArgumentNullException(nameof(accumulator8));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var r1 = new (bool, TResult1)[1];
            var r2 = new (bool, TResult2)[1];
            var r3 = new (bool, TResult3)[1];
            var r4 = new (bool, TResult4)[1];
            var r5 = new (bool, TResult5)[1];
            var r6 = new (bool, TResult6)[1];
            var r7 = new (bool, TResult7)[1];
            var r8 = new (bool, TResult8)[1];

            var subject = new Subject<T>();

            using (SubscribeSingle(accumulator1, subject, r1, nameof(accumulator1)))
            using (SubscribeSingle(accumulator2, subject, r2, nameof(accumulator2)))
            using (SubscribeSingle(accumulator3, subject, r3, nameof(accumulator3)))
            using (SubscribeSingle(accumulator4, subject, r4, nameof(accumulator4)))
            using (SubscribeSingle(accumulator5, subject, r5, nameof(accumulator5)))
            using (SubscribeSingle(accumulator6, subject, r6, nameof(accumulator6)))
            using (SubscribeSingle(accumulator7, subject, r7, nameof(accumulator7)))
            using (SubscribeSingle(accumulator8, subject, r8, nameof(accumulator8)))
            {
                foreach (var item in source)
                    subject.OnNext(item);

                subject.OnCompleted();
            }

            return resultSelector
            (
                GetAggregateResult(r1[0], nameof(accumulator1)),
                GetAggregateResult(r2[0], nameof(accumulator2)),
                GetAggregateResult(r3[0], nameof(accumulator3)),
                GetAggregateResult(r4[0], nameof(accumulator4)),
                GetAggregateResult(r5[0], nameof(accumulator5)),
                GetAggregateResult(r6[0], nameof(accumulator6)),
                GetAggregateResult(r7[0], nameof(accumulator7)),
                GetAggregateResult(r8[0], nameof(accumulator8))
            );
        }
    }
}
