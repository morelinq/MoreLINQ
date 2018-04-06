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
    using System.Runtime.ExceptionServices;

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

    sealed class MemoizedEnumerable<T> : IEnumerable<T>, IDisposable
    {
        IList<T> _cache;
        readonly object _locker;
        readonly IEnumerable<T> _source;
        IEnumerator<T> _sourceEnumerator;
        int? _errorIndex;
        ExceptionDispatchInfo _error;

        public MemoizedEnumerable(IEnumerable<T> sequence)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));

            _source = sequence;
            _locker = new object();
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_cache == null)
            {
                lock (_locker)
                {
                    if (_cache == null)
                    {
                        _error?.Throw();

                        try
                        {
                            var cache = new List<T>(); // for exception safety, allocate then...
                            _sourceEnumerator = _source.GetEnumerator(); // (because this can fail)
                            _cache = cache; // ...commit to state
                        }
                        catch (Exception ex)
                        {
                            _error = ExceptionDispatchInfo.Capture(ex);
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
                    lock (_locker)
                    {
                        if (_cache == null) // Cache disposed during iteration?
                            throw new ObjectDisposedException(nameof(MemoizedEnumerable<T>));

                        if (index >= _cache.Count)
                        {
                            if (index == _errorIndex)
                                _error.Throw();

                            if (_sourceEnumerator == null)
                                break;

                            bool moved;
                            try
                            {
                                moved = _sourceEnumerator.MoveNext();
                            }
                            catch (Exception ex)
                            {
                                _error = ExceptionDispatchInfo.Capture(ex);
                                _errorIndex = index;
                                _sourceEnumerator.Dispose();
                                _sourceEnumerator = null;
                                throw;
                            }

                            if (moved)
                            {
                                _cache.Add(_sourceEnumerator.Current);
                            }
                            else
                            {
                                _sourceEnumerator.Dispose();
                                _sourceEnumerator = null;
                                break;
                            }
                        }

                        current = _cache[index];
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
            lock (_locker)
            {
                _error = null;
                _cache = null;
                _errorIndex = null;
                _sourceEnumerator?.Dispose();
                _sourceEnumerator = null;
            }
        }
    }
}
