using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreLinq
{
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a sequence of a specified size of random elements from the original sequence
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence</typeparam>
        /// <param name="sequence">The sequence from which to return random elements</param>
        /// <param name="subsetSize">The size of the random subset to return</param>
        /// <returns>A random sequence of elements in random order from the original sequence</returns>
        public static IEnumerable<T> RandomSubset<T>(this IEnumerable<T> sequence, int subsetSize)
        {
            return RandomSubset(sequence, subsetSize, new Random());
        }

        /// <summary>
        /// Returns a sequence of a specified size of random elements from the original sequence
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence</typeparam>
        /// <param name="sequence">The sequence from which to return random elements</param>
        /// <param name="subsetSize">The size of the random subset to return</param>
        /// <param name="rand">A random generator used as part of the selection algorithm</param>
        /// <returns>A random sequence of elements in random order from the original sequence</returns>
        public static IEnumerable<T> RandomSubset<T>(this IEnumerable<T> sequence, int subsetSize, Random rand)
        {
            if (rand == null) throw new ArgumentNullException("rand");
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (subsetSize < 0) throw new ArgumentOutOfRangeException("subsetSize");

            return RandomSubsetImpl(sequence, subsetSize, rand);
        }

        /// <summary>
        /// Private implementation method that generates a random subset of a sequence
        /// </summary>
        private static IEnumerable<T> RandomSubsetImpl<T>(IEnumerable<T> sequence, int subsetSize, Random rand)
        {
            // The simplest and most efficient way to return a random subet is to perform 
            // an in-place, partial Fisher-Yates shuffle of the sequence. While we could do 
            // a full shuffle, it would be wasteful in the cases where subsetSize is shorter
            // than the length of the sequence.
            // See: http://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle

            var seqArray = sequence.ToArray();
            if (seqArray.Length < subsetSize)
                throw new ArgumentOutOfRangeException("subsetSize", "Subset size must be <= sequence.Count()");

            var m = 0;                // keeps track of count items shuffled
            var w = seqArray.Length;  // upper bound of shrinking swap range
            var g = w - 1;            // used to compute the second swap index

            // perform in-place, partial Fisher-Yates shuffle
            while (m < subsetSize)
            {
                var k = g - rand.Next(w);
                var tmp = seqArray[k];
                seqArray[k] = seqArray[m];
                seqArray[m] = tmp;
                ++m;
                --w;
            }

            // yield the random subet as a new sequence
            for (var i = 0; i < subsetSize; i++)
                yield return seqArray[i];
        }
    }
}