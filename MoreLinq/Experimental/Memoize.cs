#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Leandro F. Vieira (leandromoh). All rights reserved.
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

namespace MoreLinq.Experimental
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.ExceptionServices;

    static partial class ExperimentalEnumerable
    {
        /// <summary>
        /// Creates a sequence that lazily caches the source as it is iterated
        /// for the first time, reusing the cache thereafter for future
        /// re-iterations. If the source is already cached or buffered then it
        /// is returned verbatim.
        /// </summary>
        /// <typeparam name="T">
        /// Type of elements in <paramref name="source"/>.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>
        /// Returns a sequence that corresponds to a cached version of the input
        /// sequence.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// The returned <see cref="IEnumerable{T}"/> will cache items from
        /// <paramref name="source"/> in a thread-safe manner. Each thread can
        /// call its <see cref="IEnumerable{T}.GetEnumerator"/> to acquire an
        /// iterator  but the same iterator should not be used simultaneously
        /// from multiple threads. The sequence supplied in <paramref
        /// name="source"/> is not expected to be thread-safe but it is required
        /// to be thread-agnostic because different threads (though never
        /// simultaneously) may iterate over the sequence.
        /// </remarks>

        public static IEnumerable<T> Memoize<T>(this IEnumerable<T> source) =>
            source switch
            {
                null => throw new ArgumentNullException(nameof(source)),
                ICollection<T> => source,
                IReadOnlyCollection<T> => source,
                MemoizedEnumerable<T> => source,
                _ => new MemoizedEnumerable<T>(source),
            };
    }

    sealed class MemoizedEnumerable<T>(IEnumerable<T> sequence) : IEnumerable<T>, IDisposable
    {
        List<T>? cache;
        readonly object locker = new();
        readonly IEnumerable<T> source = sequence ?? throw new ArgumentNullException(nameof(sequence));
        IEnumerator<T>? sourceEnumerator;
        int? errorIndex;
        ExceptionDispatchInfo? error;

        public IEnumerator<T> GetEnumerator()
        {
            if (this.cache == null)
            {
                lock (this.locker)
                {
                    if (this.cache == null)
                    {
                        this.error?.Throw();

                        try
                        {
                            var cache = new List<T>(); // for exception safety, allocate then...
                            this.sourceEnumerator = this.source.GetEnumerator(); // (because this can fail)
                            this.cache = cache; // ...commit to state
                        }
                        catch (Exception ex)
                        {
                            this.error = ExceptionDispatchInfo.Capture(ex);
                            throw;
                        }
                    }
                }
            }

            return _(); IEnumerator<T> _()
            {
                var index = 0;

                while (true)
                {
                    T current;
                    lock (this.locker)
                    {
                        if (this.cache == null) // Cache disposed during iteration?
                            throw new ObjectDisposedException(nameof(MemoizedEnumerable<T>));

                        if (index >= this.cache.Count)
                        {
                            if (index == this.errorIndex)
                                Assume.NotNull(this.error).Throw();

                            if (this.sourceEnumerator == null)
                                break;

                            bool moved;
                            try
                            {
                                moved = this.sourceEnumerator.MoveNext();
                            }
                            catch (Exception ex)
                            {
                                this.error = ExceptionDispatchInfo.Capture(ex);
                                this.errorIndex = index;
                                this.sourceEnumerator.Dispose();
                                this.sourceEnumerator = null;
                                throw;
                            }

                            if (moved)
                            {
                                this.cache.Add(this.sourceEnumerator.Current);
                            }
                            else
                            {
                                this.sourceEnumerator.Dispose();
                                this.sourceEnumerator = null;
                                break;
                            }
                        }

                        current = this.cache[index];
                    }

                    yield return current;
                    index++;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Dispose()
        {
            lock (this.locker)
            {
                this.error = null;
                this.cache = null;
                this.errorIndex = null;
                this.sourceEnumerator?.Dispose();
                this.sourceEnumerator = null;
            }
        }
    }
}
