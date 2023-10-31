#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2010 Leopold Bushkin. All rights reserved.
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

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Produces a projection of a sequence by evaluating pairs of elements separated by a
        /// negative offset.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence.</typeparam>
        /// <param name="source">The sequence over which to evaluate lag.</param>
        /// <param name="offset">The offset (expressed as a positive number) by which to lag each
        /// value of the sequence.</param>
        /// <param name="resultSelector">A projection function which accepts the current and lagged
        /// items (in that order) and returns a result.</param>
        /// <returns>
        /// A sequence produced by projecting each element of the sequence with its lagged
        /// pairing.</returns>
        /// <remarks>
        /// <para>
        /// This operator evaluates in a deferred and streaming manner.</para>
        /// <para>
        /// For elements prior to the lag offset, <c>default(T)</c> is used as the lagged
        /// value.</para>
        /// </remarks>

        public static IEnumerable<TResult> Lag<TSource, TResult>(this IEnumerable<TSource> source, int offset, Func<TSource, TSource?, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return source.Select(Some)
                         .Lag(offset, default, (curr, lag) => resultSelector(curr.Value, lag is (true, var some) ? some : default));
        }

        /// <summary>
        /// Produces a projection of a sequence by evaluating pairs of elements separated by a
        /// negative offset.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence.</typeparam>
        /// <param name="source">The sequence over which to evaluate lag.</param>
        /// <param name="offset">The offset (expressed as a positive number) by which to lag each
        /// value of the sequence.</param>
        /// <param name="defaultLagValue">A default value supplied for the lagged value prior to the
        /// lag offset.</param>
        /// <param name="resultSelector">A projection function which accepts the current and lagged
        /// items (in that order) and returns a result.</param>
        /// <returns>
        /// A sequence produced by projecting each element of the sequence with its lagged
        /// pairing.</returns>
        /// <remarks>
        /// This operator evaluates in a deferred and streaming manner.
        /// </remarks>

        public static IEnumerable<TResult> Lag<TSource, TResult>(this IEnumerable<TSource> source, int offset, TSource defaultLagValue, Func<TSource, TSource, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            // NOTE: Theoretically, we could assume that negative (or zero-offset) lags could be
            //       re-written as: sequence.Lead( -lagBy, resultSelector ). However, I'm not sure
            //       that it's an intuitive - or even desirable - behavior. So it's being omitted.
            if (offset <= 0) throw new ArgumentOutOfRangeException(nameof(offset));

            return _(); IEnumerable<TResult> _()
            {
                using var iter = source.GetEnumerator();

                var i = offset;
                var lagQueue = new Queue<TSource>(offset);
                // until we progress far enough, the lagged value is defaultLagValue
                var hasMore = true;
                // NOTE: The if statement below takes advantage of short-circuit evaluation
                //       to ensure we don't advance the iterator when we reach the lag offset.
                //       Do not reorder the terms in the condition!
                while (i-- > 0 && (hasMore = iter.MoveNext()))
                {
                    lagQueue.Enqueue(iter.Current);
                    // until we reach the lag offset, the lagged value is the defaultLagValue
                    yield return resultSelector(iter.Current, defaultLagValue);
                }

                if (hasMore) // check that we didn't consume the sequence yet
                {
                    // now the lagged value is derived from the sequence
                    while (iter.MoveNext())
                    {
                        var lagValue = lagQueue.Dequeue();
                        yield return resultSelector(iter.Current, lagValue);
                        lagQueue.Enqueue(iter.Current);
                    }
                }
            }
        }

        /// <summary>
        /// Produces a sequence of tuple containing a pair of elements separated by a negative offset.
        /// </summary>
        /// <remarks>
        /// This operator evaluates in a deferred and streaming manner.<br/>
        /// For elements prior to the lag offset, <c>default(T)</c> is used as the lagged value.<br/>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
        /// <param name="source">The sequence over which to evaluate lag</param>
        /// <param name="offset">The offset (expressed as a positive number) by which to lag each value of the sequence</param>
        /// <returns>A sequence of element of the sequence with its lagged pairing</returns>

        public static IEnumerable<(TSource Item, TSource OffsetItem)> Lag<TSource>(this IEnumerable<TSource> source, int offset)
        {
            return Lag(source, offset, default, ValueTuple.Create);
        }

        /// <summary>
        /// Produces a sequence of tuple containing a pair of elements separated by a negative offset.
        /// </summary>
        /// <remarks>
        /// This operator evaluates in a deferred and streaming manner.<br/>
        /// For elements prior to the lag offset, <paramref name="defaultLagValue"/> is used as the lagged value.<br/>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
        /// <param name="source">The sequence over which to evaluate lag</param>
        /// <param name="offset">The offset (expressed as a positive number) by which to lag each value of the sequence</param>
        /// <param name="defaultLagValue">A default value supplied for the lagged value prior to the lag offset</param>
        /// <returns>A sequence of element of the sequence with its lagged pairing</returns>

        public static IEnumerable<(TSource Item, TSource OffsetItem)> Lag<TSource>(this IEnumerable<TSource> source, int offset, TSource defaultLagValue)
        {
            return Lag(source, offset, defaultLagValue, ValueTuple.Create);
        }
    }
}
