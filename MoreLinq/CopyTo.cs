#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2023 Turning Code, LLC. All rights reserved.
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

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Copies the contents of a sequence into a provided array.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of elements of <paramref name="source"/></typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="array">The array that is the destination of the elements copied from <paramref
        /// name="source"/>.</param>
        /// <returns>The number of elements actually copied.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/> is not long enough to hold the data from
        /// sequence.</exception>
        /// <remarks>
        /// <para>
        /// All data from <paramref name="source"/> will be copied to <paramref name="array"/> if possible. If <paramref
        /// name="source"/> is shorter than <paramref name="array"/>, then any remaining elements will be untouched. If
        /// <paramref name="source"/> is longer than <paramref name="array"/>, then an exception will be thrown.
        /// </para>
        /// <para>
        /// This operator executes immediately.
        /// </para>
        /// </remarks>
        public static int CopyTo<TSource>(this IEnumerable<TSource> source, TSource[] array)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (array == null) throw new ArgumentNullException(nameof(array));

            return CopyTo(source, array, 0);
        }

        static int CopyTo<TSource>(IEnumerable<TSource> source, TSource[] array, int index)
        {
            if (source is TSource[] arr)
            {
                arr.CopyTo(array, index);
                return arr.Length;
            }
            else if (source is ICollection<TSource> coll)
            {
                coll.CopyTo(array, index);
                return coll.Count;
            }
            else if (source.TryAsCollectionLike() is CollectionLike<TSource> c)
            {
                if (c.Count + index > array.Length)
                    throw new ArgumentException("Destination is not long enough.", nameof(array));

                var i = index;
                foreach (var el in source)
                    array[i++] = el;

                return i - index;
            }
            else
            {
                var i = index;
                foreach (var el in source)
                {
                    if (i >= array.Length)
                        throw new ArgumentException("Destination is not long enough.", nameof(array));

                    array[i++] = el;
                }

                return i - index;
            }
        }

        /// <summary>
        /// Copies the contents of a sequence into a provided list.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of elements of <paramref name="source"/></typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="list">The list that is the destination of the elements copied from <paramref
        /// name="source"/>.</param>
        /// <returns>The number of elements actually copied.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// <para>
        /// All data from <paramref name="source"/> will be copied to <paramref name="list"/>, starting at position 0.
        /// </para>
        /// <para>
        /// This operator executes immediately.
        /// </para>
        /// </remarks>
        public static int CopyTo<TSource>(this IEnumerable<TSource> source, IList<TSource> list)
        {
            return source.CopyTo(list, 0);
        }

        /// <summary>
        /// Copies the contents of a sequence into a provided list.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of elements of <paramref name="source"/></typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="list">The list that is the destination of the elements copied from <paramref
        /// name="source"/>.</param>
        /// <param name="index">The position in <paramref name="list"/> at which to start copying data</param>
        /// <returns>The number of elements actually copied.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
        /// <remarks>
        /// <para>
        /// All data from <paramref name="source"/> will be copied to <paramref name="list"/>, starting at position
        /// <paramref name="index"/>.
        /// </para>
        /// <para>
        /// This operator executes immediately.
        /// </para>
        /// </remarks>
        public static int CopyTo<TSource>(this IEnumerable<TSource> source, IList<TSource> list, int index)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            if (list is TSource[] array)
            {
                return CopyTo(source, array, index);
            }
            else
            {
#if NET6_0_OR_GREATER
                if (list is List<TSource> l
                    && source.TryAsCollectionLike() is CollectionLike<TSource> c)
                {
                    l.EnsureCapacity(c.Count + index);
                }
#endif

                var i = index;
                foreach (var el in source)
                {
                    if (i < list.Count)
                        list[i] = el;
                    else
                        list.Add(el);
                    i++;
                }

                return i - index;
            }
        }
    }
}
