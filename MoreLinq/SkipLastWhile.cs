#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2024 Andy Romero (armorynode). All rights reserved.
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

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Removes elements from the end of a sequence as long as a specified condition is true.
        /// </summary>
        /// <typeparam name="T">Type of the source sequence</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="predicate">The predicate to use to remove items from the tail of the sequence.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> containing the source sequence elements except for the bypassed ones at the end.
        /// </returns>
        /// <exception cref="ArgumentNullException">The source sequence is null.</exception>
        /// <exception cref="ArgumentNullException">The predicate is null.</exception>

        public static IEnumerable<T> SkipLastWhile<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return _(source, predicate);

            static IEnumerable<T> _(IEnumerable<T> source, Func<T, bool> predicate)
            {
                var list = source.ToArray();
                int tailIndex;
                for (tailIndex = list.Length - 1; tailIndex >= 0; tailIndex--)
                {
                    if (!predicate(list[tailIndex]))
                        break;
                }

                for (var returnIndex = 0; returnIndex <= tailIndex; returnIndex++)
                {
                    yield return list[returnIndex];
                }
            }
        }
    }
}
