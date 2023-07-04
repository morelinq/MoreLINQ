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
    using System.Linq;
    using System.Collections.Generic;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Inserts the elements of a sequence into another sequence at a
        /// specified index from the tail of the sequence, where zero always
        /// represents the last position, one represents the second-last
        /// element, two represents the third-last element and so on.
        /// </summary>
        /// <typeparam name="T">
        /// Type of elements in all sequences.</typeparam>
        /// <param name="first">The source sequence.</param>
        /// <param name="second">The sequence that will be inserted.</param>
        /// <param name="index">
        /// The zero-based index from the end of <paramref name="first"/> where
        /// elements from <paramref name="second"/> should be inserted.
        /// <paramref name="second"/>.</param>
        /// <returns>
        /// A sequence that contains the elements of <paramref name="first"/>
        /// plus the elements of <paramref name="second"/> inserted at
        /// the given index from the end of <paramref name="first"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="second"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="index"/> is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown lazily if <paramref name="index"/> is greater than the
        /// length of <paramref name="first"/>. The validation occurs when
        /// the resulting sequence is iterated.
        /// </exception>
        /// <remarks>
        /// This method uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<T> Backsert<T>(this IEnumerable<T> first, IEnumerable<T> second, int index)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            return index switch
            {
                < 0 => throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative."),
                0 => first.Concat(second),
                _ => _()
            };

            IEnumerable<T> _()
            {
                using var e = first.CountDown(index, ValueTuple.Create).GetEnumerator();

                if (e.MoveNext())
                {
                    var (_, countdown) = e.Current;
                    if (countdown is { } n && n != index - 1)
                        throw new ArgumentOutOfRangeException(nameof(index), "Insertion index is greater than the length of the first sequence.");

                    do
                    {
                        T a;
                        (a, countdown) = e.Current;
                        if (countdown == index - 1)
                        {
                            foreach (var b in second)
                                yield return b;
                        }

                        yield return a;
                    }
                    while (e.MoveNext());
                }
            }
        }
    }
}
