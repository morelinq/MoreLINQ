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
        /// iterator  but the same iterator should not be used simultanesouly
        /// from multiple threads. The sequence supplied in <paramref
        /// name="source"/> is not expected to be thread-safe but it is required
        /// to be thread-agnostic because different threads (though never
        /// simultaneously) may iterate over the sequence.
        /// </remarks>

        public static IEnumerable<T> Memoize<T>(this IEnumerable<T> source)
        {
            switch (source)
            {
                case null: throw new ArgumentNullException(nameof(source));
                case ICollection<T>        : // ...
                case IReadOnlyCollection<T>: // ...
                case MemoizedEnumerable<T> : return source;
                default: return new MemoizedEnumerable<T>(source);
            }
        }
    }

    sealed class MemoizedEnumerable<T> : IEnumerable<T>, IDisposable
    {
        readonly List<T> _cache = new();
        readonly object _locker;

        readonly Lazy<IEnumerator<T>> _sourceEnumerator;
        bool _running = true;
        bool _isDisposed = false;

        int? _errorIndex;
        ExceptionDispatchInfo? _error;

        public MemoizedEnumerable(IEnumerable<T> sequence)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            _sourceEnumerator = new Lazy<IEnumerator<T>>(
                sequence.GetEnumerator,
                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
            _locker = new object();
        }

        public IEnumerator<T> GetEnumerator()
        {
            var index = 0;

            while (true)
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(GetType().Name);

                T current;
                lock (_locker)
                {
                    if (index >= _cache.Count)
                    {
                        if (index == _errorIndex)
                            Assume.NotNull(_error).Throw();

                        bool moved;
                        try
                        {
                            moved = _running && _sourceEnumerator.Value.MoveNext();
                        }
                        catch (Exception ex)
                        {
                            _error = ExceptionDispatchInfo.Capture(ex);
                            _errorIndex = index;
                            _sourceEnumerator.Value.Dispose();
                            throw;
                        }

                        if (!moved)
                        {
                            _running = false;
                            _sourceEnumerator.Value.Dispose();
                            yield break;
                        }

                        _cache.Add(_sourceEnumerator.Value.Current);
                    }

                    current = _cache[index];
                }

                yield return current;
                index++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Dispose()
        {
            lock (_locker)
            {
                _error = null;
                _errorIndex = null;
                _sourceEnumerator.Value.Dispose();

                _isDisposed = true;
            }
        }
    }
}
