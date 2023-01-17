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

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the argument sequences. An exception is thrown
        /// if the input sequences are of different lengths.
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
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first"/>, <paramref name="second"/>, or <paramref
        /// name="resultSelector"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code><![CDATA[
        /// var numbers = new[] { 1, 2, 3, 4 };
        /// var letters = new[] { "A", "B", "C", "D" };
        /// var zipped  = numbers.EquiZip(letters, (n, l) => n + l);
        /// ]]></code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A",
        /// "2B", "3C", "4D" in turn.
        /// </example>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult> EquiZip<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return EquiZipImpl<TFirst, TSecond, object, object, TResult>(first, second, null, null, (a, b, _, _) => resultSelector(a, b));
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the argument sequences. An exception is thrown
        /// if the input sequences are of different lengths.
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
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref
        /// name="third"/>, or <paramref name="resultSelector"/> is <see
        /// langword="null"/>.
        /// </exception>
        /// <example>
        /// <code><![CDATA[
        /// var numbers = new[] { 1, 2, 3, 4 };
        /// var letters = new[] { "A", "B", "C", "D" };
        /// var chars   = new[] { 'a', 'b', 'c', 'd' };
        /// var zipped  = numbers.EquiZip(letters, chars, (n, l, c) => n + l + c);
        /// ]]></code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1Aa",
        /// "2Bb", "3Cc", "4Dd" in turn.
        /// </example>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult> EquiZip<T1, T2, T3, TResult>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second, IEnumerable<T3> third,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (third == null) throw new ArgumentNullException(nameof(third));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return EquiZipImpl<T1, T2, T3, object, TResult>(first, second, third, null, (a, b, c, _) => resultSelector(a, b, c));
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the argument sequences. An exception is thrown
        /// if the input sequences are of different lengths.
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
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first"/>, <paramref name="second"/>, <paramref
        /// name="third"/>, <paramref name="fourth"/>, or <paramref
        /// name="resultSelector"/> is <see langword="null"/>.
        /// </exception>
        /// <example>
        /// <code><![CDATA[
        /// var numbers = new[] { 1, 2, 3, 4 };
        /// var letters = new[] { "A", "B", "C", "D" };
        /// var chars   = new[] { 'a', 'b', 'c', 'd' };
        /// var flags   = new[] { true, false, true, false };
        /// var zipped = numbers.EquiZip(letters, chars, flags, (n, l, c, f) => n + l + c + f);
        /// ]]></code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1AaTrue",
        /// "2BbFalse", "3CcTrue", "4DdFalse" in turn.
        /// </example>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult> EquiZip<T1, T2, T3, T4, TResult>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second, IEnumerable<T3> third, IEnumerable<T4> fourth,
            Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (third == null) throw new ArgumentNullException(nameof(third));
            if (fourth == null) throw new ArgumentNullException(nameof(fourth));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return EquiZipImpl(first, second, third, fourth, resultSelector);
        }

        /// <summary>
        /// Returns tuples, where each tuple contains the N-th
        /// element from each of the argument sequences. An exception is thrown
        /// if the input sequences are of different lengths.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first sequence</typeparam>
        /// <typeparam name="T2">Type of elements in second sequence</typeparam>
        /// <param name="first">The first sequence.</param>
        /// <param name="second">The second sequence.</param>
        /// <returns>
        /// A sequence of tuples that contains elements of the two input sequences.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <example>
        /// <code><![CDATA[
        /// var numbers = new[] { 1, 2, 3, 4 };
        /// var letters = new[] { "A", "B", "C", "D" };
        /// var zipped = numbers.EquiZip(letters);
        /// ]]></code>
        /// The <c>zipped</c> variable, when iterated over, will yield the tuples : (1, A),
        /// (2, B), (3, C), (4, D) in turn.
        /// </example>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<(T1, T2)> EquiZip<T1, T2>(this IEnumerable<T1> first, IEnumerable<T2> second)
        {
            return first.EquiZip(second, ValueTuple.Create);
        }

        /// <summary>
        /// Returns tuples, where each tuple contains the N-th
        /// element from each of the argument sequences. An exception is thrown
        /// if the input sequences are of different lengths.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first sequence</typeparam>
        /// <typeparam name="T2">Type of elements in second sequence</typeparam>
        /// <typeparam name="T3">Type of elements in third sequence</typeparam>
        /// <param name="first">The first sequence.</param>
        /// <param name="second">The second sequence.</param>
        /// <param name="third">The third sequence.</param>
        /// <returns>
        /// A sequence of tuples that contains elements of the three input sequences.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <example>
        /// <code><![CDATA[
        /// var numbers = new[] { 1, 2, 3, 4 };
        /// var letters = new[] { "A", "B", "C", "D" };
        /// var chars   = new[] { 'a', 'b', 'c', 'd' };
        /// var zipped = numbers.EquiZip(letters, chars);
        /// ]]></code>
        /// The <c>zipped</c> variable, when iterated over, will yield the tuples : (1, A, a),
        /// (2, B, b), (3, C, c), (4, D, d) in turn.
        /// </example>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<(T1, T2, T3)> EquiZip<T1, T2, T3>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second, IEnumerable<T3> third)
        {
            return first.EquiZip(second, third, ValueTuple.Create);
        }

        /// <summary>
        /// Returns tuples, where each tuple contains the N-th
        /// element from each of the argument sequences. An exception is thrown
        /// if the input sequences are of different lengths.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first sequence</typeparam>
        /// <typeparam name="T2">Type of elements in second sequence</typeparam>
        /// <typeparam name="T3">Type of elements in third sequence</typeparam>
        /// <typeparam name="T4">Type of elements in fourth sequence</typeparam>
        /// <param name="first">The first sequence.</param>
        /// <param name="second">The second sequence.</param>
        /// <param name="third">The third sequence.</param>
        /// <param name="fourth">The fourth sequence.</param>
        /// <returns>
        /// A sequence of tuples that contains elements of the four input sequences.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <example>
        /// <code><![CDATA[
        /// var numbers = new[] { 1, 2, 3, 4 };
        /// var letters = new[] { "A", "B", "C", "D" };
        /// var chars   = new[] { 'a', 'b', 'c', 'd' };
        /// var flags   = new[] { true, false, true, false };
        /// var zipped = numbers.EquiZip(letters, chars, flags);
        /// ]]></code>
        /// The <c>zipped</c> variable, when iterated over, will yield the tuples : (1, A, a, True),
        /// (2, B, b, False), (3, C, c, True), (4, D, d, False) in turn.
        /// </example>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<(T1, T2, T3, T4)> EquiZip<T1, T2, T3, T4>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second, IEnumerable<T3> third, IEnumerable<T4> fourth)
        {
            return first.EquiZip(second, third, fourth, ValueTuple.Create);
        }

        static IEnumerable<TResult> EquiZipImpl<T1, T2, T3, T4, TResult>(
            IEnumerable<T1>  s1,
            IEnumerable<T2>  s2,
            IEnumerable<T3>? s3,
            IEnumerable<T4>? s4,
            Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            const int zero = 0, one = 1;

            var limit = 1 + (s3 != null ? one : zero)
                          + (s4 != null ? one : zero);

            return ZipImpl(s1, s2, s3, s4, resultSelector, limit, enumerators =>
            {
                var i = enumerators.Index().First(x => x.Value == null).Key;
                return new InvalidOperationException(OrdinalNumbers[i] + " sequence too short.");
            });
        }

        static readonly string[] OrdinalNumbers =
        {
            "First",
            "Second",
            "Third",
            "Fourth",
            // "Fifth",
            // "Sixth",
            // "Seventh",
            // "Eighth",
            // "Ninth",
            // "Tenth",
            // "Eleventh",
            // "Twelfth",
            // "Thirteenth",
            // "Fourteenth",
            // "Fifteenth",
            // "Sixteenth",
        };
    }
}
