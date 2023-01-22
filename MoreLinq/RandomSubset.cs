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
        /// Returns a sequence of a specified size of random elements from the
        /// original sequence.
        /// </summary>
        /// <typeparam name="T">The type of source sequence elements.</typeparam>
        /// <param name="source">
        /// The sequence from which to return random elements.</param>
        /// <param name="subsetSize">The size of the random subset to return.</param>
        /// <returns>
        /// A random sequence of elements in random order from the original
        /// sequence.</returns>

        public static IEnumerable<T> RandomSubset<T>(this IEnumerable<T> source, int subsetSize)
        {
            return RandomSubset(source, subsetSize, GlobalRandom.Instance);
        }

        /// <summary>
        /// Returns a sequence of a specified size of random elements from the
        /// original sequence. An additional parameter specifies a random
        /// generator to be used for the random selection algorithm.
        /// </summary>
        /// <typeparam name="T">The type of source sequence elements.</typeparam>
        /// <param name="source">
        /// The sequence from which to return random elements.</param>
        /// <param name="subsetSize">The size of the random subset to return.</param>
        /// <param name="rand">
        /// A random generator used as part of the selection algorithm.</param>
        /// <returns>
        /// A random sequence of elements in random order from the original
        /// sequence.</returns>

        public static IEnumerable<T> RandomSubset<T>(this IEnumerable<T> source, int subsetSize, Random rand)
        {
            if (rand == null) throw new ArgumentNullException(nameof(rand));
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (subsetSize < 0) throw new ArgumentOutOfRangeException(nameof(subsetSize));

            return RandomSubsetImpl(source, rand, seq => (seq.ToArray(), subsetSize));
        }

        static IEnumerable<T> RandomSubsetImpl<T>(IEnumerable<T> source, Random rand, Func<IEnumerable<T>, (T[], int)> seeder)
        {
            // The simplest and most efficient way to return a random subset is to perform
            // an in-place, partial Fisher-Yates shuffle of the sequence. While we could do
            // a full shuffle, it would be wasteful in the cases where subsetSize is shorter
            // than the length of the sequence.
            // See: http://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle

            var (array, subsetSize) = seeder(source);

            if (array.Length < subsetSize)
            {
                throw new ArgumentOutOfRangeException(nameof(subsetSize),
                    "Subset size must be less than or equal to the source length.");
            }

            var m = 0;                // keeps track of count items shuffled
            var w = array.Length;     // upper bound of shrinking swap range
            var g = w - 1;            // used to compute the second swap index

            // perform in-place, partial Fisher-Yates shuffle
            while (m < subsetSize)
            {
#pragma warning disable CA5394 // Do not use insecure randomness
                var k = g - rand.Next(w);
#pragma warning restore CA5394 // Do not use insecure randomness
                (array[k], array[m]) = (array[m], array[k]);
                ++m;
                --w;
            }

            // yield the random subset as a new sequence
            for (var i = 0; i < subsetSize; i++)
                yield return array[i];
        }
    }
}
