#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Vegard Løkken. All rights reserved.
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

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Applies a conditional mapping of the sequence.
        /// </summary>
        /// <typeparam name="T">Type of elements in sequence</typeparam>
        /// <param name="sequence">The sequence to repeat</param>
        /// <param name="condition">The condition of when to apply the map</param>
        /// <param name="then">Mapping function</param>
        /// <returns>The mapped sequence or the same based on the condition</returns>

        public static IEnumerable<T> If<T>(this IEnumerable<T> sequence, bool condition, Func<IEnumerable<T>, IEnumerable<T>> then)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (then == null) throw new ArgumentNullException(nameof(then));

            return condition ? then(sequence) : sequence;
        }

        /// <summary>
        /// Applies a conditional mapping of the sequence based on a predicate.
        /// </summary>
        /// <typeparam name="T">Type of elements in sequence</typeparam>
        /// <param name="sequence">The sequence to repeat</param>
        /// <param name="predicate">The condition of when to apply the map</param>
        /// <param name="then">Mapping function</param>
        /// <returns>The mapped sequence or the same based on the predicate</returns>

        public static IEnumerable<T> If<T>(this IEnumerable<T> sequence, Func<IEnumerable<T>, bool> predicate, Func<IEnumerable<T>, IEnumerable<T>> then)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (then == null) throw new ArgumentNullException(nameof(then));

            return predicate(sequence) ? then(sequence) : sequence;
        }
    }
}
