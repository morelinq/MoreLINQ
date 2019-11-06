#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Pierre Lando. All rights reserved.
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
        /// element from each of the input sequences. An exception is thrown
        /// if the input sequences are of different lengths.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

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

                throw new InvalidOperationException($"Sequences differ in length.");
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. An exception is thrown
        /// if the input sequences are of different lengths.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<(T1, T2)> EquiZip<T1, T2>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second)
        {
            return EquiZip(
                first,
                second,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. An exception is thrown
        /// if the input sequences are of different lengths.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="third">The third source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

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

                throw new InvalidOperationException($"Sequences differ in length.");
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. An exception is thrown
        /// if the input sequences are of different lengths.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="third">The third source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<(T1, T2, T3)> EquiZip<T1, T2, T3>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            IEnumerable<T3> third)
        {
            return EquiZip(
                first,
                second,
                third,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. An exception is thrown
        /// if the input sequences are of different lengths.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="third">The third source sequence.</param>
        /// <param name="fourth">The fourth source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

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

                throw new InvalidOperationException($"Sequences differ in length.");
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. An exception is thrown
        /// if the input sequences are of different lengths.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="third">The third source sequence.</param>
        /// <param name="fourth">The fourth source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<(T1, T2, T3, T4)> EquiZip<T1, T2, T3, T4>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            IEnumerable<T4> fourth)
        {
            return EquiZip(
                first,
                second,
                third,
                fourth,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. An exception is thrown
        /// if the input sequences are of different lengths.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="third">The third source sequence.</param>
        /// <param name="fourth">The fourth source sequence.</param>
        /// <param name="fifth">The fifth source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult> EquiZip<T1, T2, T3, T4, T5, TResult>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            IEnumerable<T4> fourth,
            IEnumerable<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (third == null) throw new ArgumentNullException(nameof(third));
            if (fourth == null) throw new ArgumentNullException(nameof(fourth));
            if (fifth == null) throw new ArgumentNullException(nameof(fifth));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = first.GetEnumerator();
                using var e2 = second.GetEnumerator();
                using var e3 = third.GetEnumerator();
                using var e4 = fourth.GetEnumerator();
                using var e5 = fifth.GetEnumerator();

                for (;;)
                {
                    if (e1.MoveNext())
                    {
                        if (e2.MoveNext() && e3.MoveNext() && e4.MoveNext() && e5.MoveNext())
                            yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current, e5.Current);
                        else
                            break;
                    }
                    else
                    {
                        if (e2.MoveNext() || e3.MoveNext() || e4.MoveNext() || e5.MoveNext())
                            break;
                        else
                            yield break;
                    }
                }

                throw new InvalidOperationException($"Sequences differ in length.");
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. An exception is thrown
        /// if the input sequences are of different lengths.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="third">The third source sequence.</param>
        /// <param name="fourth">The fourth source sequence.</param>
        /// <param name="fifth">The fifth source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<(T1, T2, T3, T4, T5)> EquiZip<T1, T2, T3, T4, T5>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            IEnumerable<T4> fourth,
            IEnumerable<T5> fifth)
        {
            return EquiZip(
                first,
                second,
                third,
                fourth,
                fifth,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. An exception is thrown
        /// if the input sequences are of different lengths.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="third">The third source sequence.</param>
        /// <param name="fourth">The fourth source sequence.</param>
        /// <param name="fifth">The fifth source sequence.</param>
        /// <param name="sixth">The sixth source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult> EquiZip<T1, T2, T3, T4, T5, T6, TResult>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            IEnumerable<T4> fourth,
            IEnumerable<T5> fifth,
            IEnumerable<T6> sixth,
            Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (third == null) throw new ArgumentNullException(nameof(third));
            if (fourth == null) throw new ArgumentNullException(nameof(fourth));
            if (fifth == null) throw new ArgumentNullException(nameof(fifth));
            if (sixth == null) throw new ArgumentNullException(nameof(sixth));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = first.GetEnumerator();
                using var e2 = second.GetEnumerator();
                using var e3 = third.GetEnumerator();
                using var e4 = fourth.GetEnumerator();
                using var e5 = fifth.GetEnumerator();
                using var e6 = sixth.GetEnumerator();

                for (;;)
                {
                    if (e1.MoveNext())
                    {
                        if (e2.MoveNext() && e3.MoveNext() && e4.MoveNext() && e5.MoveNext() && e6.MoveNext())
                            yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current, e5.Current, e6.Current);
                        else
                            break;
                    }
                    else
                    {
                        if (e2.MoveNext() || e3.MoveNext() || e4.MoveNext() || e5.MoveNext() || e6.MoveNext())
                            break;
                        else
                            yield break;
                    }
                }

                throw new InvalidOperationException($"Sequences differ in length.");
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. An exception is thrown
        /// if the input sequences are of different lengths.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="third">The third source sequence.</param>
        /// <param name="fourth">The fourth source sequence.</param>
        /// <param name="fifth">The fifth source sequence.</param>
        /// <param name="sixth">The sixth source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<(T1, T2, T3, T4, T5, T6)> EquiZip<T1, T2, T3, T4, T5, T6>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            IEnumerable<T4> fourth,
            IEnumerable<T5> fifth,
            IEnumerable<T6> sixth)
        {
            return EquiZip(
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. An exception is thrown
        /// if the input sequences are of different lengths.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <typeparam name="T7">Type of elements in seventh input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="third">The third source sequence.</param>
        /// <param name="fourth">The fourth source sequence.</param>
        /// <param name="fifth">The fifth source sequence.</param>
        /// <param name="sixth">The sixth source sequence.</param>
        /// <param name="seventh">The seventh source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult> EquiZip<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            IEnumerable<T4> fourth,
            IEnumerable<T5> fifth,
            IEnumerable<T6> sixth,
            IEnumerable<T7> seventh,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (third == null) throw new ArgumentNullException(nameof(third));
            if (fourth == null) throw new ArgumentNullException(nameof(fourth));
            if (fifth == null) throw new ArgumentNullException(nameof(fifth));
            if (sixth == null) throw new ArgumentNullException(nameof(sixth));
            if (seventh == null) throw new ArgumentNullException(nameof(seventh));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = first.GetEnumerator();
                using var e2 = second.GetEnumerator();
                using var e3 = third.GetEnumerator();
                using var e4 = fourth.GetEnumerator();
                using var e5 = fifth.GetEnumerator();
                using var e6 = sixth.GetEnumerator();
                using var e7 = seventh.GetEnumerator();

                for (;;)
                {
                    if (e1.MoveNext())
                    {
                        if (e2.MoveNext() && e3.MoveNext() && e4.MoveNext() && e5.MoveNext() && e6.MoveNext() && e7.MoveNext())
                            yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current, e5.Current, e6.Current, e7.Current);
                        else
                            break;
                    }
                    else
                    {
                        if (e2.MoveNext() || e3.MoveNext() || e4.MoveNext() || e5.MoveNext() || e6.MoveNext() || e7.MoveNext())
                            break;
                        else
                            yield break;
                    }
                }

                throw new InvalidOperationException($"Sequences differ in length.");
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. An exception is thrown
        /// if the input sequences are of different lengths.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <typeparam name="T7">Type of elements in seventh input sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="third">The third source sequence.</param>
        /// <param name="fourth">The fourth source sequence.</param>
        /// <param name="fifth">The fifth source sequence.</param>
        /// <param name="sixth">The sixth source sequence.</param>
        /// <param name="seventh">The seventh source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> EquiZip<T1, T2, T3, T4, T5, T6, T7>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            IEnumerable<T4> fourth,
            IEnumerable<T5> fifth,
            IEnumerable<T6> sixth,
            IEnumerable<T7> seventh)
        {
            return EquiZip(
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                seventh,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. An exception is thrown
        /// if the input sequences are of different lengths.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <typeparam name="T7">Type of elements in seventh input sequence.</typeparam>
        /// <typeparam name="T8">Type of elements in eighth input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="third">The third source sequence.</param>
        /// <param name="fourth">The fourth source sequence.</param>
        /// <param name="fifth">The fifth source sequence.</param>
        /// <param name="sixth">The sixth source sequence.</param>
        /// <param name="seventh">The seventh source sequence.</param>
        /// <param name="eighth">The eighth source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult> EquiZip<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            IEnumerable<T4> fourth,
            IEnumerable<T5> fifth,
            IEnumerable<T6> sixth,
            IEnumerable<T7> seventh,
            IEnumerable<T8> eighth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (third == null) throw new ArgumentNullException(nameof(third));
            if (fourth == null) throw new ArgumentNullException(nameof(fourth));
            if (fifth == null) throw new ArgumentNullException(nameof(fifth));
            if (sixth == null) throw new ArgumentNullException(nameof(sixth));
            if (seventh == null) throw new ArgumentNullException(nameof(seventh));
            if (eighth == null) throw new ArgumentNullException(nameof(eighth));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = first.GetEnumerator();
                using var e2 = second.GetEnumerator();
                using var e3 = third.GetEnumerator();
                using var e4 = fourth.GetEnumerator();
                using var e5 = fifth.GetEnumerator();
                using var e6 = sixth.GetEnumerator();
                using var e7 = seventh.GetEnumerator();
                using var e8 = eighth.GetEnumerator();

                for (;;)
                {
                    if (e1.MoveNext())
                    {
                        if (e2.MoveNext() && e3.MoveNext() && e4.MoveNext() && e5.MoveNext() && e6.MoveNext() && e7.MoveNext() && e8.MoveNext())
                            yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current, e5.Current, e6.Current, e7.Current, e8.Current);
                        else
                            break;
                    }
                    else
                    {
                        if (e2.MoveNext() || e3.MoveNext() || e4.MoveNext() || e5.MoveNext() || e6.MoveNext() || e7.MoveNext() || e8.MoveNext())
                            break;
                        else
                            yield break;
                    }
                }

                throw new InvalidOperationException($"Sequences differ in length.");
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. An exception is thrown
        /// if the input sequences are of different lengths.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <typeparam name="T7">Type of elements in seventh input sequence.</typeparam>
        /// <typeparam name="T8">Type of elements in eighth input sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="third">The third source sequence.</param>
        /// <param name="fourth">The fourth source sequence.</param>
        /// <param name="fifth">The fifth source sequence.</param>
        /// <param name="sixth">The sixth source sequence.</param>
        /// <param name="seventh">The seventh source sequence.</param>
        /// <param name="eighth">The eighth source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <exception cref="InvalidOperationException">
        /// The input sequences are of different lengths.
        /// </exception>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> EquiZip<T1, T2, T3, T4, T5, T6, T7, T8>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            IEnumerable<T4> fourth,
            IEnumerable<T5> fifth,
            IEnumerable<T6> sixth,
            IEnumerable<T7> seventh,
            IEnumerable<T8> eighth)
        {
            return EquiZip(
                first,
                second,
                third,
                fourth,
                fifth,
                sixth,
                seventh,
                eighth,
                ValueTuple.Create);
        }

    }
}
