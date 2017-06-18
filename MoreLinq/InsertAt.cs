#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Leandro F. Vieira (leandromoh). All rights reserved.
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

    static partial class MoreEnumerable
    {

        /// <summary>
        /// Inserts the elements of second sequence into the source at the specified index.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="second">The sequence whose elements should be inserted into the source.</param>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <returns>A sequence </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if either <paramref name="index"/> is negative or 
        /// <paramref name="index"/> is greater than source's length.
        /// </exception>
        
        public static IEnumerable<T> InsertAt<T>(this IEnumerable<T> source, IEnumerable<T> second, int index)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (index < 0)  throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative.");

            return _(); IEnumerable<T> _()
            {
                var i = -1;

                using (var iter = source.GetEnumerator())
                {
                    while (++i < index && iter.MoveNext())
                        yield return iter.Current;

                    if (i < index)
                       throw new ArgumentOutOfRangeException(nameof(index), "Index is greater than source's length.");

                    foreach (var item in second)
                        yield return item;

                    while (iter.MoveNext())
                        yield return iter.Current;
                }
            }
        }
    }
}
