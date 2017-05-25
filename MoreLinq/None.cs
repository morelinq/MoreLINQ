#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2009 Atif Aziz. All rights reserved.
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
    using LinqEnumerable = System.Linq.Enumerable;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Determins whether a sequence contains no elements.
        /// </summary>
        /// <typeparam name="TSource">Type of elements.</typeparam>
        /// <param name="source">The sequence to check.</param>
        /// <returns>true if the source sequence contains no elements; otherwise, false.</returns>
        public static bool None<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return !LinqEnumerable.Any(source);
        }

        /// <summary>
        /// Determines whether no element of a sequence satisfies a condition.
        /// </summary>
        /// <typeparam name="TSource">Type of elements.</typeparam>
        /// <param name="source">The sequence to check.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>true if no elements in the source sequence pass the test in the specified predicate; otherwise, false.</returns>
        public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return !LinqEnumerable.Any(source, predicate);
        }
    }
}