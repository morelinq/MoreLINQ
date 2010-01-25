using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreLinq
{
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Ranks each item in the sequence in descending ordering using a default comparer.
        /// </summary>
        /// <typeparam name="T">Type of item in the sequence</typeparam>
        /// <param name="sequence">The sequence whose items will be ranked</param>
        /// <returns>A sequence of position integers representing the ranks of the corresponding items in the sequence</returns>
        public static IEnumerable<int> Rank<T>(this IEnumerable<T> sequence)
        {
            return sequence.RankBy(x => x);
        }

        /// <summary>
        /// Rank each item in the sequence using a caller-supplied comparer.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence</typeparam>
        /// <param name="sequence">The sequence of items to rank</param>
        /// <param name="comparer">A object that defines comparison semantics for the elements in the sequence</param>
        /// <returns>A sequence of position integers representing the ranks of the corresponding items in the sequence</returns>
        public static IEnumerable<int> Rank<T>(this IEnumerable<T> sequence, IComparer<T> comparer)
        {
            return sequence.RankBy(x => x, comparer);
        }

        /// <summary>
        /// Ranks each item in the sequence in descending ordering by a specified key using a default comparer
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence</typeparam>
        /// <typeparam name="TKey">The type of the key used to rank items in the sequence</typeparam>
        /// <param name="sequence">The sequence of items to rank</param>
        /// <param name="rankKeySelector">A key selector function which returns the value by which to rank items in the sequence</param>
        /// <returns>A sequence of position integers representing the ranks of the corresponding items in the sequence</returns>
        public static IEnumerable<int> RankBy<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> rankKeySelector)
        {
            return RankBy(sequence, rankKeySelector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Ranks each item in a sequence using a specified key and a caller-supplied comparer
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence</typeparam>
        /// <typeparam name="TKey">The type of the key used to rank items in the sequence</typeparam>
        /// <param name="sequence">The sequence of items to rank</param>
        /// <param name="rankKeySelector">A key selector function which returns the value by which to rank items in the sequence</param>
        /// <param name="comparer">An object that defines the comparison semantics for keys used to rank items</param>
        /// <returns>A sequence of position integers representing the ranks of the corresponding items in the sequence</returns>
        public static IEnumerable<int> RankBy<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> rankKeySelector, IComparer<TKey> comparer)
        {
            sequence.ThrowIfNull("sequence");
            rankKeySelector.ThrowIfNull("rankKeySelector");
            comparer.ThrowIfNull("comparer");

            return RankByImpl(sequence, rankKeySelector, comparer);
        }
        
        private static IEnumerable<int> RankByImpl<T, TKey>(IEnumerable<T> sequence, Func<T, TKey> rankKeySelector, IComparer<TKey> comparer )
        {
            var rank = 0;
            var rankDictionary = new Dictionary<T, int>();
            foreach (var item in sequence.Distinct().OrderByDescending(rankKeySelector, comparer))
                rankDictionary.Add(item, ++rank);

            foreach (var item in sequence)
                yield return rankDictionary[item];
        }
    }
}