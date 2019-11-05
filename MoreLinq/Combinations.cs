#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Pierre Lando. All rights reserved.
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
