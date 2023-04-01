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

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Creates an array from an <see cref="IEnumerable{T}"/> where a
        /// function is used to determine the index at which an element will
        /// be placed in the array.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <typeparam name="T">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <returns>
        /// An array that contains the elements from the input sequence. The
        /// size of the array will be as large as the highest index returned
        /// by the <paramref name="indexSelector"/> plus 1.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>

        public static T[] ToArrayByIndex<T>(this IEnumerable<T> source,
            Func<T, int> indexSelector)
        {
            return source.ToArrayByIndex(indexSelector, (e, _) => e);
        }

        /// <summary>
        /// Creates an array from an <see cref="IEnumerable{T}"/> where a
        /// function is used to determine the index at which an element will
        /// be placed in the array. The elements are projected into the array
        /// via an additional function.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <param name="resultSelector">
        /// A function to project a source element into an element of the
        /// resulting array.</param>
        /// <typeparam name="T">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the element in the resulting array.</typeparam>
        /// <returns>
        /// An array that contains the projected elements from the input
        /// sequence. The size of the array will be as large as the highest
        /// index returned by the <paramref name="indexSelector"/> plus 1.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>

        public static TResult[] ToArrayByIndex<T, TResult>(this IEnumerable<T> source,
            Func<T, int> indexSelector, Func<T, TResult> resultSelector)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            return source.ToArrayByIndex(indexSelector, (e, _) => resultSelector(e));
        }

        /// <summary>
        /// Creates an array from an <see cref="IEnumerable{T}"/> where a
        /// function is used to determine the index at which an element will
        /// be placed in the array. The elements are projected into the array
        /// via an additional function.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <param name="resultSelector">
        /// A function to project a source element into an element of the
        /// resulting array.</param>
        /// <typeparam name="T">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the element in the resulting array.</typeparam>
        /// <returns>
        /// An array that contains the projected elements from the input
        /// sequence. The size of the array will be as large as the highest
        /// index returned by the <paramref name="indexSelector"/> plus 1.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>

        public static TResult[] ToArrayByIndex<T, TResult>(this IEnumerable<T> source,
            Func<T, int> indexSelector, Func<T, int, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (indexSelector == null) throw new ArgumentNullException(nameof(indexSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var lastIndex = -1;
            var indexed = (List<KeyValuePair<int, T>>?)null;
            List<KeyValuePair<int, T>> Indexed() => indexed ??= new List<KeyValuePair<int, T>>();

            foreach (var e in source)
            {
#pragma warning disable IDE0010 // Add missing cases (false negative)
                switch (indexSelector(e))
#pragma warning restore IDE0010 // Add missing cases
                {
                    case < 0:
#pragma warning disable CA2201 // Do not raise reserved exception types
                        throw new IndexOutOfRangeException();
#pragma warning restore CA2201 // Do not raise reserved exception types
                    case var i:
                        lastIndex = Math.Max(i, lastIndex);
                        Indexed().Add(new KeyValuePair<int, T>(i, e));
                        break;
                }
            }

            var length = lastIndex + 1;
            return length == 0
                 ? EmptyArray<TResult>.Value
                 : Indexed().ToArrayByIndex(length, e => e.Key, e => resultSelector(e.Value, e.Key));
        }

        /// <summary>
        /// Creates an array of user-specified length from an
        /// <see cref="IEnumerable{T}"/> where a function is used to determine
        /// the index at which an element will be placed in the array.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="length">The (non-negative) length of the resulting array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <typeparam name="T">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <returns>
        /// An array of size <paramref name="length"/> that contains the
        /// elements from the input sequence.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>

        public static T[] ToArrayByIndex<T>(this IEnumerable<T> source, int length,
            Func<T, int> indexSelector)
        {
            return source.ToArrayByIndex(length, indexSelector, (e, _) => e);
        }

        /// <summary>
        /// Creates an array of user-specified length from an
        /// <see cref="IEnumerable{T}"/> where a function is used to determine
        /// the index at which an element will be placed in the array. The
        /// elements are projected into the array via an additional function.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="length">The (non-negative) length of the resulting array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <param name="resultSelector">
        /// A function to project a source element into an element of the
        /// resulting array.</param>
        /// <typeparam name="T">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the element in the resulting array.</typeparam>
        /// <returns>
        /// An array of size <paramref name="length"/> that contains the
        /// projected elements from the input sequence.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>

        public static TResult[] ToArrayByIndex<T, TResult>(this IEnumerable<T> source, int length,
            Func<T, int> indexSelector, Func<T, TResult> resultSelector)
        {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            return source.ToArrayByIndex(length, indexSelector, (e, _) => resultSelector(e));
        }

        /// <summary>
        /// Creates an array of user-specified length from an
        /// <see cref="IEnumerable{T}"/> where a function is used to determine
        /// the index at which an element will be placed in the array. The
        /// elements are projected into the array via an additional function.
        /// </summary>
        /// <param name="source">The source sequence for the array.</param>
        /// <param name="length">The (non-negative) length of the resulting array.</param>
        /// <param name="indexSelector">
        /// A function that maps an element to its index.</param>
        /// <param name="resultSelector">
        /// A function to project a source element into an element of the
        /// resulting array.</param>
        /// <typeparam name="T">
        /// The type of the element in <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the element in the resulting array.</typeparam>
        /// <returns>
        /// An array of size <paramref name="length"/> that contains the
        /// projected elements from the input sequence.
        /// </returns>
        /// <remarks>
        /// This method forces immediate query evaluation. It should not be
        /// used on infinite sequences. If more than one element maps to the
        /// same index then the latter element overwrites the former in the
        /// resulting array.
        /// </remarks>

        public static TResult[] ToArrayByIndex<T, TResult>(this IEnumerable<T> source, int length,
            Func<T, int> indexSelector, Func<T, int, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (indexSelector == null) throw new ArgumentNullException(nameof(indexSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var array = new TResult[length];
            foreach (var e in source)
            {
                var i = indexSelector(e);

                if (i < 0 || i >= array.Length)
                {
#pragma warning disable CA2201 // Do not raise reserved exception types
                    throw new IndexOutOfRangeException();
#pragma warning restore CA2201 // Do not raise reserved exception types
                }

                array[i] = resultSelector(e, i);
            }
            return array;
        }
    }
}
