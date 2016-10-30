#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Leandro F. Vieira (leandromoh). All rights reserved.
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
        /// Returns a <see cref="MemoizedEnumerable{T}"/> that lazily creates an in-memory
        /// cache of the enumeration on first iteration, if it is not already an
        /// in-memory source.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <returns>A <see cref="MemoizedEnumerable{T}"/>.</returns>
        public static IBufferedEnumerable<T> Memoize<T>(this IEnumerable<T> source)
        {
            return source.Memoize(false);
        }

        /// <summary>
        /// Returns a <see cref="MemoizedEnumerable{T}"/> that lazily creates an in-memory
        /// cache of the enumeration on first iteration, if it is not already an
        /// in-memory source.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="forceBuffering">Force buffering, even if source is an <see cref="ICollection{T}" />
        /// (otherwise source is assumed to already be in-memory and enumerated quickly, and hence that
        /// buffering again would be a waste).</param>
        /// <returns>A <see cref="MemoizedEnumerable{T}"/>.</returns>
        public static IBufferedEnumerable<T> Memoize<T>(this IEnumerable<T> source, bool forceBuffering)
        {
            if (source == null) throw new ArgumentNullException("source");

            return (source as MemoizedEnumerable<T>) ?? new MemoizedEnumerable<T>(source, forceBuffering);
        }
    }

    /// <summary>
    /// An enumeration that, when being itereted, lazily iterates through the source enumeration 
    /// and cache the elements already iterated for future iterations. It supports partial iterations.
    /// </summary>
    /// <typeparam name="T">Type of the source sequence</typeparam>
    public interface IBufferedEnumerable<T> : IEnumerable<T>
    {

    }

    internal class MemoizedEnumerable<T> : IBufferedEnumerable<T>, IEnumerable<T>
    {
        private ICollection<T> collection;
        private IEnumerable<T> source;
        private IEnumerator<T> e;
        private IList<T> cache;
        private bool disposed;

        public MemoizedEnumerable(IEnumerable<T> sequence, bool forceBuffering)
        {
            if (!forceBuffering && sequence is ICollection<T>)
            {
                collection = (ICollection<T>)sequence;
            }
            else
            {
                source = sequence;
                cache = new List<T>();
            }
        }

        private IEnumerator<T> GetMemoizedEnumerator()
        {
            if (e == null) e = source.GetEnumerator();

            int index = 0;

            while (true)
            {
                while (index < cache.Count)
                {
                    yield return cache[index++];
                }

                if (!disposed && e.MoveNext())
                {
                    cache.Add(e.Current);
                    index++;
                    yield return e.Current;
                }
                else
                {
                    if (!disposed)
                    {
                        disposed = true;
                        e.Dispose();
                    }

                    yield break;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return collection != null ? collection.GetEnumerator() : GetMemoizedEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
