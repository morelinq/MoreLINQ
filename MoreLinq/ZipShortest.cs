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
        /// If the input sequences are of different lengths, the result sequence 
        /// is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// var numbers = new[] { 1, 2, 3 };
        /// var letters = new[] { "A", "B", "C", "D" };
        /// var chars   = new[] { 'a', 'b', 'c', 'd', 'e' };
        /// var zipped  = numbers.ZipShortest(letters, chars, (n, l, c) => c + n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield 
        /// "98A", "100B", "102C", in turn.
        /// </example>
        /// <typeparam name="T1">Type of elements in first sequence</typeparam>
        /// <typeparam name="T2">Type of elements in second sequence</typeparam>
        /// <typeparam name="T3">Type of elements in third sequence</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence</typeparam>
        /// <param name="first">First sequence</param>
        /// <param name="second">Second sequence</param>
        /// <param name="third">Third sequence</param>
        /// <param name="resultSelector">Function to apply to each triplet of elements</param>
        
        public static IEnumerable<TResult> ZipShortest<T1, T2, T3, TResult>(this IEnumerable<T1> first,
            IEnumerable<T2> second, IEnumerable<T3> third, Func<T1, T2, T3, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (third == null) throw new ArgumentNullException("third");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            return ZipImpl<T1, T2, T3, object, TResult>(first, second, third, null, (a, b, c, _) => resultSelector(a, b, c));
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th element 
        /// from each of the argument sequences.
        /// </summary>
        /// <remarks>
        /// If the input sequences are of different lengths, the result sequence 
        /// is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// var numbers = new[] { 1, 2, 3 };
        /// var letters = new[] { "A", "B", "C", "D" };
        /// var chars   = new[] { 'a', 'b', 'c', 'd', 'e' };
        /// var flags   = new[] { true, false };
        /// var zipped  = numbers.ZipShortest(letters, chars, flags (n, l, c, f) => n + l + c + f);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield 
        /// "1AaTrue", "2BbFalse" in turn.
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
        
        public static IEnumerable<TResult> ZipShortest<T1, T2, T3, T4, TResult>(this IEnumerable<T1> first,
            IEnumerable<T2> second, IEnumerable<T3> third, IEnumerable<T4> fourth, Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (third == null) throw new ArgumentNullException("third");
            if (fourth == null) throw new ArgumentNullException("fourth");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            return ZipImpl(first, second, third, fourth, resultSelector);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th element 
        /// from each of the argument sequences.
        /// </summary>
        /// <remarks>
        /// If the two input sequences are of different lengths, the result sequence 
        /// is terminated as soon as the shortest input sequence is exhausted.
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code>
        /// var numbers = new[] { 1, 2, 3 };
        /// var letters = new[] { "A", "B", "C", "D" };
        /// var zipped = numbers.ZipShortest(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
        /// <typeparam name="TFirst">Type of elements in first sequence</typeparam>
        /// <typeparam name="TSecond">Type of elements in second sequence</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence</typeparam>
        /// <param name="first">First sequence</param>
        /// <param name="second">Second sequence</param>
        /// <param name="resultSelector">Function to apply to each pair of elements</param>
        
        public static IEnumerable<TResult> ZipShortest<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (resultSelector == null) throw new ArgumentNullException("resultSelector");

            return ZipImpl<TFirst, TSecond, object, object, TResult>(first, second, null, null, (a, b, c, d) => resultSelector(a, b));
        }

        static IEnumerable<TResult> ZipImpl<T1, T2, T3, T4, TResult>(
            IEnumerable<T1> s1, IEnumerable<T2> s2, 
            IEnumerable<T3> s3, IEnumerable<T4> s4, 
            Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            using (var e1 = s1.GetEnumerator())
            using (var e2 = s2.GetEnumerator())
            using (var e3 = s3 != null ? s3.GetEnumerator() : null)
            using (var e4 = s4 != null ? s4.GetEnumerator() : null)
            {
                while (e1.MoveNext())
                {
                    if (e2.MoveNext() && (e3 == null || e3.MoveNext())
                                      && (e4 == null || e4.MoveNext()))
                    { 
                        yield return resultSelector(e1.Current, e2.Current,
                            e3 != null ? e3.Current : default(T3),
                            e4 != null ? e4.Current : default(T4));
                    }
                }
            }
        }
    }
}
