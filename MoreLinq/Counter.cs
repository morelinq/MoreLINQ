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
    
    /// <summary>Dictionary of values and instance counts.</summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public class Counter<T> : IEnumerable<KeyValuePair<T, Int32>> {

        private Dictionary<T, Int32> inner;
        private Int32 nullCount = 0;

        #region Construction
        /// <summary>
        /// Initializes a new instance of the <see cref="Counter{T}"/> class.
        /// </summary>
        /// <param name="source">The source collection.</param>
        /// <param name="comparer">Item equality comparer.  If <c>null</c>, 
        /// uses default equality comparer for T.</param>
        public Counter(IEnumerable<T> source, IEqualityComparer<T> comparer) {
            if (source == null) throw new ArgumentNullException("source");

            inner = new Dictionary<T, Int32>(comparer);
            foreach (var element in source) {
                Increment(element);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Counter{T}"/> class,
        /// using the default equality comparer for T.
        /// </summary>
        /// <param name="source">The source.</param>
        public Counter(IEnumerable<T> source) : this(source, null){}

        #endregion

        /// <summary>
        /// Gets the count of the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>Count of item.</returns>
        public Int32 this[T item] {
            get {
                if (item == null)
                    return nullCount;
                Int32 n = 0;
                inner.TryGetValue(item, out n);
                return n;
            }
        }

        /// <summary>
        /// Increments the specified item's count.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Increment(T item) {
            if (item == null) {
                nullCount++;
            }
            else {
                Int32 count;
                if (inner.TryGetValue(item, out count)) {
                    inner[item] = count+1;
                }
                else {
                    inner.Add(item, 1);
                }
            }
        }

        /// <summary>
        /// Decrements the specified item's count.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>True, if item's previous count was above 0.</returns>
        public Boolean Decrement(T item) {
            if (item == null) {
                if(nullCount > 0) {
                    nullCount--;
                    return true;
                }
            }
            else {
                Int32 count;
                if (inner.TryGetValue(item, out count)) {
                    if (count == 0)
                        return false;
                    inner[item] = count-1;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<T, Int32>> GetEnumerator() {
            foreach (var pair in inner) {
                if (pair.Value > 0)
                    yield return pair;
            }
            if (nullCount > 0)
                yield return new KeyValuePair<T, Int32>(default(T), nullCount);
        }
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }
}
