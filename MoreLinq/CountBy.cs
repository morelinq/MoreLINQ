#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Leandro F. Vieira (leandromoh). All rights reserved.
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
        /// Applies a key-generating function to each element of a sequence and returns a sequence of 
        /// unique keys and their number of occurrences in the original sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="keySelector">Function that transforms each item of source sequence into a key to be compared against the others</param>
        /// <returns>A sequence of unique keys and their number of occurrences in the original sequence</returns>
        public static IEnumerable<KeyValuePair<TKey, int>> CountBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.CountBy(keySelector, null);
        }

        /// <summary>
        /// Applies a key-generating function to each element of a sequence and returns a sequence of 
        /// unique keys and their number of occurrences in the original sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="keySelector">Function that transforms each item of source sequence into a key to be compared against the others</param>
        /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <c>TSource</c> is used.</param>
        /// <returns>A sequence of unique keys and their number of occurrences in the original sequence</returns>
        public static IEnumerable<KeyValuePair<TKey, int>> CountBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return CountByImpl(source, keySelector, comparer);
        }

        private static IEnumerable<KeyValuePair<TKey, int>> CountByImpl<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            var dic = new Dictionary<TKey, int>(comparer);

            foreach (var item in source)
            {
                TKey key = keySelector(item);

                if (dic.ContainsKey(key))
                {
                    dic[key]++;
                }
                else
                {
                    dic[key] = 1;
                }
            }

            return dic;
        }
    }
}
