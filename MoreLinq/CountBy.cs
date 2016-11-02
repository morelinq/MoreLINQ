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
    using System.Linq;
    using System.Runtime.CompilerServices;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Applies a key-generating function to each element of a sequence and returns a sequence of 
        /// unique keys and their number of occurrences in the original sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the projected element.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Function that transforms each item of source sequence into a key to be compared against the others.</param>
        /// <returns>A sequence of unique keys and their number of occurrences in the original sequence.</returns>
        public static IEnumerable<KeyValuePair<TKey, int>> CountBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.CountBy(keySelector, null);
        }

        /// <summary>
        /// Applies a key-generating function to each element of a sequence and returns a sequence of 
        /// unique keys and their number of occurrences in the original sequence.
        /// An additional argument specifies a comparer to use for testing equivalence of keys.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the projected element.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Function that transforms each item of source sequence into a key to be compared against the others.</param>
        /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <c>TSource</c> is used.</param>
        /// <returns>A sequence of unique keys and their number of occurrences in the original sequence.</returns>
        public static IEnumerable<KeyValuePair<TKey, int>> CountBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return CountByImpl(source, keySelector, comparer);
        }

        private static IEnumerable<KeyValuePair<TKey, int>> CountByImpl<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            var dic = new Dictionary<TKey, int>(comparer);
            var keys = new List<TKey>();
            var counts = new List<int>();
            var havePrevKey = false;
            var prevKey = default(TKey);
            var index = 0;

            foreach (var item in source)
            {
                var key = keySelector(item);

                if (// key same as the previous? then re-use the index
                    (havePrevKey && dic.Comparer.GetHashCode(prevKey) == dic.Comparer.GetHashCode(key)
                                  && dic.Comparer.Equals(prevKey, key))
                    // otherwise try & find index of the key
                    || dic.TryGetValue(key, out index))
                {
                    counts[index]++;
                }
                else
                {
                    dic[key] = keys.Count;
                    keys.Add(key);
                    counts.Add(1);
                }

                prevKey = key;
                havePrevKey = true;
            }

            // The dictionary is no longer needed from this point forward so
            // lose the reference and make it available as food for the GC.
            // This optimization is designed to help cases where a slow running
            // loop over the yielded pairs could span GC cycles. However,
            // instead of doing simply the following:
            //
            // dic = null;
            //
            // the reference is nulled through a method that the JIT compiler
            // is told not to try and inline; done so assuming that the above
            // method could have been turned into a NOP (in theory).

            Null(ref dic); // dic = null;

            for (var i = 0; i < keys.Count; i++)
                yield return new KeyValuePair<TKey, int>(keys[i], counts[i]);
        }

        // ReSharper disable once RedundantAssignment
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void Null<T>(ref T obj) where T : class { obj = null; }
    }
}
