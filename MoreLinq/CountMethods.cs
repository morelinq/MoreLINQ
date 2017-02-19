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

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Determines whether or not the number of elements in the sequence is greater than
        /// or equal to the given integer.
        /// </summary>
        /// <typeparam name="T">Element type of sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="count">The minimum number of items a sequence must have for this
        /// function to return true</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative</exception>
        /// <returns><c>true</c> if the number of elements in the sequence is greater than
        /// or equal to the given integer or <c>false</c> otherwise.</returns>
        /// <example>
        /// <code>
        /// var numbers = { 123, 456, 789 };
        /// var result = numbers.AtLeast(2);
        /// </code>
        /// The <c>result</c> variable will contain <c>true</c>.
        /// </example>
        public static bool AtLeast<T>(this IEnumerable<T> source, int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");

            return QuantityIterator(source, count, n => n >= count);
        }

        /// <summary>
        /// Determines whether or not the number of elements in the first sequence is greater than
        /// or equal to the number of elements in the second sequence.
        /// </summary>
        /// <typeparam name="T">Element type of sequence</typeparam>
        /// <param name="first">The first sequence</param>
        /// <param name="second">The second sequence</param>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="second"/> is null</exception>
        /// <returns><c>true</c> if the number of elements in the first sequence is greater than
        /// or equal to the number of elements in the second sequence or <c>false</c> otherwise.</returns>
        /// <example>
        /// <code>
        /// var numbers = { 123, 456, 789 };
        /// var other = { 111, 222 };
        /// var result = numbers.AtLeast(other);
        /// </code>
        /// The <c>result</c> variable will contain <c>true</c>.
        /// </example>
        public static bool AtLeast<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            return QuantityIterator(first, second, (a, b) => a >= b, AtLeast, AtMost);
        }

        /// <summary>
        /// Determines whether or not the number of elements in the sequence is lesser than
        /// or equal to the given integer.
        /// </summary>
        /// <typeparam name="T">Element type of sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="count">The maximun number of items a sequence must have for this
        /// function to return true</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative</exception>
        /// <returns><c>true</c> if the number of elements in the sequence is lesser than
        /// or equal to the given integer or <c>false</c> otherwise.</returns>
        /// <example>
        /// <code>
        /// var numbers = { 123, 456, 789 };
        /// var result = numbers.AtMost(2);
        /// </code>
        /// The <c>result</c> variable will contain <c>false</c>.
        /// </example>
        public static bool AtMost<T>(this IEnumerable<T> source, int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");

            return QuantityIterator(source, count + 1, n => n <= count);
        }

        /// <summary>
        /// Determines whether or not the number of elements in the first sequence is lesser than
        /// or equal to the number of elements in the second sequence.
        /// </summary>
        /// <typeparam name="T">Element type of sequence</typeparam>
        /// <param name="first">The first sequence</param>
        /// <param name="second">The second sequence</param>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="second"/> is null</exception>
        /// <returns><c>true</c> if the number of elements in the first sequence is lesser than
        /// or equal to the number of elements in the second sequence or <c>false</c> otherwise.</returns>
        /// <example>
        /// <code>
        /// var numbers = { 123, 456, 789 };
        /// var other = { 111, 222 };
        /// var result = numbers.AtMost(other);
        /// </code>
        /// The <c>result</c> variable will contain <c>false</c>.
        /// </example>
        public static bool AtMost<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            return QuantityIterator(first, second, (a, b) => a <= b, AtMost, AtLeast);
        }

        /// <summary>
        /// Determines whether or not the number of elements in the sequence is equals to the given integer.
        /// </summary>
        /// <typeparam name="T">Element type of sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="count">The exactly number of items a sequence must have for this
        /// function to return true</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative</exception>
        /// <returns><c>true</c> if the number of elements in the sequence is equals
        /// to the given integer or <c>false</c> otherwise.</returns>
        /// <example>
        /// <code>
        /// var numbers = { 123, 456, 789 };
        /// var result = numbers.Exactly(3);
        /// </code>
        /// The <c>result</c> variable will contain <c>true</c>.
        /// </example>
        public static bool Exactly<T>(this IEnumerable<T> source, int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");

            return QuantityIterator(source, count + 1, n => n == count);
        }

        /// <summary>
        /// Determines whether or not the number of elements in the first sequence is equal to 
        /// the number of elements in the second sequence.
        /// </summary>
        /// <typeparam name="T">Element type of sequence</typeparam>
        /// <param name="first">The first sequence</param>
        /// <param name="second">The second sequence</param>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="second"/> is null</exception>
        /// <returns><c>true</c> if the number of elements in the first sequence is equal
        /// to the number of elements in the second sequence or <c>false</c> otherwise.</returns>
        /// <example>
        /// <code>
        /// var numbers = { 123, 456, 789 };
        /// var other = { 111, 222, 333 };
        /// var result = numbers.Exactly(other);
        /// </code>
        /// The <c>result</c> variable will contain <c>true</c>.
        /// </example>
        public static bool Exactly<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            return QuantityIterator(first, second, (a, b) => a == b, Exactly, Exactly);
        }

        /// <summary>
        /// Determines whether or not the number of elements in the sequence is between 
        /// an inclusive range of minimum and maximum integers.
        /// </summary>
        /// <typeparam name="T">Element type of sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="min">The minimum number of items a sequence must have for this
        /// function to return true</param>
        /// <param name="max">The maximun number of items a sequence must have for this
        /// function to return true</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="min"/> is negative or <paramref name="max"/> is less than min</exception>
        /// <returns><c>true</c> if the number of elements in the sequence is between (inclusive)
        /// the min and max given integers or <c>false</c> otherwise.</returns>
        /// <example>
        /// <code>
        /// var numbers = { 123, 456, 789 };
        /// var result = numbers.CountBetween(1, 2);
        /// </code>
        /// The <c>result</c> variable will contain <c>false</c>.
        /// </example>
        public static bool CountBetween<T>(this IEnumerable<T> source, int min, int max)
        {
            if (min < 0) throw new ArgumentOutOfRangeException(nameof(min), "Minimum count cannot be negative.");
            if (max < min) throw new ArgumentOutOfRangeException(nameof(max), "Maximum count must be greater than or equal to the minimum count.");

            return QuantityIterator(source, max + 1, n => min <= n && n <= max);
        }


        static bool QuantityIterator<T>(IEnumerable<T> source, int limit, Func<int, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var col = source as ICollection<T>;
            if (col != null)
            {
                return predicate(col.Count);
            }

            var count = 0;

            using (var e = source.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    if (++count == limit)
                    {
                        break;
                    }
                }
            }

            return predicate(count);
        }

        private static bool QuantityIterator<T>(IEnumerable<T> first, IEnumerable<T> second, Func<int, int, bool> collectionPredicate, Func<IEnumerable<T>, int, bool> enumerablePredicate, Func<IEnumerable<T>, int, bool> enumerablePredicateInvert)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            var firstCol = first as ICollection<T>;
            var secondCol = second as ICollection<T>;

            if (firstCol != null)
            {
                if (secondCol != null)
                {
                    return collectionPredicate(firstCol.Count, secondCol.Count);
                }

                return enumerablePredicateInvert(second, firstCol.Count);
            }

            if (secondCol != null)
            {
                return enumerablePredicate(first, secondCol.Count);
            }

            using (var e = first.GetEnumerator())
            using (var e2 = second.GetEnumerator())
            {
                var eHasElement = false;
                var e2HasElement = false;

                do
                {
                    eHasElement = e.MoveNext();
                    e2HasElement = e2.MoveNext();
                }
                while (eHasElement && e2HasElement);

                return collectionPredicate(Convert.ToInt32(eHasElement), Convert.ToInt32(e2HasElement));
            }
        }
    }
}
