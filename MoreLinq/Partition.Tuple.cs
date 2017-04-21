#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Atif Aziz. All rights reserved.
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
    using System.Diagnostics;
    using System.Linq;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Partitions or splits a sequence in two using a predicate.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="predicate">The predicate function.</param>
        /// <typeparam name="T">Type of source elements.</typeparam>
        /// <returns>
        /// A tuple of elements staisfying the predicate and those that do not,
        /// respectively.
        /// </returns>
        /// <example>
        /// <code>
        /// var (evens, odds) =
        ///     Enumerable.Range(0, 10).Partition(x => x % 2 == 0);
        /// </code>
        /// The <c>evens</c> variable, when iterated over, will yield 0, 2, 4, 6
        /// and then 8. The <c>odds</c> variable, when iterated over, will yield
        /// 1, 3, 5, 7 and then 9.
        /// </example>

        public static (IEnumerable<T> True, IEnumerable<T> False)
            Partition<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
            source.Partition(predicate, ValueTuple.Create);
    }
}
