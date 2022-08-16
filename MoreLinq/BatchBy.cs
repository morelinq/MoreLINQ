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

            (IList<TKey> Keys, IDictionary<TKey, int> IndexByKey) BuildContext()
            {
                var keys = acceptedKeys.ToList();
                var indexByKey = new Dictionary<TKey, int>(keyComparer);
                var index = 0;
                foreach (var key in keys)
                {
                    indexByKey.Add(key, index);
                    index++;
                }

                return (keys, indexByKey);
            }
            var lazyContext = new Lazy<(IList<TKey> Keys, IDictionary<TKey, int> IndexByKey)>(BuildContext);

            return _(); IEnumerable<IDictionary<TKey, TSource>> _()
            {
                // Lazy creation of the index table and enumeration of acceptedKeys
                var (keys, indexByKey) = lazyContext.Value;
                foreach (var batch in BatchByImplementation(source, indexByKey, keySelector))
                {
                    var nextResult = new Dictionary<TKey, TSource>(keys.Count, keyComparer);
                    for (var i = 0; i < keys.Count; i++)
                    {
                        nextResult.Add(keys[i], batch[i]);
                    }

                    yield return nextResult;
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

        private static IEnumerable<IList<TSource>> BatchByImplementation<TKey, TSource>(this IEnumerable<TSource> source,
            IEnumerable<TKey> acceptedKeys,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> keyComparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
            if (acceptedKeys == null) throw new ArgumentNullException(nameof(acceptedKeys));
            keyComparer ??= EqualityComparer<TKey>.Default;

            IDictionary<TKey, int> BuildIndexByKey()
            {
                var indexByKey = new Dictionary<TKey, int>(keyComparer);
                var index = 0;
                foreach (var key in acceptedKeys)
                {
                    indexByKey.Add(key, index);
                    index++;
                }

                return indexByKey;
            }
            var lazyIndexByKey = new Lazy<IDictionary<TKey, int>>(BuildIndexByKey);

            return _(); IEnumerable<IList<TSource>> _()
            {
                // Lazy creation of the index table
                var indexByKey = lazyIndexByKey.Value;
                foreach (var batch in BatchByImplementation(source, indexByKey, keySelector))
                {
                    yield return batch;
                }
            }
        }

        private static IEnumerable<IList<TSource>> BatchByImplementation<TKey, TSource>(IEnumerable<TSource> source,
            IDictionary<TKey, int> indexByKey, Func<TSource, TKey> keySelector)
        {
            var batchSize = indexByKey.Count;

            // acceptedKeys was empty.
            if (batchSize == 0)
                yield break;

            var queues = new Queue<TSource>[batchSize];
            for (var i = 0; i < batchSize; i++)
            {
                queues[i] = new Queue<TSource>();
            }

            var batch = new TSource[batchSize];
            var takenSlots = new bool[batchSize];
            var emptySlotCount = batchSize;
            foreach (var value in source)
            {
                var key = keySelector(value);

                // reject null key
                if (key == null)
                    continue;

                // reject unknown keys
                if (!indexByKey.TryGetValue(key, out var index))
                    continue;

                // the slot is already taken, enqueue the value
                if (takenSlots[index])
                {
                    var targetQueue = queues[index];
                    targetQueue.Enqueue(value);
                    continue;
                }

                // fill the slot
                batch[index] = value;
                takenSlots[index] = true;
                emptySlotCount--;

                // there are empty slots left, can't yield yet
                if (emptySlotCount > 0)
                    continue;

                // finally can yield a batch
                yield return batch;

                // prepare next batch
                batch = new TSource[batchSize];
                for (var i = 0; i < batchSize; i++)
                {
                    var queue = queues[i];
                    if (queue.Count == 0)
                    {
                        takenSlots[i] = false;
                        emptySlotCount++;
                    }
                    else
                    {
                        batch[i] = queue.Dequeue();
                        takenSlots[i] = true;
                    }
                }

            }
        }
    }
}
