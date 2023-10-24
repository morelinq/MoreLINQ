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
        /// <para>
        /// Applies a specified function to the corresponding elements of two sequences,
        /// producing a sequence of the results.</para>
        /// <para>
        /// The resulting sequence has the same length as the input sequences.
        /// If the input sequences are of different lengths, an exception is thrown.</para>
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of the first input sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of the second input sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence.</typeparam>
        /// <param name="first">The first sequence to merge.</param>
        /// <param name="second">The second sequence to merge.</param>
        /// <param name="resultSelector">
        /// A function that specifies how to merge the elements from the two sequences.</param>
        /// <returns>
        /// An <code>IEnumerable</code> that contains merged elements of two input sequences.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first"/>,
        /// <paramref name="second"/> or
        /// <paramref name="resultSelector"/> is <code>null</code>.</exception>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.</exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.</remarks>

        public static IEnumerable<TResult> EquiZip<TFirst, TSecond, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            Func<TFirst, TSecond, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = first.GetEnumerator();
                using var e2 = second.GetEnumerator();

                for (;;)
                {
                    if (e1.MoveNext())
                    {
                        if (e2.MoveNext())
                            yield return resultSelector(e1.Current, e2.Current);
                        else
                            break;
                    }
                    else
                    {
                        if (e2.MoveNext())
                            break;
                        else
                            yield break;
                    }
                }

                throw new InvalidOperationException("The input sequences are of different lengths.");
            }
        }

        /// <summary>
        /// <para>
        /// Applies a specified function to the corresponding elements of three sequences,
        /// producing a sequence of the results.</para>
        /// <para>
        /// The resulting sequence has the same length as the input sequences.
        /// If the input sequences are of different lengths, an exception is thrown.</para>
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of the first input sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of the second input sequence.</typeparam>
        /// <typeparam name="TThird">The type of the elements of the third input sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence.</typeparam>
        /// <param name="first">The first sequence to merge.</param>
        /// <param name="second">The second sequence to merge.</param>
        /// <param name="third">The third sequence to merge.</param>
        /// <param name="resultSelector">
        /// A function that specifies how to merge the elements from the three sequences.</param>
        /// <returns>
        /// An <code>IEnumerable</code> that contains merged elements of three input sequences.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first"/>,
        /// <paramref name="second"/>,
        /// <paramref name="third"/> or
        /// <paramref name="resultSelector"/> is <code>null</code>.</exception>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.</exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.</remarks>

        public static IEnumerable<TResult> EquiZip<TFirst, TSecond, TThird, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            IEnumerable<TThird> third,
            Func<TFirst, TSecond, TThird, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (third == null) throw new ArgumentNullException(nameof(third));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = first.GetEnumerator();
                using var e2 = second.GetEnumerator();
                using var e3 = third.GetEnumerator();

                for (;;)
                {
                    if (e1.MoveNext())
                    {
                        if (e2.MoveNext() && e3.MoveNext())
                            yield return resultSelector(e1.Current, e2.Current, e3.Current);
                        else
                            break;
                    }
                    else
                    {
                        if (e2.MoveNext() || e3.MoveNext())
                            break;
                        else
                            yield break;
                    }
                }

                throw new InvalidOperationException("The input sequences are of different lengths.");
            }
        }

        /// <summary>
        /// <para>
        /// Applies a specified function to the corresponding elements of four sequences,
        /// producing a sequence of the results.</para>
        /// <para>
        /// The resulting sequence has the same length as the input sequences.
        /// If the input sequences are of different lengths, an exception is thrown.</para>
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of the first input sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of the second input sequence.</typeparam>
        /// <typeparam name="TThird">The type of the elements of the third input sequence.</typeparam>
        /// <typeparam name="TFourth">The type of the elements of the fourth input sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence.</typeparam>
        /// <param name="first">The first sequence to merge.</param>
        /// <param name="second">The second sequence to merge.</param>
        /// <param name="third">The third sequence to merge.</param>
        /// <param name="fourth">The fourth sequence to merge.</param>
        /// <param name="resultSelector">
        /// A function that specifies how to merge the elements from the four sequences.</param>
        /// <returns>
        /// An <code>IEnumerable</code> that contains merged elements of four input sequences.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first"/>,
        /// <paramref name="second"/>,
        /// <paramref name="third"/>,
        /// <paramref name="fourth"/> or
        /// <paramref name="resultSelector"/> is <code>null</code>.</exception>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.</exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.</remarks>

        public static IEnumerable<TResult> EquiZip<TFirst, TSecond, TThird, TFourth, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            IEnumerable<TThird> third,
            IEnumerable<TFourth> fourth,
            Func<TFirst, TSecond, TThird, TFourth, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (third == null) throw new ArgumentNullException(nameof(third));
            if (fourth == null) throw new ArgumentNullException(nameof(fourth));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = first.GetEnumerator();
                using var e2 = second.GetEnumerator();
                using var e3 = third.GetEnumerator();
                using var e4 = fourth.GetEnumerator();

                for (;;)
                {
                    if (e1.MoveNext())
                    {
                        if (e2.MoveNext() && e3.MoveNext() && e4.MoveNext())
                            yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current);
                        else
                            break;
                    }
                    else
                    {
                        if (e2.MoveNext() || e3.MoveNext() || e4.MoveNext())
                            break;
                        else
                            yield break;
                    }
                }

                throw new InvalidOperationException("The input sequences are of different lengths.");
            }
        }

    }
}
