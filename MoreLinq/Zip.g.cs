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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

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
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
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
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            Func<T1, T2, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = firstSource.GetEnumerator();
                using var e2 = secondSource.GetEnumerator();

                for (;;)
                {
                    if (e1.MoveNext())
                    {
                        if (e2.MoveNext())
                        {
                            yield return resultSelector(e1.Current, e2.Current);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (e2.MoveNext())
                        {
                            break;
                        }
                        else
                        {
                            yield break;
                        }
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
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
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
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource)
        {
            return EquiZip(
                firstSource,
                secondSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IEnumerable<TResult> ZipLongest<T1, T2, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            Func<T1, T2, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                IEnumerator<T1> e1 = null;
                IEnumerator<T2> e2 = null;

                try
                {
                    e1 = firstSource.GetEnumerator();
                    e2 = secondSource.GetEnumerator();

                    var v1 = default(T1);
                    var v2 = default(T2);

                    while (
                        ZipHelper.MoveNextOrDefault<T1>(ref e1, ref v1) +
                        ZipHelper.MoveNextOrDefault<T2>(ref e2, ref v2) > 0)
                    {
                        yield return resultSelector(v1, v2);
                    }
                }
                finally
                {
                    e1?.Dispose();
                    e2?.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IEnumerable<(T1, T2)> ZipLongest<T1, T2>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource)
        {
            return ZipLongest(
                firstSource,
                secondSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>

        public static IEnumerable<TResult> ZipShortest<T1, T2, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            Func<T1, T2, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = firstSource.GetEnumerator();
                using var e2 = secondSource.GetEnumerator();

                while (e1.MoveNext() && e2.MoveNext())
                {
                    yield return resultSelector(e1.Current, e2.Current);
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>

        public static IEnumerable<(T1, T2)> ZipShortest<T1, T2>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource)
        {
            return ZipShortest(
                firstSource,
                secondSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences.
        /// When the end of one or more input sequence is reached, on the next iteration, the given <paramref name="predicate"/>
        /// is called with in parameter the list of the 1-based indices of the source parameters that have not reached their end.
        /// If the call to the <paramref name="predicate"/> return <c>false</c> the zip enumeration stop.
        /// If the enumeration continues, the default value of each of the shorter sequence element types
        /// is used for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <param name="predicate">
        /// A function to test the end of the zip sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// The <paramref name="predicate"/> is not called when all the input sequence have reached their end.
        /// The <paramref name="predicate"/> is called at most one times.
        /// </remarks>
        public static IEnumerable<TResult> ZipWhile<T1, T2, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            Func<T1, T2, TResult> resultSelector,
            Func<IReadOnlyList<int>, bool> predicate)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return _(); IEnumerable<TResult> _()
            {
                IEnumerator<T1> e1 = null;
                IEnumerator<T2> e2 = null;

                try
                {
                    e1 = firstSource.GetEnumerator();
                    e2 = secondSource.GetEnumerator();

                    var v1 = default(T1);
                    var v2 = default(T2);

                    var activeSourceCount = 2;
                    for(;;)
                    {
                        var newActiveSourceCount =
                            ZipHelper.MoveNextOrDefault<T1>(ref e1, ref v1) +
                            ZipHelper.MoveNextOrDefault<T2>(ref e2, ref v2);

                        if (activeSourceCount != newActiveSourceCount)
                        {
                            if (!ZipHelper.ShouldContinue(predicate, e1, e2))
                            {
                                yield break;
                            }
                            activeSourceCount = newActiveSourceCount;
                        }

                        yield return resultSelector(v1, v2);
                    }
                }
                finally
                {
                    e1?.Dispose();
                    e2?.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences.
        /// When the end of one or more input sequence is reached, on the next iteration, the given <paramref name="predicate"/>
        /// is called with in parameter the list of the 1-based indices of the source parameters that have not reached their end.
        /// If the call to the <paramref name="predicate"/> return <c>false</c> the zip enumeration stop.
        /// If the enumeration continues, the default value of each of the shorter sequence element types
        /// is used for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="predicate">
        /// A function to test the end of the zip sequence.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// The <paramref name="predicate"/> is not called when all the input sequence have reached their end.
        /// The <paramref name="predicate"/> is called at most one times.
        /// </remarks>
        public static IEnumerable<(T1, T2)> ZipWhile<T1, T2>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            Func<IReadOnlyList<int>, bool> predicate)
        {
            return ZipWhile(
                firstSource,
                secondSource,
                ValueTuple.Create,
                predicate);
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
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
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
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = firstSource.GetEnumerator();
                using var e2 = secondSource.GetEnumerator();
                using var e3 = thirdSource.GetEnumerator();

                for (;;)
                {
                    if (e1.MoveNext())
                    {
                        if (e2.MoveNext() && e3.MoveNext())
                        {
                            yield return resultSelector(e1.Current, e2.Current, e3.Current);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (e2.MoveNext() || e3.MoveNext())
                        {
                            break;
                        }
                        else
                        {
                            yield break;
                        }
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
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
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
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource)
        {
            return EquiZip(
                firstSource,
                secondSource,
                thirdSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IEnumerable<TResult> ZipLongest<T1, T2, T3, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                IEnumerator<T1> e1 = null;
                IEnumerator<T2> e2 = null;
                IEnumerator<T3> e3 = null;

                try
                {
                    e1 = firstSource.GetEnumerator();
                    e2 = secondSource.GetEnumerator();
                    e3 = thirdSource.GetEnumerator();

                    var v1 = default(T1);
                    var v2 = default(T2);
                    var v3 = default(T3);

                    while (
                        ZipHelper.MoveNextOrDefault<T1>(ref e1, ref v1) +
                        ZipHelper.MoveNextOrDefault<T2>(ref e2, ref v2) +
                        ZipHelper.MoveNextOrDefault<T3>(ref e3, ref v3) > 0)
                    {
                        yield return resultSelector(v1, v2, v3);
                    }
                }
                finally
                {
                    e1?.Dispose();
                    e2?.Dispose();
                    e3?.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IEnumerable<(T1, T2, T3)> ZipLongest<T1, T2, T3>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource)
        {
            return ZipLongest(
                firstSource,
                secondSource,
                thirdSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>

        public static IEnumerable<TResult> ZipShortest<T1, T2, T3, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = firstSource.GetEnumerator();
                using var e2 = secondSource.GetEnumerator();
                using var e3 = thirdSource.GetEnumerator();

                while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
                {
                    yield return resultSelector(e1.Current, e2.Current, e3.Current);
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>

        public static IEnumerable<(T1, T2, T3)> ZipShortest<T1, T2, T3>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource)
        {
            return ZipShortest(
                firstSource,
                secondSource,
                thirdSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences.
        /// When the end of one or more input sequence is reached, on the next iteration, the given <paramref name="predicate"/>
        /// is called with in parameter the list of the 1-based indices of the source parameters that have not reached their end.
        /// If the call to the <paramref name="predicate"/> return <c>false</c> the zip enumeration stop.
        /// If the enumeration continues, the default value of each of the shorter sequence element types
        /// is used for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <param name="predicate">
        /// A function to test the end of the zip sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// The <paramref name="predicate"/> is not called when all the input sequence have reached their end.
        /// The <paramref name="predicate"/> is called at most two times.
        /// </remarks>
        public static IEnumerable<TResult> ZipWhile<T1, T2, T3, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            Func<T1, T2, T3, TResult> resultSelector,
            Func<IReadOnlyList<int>, bool> predicate)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return _(); IEnumerable<TResult> _()
            {
                IEnumerator<T1> e1 = null;
                IEnumerator<T2> e2 = null;
                IEnumerator<T3> e3 = null;

                try
                {
                    e1 = firstSource.GetEnumerator();
                    e2 = secondSource.GetEnumerator();
                    e3 = thirdSource.GetEnumerator();

                    var v1 = default(T1);
                    var v2 = default(T2);
                    var v3 = default(T3);

                    var activeSourceCount = 3;
                    for(;;)
                    {
                        var newActiveSourceCount =
                            ZipHelper.MoveNextOrDefault<T1>(ref e1, ref v1) +
                            ZipHelper.MoveNextOrDefault<T2>(ref e2, ref v2) +
                            ZipHelper.MoveNextOrDefault<T3>(ref e3, ref v3);

                        if (activeSourceCount != newActiveSourceCount)
                        {
                            if (!ZipHelper.ShouldContinue(predicate, e1, e2, e3))
                            {
                                yield break;
                            }
                            activeSourceCount = newActiveSourceCount;
                        }

                        yield return resultSelector(v1, v2, v3);
                    }
                }
                finally
                {
                    e1?.Dispose();
                    e2?.Dispose();
                    e3?.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences.
        /// When the end of one or more input sequence is reached, on the next iteration, the given <paramref name="predicate"/>
        /// is called with in parameter the list of the 1-based indices of the source parameters that have not reached their end.
        /// If the call to the <paramref name="predicate"/> return <c>false</c> the zip enumeration stop.
        /// If the enumeration continues, the default value of each of the shorter sequence element types
        /// is used for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="predicate">
        /// A function to test the end of the zip sequence.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// The <paramref name="predicate"/> is not called when all the input sequence have reached their end.
        /// The <paramref name="predicate"/> is called at most two times.
        /// </remarks>
        public static IEnumerable<(T1, T2, T3)> ZipWhile<T1, T2, T3>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            Func<IReadOnlyList<int>, bool> predicate)
        {
            return ZipWhile(
                firstSource,
                secondSource,
                thirdSource,
                ValueTuple.Create,
                predicate);
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
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
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
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = firstSource.GetEnumerator();
                using var e2 = secondSource.GetEnumerator();
                using var e3 = thirdSource.GetEnumerator();
                using var e4 = fourthSource.GetEnumerator();

                for (;;)
                {
                    if (e1.MoveNext())
                    {
                        if (e2.MoveNext() && e3.MoveNext() && e4.MoveNext())
                        {
                            yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (e2.MoveNext() || e3.MoveNext() || e4.MoveNext())
                        {
                            break;
                        }
                        else
                        {
                            yield break;
                        }
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
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
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
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource)
        {
            return EquiZip(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IEnumerable<TResult> ZipLongest<T1, T2, T3, T4, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                IEnumerator<T1> e1 = null;
                IEnumerator<T2> e2 = null;
                IEnumerator<T3> e3 = null;
                IEnumerator<T4> e4 = null;

                try
                {
                    e1 = firstSource.GetEnumerator();
                    e2 = secondSource.GetEnumerator();
                    e3 = thirdSource.GetEnumerator();
                    e4 = fourthSource.GetEnumerator();

                    var v1 = default(T1);
                    var v2 = default(T2);
                    var v3 = default(T3);
                    var v4 = default(T4);

                    while (
                        ZipHelper.MoveNextOrDefault<T1>(ref e1, ref v1) +
                        ZipHelper.MoveNextOrDefault<T2>(ref e2, ref v2) +
                        ZipHelper.MoveNextOrDefault<T3>(ref e3, ref v3) +
                        ZipHelper.MoveNextOrDefault<T4>(ref e4, ref v4) > 0)
                    {
                        yield return resultSelector(v1, v2, v3, v4);
                    }
                }
                finally
                {
                    e1?.Dispose();
                    e2?.Dispose();
                    e3?.Dispose();
                    e4?.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IEnumerable<(T1, T2, T3, T4)> ZipLongest<T1, T2, T3, T4>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource)
        {
            return ZipLongest(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>

        public static IEnumerable<TResult> ZipShortest<T1, T2, T3, T4, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = firstSource.GetEnumerator();
                using var e2 = secondSource.GetEnumerator();
                using var e3 = thirdSource.GetEnumerator();
                using var e4 = fourthSource.GetEnumerator();

                while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext() && e4.MoveNext())
                {
                    yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current);
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>

        public static IEnumerable<(T1, T2, T3, T4)> ZipShortest<T1, T2, T3, T4>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource)
        {
            return ZipShortest(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences.
        /// When the end of one or more input sequence is reached, on the next iteration, the given <paramref name="predicate"/>
        /// is called with in parameter the list of the 1-based indices of the source parameters that have not reached their end.
        /// If the call to the <paramref name="predicate"/> return <c>false</c> the zip enumeration stop.
        /// If the enumeration continues, the default value of each of the shorter sequence element types
        /// is used for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <param name="predicate">
        /// A function to test the end of the zip sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// The <paramref name="predicate"/> is not called when all the input sequence have reached their end.
        /// The <paramref name="predicate"/> is called at most three times.
        /// </remarks>
        public static IEnumerable<TResult> ZipWhile<T1, T2, T3, T4, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            Func<T1, T2, T3, T4, TResult> resultSelector,
            Func<IReadOnlyList<int>, bool> predicate)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return _(); IEnumerable<TResult> _()
            {
                IEnumerator<T1> e1 = null;
                IEnumerator<T2> e2 = null;
                IEnumerator<T3> e3 = null;
                IEnumerator<T4> e4 = null;

                try
                {
                    e1 = firstSource.GetEnumerator();
                    e2 = secondSource.GetEnumerator();
                    e3 = thirdSource.GetEnumerator();
                    e4 = fourthSource.GetEnumerator();

                    var v1 = default(T1);
                    var v2 = default(T2);
                    var v3 = default(T3);
                    var v4 = default(T4);

                    var activeSourceCount = 4;
                    for(;;)
                    {
                        var newActiveSourceCount =
                            ZipHelper.MoveNextOrDefault<T1>(ref e1, ref v1) +
                            ZipHelper.MoveNextOrDefault<T2>(ref e2, ref v2) +
                            ZipHelper.MoveNextOrDefault<T3>(ref e3, ref v3) +
                            ZipHelper.MoveNextOrDefault<T4>(ref e4, ref v4);

                        if (activeSourceCount != newActiveSourceCount)
                        {
                            if (!ZipHelper.ShouldContinue(predicate, e1, e2, e3, e4))
                            {
                                yield break;
                            }
                            activeSourceCount = newActiveSourceCount;
                        }

                        yield return resultSelector(v1, v2, v3, v4);
                    }
                }
                finally
                {
                    e1?.Dispose();
                    e2?.Dispose();
                    e3?.Dispose();
                    e4?.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences.
        /// When the end of one or more input sequence is reached, on the next iteration, the given <paramref name="predicate"/>
        /// is called with in parameter the list of the 1-based indices of the source parameters that have not reached their end.
        /// If the call to the <paramref name="predicate"/> return <c>false</c> the zip enumeration stop.
        /// If the enumeration continues, the default value of each of the shorter sequence element types
        /// is used for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="predicate">
        /// A function to test the end of the zip sequence.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// The <paramref name="predicate"/> is not called when all the input sequence have reached their end.
        /// The <paramref name="predicate"/> is called at most three times.
        /// </remarks>
        public static IEnumerable<(T1, T2, T3, T4)> ZipWhile<T1, T2, T3, T4>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            Func<IReadOnlyList<int>, bool> predicate)
        {
            return ZipWhile(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                ValueTuple.Create,
                predicate);
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
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
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
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            Func<T1, T2, T3, T4, T5, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = firstSource.GetEnumerator();
                using var e2 = secondSource.GetEnumerator();
                using var e3 = thirdSource.GetEnumerator();
                using var e4 = fourthSource.GetEnumerator();
                using var e5 = fifthSource.GetEnumerator();

                for (;;)
                {
                    if (e1.MoveNext())
                    {
                        if (e2.MoveNext() && e3.MoveNext() && e4.MoveNext() && e5.MoveNext())
                        {
                            yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current, e5.Current);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (e2.MoveNext() || e3.MoveNext() || e4.MoveNext() || e5.MoveNext())
                        {
                            break;
                        }
                        else
                        {
                            yield break;
                        }
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
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
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
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource)
        {
            return EquiZip(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                fifthSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IEnumerable<TResult> ZipLongest<T1, T2, T3, T4, T5, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            Func<T1, T2, T3, T4, T5, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                IEnumerator<T1> e1 = null;
                IEnumerator<T2> e2 = null;
                IEnumerator<T3> e3 = null;
                IEnumerator<T4> e4 = null;
                IEnumerator<T5> e5 = null;

                try
                {
                    e1 = firstSource.GetEnumerator();
                    e2 = secondSource.GetEnumerator();
                    e3 = thirdSource.GetEnumerator();
                    e4 = fourthSource.GetEnumerator();
                    e5 = fifthSource.GetEnumerator();

                    var v1 = default(T1);
                    var v2 = default(T2);
                    var v3 = default(T3);
                    var v4 = default(T4);
                    var v5 = default(T5);

                    while (
                        ZipHelper.MoveNextOrDefault<T1>(ref e1, ref v1) +
                        ZipHelper.MoveNextOrDefault<T2>(ref e2, ref v2) +
                        ZipHelper.MoveNextOrDefault<T3>(ref e3, ref v3) +
                        ZipHelper.MoveNextOrDefault<T4>(ref e4, ref v4) +
                        ZipHelper.MoveNextOrDefault<T5>(ref e5, ref v5) > 0)
                    {
                        yield return resultSelector(v1, v2, v3, v4, v5);
                    }
                }
                finally
                {
                    e1?.Dispose();
                    e2?.Dispose();
                    e3?.Dispose();
                    e4?.Dispose();
                    e5?.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IEnumerable<(T1, T2, T3, T4, T5)> ZipLongest<T1, T2, T3, T4, T5>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource)
        {
            return ZipLongest(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                fifthSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>

        public static IEnumerable<TResult> ZipShortest<T1, T2, T3, T4, T5, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            Func<T1, T2, T3, T4, T5, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = firstSource.GetEnumerator();
                using var e2 = secondSource.GetEnumerator();
                using var e3 = thirdSource.GetEnumerator();
                using var e4 = fourthSource.GetEnumerator();
                using var e5 = fifthSource.GetEnumerator();

                while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext() && e4.MoveNext() && e5.MoveNext())
                {
                    yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current, e5.Current);
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>

        public static IEnumerable<(T1, T2, T3, T4, T5)> ZipShortest<T1, T2, T3, T4, T5>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource)
        {
            return ZipShortest(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                fifthSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences.
        /// When the end of one or more input sequence is reached, on the next iteration, the given <paramref name="predicate"/>
        /// is called with in parameter the list of the 1-based indices of the source parameters that have not reached their end.
        /// If the call to the <paramref name="predicate"/> return <c>false</c> the zip enumeration stop.
        /// If the enumeration continues, the default value of each of the shorter sequence element types
        /// is used for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <param name="predicate">
        /// A function to test the end of the zip sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// The <paramref name="predicate"/> is not called when all the input sequence have reached their end.
        /// The <paramref name="predicate"/> is called at most four times.
        /// </remarks>
        public static IEnumerable<TResult> ZipWhile<T1, T2, T3, T4, T5, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            Func<T1, T2, T3, T4, T5, TResult> resultSelector,
            Func<IReadOnlyList<int>, bool> predicate)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return _(); IEnumerable<TResult> _()
            {
                IEnumerator<T1> e1 = null;
                IEnumerator<T2> e2 = null;
                IEnumerator<T3> e3 = null;
                IEnumerator<T4> e4 = null;
                IEnumerator<T5> e5 = null;

                try
                {
                    e1 = firstSource.GetEnumerator();
                    e2 = secondSource.GetEnumerator();
                    e3 = thirdSource.GetEnumerator();
                    e4 = fourthSource.GetEnumerator();
                    e5 = fifthSource.GetEnumerator();

                    var v1 = default(T1);
                    var v2 = default(T2);
                    var v3 = default(T3);
                    var v4 = default(T4);
                    var v5 = default(T5);

                    var activeSourceCount = 5;
                    for(;;)
                    {
                        var newActiveSourceCount =
                            ZipHelper.MoveNextOrDefault<T1>(ref e1, ref v1) +
                            ZipHelper.MoveNextOrDefault<T2>(ref e2, ref v2) +
                            ZipHelper.MoveNextOrDefault<T3>(ref e3, ref v3) +
                            ZipHelper.MoveNextOrDefault<T4>(ref e4, ref v4) +
                            ZipHelper.MoveNextOrDefault<T5>(ref e5, ref v5);

                        if (activeSourceCount != newActiveSourceCount)
                        {
                            if (!ZipHelper.ShouldContinue(predicate, e1, e2, e3, e4, e5))
                            {
                                yield break;
                            }
                            activeSourceCount = newActiveSourceCount;
                        }

                        yield return resultSelector(v1, v2, v3, v4, v5);
                    }
                }
                finally
                {
                    e1?.Dispose();
                    e2?.Dispose();
                    e3?.Dispose();
                    e4?.Dispose();
                    e5?.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences.
        /// When the end of one or more input sequence is reached, on the next iteration, the given <paramref name="predicate"/>
        /// is called with in parameter the list of the 1-based indices of the source parameters that have not reached their end.
        /// If the call to the <paramref name="predicate"/> return <c>false</c> the zip enumeration stop.
        /// If the enumeration continues, the default value of each of the shorter sequence element types
        /// is used for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="predicate">
        /// A function to test the end of the zip sequence.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// The <paramref name="predicate"/> is not called when all the input sequence have reached their end.
        /// The <paramref name="predicate"/> is called at most four times.
        /// </remarks>
        public static IEnumerable<(T1, T2, T3, T4, T5)> ZipWhile<T1, T2, T3, T4, T5>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            Func<IReadOnlyList<int>, bool> predicate)
        {
            return ZipWhile(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                fifthSource,
                ValueTuple.Create,
                predicate);
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
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
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
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = firstSource.GetEnumerator();
                using var e2 = secondSource.GetEnumerator();
                using var e3 = thirdSource.GetEnumerator();
                using var e4 = fourthSource.GetEnumerator();
                using var e5 = fifthSource.GetEnumerator();
                using var e6 = sixthSource.GetEnumerator();

                for (;;)
                {
                    if (e1.MoveNext())
                    {
                        if (e2.MoveNext() && e3.MoveNext() && e4.MoveNext() && e5.MoveNext() && e6.MoveNext())
                        {
                            yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current, e5.Current, e6.Current);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (e2.MoveNext() || e3.MoveNext() || e4.MoveNext() || e5.MoveNext() || e6.MoveNext())
                        {
                            break;
                        }
                        else
                        {
                            yield break;
                        }
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
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
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
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource)
        {
            return EquiZip(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                fifthSource,
                sixthSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IEnumerable<TResult> ZipLongest<T1, T2, T3, T4, T5, T6, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                IEnumerator<T1> e1 = null;
                IEnumerator<T2> e2 = null;
                IEnumerator<T3> e3 = null;
                IEnumerator<T4> e4 = null;
                IEnumerator<T5> e5 = null;
                IEnumerator<T6> e6 = null;

                try
                {
                    e1 = firstSource.GetEnumerator();
                    e2 = secondSource.GetEnumerator();
                    e3 = thirdSource.GetEnumerator();
                    e4 = fourthSource.GetEnumerator();
                    e5 = fifthSource.GetEnumerator();
                    e6 = sixthSource.GetEnumerator();

                    var v1 = default(T1);
                    var v2 = default(T2);
                    var v3 = default(T3);
                    var v4 = default(T4);
                    var v5 = default(T5);
                    var v6 = default(T6);

                    while (
                        ZipHelper.MoveNextOrDefault<T1>(ref e1, ref v1) +
                        ZipHelper.MoveNextOrDefault<T2>(ref e2, ref v2) +
                        ZipHelper.MoveNextOrDefault<T3>(ref e3, ref v3) +
                        ZipHelper.MoveNextOrDefault<T4>(ref e4, ref v4) +
                        ZipHelper.MoveNextOrDefault<T5>(ref e5, ref v5) +
                        ZipHelper.MoveNextOrDefault<T6>(ref e6, ref v6) > 0)
                    {
                        yield return resultSelector(v1, v2, v3, v4, v5, v6);
                    }
                }
                finally
                {
                    e1?.Dispose();
                    e2?.Dispose();
                    e3?.Dispose();
                    e4?.Dispose();
                    e5?.Dispose();
                    e6?.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6)> ZipLongest<T1, T2, T3, T4, T5, T6>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource)
        {
            return ZipLongest(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                fifthSource,
                sixthSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>

        public static IEnumerable<TResult> ZipShortest<T1, T2, T3, T4, T5, T6, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = firstSource.GetEnumerator();
                using var e2 = secondSource.GetEnumerator();
                using var e3 = thirdSource.GetEnumerator();
                using var e4 = fourthSource.GetEnumerator();
                using var e5 = fifthSource.GetEnumerator();
                using var e6 = sixthSource.GetEnumerator();

                while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext() && e4.MoveNext() && e5.MoveNext() && e6.MoveNext())
                {
                    yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current, e5.Current, e6.Current);
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>

        public static IEnumerable<(T1, T2, T3, T4, T5, T6)> ZipShortest<T1, T2, T3, T4, T5, T6>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource)
        {
            return ZipShortest(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                fifthSource,
                sixthSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences.
        /// When the end of one or more input sequence is reached, on the next iteration, the given <paramref name="predicate"/>
        /// is called with in parameter the list of the 1-based indices of the source parameters that have not reached their end.
        /// If the call to the <paramref name="predicate"/> return <c>false</c> the zip enumeration stop.
        /// If the enumeration continues, the default value of each of the shorter sequence element types
        /// is used for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <param name="predicate">
        /// A function to test the end of the zip sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// The <paramref name="predicate"/> is not called when all the input sequence have reached their end.
        /// The <paramref name="predicate"/> is called at most five times.
        /// </remarks>
        public static IEnumerable<TResult> ZipWhile<T1, T2, T3, T4, T5, T6, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector,
            Func<IReadOnlyList<int>, bool> predicate)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return _(); IEnumerable<TResult> _()
            {
                IEnumerator<T1> e1 = null;
                IEnumerator<T2> e2 = null;
                IEnumerator<T3> e3 = null;
                IEnumerator<T4> e4 = null;
                IEnumerator<T5> e5 = null;
                IEnumerator<T6> e6 = null;

                try
                {
                    e1 = firstSource.GetEnumerator();
                    e2 = secondSource.GetEnumerator();
                    e3 = thirdSource.GetEnumerator();
                    e4 = fourthSource.GetEnumerator();
                    e5 = fifthSource.GetEnumerator();
                    e6 = sixthSource.GetEnumerator();

                    var v1 = default(T1);
                    var v2 = default(T2);
                    var v3 = default(T3);
                    var v4 = default(T4);
                    var v5 = default(T5);
                    var v6 = default(T6);

                    var activeSourceCount = 6;
                    for(;;)
                    {
                        var newActiveSourceCount =
                            ZipHelper.MoveNextOrDefault<T1>(ref e1, ref v1) +
                            ZipHelper.MoveNextOrDefault<T2>(ref e2, ref v2) +
                            ZipHelper.MoveNextOrDefault<T3>(ref e3, ref v3) +
                            ZipHelper.MoveNextOrDefault<T4>(ref e4, ref v4) +
                            ZipHelper.MoveNextOrDefault<T5>(ref e5, ref v5) +
                            ZipHelper.MoveNextOrDefault<T6>(ref e6, ref v6);

                        if (activeSourceCount != newActiveSourceCount)
                        {
                            if (!ZipHelper.ShouldContinue(predicate, e1, e2, e3, e4, e5, e6))
                            {
                                yield break;
                            }
                            activeSourceCount = newActiveSourceCount;
                        }

                        yield return resultSelector(v1, v2, v3, v4, v5, v6);
                    }
                }
                finally
                {
                    e1?.Dispose();
                    e2?.Dispose();
                    e3?.Dispose();
                    e4?.Dispose();
                    e5?.Dispose();
                    e6?.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences.
        /// When the end of one or more input sequence is reached, on the next iteration, the given <paramref name="predicate"/>
        /// is called with in parameter the list of the 1-based indices of the source parameters that have not reached their end.
        /// If the call to the <paramref name="predicate"/> return <c>false</c> the zip enumeration stop.
        /// If the enumeration continues, the default value of each of the shorter sequence element types
        /// is used for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="predicate">
        /// A function to test the end of the zip sequence.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// The <paramref name="predicate"/> is not called when all the input sequence have reached their end.
        /// The <paramref name="predicate"/> is called at most five times.
        /// </remarks>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6)> ZipWhile<T1, T2, T3, T4, T5, T6>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            Func<IReadOnlyList<int>, bool> predicate)
        {
            return ZipWhile(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                fifthSource,
                sixthSource,
                ValueTuple.Create,
                predicate);
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
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="seventhSource">The seventh source sequence.</param>
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
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            IEnumerable<T7> seventhSource,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (seventhSource == null) throw new ArgumentNullException(nameof(seventhSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = firstSource.GetEnumerator();
                using var e2 = secondSource.GetEnumerator();
                using var e3 = thirdSource.GetEnumerator();
                using var e4 = fourthSource.GetEnumerator();
                using var e5 = fifthSource.GetEnumerator();
                using var e6 = sixthSource.GetEnumerator();
                using var e7 = seventhSource.GetEnumerator();

                for (;;)
                {
                    if (e1.MoveNext())
                    {
                        if (e2.MoveNext() && e3.MoveNext() && e4.MoveNext() && e5.MoveNext() && e6.MoveNext() && e7.MoveNext())
                        {
                            yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current, e5.Current, e6.Current, e7.Current);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (e2.MoveNext() || e3.MoveNext() || e4.MoveNext() || e5.MoveNext() || e6.MoveNext() || e7.MoveNext())
                        {
                            break;
                        }
                        else
                        {
                            yield break;
                        }
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
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="seventhSource">The seventh source sequence.</param>
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
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            IEnumerable<T7> seventhSource)
        {
            return EquiZip(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                fifthSource,
                sixthSource,
                seventhSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <typeparam name="T7">Type of elements in seventh input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="seventhSource">The seventh source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IEnumerable<TResult> ZipLongest<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            IEnumerable<T7> seventhSource,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (seventhSource == null) throw new ArgumentNullException(nameof(seventhSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                IEnumerator<T1> e1 = null;
                IEnumerator<T2> e2 = null;
                IEnumerator<T3> e3 = null;
                IEnumerator<T4> e4 = null;
                IEnumerator<T5> e5 = null;
                IEnumerator<T6> e6 = null;
                IEnumerator<T7> e7 = null;

                try
                {
                    e1 = firstSource.GetEnumerator();
                    e2 = secondSource.GetEnumerator();
                    e3 = thirdSource.GetEnumerator();
                    e4 = fourthSource.GetEnumerator();
                    e5 = fifthSource.GetEnumerator();
                    e6 = sixthSource.GetEnumerator();
                    e7 = seventhSource.GetEnumerator();

                    var v1 = default(T1);
                    var v2 = default(T2);
                    var v3 = default(T3);
                    var v4 = default(T4);
                    var v5 = default(T5);
                    var v6 = default(T6);
                    var v7 = default(T7);

                    while (
                        ZipHelper.MoveNextOrDefault<T1>(ref e1, ref v1) +
                        ZipHelper.MoveNextOrDefault<T2>(ref e2, ref v2) +
                        ZipHelper.MoveNextOrDefault<T3>(ref e3, ref v3) +
                        ZipHelper.MoveNextOrDefault<T4>(ref e4, ref v4) +
                        ZipHelper.MoveNextOrDefault<T5>(ref e5, ref v5) +
                        ZipHelper.MoveNextOrDefault<T6>(ref e6, ref v6) +
                        ZipHelper.MoveNextOrDefault<T7>(ref e7, ref v7) > 0)
                    {
                        yield return resultSelector(v1, v2, v3, v4, v5, v6, v7);
                    }
                }
                finally
                {
                    e1?.Dispose();
                    e2?.Dispose();
                    e3?.Dispose();
                    e4?.Dispose();
                    e5?.Dispose();
                    e6?.Dispose();
                    e7?.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <typeparam name="T7">Type of elements in seventh input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="seventhSource">The seventh source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ZipLongest<T1, T2, T3, T4, T5, T6, T7>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            IEnumerable<T7> seventhSource)
        {
            return ZipLongest(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                fifthSource,
                sixthSource,
                seventhSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <typeparam name="T7">Type of elements in seventh input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="seventhSource">The seventh source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>

        public static IEnumerable<TResult> ZipShortest<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            IEnumerable<T7> seventhSource,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (seventhSource == null) throw new ArgumentNullException(nameof(seventhSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = firstSource.GetEnumerator();
                using var e2 = secondSource.GetEnumerator();
                using var e3 = thirdSource.GetEnumerator();
                using var e4 = fourthSource.GetEnumerator();
                using var e5 = fifthSource.GetEnumerator();
                using var e6 = sixthSource.GetEnumerator();
                using var e7 = seventhSource.GetEnumerator();

                while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext() && e4.MoveNext() && e5.MoveNext() && e6.MoveNext() && e7.MoveNext())
                {
                    yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current, e5.Current, e6.Current, e7.Current);
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <typeparam name="T7">Type of elements in seventh input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="seventhSource">The seventh source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>

        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ZipShortest<T1, T2, T3, T4, T5, T6, T7>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            IEnumerable<T7> seventhSource)
        {
            return ZipShortest(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                fifthSource,
                sixthSource,
                seventhSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences.
        /// When the end of one or more input sequence is reached, on the next iteration, the given <paramref name="predicate"/>
        /// is called with in parameter the list of the 1-based indices of the source parameters that have not reached their end.
        /// If the call to the <paramref name="predicate"/> return <c>false</c> the zip enumeration stop.
        /// If the enumeration continues, the default value of each of the shorter sequence element types
        /// is used for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <typeparam name="T7">Type of elements in seventh input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="seventhSource">The seventh source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <param name="predicate">
        /// A function to test the end of the zip sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// The <paramref name="predicate"/> is not called when all the input sequence have reached their end.
        /// The <paramref name="predicate"/> is called at most six times.
        /// </remarks>
        public static IEnumerable<TResult> ZipWhile<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            IEnumerable<T7> seventhSource,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector,
            Func<IReadOnlyList<int>, bool> predicate)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (seventhSource == null) throw new ArgumentNullException(nameof(seventhSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return _(); IEnumerable<TResult> _()
            {
                IEnumerator<T1> e1 = null;
                IEnumerator<T2> e2 = null;
                IEnumerator<T3> e3 = null;
                IEnumerator<T4> e4 = null;
                IEnumerator<T5> e5 = null;
                IEnumerator<T6> e6 = null;
                IEnumerator<T7> e7 = null;

                try
                {
                    e1 = firstSource.GetEnumerator();
                    e2 = secondSource.GetEnumerator();
                    e3 = thirdSource.GetEnumerator();
                    e4 = fourthSource.GetEnumerator();
                    e5 = fifthSource.GetEnumerator();
                    e6 = sixthSource.GetEnumerator();
                    e7 = seventhSource.GetEnumerator();

                    var v1 = default(T1);
                    var v2 = default(T2);
                    var v3 = default(T3);
                    var v4 = default(T4);
                    var v5 = default(T5);
                    var v6 = default(T6);
                    var v7 = default(T7);

                    var activeSourceCount = 7;
                    for(;;)
                    {
                        var newActiveSourceCount =
                            ZipHelper.MoveNextOrDefault<T1>(ref e1, ref v1) +
                            ZipHelper.MoveNextOrDefault<T2>(ref e2, ref v2) +
                            ZipHelper.MoveNextOrDefault<T3>(ref e3, ref v3) +
                            ZipHelper.MoveNextOrDefault<T4>(ref e4, ref v4) +
                            ZipHelper.MoveNextOrDefault<T5>(ref e5, ref v5) +
                            ZipHelper.MoveNextOrDefault<T6>(ref e6, ref v6) +
                            ZipHelper.MoveNextOrDefault<T7>(ref e7, ref v7);

                        if (activeSourceCount != newActiveSourceCount)
                        {
                            if (!ZipHelper.ShouldContinue(predicate, e1, e2, e3, e4, e5, e6, e7))
                            {
                                yield break;
                            }
                            activeSourceCount = newActiveSourceCount;
                        }

                        yield return resultSelector(v1, v2, v3, v4, v5, v6, v7);
                    }
                }
                finally
                {
                    e1?.Dispose();
                    e2?.Dispose();
                    e3?.Dispose();
                    e4?.Dispose();
                    e5?.Dispose();
                    e6?.Dispose();
                    e7?.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences.
        /// When the end of one or more input sequence is reached, on the next iteration, the given <paramref name="predicate"/>
        /// is called with in parameter the list of the 1-based indices of the source parameters that have not reached their end.
        /// If the call to the <paramref name="predicate"/> return <c>false</c> the zip enumeration stop.
        /// If the enumeration continues, the default value of each of the shorter sequence element types
        /// is used for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <typeparam name="T7">Type of elements in seventh input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="seventhSource">The seventh source sequence.</param>
        /// <param name="predicate">
        /// A function to test the end of the zip sequence.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// The <paramref name="predicate"/> is not called when all the input sequence have reached their end.
        /// The <paramref name="predicate"/> is called at most six times.
        /// </remarks>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ZipWhile<T1, T2, T3, T4, T5, T6, T7>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            IEnumerable<T7> seventhSource,
            Func<IReadOnlyList<int>, bool> predicate)
        {
            return ZipWhile(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                fifthSource,
                sixthSource,
                seventhSource,
                ValueTuple.Create,
                predicate);
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
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="seventhSource">The seventh source sequence.</param>
        /// <param name="eighthSource">The eighth source sequence.</param>
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
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            IEnumerable<T7> seventhSource,
            IEnumerable<T8> eighthSource,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (seventhSource == null) throw new ArgumentNullException(nameof(seventhSource));
            if (eighthSource == null) throw new ArgumentNullException(nameof(eighthSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = firstSource.GetEnumerator();
                using var e2 = secondSource.GetEnumerator();
                using var e3 = thirdSource.GetEnumerator();
                using var e4 = fourthSource.GetEnumerator();
                using var e5 = fifthSource.GetEnumerator();
                using var e6 = sixthSource.GetEnumerator();
                using var e7 = seventhSource.GetEnumerator();
                using var e8 = eighthSource.GetEnumerator();

                for (;;)
                {
                    if (e1.MoveNext())
                    {
                        if (e2.MoveNext() && e3.MoveNext() && e4.MoveNext() && e5.MoveNext() && e6.MoveNext() && e7.MoveNext() && e8.MoveNext())
                        {
                            yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current, e5.Current, e6.Current, e7.Current, e8.Current);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (e2.MoveNext() || e3.MoveNext() || e4.MoveNext() || e5.MoveNext() || e6.MoveNext() || e7.MoveNext() || e8.MoveNext())
                        {
                            break;
                        }
                        else
                        {
                            yield break;
                        }
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
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="seventhSource">The seventh source sequence.</param>
        /// <param name="eighthSource">The eighth source sequence.</param>
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
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            IEnumerable<T7> seventhSource,
            IEnumerable<T8> eighthSource)
        {
            return EquiZip(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                fifthSource,
                sixthSource,
                seventhSource,
                eighthSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
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
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="seventhSource">The seventh source sequence.</param>
        /// <param name="eighthSource">The eighth source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IEnumerable<TResult> ZipLongest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            IEnumerable<T7> seventhSource,
            IEnumerable<T8> eighthSource,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (seventhSource == null) throw new ArgumentNullException(nameof(seventhSource));
            if (eighthSource == null) throw new ArgumentNullException(nameof(eighthSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                IEnumerator<T1> e1 = null;
                IEnumerator<T2> e2 = null;
                IEnumerator<T3> e3 = null;
                IEnumerator<T4> e4 = null;
                IEnumerator<T5> e5 = null;
                IEnumerator<T6> e6 = null;
                IEnumerator<T7> e7 = null;
                IEnumerator<T8> e8 = null;

                try
                {
                    e1 = firstSource.GetEnumerator();
                    e2 = secondSource.GetEnumerator();
                    e3 = thirdSource.GetEnumerator();
                    e4 = fourthSource.GetEnumerator();
                    e5 = fifthSource.GetEnumerator();
                    e6 = sixthSource.GetEnumerator();
                    e7 = seventhSource.GetEnumerator();
                    e8 = eighthSource.GetEnumerator();

                    var v1 = default(T1);
                    var v2 = default(T2);
                    var v3 = default(T3);
                    var v4 = default(T4);
                    var v5 = default(T5);
                    var v6 = default(T6);
                    var v7 = default(T7);
                    var v8 = default(T8);

                    while (
                        ZipHelper.MoveNextOrDefault<T1>(ref e1, ref v1) +
                        ZipHelper.MoveNextOrDefault<T2>(ref e2, ref v2) +
                        ZipHelper.MoveNextOrDefault<T3>(ref e3, ref v3) +
                        ZipHelper.MoveNextOrDefault<T4>(ref e4, ref v4) +
                        ZipHelper.MoveNextOrDefault<T5>(ref e5, ref v5) +
                        ZipHelper.MoveNextOrDefault<T6>(ref e6, ref v6) +
                        ZipHelper.MoveNextOrDefault<T7>(ref e7, ref v7) +
                        ZipHelper.MoveNextOrDefault<T8>(ref e8, ref v8) > 0)
                    {
                        yield return resultSelector(v1, v2, v3, v4, v5, v6, v7, v8);
                    }
                }
                finally
                {
                    e1?.Dispose();
                    e2?.Dispose();
                    e3?.Dispose();
                    e4?.Dispose();
                    e5?.Dispose();
                    e6?.Dispose();
                    e7?.Dispose();
                    e8?.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <typeparam name="T7">Type of elements in seventh input sequence.</typeparam>
        /// <typeparam name="T8">Type of elements in eighth input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="seventhSource">The seventh source sequence.</param>
        /// <param name="eighthSource">The eighth source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ZipLongest<T1, T2, T3, T4, T5, T6, T7, T8>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            IEnumerable<T7> seventhSource,
            IEnumerable<T8> eighthSource)
        {
            return ZipLongest(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                fifthSource,
                sixthSource,
                seventhSource,
                eighthSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// is as short as the shortest input sequence.
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
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="seventhSource">The seventh source sequence.</param>
        /// <param name="eighthSource">The eighth source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>

        public static IEnumerable<TResult> ZipShortest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            IEnumerable<T7> seventhSource,
            IEnumerable<T8> eighthSource,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (seventhSource == null) throw new ArgumentNullException(nameof(seventhSource));
            if (eighthSource == null) throw new ArgumentNullException(nameof(eighthSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var e1 = firstSource.GetEnumerator();
                using var e2 = secondSource.GetEnumerator();
                using var e3 = thirdSource.GetEnumerator();
                using var e4 = fourthSource.GetEnumerator();
                using var e5 = fifthSource.GetEnumerator();
                using var e6 = sixthSource.GetEnumerator();
                using var e7 = seventhSource.GetEnumerator();
                using var e8 = eighthSource.GetEnumerator();

                while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext() && e4.MoveNext() && e5.MoveNext() && e6.MoveNext() && e7.MoveNext() && e8.MoveNext())
                {
                    yield return resultSelector(e1.Current, e2.Current, e3.Current, e4.Current, e5.Current, e6.Current, e7.Current, e8.Current);
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// is as short as the shortest input sequence.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <typeparam name="T7">Type of elements in seventh input sequence.</typeparam>
        /// <typeparam name="T8">Type of elements in eighth input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="seventhSource">The seventh source sequence.</param>
        /// <param name="eighthSource">The eighth source sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// <para>
        /// If the input sequences are of different lengths, the result sequence
        /// is terminated as soon as the shortest input sequence is exhausted
        /// and remainder elements from the longer sequences are never consumed.
        /// </para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>

        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ZipShortest<T1, T2, T3, T4, T5, T6, T7, T8>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            IEnumerable<T7> seventhSource,
            IEnumerable<T8> eighthSource)
        {
            return ZipShortest(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                fifthSource,
                sixthSource,
                seventhSource,
                eighthSource,
                ValueTuple.Create);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th
        /// element from each of the input sequences.
        /// When the end of one or more input sequence is reached, on the next iteration, the given <paramref name="predicate"/>
        /// is called with in parameter the list of the 1-based indices of the source parameters that have not reached their end.
        /// If the call to the <paramref name="predicate"/> return <c>false</c> the zip enumeration stop.
        /// If the enumeration continues, the default value of each of the shorter sequence element types
        /// is used for padding.
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
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="seventhSource">The seventh source sequence.</param>
        /// <param name="eighthSource">The eighth source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to each tuple of elements.</param>
        /// <param name="predicate">
        /// A function to test the end of the zip sequence.</param>
        /// <returns>
        /// A sequence of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// The <paramref name="predicate"/> is not called when all the input sequence have reached their end.
        /// The <paramref name="predicate"/> is called at most seven times.
        /// </remarks>
        public static IEnumerable<TResult> ZipWhile<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            IEnumerable<T7> seventhSource,
            IEnumerable<T8> eighthSource,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector,
            Func<IReadOnlyList<int>, bool> predicate)
        {
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (seventhSource == null) throw new ArgumentNullException(nameof(seventhSource));
            if (eighthSource == null) throw new ArgumentNullException(nameof(eighthSource));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return _(); IEnumerable<TResult> _()
            {
                IEnumerator<T1> e1 = null;
                IEnumerator<T2> e2 = null;
                IEnumerator<T3> e3 = null;
                IEnumerator<T4> e4 = null;
                IEnumerator<T5> e5 = null;
                IEnumerator<T6> e6 = null;
                IEnumerator<T7> e7 = null;
                IEnumerator<T8> e8 = null;

                try
                {
                    e1 = firstSource.GetEnumerator();
                    e2 = secondSource.GetEnumerator();
                    e3 = thirdSource.GetEnumerator();
                    e4 = fourthSource.GetEnumerator();
                    e5 = fifthSource.GetEnumerator();
                    e6 = sixthSource.GetEnumerator();
                    e7 = seventhSource.GetEnumerator();
                    e8 = eighthSource.GetEnumerator();

                    var v1 = default(T1);
                    var v2 = default(T2);
                    var v3 = default(T3);
                    var v4 = default(T4);
                    var v5 = default(T5);
                    var v6 = default(T6);
                    var v7 = default(T7);
                    var v8 = default(T8);

                    var activeSourceCount = 8;
                    for(;;)
                    {
                        var newActiveSourceCount =
                            ZipHelper.MoveNextOrDefault<T1>(ref e1, ref v1) +
                            ZipHelper.MoveNextOrDefault<T2>(ref e2, ref v2) +
                            ZipHelper.MoveNextOrDefault<T3>(ref e3, ref v3) +
                            ZipHelper.MoveNextOrDefault<T4>(ref e4, ref v4) +
                            ZipHelper.MoveNextOrDefault<T5>(ref e5, ref v5) +
                            ZipHelper.MoveNextOrDefault<T6>(ref e6, ref v6) +
                            ZipHelper.MoveNextOrDefault<T7>(ref e7, ref v7) +
                            ZipHelper.MoveNextOrDefault<T8>(ref e8, ref v8);

                        if (activeSourceCount != newActiveSourceCount)
                        {
                            if (!ZipHelper.ShouldContinue(predicate, e1, e2, e3, e4, e5, e6, e7, e8))
                            {
                                yield break;
                            }
                            activeSourceCount = newActiveSourceCount;
                        }

                        yield return resultSelector(v1, v2, v3, v4, v5, v6, v7, v8);
                    }
                }
                finally
                {
                    e1?.Dispose();
                    e2?.Dispose();
                    e3?.Dispose();
                    e4?.Dispose();
                    e5?.Dispose();
                    e6?.Dispose();
                    e7?.Dispose();
                    e8?.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple contains the N-th
        /// element from each of the input sequences.
        /// When the end of one or more input sequence is reached, on the next iteration, the given <paramref name="predicate"/>
        /// is called with in parameter the list of the 1-based indices of the source parameters that have not reached their end.
        /// If the call to the <paramref name="predicate"/> return <c>false</c> the zip enumeration stop.
        /// If the enumeration continues, the default value of each of the shorter sequence element types
        /// is used for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="T4">Type of elements in fourth input sequence.</typeparam>
        /// <typeparam name="T5">Type of elements in fifth input sequence.</typeparam>
        /// <typeparam name="T6">Type of elements in sixth input sequence.</typeparam>
        /// <typeparam name="T7">Type of elements in seventh input sequence.</typeparam>
        /// <typeparam name="T8">Type of elements in eighth input sequence.</typeparam>
        /// <param name="firstSource">The first source sequence.</param>
        /// <param name="secondSource">The second source sequence.</param>
        /// <param name="thirdSource">The third source sequence.</param>
        /// <param name="fourthSource">The fourth source sequence.</param>
        /// <param name="fifthSource">The fifth source sequence.</param>
        /// <param name="sixthSource">The sixth source sequence.</param>
        /// <param name="seventhSource">The seventh source sequence.</param>
        /// <param name="eighthSource">The eighth source sequence.</param>
        /// <param name="predicate">
        /// A function to test the end of the zip sequence.</param>
        /// <returns>
        /// A projection of tuples, where each tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        ///
        /// The <paramref name="predicate"/> is not called when all the input sequence have reached their end.
        /// The <paramref name="predicate"/> is called at most seven times.
        /// </remarks>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ZipWhile<T1, T2, T3, T4, T5, T6, T7, T8>(
            this IEnumerable<T1> firstSource,
            IEnumerable<T2> secondSource,
            IEnumerable<T3> thirdSource,
            IEnumerable<T4> fourthSource,
            IEnumerable<T5> fifthSource,
            IEnumerable<T6> sixthSource,
            IEnumerable<T7> seventhSource,
            IEnumerable<T8> eighthSource,
            Func<IReadOnlyList<int>, bool> predicate)
        {
            return ZipWhile(
                firstSource,
                secondSource,
                thirdSource,
                fourthSource,
                fifthSource,
                sixthSource,
                seventhSource,
                eighthSource,
                ValueTuple.Create,
                predicate);
        }


        static class ZipHelper
        {
            public static int MoveNextOrDefault<T>(ref IEnumerator<T> enumerator, ref T value)
            {
                if (enumerator == null)
                {
                    return 0;
                }

                if (enumerator.MoveNext())
                {
                    value = enumerator.Current;
                    return 1;
                }

                enumerator.Dispose();
                enumerator = null;
                value = default;
                return 0;
            }

            public static bool ShouldContinue(Func<IReadOnlyList<int>, bool> predicate, params IEnumerator[] enumerators)
            {
                if (enumerators.All(e => e == null))
                {
                    return false;
                }

                return predicate(
                    enumerators
                        .Select((enumerator, index) => new {enumerator, index})
                        .Where(t => t.enumerator != null)
                        .Select(t => t.index + 1)
                        .ToList());
            }
        }
    }
}
