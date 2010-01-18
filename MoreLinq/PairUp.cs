using System;
using System.Collections.Generic;

namespace MoreLinq
{
    /// <summary>
    /// Enumeration defining strategies used by PairUp() for handling sequences that are not even in length.
    /// </summary>
    public enum PairUpImbalanceStrategy
    {
        /// <summary>
        /// Pad the sequence with a default value (which is paired with the terminal element)
        /// </summary>
        Pad = 0,
        /// <summary>
        /// Skip the terminal element, and omit it from the resulting sequence
        /// </summary>
        Skip = 1,
    }

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Evaluates adjacent pairs of elements in the source sequence and returns the result of
        /// applying a user-defined projection to the pair. Uses the Pad imbalance strategy if the
        /// source sequence contains an odd number of elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
        /// <param name="sequence">The sequence of elements to pair up</param>
        /// <param name="resultSelector">A projection applied to each pair of elements in the source sequence</param>
        /// <returns>A sequence of projected elements produced from pairs in the source sequence</returns>
        public static IEnumerable<TResult> PairUp<TSource, TResult>(this IEnumerable<TSource> sequence, Func<TSource, TSource, TResult> resultSelector)
        {
            return PairUp(sequence, resultSelector, PairUpImbalanceStrategy.Pad);
        }

        /// <summary>
        /// Evaluates adjacent pairs of elements in the source sequence and returns the result of
        /// applying a user-defined projection to the pair.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
        /// <param name="sequence">The sequence of elements to pair up</param>
        /// <param name="resultSelector">A projection applied to each pair of elements in the source sequence</param>
        /// <param name="imbalanceStrategy">An imbalance strategy enumeration that controls behavior when the
        /// source sequence contains an odd number of elements.</param>
        /// <returns>A sequence of projected elements produced from pairs in the source sequence</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="sequence"/> or <paramref name="resultSelector"/> are <see langword="null"/>.
        /// </exception>
        public static IEnumerable<TResult> PairUp<TSource, TResult>(this IEnumerable<TSource> sequence, Func<TSource, TSource, TResult> resultSelector, PairUpImbalanceStrategy imbalanceStrategy)
        {
            sequence.ThrowIfNull("sequence");
            resultSelector.ThrowIfNull("resultSelector");
            
            return PairUpImpl(sequence, resultSelector, imbalanceStrategy);
        }

        /// <summary>
        /// Private implementation of the PairUp operator.
        /// </summary>
        private static IEnumerable<TResult> PairUpImpl<TSource, TResult>(this IEnumerable<TSource> sequence, Func<TSource, TSource, TResult> resultSelector, PairUpImbalanceStrategy imbalanceStrategy)
        {
            using (var iter = sequence.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    var first = iter.Current;
                    if (iter.MoveNext())
                    {
                        var second = iter.Current;
                        yield return resultSelector(first, second);
                    }
                    else
                    {
                        // handles case when the source sequence
                        // contains an odd number of elements
                        switch (imbalanceStrategy)
                        {
                            case PairUpImbalanceStrategy.Pad:
                                var second = default(TSource);
                                yield return resultSelector(first, second);
                                break;
                            
                            case PairUpImbalanceStrategy.Skip:
                                continue;
                        }
                    }
                }
            }
        }
    }
}