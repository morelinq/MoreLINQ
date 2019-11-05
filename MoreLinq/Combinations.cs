using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreLinq
{
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Generate all the possible combination of the items from the input sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="sequence">Sequence for which to produce combination</param>
        /// <returns>A sequence of all combination from the input sequence</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <see langword="null"/></exception>
        public static IEnumerable<IList<T>> Combinations<T>(this IEnumerable<T> sequence)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));

            return sequence.Subsets().SelectMany(Permutations);
        }

        /// <summary>
        /// Generate all the possible combination of <paramref name="size"/> items from the input <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="sequence">Sequence for which to produce combination</param>
        /// <param name="size">The combinations size</param>
        /// <returns>A sequence of all combination from the input sequence</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sequence"/> is <see langword="null"/></exception>
        public static IEnumerable<IList<T>> Combinations<T>(this IEnumerable<T> sequence, int size)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));

            return sequence.Subsets(size).SelectMany(Permutations);
        }
    }
}
