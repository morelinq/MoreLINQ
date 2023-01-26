#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Andreas Gullberg Larsen. All rights reserved.
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
        /// Determines whether the beginning of the first sequence is
        /// equivalent to the second sequence, using the default equality
        /// comparer.
        /// </summary>
        /// <typeparam name="T">Type of elements.</typeparam>
        /// <param name="first">The sequence to check.</param>
        /// <param name="second">The sequence to compare to.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="first" /> begins with elements
        /// equivalent to <paramref name="second" />.
        /// </returns>
        /// <remarks>
        /// This is the <see cref="IEnumerable{T}" /> equivalent of
        /// <see cref="string.StartsWith(string)" /> and it calls
        /// <see cref="IEqualityComparer{T}.Equals(T,T)" /> using
        /// <see cref="EqualityComparer{T}.Default"/> on pairs of elements at
        /// the same index.
        /// </remarks>

        public static bool StartsWith<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            return StartsWith(first, second, null);
        }

        /// <summary>
        /// Determines whether the beginning of the first sequence is
        /// equivalent to the second sequence, using the specified element
        /// equality comparer.
        /// </summary>
        /// <typeparam name="T">Type of elements.</typeparam>
        /// <param name="first">The sequence to check.</param>
        /// <param name="second">The sequence to compare to.</param>
        /// <param name="comparer">Equality comparer to use.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="first" /> begins with elements
        /// equivalent to <paramref name="second" />.
        /// </returns>
        /// <remarks>
        /// This is the <see cref="IEnumerable{T}" /> equivalent of
        /// <see cref="string.StartsWith(string)" /> and
        /// it calls <see cref="IEqualityComparer{T}.Equals(T,T)" /> on pairs
        /// of elements at the same index.
        /// </remarks>

        public static bool StartsWith<T>(this IEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T>? comparer)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            if (first.TryAsCollectionLike() is { Count: var firstCount } &&
                second.TryAsCollectionLike() is { Count: var secondCount } &&
                secondCount > firstCount)
            {
                return false;
            }

            comparer ??= EqualityComparer<T>.Default;

            using var firstIter = first.GetEnumerator();
            return second.All(item => firstIter.MoveNext() && comparer.Equals(firstIter.Current, item));
        }
    }
}
