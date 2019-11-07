#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Pierre Lando. All rights reserved.
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

using System.Linq;

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Each buckets contains all <paramref name="acceptedKeys"/> keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals
        /// relatively to the <paramref name="keyComparer"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the returned buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="acceptedKeys">Sequence of accepted keys.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <param name="keyComparer">The comparer used to evaluate keys equality.</param>
        /// <returns>The build up sequence of buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// The <paramref name="acceptedKeys"/> sequence is fully consumed on first iteration.
        /// If <paramref name="acceptedKeys"/> is empty, <paramref name="source"/> is not enumerated.
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// If <paramref name="keyComparer"/> is null, <c>EqualityComparer.Default</c> is used.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="acceptedKeys"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentNullException"><paramref name="acceptedKeys"/> contains <c>null</c></exception>
        /// <exception cref="ArgumentException"><paramref name="acceptedKeys"/> contains duplicate keys relatively to
        /// <paramref name="keyComparer"/>.</exception>
        public static IEnumerable<IDictionary<TKey, TSource>> BatchBy<TKey, TSource>(this IEnumerable<TSource> source,
            IEnumerable<TKey> acceptedKeys,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> keyComparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (acceptedKeys == null) throw new ArgumentNullException(nameof(acceptedKeys));
            keyComparer ??= EqualityComparer<TKey>.Default;

            return _(); IEnumerable<IDictionary<TKey, TSource>> _()
            {
                var queues = acceptedKeys.ToDictionary(k => k, k => new Queue<TSource>(), keyComparer);

                // early break
                if (queues.Count == 0)
                    yield break;

                var emptyQueueCount = queues.Count;
                foreach (var value in source)
                {
                    var key = keySelector(value);

                    if (key != null && queues.TryGetValue(key, out var queue))
                    {
                        queue.Enqueue(value);
                        if (queue.Count == 1)
                            emptyQueueCount--;
                    }

                    // We need more elements
                    if (emptyQueueCount > 0)
                        continue;

                    yield return queues.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Dequeue());
                    emptyQueueCount = queues.Values.Count(q => q.Count == 0);
                }
            }
        }

        /// <summary>
        /// Batch the <paramref name="source"/> sequence into buckets that are <c>IDictionary</c>.
        /// Each buckets contains all <paramref name="acceptedKeys"/> keys and for each of this
        /// keys a matching value from the <paramref name="source"/> sequence.
        /// The matching is done by the <paramref name="keySelector"/>.
        ///
        /// Values from <paramref name="source"/> sequence that doesn't have a matching key are discarded.
        ///
        /// For each key/value pair in a buckets, <c>key</c> and <c>keySelector(value)</c> are equals
        /// relatively to the default equality comparer.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys of the returned buckets.</typeparam>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="acceptedKeys">Sequence of accepted keys.</param>
        /// <param name="keySelector">Build the key for elements from the <paramref name="source"/> sequence.</param>
        /// <returns>The build up sequence of buckets.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// The <paramref name="acceptedKeys"/> sequence is fully consumed on first iteration.
        /// If <paramref name="acceptedKeys"/> is empty, <paramref name="source"/> is not enumerated.
        /// Values from <paramref name="source"/> that correspond to a <c>null</c> key are discarded.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="acceptedKeys"/> or
        /// <paramref name="keySelector"/> is <c>null</c></exception>
        /// <exception cref="ArgumentNullException"><paramref name="acceptedKeys"/> contains <c>null</c></exception>
        /// <exception cref="ArgumentException"><paramref name="acceptedKeys"/> contains duplicate keys.</exception>
        public static IEnumerable<IDictionary<TKey, TSource>> BatchBy<TKey, TSource>(this IEnumerable<TSource> source,
            IEnumerable<TKey> acceptedKeys,
            Func<TSource, TKey> keySelector)
        {
            return BatchBy(source, acceptedKeys, keySelector, EqualityComparer<TKey>.Default);
        }
    }
}
