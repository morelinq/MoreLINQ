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
        /// Returns a projection of tuples, where each tuple contains the N-th element 
        /// from each of the argument sequences.
        /// </summary>
        /// <remarks>
        /// If the two input sequences are of different lengths then 
        /// <see cref="InvalidOperationException"/> is thrown.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3, 4 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// var zipped = numbers.EquiZip(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", "4D" in turn.
        /// </example>
        /// <typeparam name="TFirst">Type of elements in first sequence</typeparam>
        /// <typeparam name="TSecond">Type of elements in second sequence</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence</typeparam>
        /// <param name="first">First sequence</param>
        /// <param name="second">Second sequence</param>
        /// <param name="resultSelector">Function to apply to each pair of elements</param>
        
        public static IEnumerable<TResult> EquiZip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first,
             IEnumerable<TSecond> second,
             Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            return EquiZipImpl<TFirst, TSecond, object, object, TResult>(first, second, null, null, (a, b, c, d) => resultSelector(a, b));
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th element 
        /// from each of the argument sequences.
        /// </summary>
        /// <remarks>
        /// If the three input sequences are of different lengths then 
        /// <see cref="InvalidOperationException"/> is thrown.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// var numbers = { 1, 2, 3, 4 };
        /// var letters = { "A", "B", "C", "D" };
        /// var chars    = { 'a', 'b', 'c', 'd' };
        /// var zipped = numbers.EquiZip(letters, chars, (n, l, c) => n + l + c);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1Aa", "2Bb", "3Cc", "4Dd" in turn.
        /// </example>
        /// <typeparam name="T1">Type of elements in first sequence</typeparam>
        /// <typeparam name="T2">Type of elements in second sequence</typeparam>
        /// <typeparam name="T3">Type of elements in third sequence</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence</typeparam>
        /// <param name="first">First sequence</param>
        /// <param name="second">Second sequence</param>
        /// <param name="third">Third sequence</param>
        /// <param name="resultSelector">Function to apply to each triplet of elements</param>

        public static IEnumerable<TResult> EquiZip<T1, T2, T3, TResult>(this IEnumerable<T1> first,
             IEnumerable<T2> second, IEnumerable<T3> third,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (third == null) throw new ArgumentNullException("third");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            return EquiZipImpl<T1, T2, T3, object, TResult>(first, second, third, null, (a, b, c, _) => resultSelector(a, b, c));
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th element 
        /// from each of the argument sequences.
        /// </summary>
        /// <remarks>
        /// If the three input sequences are of different lengths then 
        /// <see cref="InvalidOperationException"/> is thrown.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// var numbers = { 1, 2, 3, 4 };
        /// var letters = { "A", "B", "C", "D" };
        /// var chars   = { 'a', 'b', 'c', 'd' };
        /// var flags   = { true, false, true, false };
        /// var zipped = numbers.EquiZip(letters, chars, flags, (n, l, c, f) => n + l + c + f);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1AaTrue", "2BbFalse", "3CcTrue", "4DdFalse" in turn.
        /// </example>
        /// <typeparam name="T1">Type of elements in first sequence</typeparam>
        /// <typeparam name="T2">Type of elements in second sequence</typeparam>
        /// <typeparam name="T3">Type of elements in third sequence</typeparam>
        /// <typeparam name="T4">Type of elements in fourth sequence</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence</typeparam>
        /// <param name="first">First sequence</param>
        /// <param name="second">Second sequence</param>
        /// <param name="third">Third sequence</param>
        /// <param name="fourth">Fourth sequence</param>
        /// <param name="resultSelector">Function to apply to each quadruplet of elements</param>

        public static IEnumerable<TResult> EquiZip<T1, T2, T3, T4, TResult>(this IEnumerable<T1> first,
             IEnumerable<T2> second, IEnumerable<T3> third, IEnumerable<T4> fourth, 
            Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (third == null) throw new ArgumentNullException("third");
            if (third == null) throw new ArgumentNullException("fourth");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            return EquiZipImpl(first, second, third, fourth, resultSelector);
        }

        static IEnumerable<TResult> EquiZipImpl<T1, T2, T3, T4, TResult>(
            IEnumerable<T1> first,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            IEnumerable<T4> fourth,
            Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            using (var e1 = first.GetEnumerator())
            using (var e2 = second.GetEnumerator())
            using (var e3 = third  != null ? third.GetEnumerator()  : null)
            using (var e4 = fourth != null ? fourth.GetEnumerator() : null)
            {
                while (e1.MoveNext())
                {
                    bool m2, m3 = false;
                    if ((m2 = e2.MoveNext()) && (m3 = (e3 == null || e3.MoveNext()))
                                             && ((e4 == null || e4.MoveNext())))
                    {
                        yield return resultSelector(e1.Current, e2.Current,
                                                    e3 != null ? e3.Current : default(T3),
                                                    e4 != null ? e4.Current : default(T4));
                    }
                    else
                    {
                        var message = string.Format("{0} sequence too short.", !m2 ? "Second" : !m3 ? "Third" : "Fourth");
                        throw new InvalidOperationException(message);
                    }
                }
                if (e2.MoveNext() || (e3 != null && e3.MoveNext())
                                  || (e4 != null && e4.MoveNext()))
                {
                    throw new InvalidOperationException("First sequence too short.");
                }
            }
        }
    }
}
