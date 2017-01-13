#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Atif Aziz. All rights reserved.
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
    using System.Collections.Generic;
    using System.Linq;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns the positions (one-based index) at which the elements
        /// of a sequence equal a sought value.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="sought">The sought value.</param>
        /// <typeparam name="T">Type of elements in the source sequence.</typeparam>
        /// <returns>
        /// Return a sequence of positions where the condition matched.
        /// </returns>

        public static IEnumerable<int> Positions<T>(this IEnumerable<T> source, T sought)
        {
            return source.Positions(sought, null);
        }

        /// <summary>
        /// Returns the positions (one-based index) at which the elements
        /// of a sequence equal a sought value. An additional parameter
        /// specifies how to compare the a sequence element and the sought
        /// value for equality.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="sought">The sought value.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> to
        /// use to compare each element of the sequence for equality
        /// with <paramref name="sought"/>. If the argument specifies a null
        /// reference then the default equality comparer is used for
        /// <typeparamref name="T"/></param>
        /// <typeparam name="T">Type of elements in the source sequence.</typeparam>
        /// <returns>
        /// Return a sequence of positions where the condition matched.
        /// </returns>

        public static IEnumerable<int> Positions<T>(this IEnumerable<T> source, T sought, IEqualityComparer<T> comparer)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            return source.Positions(e => comparer.Equals(e, sought));
        }

        /// <summary>
        /// Returns the positions (one-based index) at which the elements
        /// of a sequence match a user-supplied condition.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="predicate">The condition function.</param>
        /// <typeparam name="T">Type of elements in the source sequence.</typeparam>
        /// <returns>
        /// Return a sequence of positions where the condition matched.
        /// </returns>

        public static IEnumerable<int> Positions<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");
            return from e in source.Index(1)
                   where predicate(e.Value)
                   select e.Key;
        }
    }
}
