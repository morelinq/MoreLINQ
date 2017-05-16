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

    partial class MoreEnumerable {

        /// <summary>
        /// Returns the sequence of elements in the first sequence which are also in the second sequence.
        /// </summary>
        /// <remarks>
        /// This is not a set operation like <see cref="System.Linq.Enumerable.Intersect{TSource}(
        /// System.Collections.Generic.IEnumerable{TSource}, System.Collections.Generic.IEnumerable{TSource})"/>; 
        /// if multiple elements in <paramref name="first"/> are equal, all such elements are returned.
        /// This operator uses deferred execution and streams results from <paramref name="first"/>,
        /// but the entire set of elements from <paramref name="second"/> is cached as soon as execution begins.
        /// Duplicate values in <paramref name="second"/> are not relevant and are discarded when <paramref name="second"/> is cached.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">The sequence of potentially included elements.</param>
        /// <param name="second">The set of elements which may allow elements in <paramref name="first"/> to be returned.</param>
        /// <param name="comparer">The element comparer. If <c>null</c>, uses the default equality comparer for <typeparamref name="TSource"/>.</param>
        /// <returns>The sequence of elements from <paramref name="first"/> that are also in <paramref name="second"/>, 
        /// including duplicate values from <paramref name="first"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="first"/> or <paramref name="second"/> is <c>null</c>. 
        /// If <paramref name="comparer"/> is <c>null</c>, the default equality comparer for <typeparamref name="TSource"/> is used.</exception>
        public static IEnumerable<TSource> IntersectAll<TSource>(this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer) {

            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");

            return SetFilterImpl(first, second, x => x, comparer, (hs, k) => hs.Contains(k));
        }

    }
}
