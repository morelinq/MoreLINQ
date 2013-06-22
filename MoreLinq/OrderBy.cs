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
    using System.Linq;

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Sorts the elements of a sequence in a particular direction (ascending, descending) according to a key
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence</typeparam>
        /// <typeparam name="TKey">The type of the key used to order elements</typeparam>
        /// <param name="sequence">The sequence to order</param>
        /// <param name="keySelector">A key selector function</param>
        /// <param name="direction">A direction in which to order the elements (ascending, descending)</param>
        /// <returns>An ordered copy of the source sequence</returns>
        
        public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> keySelector, OrderByDirection direction)
        {
            return direction == OrderByDirection.Ascending
                       ? sequence.OrderBy(keySelector)
                       : sequence.OrderByDescending(keySelector);
        }

        /// <summary>
        /// Sorts the elements of a sequence in a particular direction (ascending, descending) according to a key
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence</typeparam>
        /// <typeparam name="TKey">The type of the key used to order elements</typeparam>
        /// <param name="sequence">The sequence to order</param>
        /// <param name="keySelector">A key selector function</param>
        /// <param name="direction">A direction in which to order the elements (ascending, descending)</param>
        /// <param name="comparer">A comparer used to define the semantics of element comparison</param>
        /// <returns>An ordered copy of the source sequence</returns>
        
        public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> sequence, Func<T, TKey> keySelector, IComparer<TKey> comparer, OrderByDirection direction)
        {
            return direction == OrderByDirection.Ascending
                       ? sequence.OrderBy(keySelector, comparer)
                       : sequence.OrderByDescending(keySelector, comparer);
        }

        /// <summary>
        /// Performs a subsequent ordering of elements in a sequence in a particular direction (ascending, descending) according to a key
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence</typeparam>
        /// <typeparam name="TKey">The type of the key used to order elements</typeparam>
        /// <param name="sequence">The sequence to order</param>
        /// <param name="keySelector">A key selector function</param>
        /// <param name="direction">A direction in which to order the elements (ascending, descending)</param>
        /// <returns>An ordered copy of the source sequence</returns>
        
        public static IOrderedEnumerable<T> ThenBy<T, TKey>(this IOrderedEnumerable<T> sequence, Func<T, TKey> keySelector, OrderByDirection direction)
        {
            return direction == OrderByDirection.Ascending
                       ? sequence.ThenBy(keySelector)
                       : sequence.ThenByDescending(keySelector);
        }

        /// <summary>
        /// Performs a subsequent ordering of elements in a sequence in a particular direction (ascending, descending) according to a key
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence</typeparam>
        /// <typeparam name="TKey">The type of the key used to order elements</typeparam>
        /// <param name="sequence">The sequence to order</param>
        /// <param name="keySelector">A key selector function</param>
        /// <param name="direction">A direction in which to order the elements (ascending, descending)</param>
        /// <param name="comparer">A comparer used to define the semantics of element comparison</param>
        /// <returns>An ordered copy of the source sequence</returns>
        
        public static IOrderedEnumerable<T> ThenBy<T, TKey>(this IOrderedEnumerable<T> sequence, Func<T, TKey> keySelector, IComparer<TKey> comparer, OrderByDirection direction)
        {
            return direction == OrderByDirection.Ascending
                       ? sequence.ThenBy(keySelector, comparer)
                       : sequence.ThenByDescending(keySelector, comparer);
        }
    }
}