using System;
using System.Collections.Generic;

namespace MoreLinq
{
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns the Cartesian product of two sequences by combining each element of the first set with each in the second
        /// and applying the user=define projection to the pair.
        /// </summary>
        /// <typeparam name="TA">The type of the elements of <paramref name="sequenceA"/></typeparam>
        /// <typeparam name="TB">The type of the elements of <paramref name="sequenceB"/></typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence</typeparam>
        /// <param name="sequenceA">The first sequence of elements</param>
        /// <param name="sequenceB">The second sequence of elements</param>
        /// <param name="resultSelector">A projection function that combines elements from both sequences</param>
        /// <returns>A sequence representing the Cartesian product of the two source sequences</returns>
        public static IEnumerable<TResult> Cartesian<TA, TB, TResult>(this IEnumerable<TA> sequenceA, IEnumerable<TB> sequenceB, Func<TA, TB, TResult> resultSelector)
        {
            sequenceA.ThrowIfNull("sequenceA");
            sequenceB.ThrowIfNull("sequenceB");
            resultSelector.ThrowIfNull("resultSelector");

            return CartesianImpl(sequenceA, sequenceB, resultSelector);
        }

        private static IEnumerable<TResult> CartesianImpl<TA, TB, TResult>(this IEnumerable<TA> sequenceA, IEnumerable<TB> sequenceB, Func<TA, TB, TResult> resultSelector)
        {
            foreach (var itemA in sequenceA)
                foreach (var itemB in sequenceB)
                    yield return resultSelector(itemA, itemB);
        }
    }
}