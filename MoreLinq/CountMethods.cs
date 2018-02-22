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

            return QuantityIterator(source, count, count, int.MaxValue);
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

            return QuantityIterator(source, count + 1, 0, count);
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

            return QuantityIterator(source, count + 1, count, count);
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

            return QuantityIterator(source, max + 1, min, max);
        }

        static bool QuantityIterator<T>(IEnumerable<T> source, int limit, int min, int max)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var count = 0;

            if (source is ICollection<T> col)
            {
                count = col.Count;
            }
            else
            {
                using (var e = source.GetEnumerator())
                {
                    while (e.MoveNext())
                    {
                        if (++count == limit)
                            break;
                    }
                }
            }

            return count >= min && count <= max;
        }

        /// <summary>
        /// Compares two sequences and returns an integer that indicates whether the first sequence
        /// has fewer, the same or more elements than the second sequence.
        /// </summary>
        /// <typeparam name="TFirst">Element type of the first sequence</typeparam>
        /// <typeparam name="TSecond">Element type of the second sequence</typeparam>
        /// <param name="first">The first sequence</param>
        /// <param name="second">The second sequence</param>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> is null</exception>
        /// <exception cref="ArgumentNullException"><paramref name="second"/> is null</exception>
        /// <returns><c>-1</c> if the first sequence has the fewest elements, <c>0</c> if the two sequences have the same number of elements
        /// or <c>1</c> if the first sequence has the most elements.</returns>
        /// <example>
        /// <code>
        /// var first = { 123, 456 };
        /// var second = { 789 };
        /// var result = first.CompareCount(second);
        /// </code>
        /// The <c>result</c> variable will contain <c>1</c>.
        /// </example>
        public static int CompareCount<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));

            if (first is ICollection<TFirst> firstCol)
            {
                return firstCol.Count.CompareTo(second is ICollection<TSecond> secondCol
                                                ? secondCol.Count
                                                : PartialCount(second, firstCol.Count + 1));
            }
            else
            {
                if (second is ICollection<TSecond> secondCol)
                    return PartialCount(first, secondCol.Count + 1).CompareTo(secondCol.Count);

                bool firstHasNext;
                bool secondHasNext;

                using (var e1 = first.GetEnumerator())
                using (var e2 = second.GetEnumerator())
                {
                    do
                    {
                        firstHasNext = e1.MoveNext();
                        secondHasNext = e2.MoveNext();
                    }
                    while (firstHasNext && secondHasNext);
                }

                return Convert.ToInt32(firstHasNext).CompareTo(Convert.ToInt32(secondHasNext));
            }

            int PartialCount<T>(IEnumerable<T> source, int limit)
            {
                var count = 0;

                using (var e = source.GetEnumerator())
                {
                    while (e.MoveNext())
                    {
                        if (++count == limit)
                            break;
                    }
                }

                return count;
            }
        }
    }
}
