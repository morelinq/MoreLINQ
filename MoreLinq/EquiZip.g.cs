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
        /// Returns a sequence of projections, each projection is build from two elements.
        /// For the N-th projection, these two elements are those located
        /// at the N-th position of the two input sequences.</para>
        /// <para>
        /// The resulting sequence has the same length as the input sequences.
        /// If the input sequences are of different lengths, an exception is thrown.</para>
        /// </summary>
        /// <typeparam name="T1">Type of elements in the first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in the second input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="resultSelector">
        /// The function that make projections of two elements.</param>
        /// <returns>
        /// A sequence of projections built from two elements,
        /// each element coming from one of the two input sequences.</returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.</exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.</remarks>

        public static IEnumerable<TResult> EquiZip<T1, T2, TResult>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            Func<T1, T2, TResult> resultSelector)
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

                throw new InvalidOperationException("Sequences differ in length.");
            }
        }

        /// <summary>
        /// <para>
        /// Returns a sequence of projections, each projection is build from three elements.
        /// For the N-th projection, these three elements are those located
        /// at the N-th position of the three input sequences.</para>
        /// <para>
        /// The resulting sequence has the same length as the input sequences.
        /// If the input sequences are of different lengths, an exception is thrown.</para>
        /// </summary>
        /// <typeparam name="T1">Type of elements in the first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in the second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in the third input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="third">The third source sequence.</param>
        /// <param name="resultSelector">
        /// The function that make projections of three elements.</param>
        /// <returns>
        /// A sequence of projections built from three elements,
        /// each element coming from one of the three input sequences.</returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.</exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.</remarks>

        public static IEnumerable<TResult> EquiZip<T1, T2, T3, TResult>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            Func<T1, T2, T3, TResult> resultSelector)
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

                throw new InvalidOperationException("Sequences differ in length.");
            }
        }

        /// <summary>
        /// <para>
        /// Returns a sequence of projections, each projection is build from four elements.
        /// For the N-th projection, these four elements are those located
        /// at the N-th position of the four input sequences.</para>
        /// <para>
        /// The resulting sequence has the same length as the input sequences.
        /// If the input sequences are of different lengths, an exception is thrown.</para>
        /// </summary>
        /// <typeparam name="T1">Type of elements in the first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in the second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in the third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in the fourth input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in the returned sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="third">The third source sequence.</param>
        /// <param name="fourth">The fourth source sequence.</param>
        /// <param name="resultSelector">
        /// The function that make projections of four elements.</param>
        /// <returns>
        /// A sequence of projections built from four elements,
        /// each element coming from one of the four input sequences.</returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.</exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.</remarks>

        public static IEnumerable<TResult> EquiZip<T1, T2, T3, T4, TResult>(
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

                throw new InvalidOperationException("Sequences differ in length.");
            }
        }

    }
}
