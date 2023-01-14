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
        /// Ranks each item in the sequence in descending ordering using a default comparer.
        /// </summary>
        /// <typeparam name="TSource">Type of item in the sequence</typeparam>
        /// <param name="source">The sequence whose items will be ranked</param>
        /// <returns>A sequence of position integers representing the ranks of the corresponding items in the sequence</returns>

        public static IEnumerable<int> Rank<TSource>(this IEnumerable<TSource> source)
        {
            return source.RankBy(IdFn);
        }

        /// <summary>
        /// Rank each item in the sequence using a caller-supplied comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
        /// <param name="source">The sequence of items to rank</param>
        /// <param name="comparer">A object that defines comparison semantics for the elements in the sequence</param>
        /// <returns>A sequence of position integers representing the ranks of the corresponding items in the sequence</returns>

        public static IEnumerable<int> Rank<TSource>(this IEnumerable<TSource> source, IComparer<TSource>? comparer)
        {
            return source.RankBy(IdFn, comparer);
        }

        /// <summary>
        /// Ranks each item in the sequence in descending ordering by a specified key using a default comparer
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
        /// <typeparam name="TKey">The type of the key used to rank items in the sequence</typeparam>
        /// <param name="source">The sequence of items to rank</param>
        /// <param name="keySelector">A key selector function which returns the value by which to rank items in the sequence</param>
        /// <returns>A sequence of position integers representing the ranks of the corresponding items in the sequence</returns>

        public static IEnumerable<int> RankBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return RankBy(source, keySelector, null);
        }

        /// <summary>
        /// Ranks each item in a sequence using a specified key and a caller-supplied comparer
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
        /// <typeparam name="TKey">The type of the key used to rank items in the sequence</typeparam>
        /// <param name="source">The sequence of items to rank</param>
        /// <param name="keySelector">A key selector function which returns the value by which to rank items in the sequence</param>
        /// <param name="comparer">An object that defines the comparison semantics for keys used to rank items</param>
        /// <returns>A sequence of position integers representing the ranks of the corresponding items in the sequence</returns>

        public static IEnumerable<int> RankBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            return _(comparer ?? Comparer<TKey>.Default);

            IEnumerable<int> _(IComparer<TKey> comparer)
            {
                source = source.ToArray(); // avoid enumerating source twice

                var rankDictionary = new Collections.Dictionary<TSource, int>(EqualityComparer<TSource>.Default);
                var i = 1;
                foreach (var item in source.Distinct()
                                           .OrderByDescending(keySelector, comparer))
                {
                    rankDictionary[item] = i++;
                }

                // The following loop should not be be converted to a query to
                // keep this RankBy lazy.

                // ReSharper disable LoopCanBeConvertedToQuery

                foreach (var item in source)
                    yield return rankDictionary[item];

                // ReSharper restore LoopCanBeConvertedToQuery
            }
        }
    }
}
