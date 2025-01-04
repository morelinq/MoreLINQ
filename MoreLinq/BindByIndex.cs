#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Atif Aziz. All rights reserved.
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
    using System.Collections;
    using System.Collections.Generic;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// TODO Complete documentation
        /// </summary>
        /// <typeparam name="T">
        /// Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of result elements returned.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="indices">The sequence of indices.</param>
        /// <param name="missingSelector">
        /// TODO Complete documentation
        /// </param>
        /// <param name="resultSelector">
        /// TODO Complete documentation
        /// </param>
        /// <returns>
        /// TODO Complete documentation
        /// </returns>

        public static IEnumerable<TResult>
            BindByIndex<T, TResult>(this IEnumerable<T> source, IEnumerable<int> indices,
                Func<int, TResult> missingSelector, Func<T, int, TResult> resultSelector) =>
            BindByIndex(source, indices, null, missingSelector, resultSelector);

        /// <summary>
        /// TODO Complete documentation
        /// </summary>
        /// <typeparam name="T">
        /// Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <typeparam name="TResult">Type of result elements returned.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="indices">The sequence of indices.</param>
        /// <param name="lookBackSize">Size of look-back buffer.</param>
        /// <param name="missingSelector">
        /// TODO Complete documentation
        /// </param>
        /// <param name="resultSelector">
        /// TODO Complete documentation
        /// </param>
        /// <returns>
        /// TODO Complete documentation
        /// </returns>

        public static IEnumerable<TResult>
            BindByIndex<T, TResult>(this IEnumerable<T> source, IEnumerable<int> indices,
                                    int lookBackSize,
                                    Func<int, TResult> missingSelector,
                                    Func<T, int, TResult> resultSelector) =>
            BindByIndex(source, indices, (int?)lookBackSize, missingSelector, resultSelector);

        static IEnumerable<TResult>
            BindByIndex<T, TResult>(IEnumerable<T> source, IEnumerable<int> indices,
                                    int? lookBackSize,
                                    Func<int, TResult> missingSelector,
                                    Func<T, int, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (indices == null) throw new ArgumentNullException(nameof(indices));
            if (lookBackSize < 0) throw new ArgumentOutOfRangeException(nameof(lookBackSize));
            if (missingSelector == null) throw new ArgumentNullException(nameof(missingSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            // TODO A version optimized for lists

            return _(lookBackSize switch
            {
                { } lbs and > 0 => new Queue<T>(lbs, lbs),
                0 => null,
                _ => new Queue<T>()
            });

            IEnumerable<TResult> _(Queue<T>? queue)
            {
                using var rie = indices.GetEnumerator();
                if (!rie.MoveNext())
                    yield break;

                while (rie.Current < 0)
                {
                    yield return missingSelector(rie.Current);
                    if (!rie.MoveNext())
                        yield break;
                }

                var ri = rie.Current;
                var si = 0;

                foreach (var item in source)
                {
                    while (si == ri)
                    {
                        yield return resultSelector(item, si);
                        do
                        {
                            if (!rie.MoveNext())
                                yield break;
                            ri = rie.Current;
                            if (ri < si)
                            {
                                if (queue is { } q && si - q.Count is var qi && ri >= qi)
                                    yield return resultSelector(q[ri - qi], ri);
                                else
                                    yield return missingSelector(ri);
                            }
                        }
                        while (ri < si);
                    }

                    queue?.Enqueue(item);
                    si++;
                }

                if (ri != si)
                {
                    yield return missingSelector(ri);
                    while (rie.MoveNext())
                        yield return missingSelector(rie.Current);
                }
            }
        }
    }

    /// <summary>
    /// A queue implementation similar to
    /// <see cref="System.Collections.Generic.Queue{T}"/> but which
    /// supports a maximum count (exceeding which will cause an item to be
    /// dequeued each to make space for a new one being queued) as well as
    /// directly indexing into the queue to retrieve any one item.
    /// </summary>

    file sealed class Queue<T>(int maxCount = 0, int capacity = 0) : IReadOnlyList<T>
    {
        T[] items = capacity > 0 ? new T[capacity] : [];
        int firstIndex;
        readonly int maxCount = maxCount;

        int Capacity => this.items.Length;
        public int Count { get; private set; }

        T IReadOnlyList<T>.this[int index] => this[index];

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
#pragma warning disable CA2201 // Do not raise reserved exception types
                    throw new IndexOutOfRangeException();
#pragma warning restore CA2201
                }

                return Cell(index);
            }
        }

        ref T Cell(int index) => ref this.items[(this.firstIndex + index) % Capacity];

        public void Enqueue(T item)
        {
            if (this.maxCount > 0 && Count == this.maxCount)
                _ = Dequeue();

            if (Count == Capacity)
            {
                var array = new T[Math.Max(4, Capacity * 2)];
                for (var i = 0; i < Count; i++)
                    array[i] = this[i];
                this.firstIndex = 0;
                this.items = array;
            }

            Cell(Count++) = item;
        }

        public T Dequeue()
        {
            if (Count == 0)
                throw new InvalidOperationException();
            var result = this[0];
            this.firstIndex++;
            --Count;
            return result;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
                yield return this[i];
        }
    }
}
