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
        /// <returns>The source <see cref="IEnumerable{T}"/> cached.</returns>
        public static IEnumerable<T> Memoize<T>(this IEnumerable<T> source)
        {
            return source.Memoize(false, false);
        }

        /// <summary>
        /// Returns a <see cref="IEnumerable{T}"/> that lazily creates an in-memory
        /// cache of the enumeration on first iteration, if it is not already an
        /// in-memory source.
        /// An additional argument specifies if buffering must happen even if the source implements
        /// <see cref="ICollection{T}"/> (otherwise it is assumed that the source already serves as 
        /// a buffer adequately).
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="forceBuffering">Force buffering, even if source is an <see cref="ICollection{T}" />
        /// (otherwise source is assumed to already be in-memory and enumerated quickly, and hence that
        /// buffering again would be a waste).</param>
        /// <param name="disposeOnEarlyExit">Indicates if the call to dispose method of source's enumerator, 
        /// and therefore the close of the buffering, must happen at the end of the first iteration (true) 
        /// or only when source is entirely iterated (false).</param>
        /// <returns>The source <see cref="IEnumerable{T}"/> cached.</returns>
        public static IEnumerable<T> Memoize<T>(this IEnumerable<T> source, bool forceBuffering, bool disposeOnEarlyExit)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (!forceBuffering && source is ICollection<T>)
            {
                return source;
            }

            return (source as MemoizedEnumerable<T>) ?? new MemoizedEnumerable<T>(source, forceBuffering, disposeOnEarlyExit);
        }
    }

    internal class MemoizedEnumerable<T> : IEnumerable<T>
    {
        private readonly IList<T> cache;
        private readonly bool disposeOnEarlyExit;
        private IEnumerable<T> source;
        private IEnumerator<T> sourceEnumerator;
        private bool disposed;

        public MemoizedEnumerable(IEnumerable<T> sequence, bool forceBuffering, bool shouldDisposeOnEarlyExit)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));

            source = sequence;
            cache = new List<T>();
            disposeOnEarlyExit = shouldDisposeOnEarlyExit;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (sourceEnumerator == null && !disposed)
                sourceEnumerator = source.GetEnumerator();

            int index = 0;
            bool hasValue = false;

            try
            {
                while (true)
                {
                    if (index < cache.Count)
                    {
                        hasValue = true;
                    }

                    else if ((hasValue = !disposed && sourceEnumerator.MoveNext()))
                    {
                        cache.Add(sourceEnumerator.Current);
                    }

                    else if (!disposed)
                    {
                        DisposeSourceResources();
                    }

                    if (hasValue)
                        yield return cache[index];
                    else
                        break;

                    index++;
                }
            }
            finally
            {
                if (!disposed && disposeOnEarlyExit)
                {
                    DisposeSourceResources();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void DisposeSourceResources()
        {
            disposed = true;
            sourceEnumerator.Dispose();
            source = null;
            sourceEnumerator = null;
        }
    }
}
