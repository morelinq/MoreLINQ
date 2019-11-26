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

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the argument sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="TFirst">Type of elements in first sequence.</typeparam>
        /// <typeparam name="TSecond">Type of elements in second sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="first">The first sequence.</param>
        /// <param name="second">The second sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each pair of elements.</param>
        /// <returns>
        /// A sequence that contains elements of the two input sequences,
        /// combined by <paramref name="resultSelector"/>.
        /// </returns>
        /// <example>
        /// <code><![CDATA[
        /// var numbers = { 1, 2, 3 };
        /// var letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipLongest(letters, (n, l) => n + l);
        /// ]]></code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A",
        /// "2B", "3C", "0D" in turn.
        /// </example>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult> ZipLongest<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return ZipImpl<TFirst, TSecond, object, object, TResult>(first, second, null, null, (a, b, c, d) => resultSelector(a, b), 1);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the argument sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="first">The first sequence.</param>
        /// <param name="second">The second sequence.</param>
        /// <param name="third">The third sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each triplet of elements.</param>
        /// <returns>
        /// A sequence that contains elements of the three input sequences,
        /// combined by <paramref name="resultSelector"/>.
        /// </returns>
        /// <example>
        /// <code><![CDATA[
        /// var numbers = new[] { 1, 2, 3 };
        /// var letters = new[] { "A", "B", "C", "D" };
        /// var chars   = new[] { 'a', 'b', 'c', 'd', 'e' };
        /// var zipped  = numbers.ZipLongest(letters, chars, (n, l, c) => n + l + c);
        /// ]]></code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1Aa",
        /// "2Bb", "3Cc", "0Dd", "0e" in turn.
        /// </example>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult> ZipLongest<T1, T2, T3, TResult>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (third == null) throw new ArgumentNullException(nameof(third));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return ZipImpl<T1, T2, T3, object, TResult>(first, second, third, null, (a, b, c, d) => resultSelector(a, b, c), 2);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the argument sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first sequence</typeparam>
        /// <typeparam name="T2">Type of elements in second sequence</typeparam>
        /// <typeparam name="T3">Type of elements in third sequence</typeparam>
        /// <typeparam name="T4">Type of elements in fourth sequence</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence</typeparam>
        /// <param name="first">The first sequence.</param>
        /// <param name="second">The second sequence.</param>
        /// <param name="third">The third sequence.</param>
        /// <param name="fourth">The fourth sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each quadruplet of elements.</param>
        /// <returns>
        /// A sequence that contains elements of the four input sequences,
        /// combined by <paramref name="resultSelector"/>.
        /// </returns>
        /// <example>
        /// <code><![CDATA[
        /// var numbers = new[] { 1, 2, 3 };
        /// var letters = new[] { "A", "B", "C", "D" };
        /// var chars   = new[] { 'a', 'b', 'c', 'd', 'e' };
        /// var flags   = new[] { true, false, true, false, true, false };
        /// var zipped  = numbers.ZipLongest(letters, chars, flags, (n, l, c, f) => n + l + c + f);
        /// ]]></code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1AaTrue",
        /// "2BbFalse", "3CcTrue", "0DdFalse", "0eTrue", "0\0False" in turn.
        /// </example>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult> ZipLongest<T1, T2, T3, T4, TResult>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            IEnumerable<T4> fourth,
            Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (third == null) throw new ArgumentNullException(nameof(third));
            if (fourth == null) throw new ArgumentNullException(nameof(fourth));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return ZipImpl(first, second, third, fourth, resultSelector, 3);
        }
    }
}
