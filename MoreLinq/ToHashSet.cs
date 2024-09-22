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

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;

    // TODO: Tests! (The code is simple enough I trust it not to fail, mind you...)
    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a <see cref="HashSet{T}"/> of the source items using the default equality
        /// comparer for the type.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in source sequence.</typeparam>
        /// <param name="source">Source sequence</param>
        /// <returns>A hash set of the items in the sequence, using the default equality comparer.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <remarks>
        /// This evaluates the input sequence completely.
        /// </remarks>

#if NETSTANDARD2_1 || NET472_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        public static HashSet<TSource> ToHashSet<TSource>(IEnumerable<TSource> source)
#else
        public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source)
#endif
        {
            return ToHashSet(source, null);
        }

        /// <summary>
        /// Returns a <see cref="HashSet{T}"/> of the source items using the specified equality
        /// comparer for the type.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in source sequence.</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="comparer">Equality comparer to use; a value of null will cause the type's default equality comparer to be used</param>
        /// <returns>A hash set of the items in the sequence, using the default equality comparer.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <remarks>
        /// This evaluates the input sequence completely.
        /// </remarks>

#if NETSTANDARD2_1 || NET472_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        public static HashSet<TSource> ToHashSet<TSource>(IEnumerable<TSource> source, IEqualityComparer<TSource>? comparer)
#else
        public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource>? comparer)
#endif
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return new HashSet<TSource>(source, comparer);
        }
    }
}
