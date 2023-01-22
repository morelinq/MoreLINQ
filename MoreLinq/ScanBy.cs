#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Atif Aziz, Leandro F. Vieira (leandromoh). All rights reserved.
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
        /// Applies an accumulator function over sequence element keys,
        /// returning the keys along with intermediate accumulator states.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TState">Type of the state.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="keySelector">
        /// A function that returns the key given an element.</param>
        /// <param name="seedSelector">
        /// A function to determine the initial value for the accumulator that is
        /// invoked once per key encountered.</param>
        /// <param name="accumulator">
        /// An accumulator function invoked for each element.</param>
        /// <returns>
        /// A sequence of keys paired with intermediate accumulator states.
        /// </returns>

        public static IEnumerable<KeyValuePair<TKey, TState>> ScanBy<TSource, TKey, TState>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TKey, TState> seedSelector,
            Func<TState, TKey, TSource, TState> accumulator)
        {
            return source.ScanBy(keySelector, seedSelector, accumulator, null);
        }

        /// <summary>
        /// Applies an accumulator function over sequence element keys,
        /// returning the keys along with intermediate accumulator states. An
        /// additional parameter specifies the comparer to use to compare keys.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TState">Type of the state.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="keySelector">
        /// A function that returns the key given an element.</param>
        /// <param name="seedSelector">
        /// A function to determine the initial value for the accumulator that is
        /// invoked once per key encountered.</param>
        /// <param name="accumulator">
        /// An accumulator function invoked for each element.</param>
        /// <param name="comparer">The equality comparer to use to determine
        /// whether or not keys are equal. If <c>null</c>, the default equality
        /// comparer for <typeparamref name="TSource"/> is used.</param>
        /// <returns>
        /// A sequence of keys paired with intermediate accumulator states.
        /// </returns>

        public static IEnumerable<KeyValuePair<TKey, TState>> ScanBy<TSource, TKey, TState>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TKey, TState> seedSelector,
            Func<TState, TKey, TSource, TState> accumulator,
            IEqualityComparer<TKey>? comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (seedSelector == null) throw new ArgumentNullException(nameof(seedSelector));
            if (accumulator == null) throw new ArgumentNullException(nameof(accumulator));

            return _(comparer ?? EqualityComparer<TKey>.Default);

            IEnumerable<KeyValuePair<TKey, TState>> _(IEqualityComparer<TKey> comparer)
            {
                var stateByKey = new Collections.Dictionary<TKey, TState>(comparer);

                foreach (var item in source)
                {
                    var key = keySelector(item);
                    var state = stateByKey.TryGetValue(key, out var s) ? s : seedSelector(key);
                    state = accumulator(state, key, item);
                    stateByKey[key] = state;
                    yield return new KeyValuePair<TKey, TState>(key, state);
                }
            }
        }
    }
}
