#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2010 Leopold Bushkin. All rights reserved.
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
        /// If condition is true, it filters a sequence of values based on a predicate. If not, it returns source unmodified
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to filter</param>
        /// <param name="predicate">A function to test each element for a condition</param>
        /// <param name="condition">If true, source gets filtered based on condition</param>
        /// <returns>An System.Collections.Generic.IEnumerable`1 that contains elements from the input sequence that satisfy the predicate if condition is true, or unmodified source if condition is false</returns>
        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// If condition is true, it filters a sequence of values based on a predicate. If not, it returns source unmodified
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the source sequence</typeparam>
        /// <param name="source">An System.Collections.Generic.IEnumerable`1 to filter</param>
        /// <param name="predicate">A function to test each source element for a condition; the second parameter of the function represents the index of the source element</param>
        /// <param name="condition">If true, source gets filtered based on condition</param>
        /// <returns>An System.Collections.Generic.IEnumerable`1 that contains elements from the input sequence that satisfy the predicate if condition is true, or unmodified source if condition is false</returns>

        public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, int, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return condition ? source.Where(predicate) : source;
        }
    }
}