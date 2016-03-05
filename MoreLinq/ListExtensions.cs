#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Sergiy Zinovyev. All rights reserved.
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

    /// <summary>
    /// ListExtensions class extends List class with the following methods:
    ///  - AddSorted
    ///  - BinarySearchBy
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Adds item in sorted list.
        /// </summary>
        /// <param name="list">The primary list where to add new item. It must be ordered ascending.</param>
        /// <param name="item">New item to insert into the list.</param>
        /// <typeparam name="T">The type of the items of the list.</typeparam>
        /// <returns>Original instance of the source list with the new item added and maintained order.</returns>
        public static List<T> AddSorted<T>(this List<T> list, T item)
        {
            return list.AddSorted(item, (IComparer<T>)null);
        }

        /// <summary>
        /// Adds item in sorted list.
        /// </summary>
        /// <param name="list">The primary list where to add new item. It must be ordered ascending.</param>
        /// <param name="item">New item to insert into the list.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
        /// <typeparam name="T">The type of the items of the list.</typeparam>
        /// <returns>Original instance of the source list with the new item added and maintained order.</returns>
        public static List<T> AddSorted<T>(this List<T> list, T item, IComparer<T> comparer)
        {
            return list.AddSorted(item, comparer, OrderByDirection.Ascending);
        }

        /// <summary>
        /// Adds item in sorted list.
        /// </summary>
        /// <param name="list">The primary list where to add new item. It must be ordered ascending.</param>
        /// <param name="item">New item to insert into the list.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
        /// <param name="direction">The ordering that all sequences must already exhibit.</param>
        /// <typeparam name="T">The type of the items of the list.</typeparam>
        /// <returns>Original instance of the source list with the new item added and maintained order.</returns>
        public static List<T> AddSorted<T>(this List<T> list, T item, IComparer<T> comparer, OrderByDirection direction)
        {
            return list.AddSorted(item, v => v, comparer, direction);
        }

        /// <summary>
        /// Adds item in sorted list.
        /// </summary>
        /// <param name="list">The primary list where to add new item. It must be ordered ascending.</param>
        /// <param name="item">New item to insert into the list.</param>
        /// <param name="direction">The ordering that all sequences must already exhibit.</param>
        /// <typeparam name="T">The type of the items of the list.</typeparam>
        /// <returns>Original instance of the source list with the new item added and maintained order.</returns>
        public static List<T> AddSorted<T>(this List<T> list, T item, OrderByDirection direction)
        {
            return list.AddSorted(item, v => v, (IComparer<T>)null, direction);
        }

        /// <summary>
        /// Adds item in sorted list.
        /// </summary>
        /// <param name="list">The primary list where to add new item. It must be ordered ascending.</param>
        /// <param name="item">New item to insert into the list.</param>
        /// <param name="keySelector">Function to extract a key given an element from the list.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
        /// <param name="direction">The ordering that all sequences must already exhibit.</param>
        /// <typeparam name="T">The type of the items of the list.</typeparam>
        /// <typeparam name="TKey">Type of keys used for ordering.</typeparam>
        /// <returns>Original instance of the source list with the new item added and maintained order.</returns>
        public static List<T> AddSorted<T, TKey>(this List<T> list, T item, Func<T, TKey> keySelector, IComparer<TKey> comparer, OrderByDirection direction)
        {
            return list.AddSortedImpl(item, keySelector, comparer, direction);
        }


        private static List<T> AddSortedImpl<T, TKey>(this List<T> list, T item, Func<T, TKey> keySelector, IComparer<TKey> comparer, OrderByDirection direction)
        {
            comparer = comparer ?? Comparer<TKey>.Default;

            if (list.Count == 0)
            {
                list.Add(item);
                return list;
            }

            int dir = (direction == OrderByDirection.Ascending) ? 1 : -1;

            if (dir * comparer.Compare(keySelector(list.Last()), keySelector(item)) <= 0)
            {
                list.Add(item);
                return list;
            }

            if (dir * comparer.Compare(keySelector(list.First()), keySelector(item)) >= 0)
            {
                list.Insert(0, item);
                return list;
            }

            int index = list.BinarySearchBy(item, keySelector, comparer, direction);
            if (index < 0)
            {
                index = ~index;
            }

            list.Insert(index, item);
            return list;
        }


        /// <summary>
        /// Searches for an item using binary search in ordered list by field.
        /// Searches a range of elements in the sorted <see cref="List{T}"/> for an element using the specified comparer and returns the zero-based index of the element.
        /// </summary>
        /// <param name="list">The primary list where to add new item. It must be ordered ascending.</param>
        /// <param name="item">The object to locate. The value can be null for reference types.</param>
        /// <param name="keySelector">Function to extract a key given an element from a sequence.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
        /// <param name="direction">The ordering that all sequences must already exhibit.</param>
        /// <typeparam name="T">The type of the items of the list.</typeparam>
        /// <typeparam name="TKey">Type of keys used for ordering.</typeparam>
        /// <returns>
        /// The zero-based index of item in the sorted <see cref="List{T}"/>, if item is found; otherwise,
        /// a negative number that is the bitwise complement of the index of the next element that is larger (for ascending ordering, smaller for descending ordering) than item or,
        /// if there is no larger (for ascending ordering, smaller for descending ordering) element, the bitwise complement of Count.
        /// </returns>
        /// <remarks>Used by <c>AddSorted</c> to get the position for inserted item.</remarks>
        public static int BinarySearchBy<T, TKey>(this List<T> list, T item, Func<T, TKey> keySelector, IComparer<TKey> comparer, OrderByDirection direction)
        {
            int dir = (direction == OrderByDirection.Ascending) ? 1 : -1;

            TKey key = keySelector(item);

            int lo = 0;
            int hi = list.Count - 1;

            while (lo <= hi)
            {
                int i = (lo + hi) / 2;

                int c = dir * comparer.Compare(keySelector(list[i]), key);

                if (c == 0)
                {
                    return i;
                }

                if (c < 0)
                {
                    lo = i + 1;
                }
                else
                {
                    hi = i - 1;
                }
            }

            return ~lo;
        }
    }
}
