#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
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