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
        /// Prepends a single value to a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to prepend to.</param>
        /// <param name="value">The value to prepend.</param>
        /// <returns>
        /// Returns a sequence where a value is prepended to it.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// IEnumerable&lt;int&gt; result = numbers.Prepend(0);
        /// </code>
        /// The <c>result</c> variable, when iterated over, will yield 
        /// 0, 1, 2 and 3, in turn.

        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource value)
        {
            if (source == null) throw new ArgumentNullException("source");
            return LinqEnumerable.Concat(LinqEnumerable.Repeat(value, 1), source);
        }
    }
}