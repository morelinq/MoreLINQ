#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Leandro F. Vieira (leandromoh). All rights reserved.
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
        /// Returns a sequence of elements in random order from the original
        /// sequence.
        /// </summary>
        /// <typeparam name="T">The type of source sequence elements.</typeparam>
        /// <param name="sequence">
        /// The sequence from which to return random elements.</param>
        /// <returns>
        /// A sequence of elements <paramref name="sequence"/> randomized in
        /// their order.
        /// </returns>

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> sequence)
        {
            return Shuffle(sequence, new Random());
        }

        /// <summary>
        /// Returns a sequence of elements in random order from the original
        /// sequence. An additional parameter specifies a random generator to be
        /// used for the random selection algorithm.
        /// </summary>
        /// <typeparam name="T">The type of source sequence elements.</typeparam>
        /// <param name="sequence">
        /// The sequence from which to return random elements.</param>
        /// <param name="rand">
        /// A random generator used as part of the selection algorithm.</param>
        /// <returns>
        /// A sequence of elements <paramref name="sequence"/> randomized in
        /// their order.
        /// </returns>

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> sequence, Random rand)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (rand == null) throw new ArgumentNullException(nameof(rand));

            return RandomSubsetImpl(sequence, rand, source =>
            {
                var array = source.ToArray();
                return (array, array.Length);
            });
        }
    }
}
