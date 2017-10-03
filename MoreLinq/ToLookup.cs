#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Atif Aziz. All rights reserved.
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
        /// Creates a <see cref="ILookup{TKey,TValue}" /> from a sequence of
        /// <see cref="KeyValuePair{TKey,TValue}" /> elements.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source sequence of key-value pairs.</param>
        /// <returns>
        /// A <see cref="ILookup{TKey,TValue}"/> containing the values
        /// mapped to their keys.
        /// </returns>

        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source) =>
            source.ToLookup(null);

        /// <summary>
        /// Creates a <see cref="ILookup{TKey,TValue}" /> from a sequence of
        /// <see cref="KeyValuePair{TKey,TValue}" /> elements. An additional
        /// parameter specifies a comparer for keys.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source sequence of key-value pairs.</param>
        /// <param name="comparer">The comparer for keys.</param>
        /// <returns>
        /// A <see cref="ILookup{TKey,TValue}"/> containing the values
        /// mapped to their keys.
        /// </returns>

        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source,
            IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.ToLookup(e => e.Key, e => e.Value, comparer);
        }

        #if !NO_VALUE_TUPLES

        /// <summary>
        /// Creates a <see cref="Lookup{TKey,TValue}" /> from a sequence of
        /// tuples of 2 where the first item is the key and the second the
        /// value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source sequence of tuples of 2.</param>
        /// <returns>
        /// A <see cref="Lookup{TKey, TValue}"/> containing the values
        /// mapped to their keys.
        /// </returns>

        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IEnumerable<(TKey Key, TValue Value)> source) =>
            source.ToLookup(null);

        /// <summary>
        /// Creates a <see cref="Lookup{TKey,TValue}" /> from a sequence of
        /// tuples of 2 where the first item is the key and the second the
        /// value. An additional parameter specifies a comparer for keys.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source sequence of tuples of 2.</param>
        /// <param name="comparer">The comparer for keys.</param>
        /// <returns>
        /// A <see cref="Lookup{TKey, TValue}"/> containing the values
        /// mapped to their keys.
        /// </returns>

        public static ILookup<TKey, TValue> ToLookup<TKey, TValue>(this IEnumerable<(TKey Key, TValue Value)> source,
            IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.ToLookup(e => e.Key, e => e.Value, comparer);
        }

        #endif
    }
}
