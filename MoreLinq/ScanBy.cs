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
        /// Scans the source sequence and performs accumulations of state for each group of keys.
        /// Returns a sequence of states per key, that is, the key of each element of the
        /// source sequence, and the current state for that key.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the projected key.</typeparam>
        /// <typeparam name="TState">Type of state elements.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Function that transforms each item of source sequence into a key.</param>
        /// <param name="seedSelector">Function that defines the initial state for that key.</param>
        /// <param name="accumulator">Function that defines the current state for that key.</param>
        /// <returns>
        /// Returns a sequence of states per key, that is, the key of each element of the
        /// source sequence, and the current state for that key.
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
        /// Scans the source sequence and performs accumulations of state for each group of keys.
        /// Returns a sequence of states per key, that is, the key of each element of the
        /// source sequence, and the current state for that key.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TKey">Type of the projected key.</typeparam>
        /// <typeparam name="TState">Type of state elements.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="keySelector">Function that transforms each item of source sequence into a key.</param>
        /// <param name="seedSelector">Function that defines the initial state for that key.</param>
        /// <param name="accumulator">Function that defines the current state for that key.</param>
        /// <param name="comparer">The equality comparer to use to determine whether or not keys are equal.
        /// If null, the default equality comparer for <typeparamref name="TSource"/> is used.</param>
        /// <returns>
        /// Returns a sequence of states per key, that is, the key of each element of the
        /// source sequence, and the current state for that key.
        /// </returns>

        public static IEnumerable<KeyValuePair<TKey, TState>> ScanBy<TSource, TKey, TState>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TKey, TState> seedSelector,
            Func<TState, TKey, TSource, TState> accumulator,
            IEqualityComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (seedSelector == null) throw new ArgumentNullException(nameof(seedSelector));
            if (accumulator == null) throw new ArgumentNullException(nameof(accumulator));

            return _(); IEnumerable<KeyValuePair<TKey, TState>> _()
            {
                comparer = comparer ?? EqualityComparer<TKey>.Default;

                var stateByKey = new Dictionary<TKey, TState>(comparer);
                var prevKey = (HasValue: false, Value: default(TKey));
                var nullKeyState = (HasValue: false, Value: default(TState));
                var state = default(TState);

                bool TryGetState(TKey key, out TState value)
                {
                    if (key == null)
                    {
                        value = nullKeyState.Value;
                        return nullKeyState.HasValue;
                    }

                    return stateByKey.TryGetValue(key, out value);
                }

                foreach (var item in source)
                {
                    var key = keySelector(item);

                    if (!(prevKey.HasValue
                       // key same as the previous? then re-use the state
                       && comparer.GetHashCode(prevKey.Value) == comparer.GetHashCode(key)
                       && comparer.Equals(prevKey.Value, key)
                       // otherwise try & find state of the key
                       || TryGetState(key, out state)))
                    {
                        state = seedSelector(key);
                    }

                    state = accumulator(state, key, item);

                    if (key != null)
                        stateByKey[key] = state;
                    else
                        nullKeyState = (true, state);

                    yield return new KeyValuePair<TKey, TState>(key, state);

                    prevKey = (true, key);
                }
            }
        }
    }
}
