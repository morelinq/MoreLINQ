#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Leandro F. Vieira (leandromoh). All rights reserved.
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
        /// Returns the nth element of the sequence.
        /// If the sequence does not have an nth element, the default value is returned. 
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="nth">The position of the desired element.</param>
        /// <returns>Returns the nth element of the sequence or the default value.</returns>
        public static T NthOrDefault<T>(this IEnumerable<T> source, int nth)
        {
            return NthIterator(source, nth, true);
        }

        /// <summary>
        /// Returns the nth element of the sequence that matches the predicate.
        /// If the sequence does not have an nth element that matches the predicate, the default value is returned. 
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="nth">The position of the desired element among those who satisfy the predicate.</param>
        /// <param name="predicate">The predicate to indicate if the element is valid.</param>
        /// <returns>Returns the nth element of the sequence that matches the predicate or the default value.</returns>
        public static T NthOrDefault<T>(this IEnumerable<T> source, int nth, Func<T, bool> predicate)
        {
            return NthIterator(source, nth, true, predicate);
        }

        /// <summary>
        /// Returns the nth element of the sequence.
        /// If the sequence does not have an nth element, the method throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="nth">The position of the desired element.</param>
        /// <returns>Returns the nth element of the sequence.</returns>
        public static T Nth<T>(this IEnumerable<T> source, int nth)
        {
            return NthIterator(source, nth, false);
        }

        /// <summary>
        /// Returns the nth element of the sequence that matches the predicate.
        /// If the sequence does not have an nth element that matches the predicate, 
        /// the method throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="nth">The position of the desired element among those who satisfy the predicate.</param>
        /// <param name="predicate">The predicate to indicate if the element is valid.</param>
        /// <returns>Returns the nth element of the sequence that matches the predicate.</returns>
        public static T Nth<T>(this IEnumerable<T> source, int nth, Func<T, bool> predicate)
        {
            return NthIterator(source, nth, false, predicate);
        }

        private static T NthIterator<T>(IEnumerable<T> source, int nth, bool useDefault)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (nth <= 0) throw new ArgumentOutOfRangeException("nth", "nth must be greater than zero.");

            int found = 0;

            foreach (var item in source)
            {
                if (++found == nth)
                {
                    return item;
                }
            }

            if (!useDefault)
                throw new InvalidOperationException("Sequence does not contains " + nth + " elements");

            return default(T);
        }

        private static T NthIterator<T>(IEnumerable<T> source, int nth, bool useDefault, Func<T, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");
            if (nth <= 0) throw new ArgumentOutOfRangeException("nth", "nth must be greater than zero.");

            int found = 0;

            foreach (var item in source)
            {
                if (predicate(item) && ++found == nth)
                {
                    return item;
                }
            }

            if (!useDefault)
                throw new InvalidOperationException("Sequence does not contains " + nth + " elements that matches the predicate");

            return default(T);
        }
    }
}