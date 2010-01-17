using System;
using System.Collections.Generic;

namespace MoreLinq
{
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Generates all subsets and all permutations of those subsets for a given sequence 
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="sequence">The sequence for which to generate permuted subsets</param>
        /// <returns>A sequence of lists representing the permuted subsets of the original sequence</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="sequence"/> is <see langword="null"/>
        /// </exception>
        public static IEnumerable<IList<T>> PermutedSubsets<T>(this IEnumerable<T> sequence)
        {
            sequence.ThrowIfNull("sequence");

            return PermutedSubsetsImpl(sequence);
        }

        /// <summary>
        /// Generates all k-sized subsets and all permutations of those subsets for a given sequence
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="sequence">The sequence for which to generate permuted subsets</param>
        /// <param name="subsetSize">The size of subsets to produce (all others are omitted)</param>
        /// <returns>A sequence of lists representing the permuted subsets of the original sequence</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="sequence"/> is <see langword="null"/>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="subsetSize"/> is less than zero or greater than <c>sequence.Count()</c>
        /// </exception>
        public static IEnumerable<IList<T>> PermutedSubsets<T>(this IEnumerable<T> sequence, int subsetSize)
        {
            sequence.ThrowIfNull("sequence");
            subsetSize.ThrowIfNegative("sequence");

            return PermutedSubsetsImpl(sequence, subsetSize);
        }

        /// <summary>
        /// Private implementation that returns all permuted subsets of an original sequence.
        /// </summary>
        private static IEnumerable<IList<T>> PermutedSubsetsImpl<T>(IEnumerable<T> sequence)
        {
            foreach (var subset in sequence.Subsets())
                foreach (var permutation in subset.Permutations())
                    yield return permutation;
        }

        /// <summary>
        /// Private implementation that returns k-sized permuted subsets of an original sequence.
        /// </summary>
        private static IEnumerable<IList<T>> PermutedSubsetsImpl<T>(IEnumerable<T> sequence, int subsetSize)
        {
            foreach (var subset in sequence.Subsets(subsetSize))
                foreach (var permutation in subset.Permutations())
                    yield return permutation;
        }
    }
}