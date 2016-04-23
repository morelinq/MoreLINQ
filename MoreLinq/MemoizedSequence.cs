#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
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

namespace MoreLinq {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    /// <summary>
    /// Sequence that lazily fetches elements from a source sequence, 
    /// and internally caches the results for repeated access.
    /// </summary>
    /// <typeparam name="T">Type of sequence.</typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerable{T}" />
    public class MemoizedSequence<T> : IEnumerable<T> {

        private IEnumerable<T> source;
        private List<T> cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoizedSequence{T}"/> class, 
        /// using the given source sequence.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        public MemoizedSequence(IEnumerable<T> source) {
            if (source == null) throw new ArgumentNullException("source");
            this.source = source;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        /// <remarks>The first iteration will lazily fetch elements from the source collection and cache the results. 
        /// Further iterations will fetch elements from the internal cache.</remarks>
        public IEnumerator<T> GetEnumerator() {
            //Cache will only be null on the first iteration
            //If there's no cache, create one; otherwise, iterate cache
            return (cache == null)
                ? CacheSource()
                : cache.GetEnumerator();
        }
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        /// <remarks>The first iteration will lazily fetch elements from the source collection and cache the results. 
        /// Further iterations will fetch elements from the internal cache.</remarks>
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        private IEnumerator<T> CacheSource() {
            //Create cache
            this.cache = new List<T>();

            //Fill and yield cache
            foreach (var element in source) {
                this.cache.Add(element);
                yield return element;
            }

            //Dispose of source
            this.source = null;
        }
    }
}
