using System;
using System.Collections.Generic;

namespace MoreLinq
{
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Produces a projection of a sequence by evaluating pairs of elements separated by a negative offset.
        /// </summary>
        /// <remarks>
        /// This operator evaluates in a deferred and streaming manner.<br/>
        /// For elements prior to the lag offset, <c>default(T) is used as the lagged value.</c><br/>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence</typeparam>
        /// <param name="sequence">The sequence over which to evaluate lag</param>
        /// <param name="lagBy">The offset (expressed as a positive number) by which to lag each value of the sequence</param>
        /// <param name="resultSelector">A projection function which accepts the current and lagged items (in that order) and returns a result</param>
        /// <returns>A sequence produced by projecting each element of the sequence with its lagged pairing</returns>
        public static IEnumerable<TResult> Lag<TSource, TResult>(this IEnumerable<TSource> sequence, int lagBy, Func<TSource, TSource, TResult> resultSelector)
        {
            return Lag(sequence, lagBy, default(TSource), resultSelector);
        }

        /// <summary>
        /// Produces a projection of a sequence by evaluating pairs of elements separated by a negative offset.
        /// </summary>
        /// <remarks>
        /// This operator evaluates in a deferred and streaming manner.<br/>
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence</typeparam>
        /// <param name="sequence">The sequence over which to evaluate lag</param>
        /// <param name="lagBy">The offset (expressed as a positive number) by which to lag each value of the sequence</param>
        /// <param name="defaultLagValue">A default value supplied for the lagged value prior to the lag offset</param>
        /// <param name="resultSelector">A projection function which accepts the current and lagged items (in that order) and returns a result</param>
        /// <returns>A sequence produced by projecting each element of the sequence with its lagged pairing</returns>
        public static IEnumerable<TResult> Lag<TSource, TResult>(this IEnumerable<TSource> sequence, int lagBy, TSource defaultLagValue, Func<TSource, TSource, TResult> resultSelector)
        {
            sequence.ThrowIfNull("sequence");
            resultSelector.ThrowIfNull("resultSelector");
            // NOTE: Theoretically, we could assume that negative (or zero-offset) lags could be
            //       re-written as: sequence.Lead( -lagBy, resultSelector ). However, I'm not sure
            //       that it's an intuitive - or even desirable - behavior. So it's being omitted.
            lagBy.ThrowIfNonPositive("lagBy");
            
            return LagImpl(sequence, lagBy, defaultLagValue, resultSelector);
        }

        private static IEnumerable<TResult> LagImpl<TSource, TResult>(IEnumerable<TSource> sequence, int lagBy, TSource defaultLagValue, Func<TSource, TSource, TResult> resultSelector)
        {
            using (var iter = sequence.GetEnumerator())
            {
                var lagOffset = lagBy;
                // until we progress far enough, the lagged value is defaultLagValue
                var hasMore = true;
                // NOTE: The if statement below takes advantage of short-circuit evaluation
                //       to ensure we don't advance the iterator when we reach the lag offset.
                //       Do not reorder the terms in the condition!
                while (lagOffset-- > 0 && (hasMore = iter.MoveNext()))
                {
                    // until we reach the lag offset, the lagged value is the defaultLagValue
                    yield return resultSelector(iter.Current, defaultLagValue);
                }

                if (hasMore) // check that we didn't consume the sequence yet
                {
                    // now the lagged value is derived from the sequence
                    var lagValue = iter.Current;
                    while (iter.MoveNext())
                    {
                        yield return resultSelector(iter.Current, lagValue);
                        lagValue = iter.Current;
                    }
                }
            }
        }
    }
}