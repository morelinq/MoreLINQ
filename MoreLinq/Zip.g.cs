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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.ThrowOnShort,
                secondSource, ZipSourceConfiguration<T2>.ThrowOnShort,
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.ThrowOnShort,
                secondSource, ZipSourceConfiguration<T2>.ThrowOnShort,
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.PaddingWith(default),
                secondSource, ZipSourceConfiguration<T2>.PaddingWith(default),
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.PaddingWith(default),
                secondSource, ZipSourceConfiguration<T2>.PaddingWith(default),
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.StopOnShort,
                secondSource, ZipSourceConfiguration<T2>.StopOnShort,
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.StopOnShort,
                secondSource, ZipSourceConfiguration<T2>.StopOnShort,
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.ThrowOnShort,
                secondSource, ZipSourceConfiguration<T2>.ThrowOnShort,
                thirdSource, ZipSourceConfiguration<T3>.ThrowOnShort,
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.ThrowOnShort,
                secondSource, ZipSourceConfiguration<T2>.ThrowOnShort,
                thirdSource, ZipSourceConfiguration<T3>.ThrowOnShort,
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.PaddingWith(default),
                secondSource, ZipSourceConfiguration<T2>.PaddingWith(default),
                thirdSource, ZipSourceConfiguration<T3>.PaddingWith(default),
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.PaddingWith(default),
                secondSource, ZipSourceConfiguration<T2>.PaddingWith(default),
                thirdSource, ZipSourceConfiguration<T3>.PaddingWith(default),
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.StopOnShort,
                secondSource, ZipSourceConfiguration<T2>.StopOnShort,
                thirdSource, ZipSourceConfiguration<T3>.StopOnShort,
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.StopOnShort,
                secondSource, ZipSourceConfiguration<T2>.StopOnShort,
                thirdSource, ZipSourceConfiguration<T3>.StopOnShort,
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.ThrowOnShort,
                secondSource, ZipSourceConfiguration<T2>.ThrowOnShort,
                thirdSource, ZipSourceConfiguration<T3>.ThrowOnShort,
                fourthSource, ZipSourceConfiguration<T4>.ThrowOnShort,
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.ThrowOnShort,
                secondSource, ZipSourceConfiguration<T2>.ThrowOnShort,
                thirdSource, ZipSourceConfiguration<T3>.ThrowOnShort,
                fourthSource, ZipSourceConfiguration<T4>.ThrowOnShort,
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.PaddingWith(default),
                secondSource, ZipSourceConfiguration<T2>.PaddingWith(default),
                thirdSource, ZipSourceConfiguration<T3>.PaddingWith(default),
                fourthSource, ZipSourceConfiguration<T4>.PaddingWith(default),
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.PaddingWith(default),
                secondSource, ZipSourceConfiguration<T2>.PaddingWith(default),
                thirdSource, ZipSourceConfiguration<T3>.PaddingWith(default),
                fourthSource, ZipSourceConfiguration<T4>.PaddingWith(default),
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.StopOnShort,
                secondSource, ZipSourceConfiguration<T2>.StopOnShort,
                thirdSource, ZipSourceConfiguration<T3>.StopOnShort,
                fourthSource, ZipSourceConfiguration<T4>.StopOnShort,
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.StopOnShort,
                secondSource, ZipSourceConfiguration<T2>.StopOnShort,
                thirdSource, ZipSourceConfiguration<T3>.StopOnShort,
                fourthSource, ZipSourceConfiguration<T4>.StopOnShort,
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.ThrowOnShort,
                secondSource, ZipSourceConfiguration<T2>.ThrowOnShort,
                thirdSource, ZipSourceConfiguration<T3>.ThrowOnShort,
                fourthSource, ZipSourceConfiguration<T4>.ThrowOnShort,
                fifthSource, ZipSourceConfiguration<T5>.ThrowOnShort,
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.ThrowOnShort,
                secondSource, ZipSourceConfiguration<T2>.ThrowOnShort,
                thirdSource, ZipSourceConfiguration<T3>.ThrowOnShort,
                fourthSource, ZipSourceConfiguration<T4>.ThrowOnShort,
                fifthSource, ZipSourceConfiguration<T5>.ThrowOnShort,
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.PaddingWith(default),
                secondSource, ZipSourceConfiguration<T2>.PaddingWith(default),
                thirdSource, ZipSourceConfiguration<T3>.PaddingWith(default),
                fourthSource, ZipSourceConfiguration<T4>.PaddingWith(default),
                fifthSource, ZipSourceConfiguration<T5>.PaddingWith(default),
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.PaddingWith(default),
                secondSource, ZipSourceConfiguration<T2>.PaddingWith(default),
                thirdSource, ZipSourceConfiguration<T3>.PaddingWith(default),
                fourthSource, ZipSourceConfiguration<T4>.PaddingWith(default),
                fifthSource, ZipSourceConfiguration<T5>.PaddingWith(default),
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.StopOnShort,
                secondSource, ZipSourceConfiguration<T2>.StopOnShort,
                thirdSource, ZipSourceConfiguration<T3>.StopOnShort,
                fourthSource, ZipSourceConfiguration<T4>.StopOnShort,
                fifthSource, ZipSourceConfiguration<T5>.StopOnShort,
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.StopOnShort,
                secondSource, ZipSourceConfiguration<T2>.StopOnShort,
                thirdSource, ZipSourceConfiguration<T3>.StopOnShort,
                fourthSource, ZipSourceConfiguration<T4>.StopOnShort,
                fifthSource, ZipSourceConfiguration<T5>.StopOnShort,
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.ThrowOnShort,
                secondSource, ZipSourceConfiguration<T2>.ThrowOnShort,
                thirdSource, ZipSourceConfiguration<T3>.ThrowOnShort,
                fourthSource, ZipSourceConfiguration<T4>.ThrowOnShort,
                fifthSource, ZipSourceConfiguration<T5>.ThrowOnShort,
                sixthSource, ZipSourceConfiguration<T6>.ThrowOnShort,
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.ThrowOnShort,
                secondSource, ZipSourceConfiguration<T2>.ThrowOnShort,
                thirdSource, ZipSourceConfiguration<T3>.ThrowOnShort,
                fourthSource, ZipSourceConfiguration<T4>.ThrowOnShort,
                fifthSource, ZipSourceConfiguration<T5>.ThrowOnShort,
                sixthSource, ZipSourceConfiguration<T6>.ThrowOnShort,
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.PaddingWith(default),
                secondSource, ZipSourceConfiguration<T2>.PaddingWith(default),
                thirdSource, ZipSourceConfiguration<T3>.PaddingWith(default),
                fourthSource, ZipSourceConfiguration<T4>.PaddingWith(default),
                fifthSource, ZipSourceConfiguration<T5>.PaddingWith(default),
                sixthSource, ZipSourceConfiguration<T6>.PaddingWith(default),
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.PaddingWith(default),
                secondSource, ZipSourceConfiguration<T2>.PaddingWith(default),
                thirdSource, ZipSourceConfiguration<T3>.PaddingWith(default),
                fourthSource, ZipSourceConfiguration<T4>.PaddingWith(default),
                fifthSource, ZipSourceConfiguration<T5>.PaddingWith(default),
                sixthSource, ZipSourceConfiguration<T6>.PaddingWith(default),
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.StopOnShort,
                secondSource, ZipSourceConfiguration<T2>.StopOnShort,
                thirdSource, ZipSourceConfiguration<T3>.StopOnShort,
                fourthSource, ZipSourceConfiguration<T4>.StopOnShort,
                fifthSource, ZipSourceConfiguration<T5>.StopOnShort,
                sixthSource, ZipSourceConfiguration<T6>.StopOnShort,
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.StopOnShort,
                secondSource, ZipSourceConfiguration<T2>.StopOnShort,
                thirdSource, ZipSourceConfiguration<T3>.StopOnShort,
                fourthSource, ZipSourceConfiguration<T4>.StopOnShort,
                fifthSource, ZipSourceConfiguration<T5>.StopOnShort,
                sixthSource, ZipSourceConfiguration<T6>.StopOnShort,
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.ThrowOnShort,
                secondSource, ZipSourceConfiguration<T2>.ThrowOnShort,
                thirdSource, ZipSourceConfiguration<T3>.ThrowOnShort,
                fourthSource, ZipSourceConfiguration<T4>.ThrowOnShort,
                fifthSource, ZipSourceConfiguration<T5>.ThrowOnShort,
                sixthSource, ZipSourceConfiguration<T6>.ThrowOnShort,
                seventhSource, ZipSourceConfiguration<T7>.ThrowOnShort,
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (seventhSource == null) throw new ArgumentNullException(nameof(seventhSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.ThrowOnShort,
                secondSource, ZipSourceConfiguration<T2>.ThrowOnShort,
                thirdSource, ZipSourceConfiguration<T3>.ThrowOnShort,
                fourthSource, ZipSourceConfiguration<T4>.ThrowOnShort,
                fifthSource, ZipSourceConfiguration<T5>.ThrowOnShort,
                sixthSource, ZipSourceConfiguration<T6>.ThrowOnShort,
                seventhSource, ZipSourceConfiguration<T7>.ThrowOnShort,
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.PaddingWith(default),
                secondSource, ZipSourceConfiguration<T2>.PaddingWith(default),
                thirdSource, ZipSourceConfiguration<T3>.PaddingWith(default),
                fourthSource, ZipSourceConfiguration<T4>.PaddingWith(default),
                fifthSource, ZipSourceConfiguration<T5>.PaddingWith(default),
                sixthSource, ZipSourceConfiguration<T6>.PaddingWith(default),
                seventhSource, ZipSourceConfiguration<T7>.PaddingWith(default),
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (seventhSource == null) throw new ArgumentNullException(nameof(seventhSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.PaddingWith(default),
                secondSource, ZipSourceConfiguration<T2>.PaddingWith(default),
                thirdSource, ZipSourceConfiguration<T3>.PaddingWith(default),
                fourthSource, ZipSourceConfiguration<T4>.PaddingWith(default),
                fifthSource, ZipSourceConfiguration<T5>.PaddingWith(default),
                sixthSource, ZipSourceConfiguration<T6>.PaddingWith(default),
                seventhSource, ZipSourceConfiguration<T7>.PaddingWith(default),
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.StopOnShort,
                secondSource, ZipSourceConfiguration<T2>.StopOnShort,
                thirdSource, ZipSourceConfiguration<T3>.StopOnShort,
                fourthSource, ZipSourceConfiguration<T4>.StopOnShort,
                fifthSource, ZipSourceConfiguration<T5>.StopOnShort,
                sixthSource, ZipSourceConfiguration<T6>.StopOnShort,
                seventhSource, ZipSourceConfiguration<T7>.StopOnShort,
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (seventhSource == null) throw new ArgumentNullException(nameof(seventhSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.StopOnShort,
                secondSource, ZipSourceConfiguration<T2>.StopOnShort,
                thirdSource, ZipSourceConfiguration<T3>.StopOnShort,
                fourthSource, ZipSourceConfiguration<T4>.StopOnShort,
                fifthSource, ZipSourceConfiguration<T5>.StopOnShort,
                sixthSource, ZipSourceConfiguration<T6>.StopOnShort,
                seventhSource, ZipSourceConfiguration<T7>.StopOnShort,
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.ThrowOnShort,
                secondSource, ZipSourceConfiguration<T2>.ThrowOnShort,
                thirdSource, ZipSourceConfiguration<T3>.ThrowOnShort,
                fourthSource, ZipSourceConfiguration<T4>.ThrowOnShort,
                fifthSource, ZipSourceConfiguration<T5>.ThrowOnShort,
                sixthSource, ZipSourceConfiguration<T6>.ThrowOnShort,
                seventhSource, ZipSourceConfiguration<T7>.ThrowOnShort,
                eighthSource, ZipSourceConfiguration<T8>.ThrowOnShort,
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (seventhSource == null) throw new ArgumentNullException(nameof(seventhSource));
            if (eighthSource == null) throw new ArgumentNullException(nameof(eighthSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.ThrowOnShort,
                secondSource, ZipSourceConfiguration<T2>.ThrowOnShort,
                thirdSource, ZipSourceConfiguration<T3>.ThrowOnShort,
                fourthSource, ZipSourceConfiguration<T4>.ThrowOnShort,
                fifthSource, ZipSourceConfiguration<T5>.ThrowOnShort,
                sixthSource, ZipSourceConfiguration<T6>.ThrowOnShort,
                seventhSource, ZipSourceConfiguration<T7>.ThrowOnShort,
                eighthSource, ZipSourceConfiguration<T8>.ThrowOnShort,
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.PaddingWith(default),
                secondSource, ZipSourceConfiguration<T2>.PaddingWith(default),
                thirdSource, ZipSourceConfiguration<T3>.PaddingWith(default),
                fourthSource, ZipSourceConfiguration<T4>.PaddingWith(default),
                fifthSource, ZipSourceConfiguration<T5>.PaddingWith(default),
                sixthSource, ZipSourceConfiguration<T6>.PaddingWith(default),
                seventhSource, ZipSourceConfiguration<T7>.PaddingWith(default),
                eighthSource, ZipSourceConfiguration<T8>.PaddingWith(default),
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (seventhSource == null) throw new ArgumentNullException(nameof(seventhSource));
            if (eighthSource == null) throw new ArgumentNullException(nameof(eighthSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.PaddingWith(default),
                secondSource, ZipSourceConfiguration<T2>.PaddingWith(default),
                thirdSource, ZipSourceConfiguration<T3>.PaddingWith(default),
                fourthSource, ZipSourceConfiguration<T4>.PaddingWith(default),
                fifthSource, ZipSourceConfiguration<T5>.PaddingWith(default),
                sixthSource, ZipSourceConfiguration<T6>.PaddingWith(default),
                seventhSource, ZipSourceConfiguration<T7>.PaddingWith(default),
                eighthSource, ZipSourceConfiguration<T8>.PaddingWith(default),
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

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.StopOnShort,
                secondSource, ZipSourceConfiguration<T2>.StopOnShort,
                thirdSource, ZipSourceConfiguration<T3>.StopOnShort,
                fourthSource, ZipSourceConfiguration<T4>.StopOnShort,
                fifthSource, ZipSourceConfiguration<T5>.StopOnShort,
                sixthSource, ZipSourceConfiguration<T6>.StopOnShort,
                seventhSource, ZipSourceConfiguration<T7>.StopOnShort,
                eighthSource, ZipSourceConfiguration<T8>.StopOnShort,
                resultSelector);
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
            if (firstSource == null) throw new ArgumentNullException(nameof(firstSource));
            if (secondSource == null) throw new ArgumentNullException(nameof(secondSource));
            if (thirdSource == null) throw new ArgumentNullException(nameof(thirdSource));
            if (fourthSource == null) throw new ArgumentNullException(nameof(fourthSource));
            if (fifthSource == null) throw new ArgumentNullException(nameof(fifthSource));
            if (sixthSource == null) throw new ArgumentNullException(nameof(sixthSource));
            if (seventhSource == null) throw new ArgumentNullException(nameof(seventhSource));
            if (eighthSource == null) throw new ArgumentNullException(nameof(eighthSource));

            return CustomZip(
                firstSource, ZipSourceConfiguration<T1>.StopOnShort,
                secondSource, ZipSourceConfiguration<T2>.StopOnShort,
                thirdSource, ZipSourceConfiguration<T3>.StopOnShort,
                fourthSource, ZipSourceConfiguration<T4>.StopOnShort,
                fifthSource, ZipSourceConfiguration<T5>.StopOnShort,
                sixthSource, ZipSourceConfiguration<T6>.StopOnShort,
                seventhSource, ZipSourceConfiguration<T7>.StopOnShort,
                eighthSource, ZipSourceConfiguration<T8>.StopOnShort,
                ValueTuple.Create);
        }


        internal static IEnumerable<TResult> CustomZip<T1, T2, TResult>(
            this IEnumerable<T1> firstSource, ZipSourceConfiguration<T1> firstSourceConfiguration,
            IEnumerable<T2> secondSource, ZipSourceConfiguration<T2> secondSourceConfiguration,
            Func<T1, T2, TResult> resultSelector)
        {
            using var firstEnumerator = new ZipEnumerator<T1>(firstSource.GetEnumerator(), nameof(firstSource), firstSourceConfiguration);
            using var secondEnumerator = new ZipEnumerator<T2>(secondSource.GetEnumerator(), nameof(secondSource), secondSourceConfiguration);

            while (MoveNext(
                firstEnumerator,
                secondEnumerator))
            {
                yield return resultSelector(
                    firstEnumerator.Current,
                    secondEnumerator.Current
                );
            }
        }
        internal static IEnumerable<TResult> CustomZip<T1, T2, T3, TResult>(
            this IEnumerable<T1> firstSource, ZipSourceConfiguration<T1> firstSourceConfiguration,
            IEnumerable<T2> secondSource, ZipSourceConfiguration<T2> secondSourceConfiguration,
            IEnumerable<T3> thirdSource, ZipSourceConfiguration<T3> thirdSourceConfiguration,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            using var firstEnumerator = new ZipEnumerator<T1>(firstSource.GetEnumerator(), nameof(firstSource), firstSourceConfiguration);
            using var secondEnumerator = new ZipEnumerator<T2>(secondSource.GetEnumerator(), nameof(secondSource), secondSourceConfiguration);
            using var thirdEnumerator = new ZipEnumerator<T3>(thirdSource.GetEnumerator(), nameof(thirdSource), thirdSourceConfiguration);

            while (MoveNext(
                firstEnumerator,
                secondEnumerator,
                thirdEnumerator))
            {
                yield return resultSelector(
                    firstEnumerator.Current,
                    secondEnumerator.Current,
                    thirdEnumerator.Current
                );
            }
        }
        internal static IEnumerable<TResult> CustomZip<T1, T2, T3, T4, TResult>(
            this IEnumerable<T1> firstSource, ZipSourceConfiguration<T1> firstSourceConfiguration,
            IEnumerable<T2> secondSource, ZipSourceConfiguration<T2> secondSourceConfiguration,
            IEnumerable<T3> thirdSource, ZipSourceConfiguration<T3> thirdSourceConfiguration,
            IEnumerable<T4> fourthSource, ZipSourceConfiguration<T4> fourthSourceConfiguration,
            Func<T1, T2, T3, T4, TResult> resultSelector)
        {
            using var firstEnumerator = new ZipEnumerator<T1>(firstSource.GetEnumerator(), nameof(firstSource), firstSourceConfiguration);
            using var secondEnumerator = new ZipEnumerator<T2>(secondSource.GetEnumerator(), nameof(secondSource), secondSourceConfiguration);
            using var thirdEnumerator = new ZipEnumerator<T3>(thirdSource.GetEnumerator(), nameof(thirdSource), thirdSourceConfiguration);
            using var fourthEnumerator = new ZipEnumerator<T4>(fourthSource.GetEnumerator(), nameof(fourthSource), fourthSourceConfiguration);

            while (MoveNext(
                firstEnumerator,
                secondEnumerator,
                thirdEnumerator,
                fourthEnumerator))
            {
                yield return resultSelector(
                    firstEnumerator.Current,
                    secondEnumerator.Current,
                    thirdEnumerator.Current,
                    fourthEnumerator.Current
                );
            }
        }
        internal static IEnumerable<TResult> CustomZip<T1, T2, T3, T4, T5, TResult>(
            this IEnumerable<T1> firstSource, ZipSourceConfiguration<T1> firstSourceConfiguration,
            IEnumerable<T2> secondSource, ZipSourceConfiguration<T2> secondSourceConfiguration,
            IEnumerable<T3> thirdSource, ZipSourceConfiguration<T3> thirdSourceConfiguration,
            IEnumerable<T4> fourthSource, ZipSourceConfiguration<T4> fourthSourceConfiguration,
            IEnumerable<T5> fifthSource, ZipSourceConfiguration<T5> fifthSourceConfiguration,
            Func<T1, T2, T3, T4, T5, TResult> resultSelector)
        {
            using var firstEnumerator = new ZipEnumerator<T1>(firstSource.GetEnumerator(), nameof(firstSource), firstSourceConfiguration);
            using var secondEnumerator = new ZipEnumerator<T2>(secondSource.GetEnumerator(), nameof(secondSource), secondSourceConfiguration);
            using var thirdEnumerator = new ZipEnumerator<T3>(thirdSource.GetEnumerator(), nameof(thirdSource), thirdSourceConfiguration);
            using var fourthEnumerator = new ZipEnumerator<T4>(fourthSource.GetEnumerator(), nameof(fourthSource), fourthSourceConfiguration);
            using var fifthEnumerator = new ZipEnumerator<T5>(fifthSource.GetEnumerator(), nameof(fifthSource), fifthSourceConfiguration);

            while (MoveNext(
                firstEnumerator,
                secondEnumerator,
                thirdEnumerator,
                fourthEnumerator,
                fifthEnumerator))
            {
                yield return resultSelector(
                    firstEnumerator.Current,
                    secondEnumerator.Current,
                    thirdEnumerator.Current,
                    fourthEnumerator.Current,
                    fifthEnumerator.Current
                );
            }
        }
        internal static IEnumerable<TResult> CustomZip<T1, T2, T3, T4, T5, T6, TResult>(
            this IEnumerable<T1> firstSource, ZipSourceConfiguration<T1> firstSourceConfiguration,
            IEnumerable<T2> secondSource, ZipSourceConfiguration<T2> secondSourceConfiguration,
            IEnumerable<T3> thirdSource, ZipSourceConfiguration<T3> thirdSourceConfiguration,
            IEnumerable<T4> fourthSource, ZipSourceConfiguration<T4> fourthSourceConfiguration,
            IEnumerable<T5> fifthSource, ZipSourceConfiguration<T5> fifthSourceConfiguration,
            IEnumerable<T6> sixthSource, ZipSourceConfiguration<T6> sixthSourceConfiguration,
            Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector)
        {
            using var firstEnumerator = new ZipEnumerator<T1>(firstSource.GetEnumerator(), nameof(firstSource), firstSourceConfiguration);
            using var secondEnumerator = new ZipEnumerator<T2>(secondSource.GetEnumerator(), nameof(secondSource), secondSourceConfiguration);
            using var thirdEnumerator = new ZipEnumerator<T3>(thirdSource.GetEnumerator(), nameof(thirdSource), thirdSourceConfiguration);
            using var fourthEnumerator = new ZipEnumerator<T4>(fourthSource.GetEnumerator(), nameof(fourthSource), fourthSourceConfiguration);
            using var fifthEnumerator = new ZipEnumerator<T5>(fifthSource.GetEnumerator(), nameof(fifthSource), fifthSourceConfiguration);
            using var sixthEnumerator = new ZipEnumerator<T6>(sixthSource.GetEnumerator(), nameof(sixthSource), sixthSourceConfiguration);

            while (MoveNext(
                firstEnumerator,
                secondEnumerator,
                thirdEnumerator,
                fourthEnumerator,
                fifthEnumerator,
                sixthEnumerator))
            {
                yield return resultSelector(
                    firstEnumerator.Current,
                    secondEnumerator.Current,
                    thirdEnumerator.Current,
                    fourthEnumerator.Current,
                    fifthEnumerator.Current,
                    sixthEnumerator.Current
                );
            }
        }
        internal static IEnumerable<TResult> CustomZip<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this IEnumerable<T1> firstSource, ZipSourceConfiguration<T1> firstSourceConfiguration,
            IEnumerable<T2> secondSource, ZipSourceConfiguration<T2> secondSourceConfiguration,
            IEnumerable<T3> thirdSource, ZipSourceConfiguration<T3> thirdSourceConfiguration,
            IEnumerable<T4> fourthSource, ZipSourceConfiguration<T4> fourthSourceConfiguration,
            IEnumerable<T5> fifthSource, ZipSourceConfiguration<T5> fifthSourceConfiguration,
            IEnumerable<T6> sixthSource, ZipSourceConfiguration<T6> sixthSourceConfiguration,
            IEnumerable<T7> seventhSource, ZipSourceConfiguration<T7> seventhSourceConfiguration,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector)
        {
            using var firstEnumerator = new ZipEnumerator<T1>(firstSource.GetEnumerator(), nameof(firstSource), firstSourceConfiguration);
            using var secondEnumerator = new ZipEnumerator<T2>(secondSource.GetEnumerator(), nameof(secondSource), secondSourceConfiguration);
            using var thirdEnumerator = new ZipEnumerator<T3>(thirdSource.GetEnumerator(), nameof(thirdSource), thirdSourceConfiguration);
            using var fourthEnumerator = new ZipEnumerator<T4>(fourthSource.GetEnumerator(), nameof(fourthSource), fourthSourceConfiguration);
            using var fifthEnumerator = new ZipEnumerator<T5>(fifthSource.GetEnumerator(), nameof(fifthSource), fifthSourceConfiguration);
            using var sixthEnumerator = new ZipEnumerator<T6>(sixthSource.GetEnumerator(), nameof(sixthSource), sixthSourceConfiguration);
            using var seventhEnumerator = new ZipEnumerator<T7>(seventhSource.GetEnumerator(), nameof(seventhSource), seventhSourceConfiguration);

            while (MoveNext(
                firstEnumerator,
                secondEnumerator,
                thirdEnumerator,
                fourthEnumerator,
                fifthEnumerator,
                sixthEnumerator,
                seventhEnumerator))
            {
                yield return resultSelector(
                    firstEnumerator.Current,
                    secondEnumerator.Current,
                    thirdEnumerator.Current,
                    fourthEnumerator.Current,
                    fifthEnumerator.Current,
                    sixthEnumerator.Current,
                    seventhEnumerator.Current
                );
            }
        }
        internal static IEnumerable<TResult> CustomZip<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this IEnumerable<T1> firstSource, ZipSourceConfiguration<T1> firstSourceConfiguration,
            IEnumerable<T2> secondSource, ZipSourceConfiguration<T2> secondSourceConfiguration,
            IEnumerable<T3> thirdSource, ZipSourceConfiguration<T3> thirdSourceConfiguration,
            IEnumerable<T4> fourthSource, ZipSourceConfiguration<T4> fourthSourceConfiguration,
            IEnumerable<T5> fifthSource, ZipSourceConfiguration<T5> fifthSourceConfiguration,
            IEnumerable<T6> sixthSource, ZipSourceConfiguration<T6> sixthSourceConfiguration,
            IEnumerable<T7> seventhSource, ZipSourceConfiguration<T7> seventhSourceConfiguration,
            IEnumerable<T8> eighthSource, ZipSourceConfiguration<T8> eighthSourceConfiguration,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector)
        {
            using var firstEnumerator = new ZipEnumerator<T1>(firstSource.GetEnumerator(), nameof(firstSource), firstSourceConfiguration);
            using var secondEnumerator = new ZipEnumerator<T2>(secondSource.GetEnumerator(), nameof(secondSource), secondSourceConfiguration);
            using var thirdEnumerator = new ZipEnumerator<T3>(thirdSource.GetEnumerator(), nameof(thirdSource), thirdSourceConfiguration);
            using var fourthEnumerator = new ZipEnumerator<T4>(fourthSource.GetEnumerator(), nameof(fourthSource), fourthSourceConfiguration);
            using var fifthEnumerator = new ZipEnumerator<T5>(fifthSource.GetEnumerator(), nameof(fifthSource), fifthSourceConfiguration);
            using var sixthEnumerator = new ZipEnumerator<T6>(sixthSource.GetEnumerator(), nameof(sixthSource), sixthSourceConfiguration);
            using var seventhEnumerator = new ZipEnumerator<T7>(seventhSource.GetEnumerator(), nameof(seventhSource), seventhSourceConfiguration);
            using var eighthEnumerator = new ZipEnumerator<T8>(eighthSource.GetEnumerator(), nameof(eighthSource), eighthSourceConfiguration);

            while (MoveNext(
                firstEnumerator,
                secondEnumerator,
                thirdEnumerator,
                fourthEnumerator,
                fifthEnumerator,
                sixthEnumerator,
                seventhEnumerator,
                eighthEnumerator))
            {
                yield return resultSelector(
                    firstEnumerator.Current,
                    secondEnumerator.Current,
                    thirdEnumerator.Current,
                    fourthEnumerator.Current,
                    fifthEnumerator.Current,
                    sixthEnumerator.Current,
                    seventhEnumerator.Current,
                    eighthEnumerator.Current
                );
            }
        }

        private static bool MoveNext(params IZipEnumerator[] enumerators)
        {
            var hasNext = false;
            IZipEnumerator equiStopper = null;

            foreach (var enumerator in enumerators)
            {
                switch (enumerator.MoveNext())
                {
                    case ZipEnumeratorStatus.AskForStop:
                        return false;
                    case ZipEnumeratorStatus.AskForEquiStop:
                        if (hasNext) // there is some sequences ahead
                        {
                            enumerator.ThrowToShort();
                        }
                        equiStopper = enumerator;
                        break;
                    case ZipEnumeratorStatus.Continue:
                        equiStopper?.ThrowToShort();
                        hasNext = true;
                        break;
                    case ZipEnumeratorStatus.EndOfStream:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return hasNext;
        }
    }

    internal interface IZipEnumerator
    {
        ZipEnumeratorStatus MoveNext();
        void ThrowToShort();
    }

    internal class ZipEnumerator<T> : IZipEnumerator, IDisposable
    {
        private readonly ZipSourceConfiguration<T> _configuration;
        private readonly string _name;
        private IEnumerator<T> _source;

        public ZipEnumerator(IEnumerator<T> source, string name, ZipSourceConfiguration<T> configuration)
        {
            _source = source;
            _name = name;
            _configuration = configuration;
        }

        public T Current => _source == null ? _configuration.PaddingValue : _source.Current;

        public void Dispose() => _source?.Dispose();

        public ZipEnumeratorStatus MoveNext()
        {
            if (_source?.MoveNext() == false)
            {
                _source.Dispose();
                _source = null;
            }

            if (_source != null)
            {
                return ZipEnumeratorStatus.Continue;
            }

            switch (_configuration.Behavior)
            {
                case ZipEnumeratorBehavior.StopOnShort:
                    return ZipEnumeratorStatus.AskForStop;
                case ZipEnumeratorBehavior.Padding:
                    return ZipEnumeratorStatus.EndOfStream;
                case ZipEnumeratorBehavior.ThrowOnShort:
                    return ZipEnumeratorStatus.AskForEquiStop;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Reset() => _source.Reset();

        public void ThrowToShort() => throw new InvalidOperationException($"{_name} sequence too short.");
    }

    internal enum ZipEnumeratorBehavior
    {
        StopOnShort,
        ThrowOnShort,
        Padding
    }

    internal enum ZipEnumeratorStatus
    {
        AskForStop,
        AskForEquiStop,
        Continue,
        EndOfStream
    }

    internal class ZipSourceConfiguration<T>
    {
        public static ZipSourceConfiguration<T> StopOnShort { get; } = new ZipSourceConfiguration<T>(ZipEnumeratorBehavior.StopOnShort, default);
        public static ZipSourceConfiguration<T> ThrowOnShort { get; } = new ZipSourceConfiguration<T>(ZipEnumeratorBehavior.ThrowOnShort, default);
        public static ZipSourceConfiguration<T> PaddingWith(T paddingValue) => new ZipSourceConfiguration<T>(ZipEnumeratorBehavior.Padding, paddingValue);

        ZipSourceConfiguration(ZipEnumeratorBehavior behavior, T paddingValue)
        {
            Behavior = behavior;
            PaddingValue = paddingValue;
        }

        public ZipEnumeratorBehavior Behavior { get; }
        public T PaddingValue { get; }
    }
}
