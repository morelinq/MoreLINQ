#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2023 Julien Aspirot. All rights reserved.
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
        /// Returns all duplicate elements of the given source.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <returns>All elements that are duplicated.</returns>
        /// <remarks>This operator uses deferred execution and streams its results.</remarks>

        public static IEnumerable<TSource> Duplicates<TSource>(this IEnumerable<TSource> source) =>
            Duplicates(source, null);

        /// <summary>
        /// Returns all duplicate elements of the given source, using the specified equality
        /// comparer.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="comparer">
        /// The equality comparer to use to determine whether one <typeparamref name="TSource"/>
        /// equals another. If <see langword="null"/>, the default equality comparer for
        /// <typeparamref name="TSource"/> is used.</param>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <returns>All elements that are duplicated.</returns>
        /// <remarks>This operator uses deferred execution and streams its results.</remarks>

        public static IEnumerable<TSource> Duplicates<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource>? comparer)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            return from e in source.ScanBy(static e => e,
                                           static _ => 0,
                                           static (count, _, _) => unchecked(Math.Min(count + 1, 3)),
                                           comparer)
                   where e.Value is 2
                   select e.Key;
        }
    }
}
