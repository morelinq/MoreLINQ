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

namespace MoreLinq
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a <see cref="IEnumerable{T}"/> that lazily creates an in-memory
        /// cache of the enumeration on first iteration, if it is not already an
        /// in-memory source.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <returns>Returns a sequence that corresponds to a cached version of the input sequence.</returns>
        /// <remarks>
        /// The returned <see cref="IEnumerable{T}"/> will cache items from
        /// <paramref name="source"/> in a thread-safe manner such that it can
        /// be shared between threads. Each thread can call its
        /// <see cref="IEnumerable{T}.GetEnumerator"/> to acquire an iterator
        /// but the same iterator should not be used simultanesouly from
        /// multiple threads. The sequence supplied in <paramref name="source"/>
        /// is not expected to be thread-safe but it is required to be
        /// thread-agnostic because different threads (though never
        /// simultaneously) may iterate over the sequence.
        /// </remarks>

        public static IEnumerable<T> Memoize<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source is ICollection<T>)
            {
                return source;
            }

            return (source as MemoizedEnumerable<T>) ?? new MemoizedEnumerable<T>(source);
        }
    }

    internal class MemoizedEnumerable<T> : IEnumerable<T>, IDisposable
    {
        private IList<T> cache;
        private readonly object locker;
        private readonly IEnumerable<T> source;
        private IEnumerator<T> sourceEnumerator;
        private int? errorIndex;
        private Exception error;

        public MemoizedEnumerable(IEnumerable<T> sequence)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));

            source = sequence;
            locker = new object();
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (cache == null)
            {
                lock (locker)
                {
                    if (cache == null)
                    {
                        if (error != null)
                            throw error;

                        try
                        {
                            var cache = new List<T>(); // for exception safety, allocate then...
                            sourceEnumerator = source.GetEnumerator(); // (because this can fail)
                            this.cache = cache; // ...commit to state
                        }
                        catch (Exception ex)
                        {
                            // TODO preserve stack trace for throw later
                            // This requires ExceptionDispatchInfo that is
                            // available from .NET Framework 4.5 and onward.

                            error = ex;
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
                    lock (locker)
                    {
                        if (cache == null) // Cache disposed during iteration?
                            throw new ObjectDisposedException(nameof(MemoizedEnumerable<T>));

                        if (index >= cache.Count)
                        {
                            if (index == errorIndex)
                                throw error;

                            if (sourceEnumerator == null)
                                break;

                            bool moved;
                            try
                            {
                                moved = sourceEnumerator.MoveNext();
                            }
                            catch (Exception ex)
                            {
                                // TODO preserve stack trace for throw later
                                // This requires ExceptionDispatchInfo that is
                                // available from .NET Framework 4.5 and onward.

                                this.error = ex;
                                errorIndex = index;
                                sourceEnumerator.Dispose();
                                sourceEnumerator = null;
                                throw;
                            }

                            if (moved)
                            {
                                cache.Add(sourceEnumerator.Current);
                            }
                            else
                            {
                                sourceEnumerator.Dispose();
                                sourceEnumerator = null;
                                break;
                            }
                        }

                        current = cache[index];
                    }

                    yield return current;
                    index++;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            lock (locker)
            {
                error = null;
                cache = null;
                errorIndex = null;
                sourceEnumerator?.Dispose();
                sourceEnumerator = null;
            }
        }
    }
}
