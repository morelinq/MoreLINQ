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
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Simple equality comparer that can be constructed with delegates.
    /// </summary>
    /// <typeparam name="T">Type to compare.</typeparam>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{T}" />
    /// <seealso cref="System.Collections.Generic.EqualityComparer{T}" />
    public class DelegateEqualityComparer<T> : IEqualityComparer<T> {

        private Func<T, T, bool> equals;
        private Func<T, int> getHashCode;

        #region Construction
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateEqualityComparer{T}"/> class
        /// that uses the given equality comparer and hash code generator.
        /// </summary>
        /// <param name="equals">The equality comparer.</param>
        /// <param name="getHashCode">The hash code generator.</param>
        public DelegateEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode) {
            if (equals == null) throw new ArgumentNullException("equals");
            if (getHashCode == null) throw new ArgumentNullException("getHashCode");
            InitializeDelegates(equals, getHashCode);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateEqualityComparer{T}"/> class
        /// that uses the given equality comparer and default hash code generator for T.
        /// </summary>
        /// <param name="equals">The equality comparer.</param>
        public DelegateEqualityComparer(Func<T, T, bool> equals) {
            if (equals == null) throw new ArgumentNullException("equals");
            InitializeDelegates(equals, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateEqualityComparer{T}"/> class
        /// that uses the given hash code generator and default equality comparer for T.
        /// </summary>
        /// <param name="getHashCode">The hash code generator.</param>
        public DelegateEqualityComparer(Func<T, int> getHashCode) {
            if (getHashCode == null) throw new ArgumentNullException("getHashCode");
            InitializeDelegates(null, getHashCode);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateEqualityComparer{T}"/> class
        /// that uses the default equality comparer and hash code generator for T.
        /// </summary>
        /// <remarks>Behaves like instance from EqualityComparer{T}.Default property.</remarks>
        public DelegateEqualityComparer() {
            InitializeDelegates(null, null);
        }

        private void InitializeDelegates(Func<T, T, bool> equals, Func<T, int> getHashCode) {
            var eq = EqualityComparer<T>.Default;

            if (equals == null) equals = eq.Equals;
            if (getHashCode == null) getHashCode = eq.GetHashCode;

            this.equals = equals;
            this.getHashCode = getHashCode;
        }
        #endregion

        /// <summary>
        /// Determines if two instances are equal.
        /// </summary>
        /// <param name="first">The first instance.</param>
        /// <param name="second">The second instance.</param>
        /// <returns><c>true</c>, if both instances are equal.</returns>
        public bool Equals(T first, T second) {
            return equals(first, second);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(T obj) {
            return getHashCode(obj);
        }
    }
}
