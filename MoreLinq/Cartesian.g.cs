#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2010 Leopold Bushkin. All rights reserved.
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

#nullable enable // required for auto-generated sources (see below why)

// > Older code generation strategies may not be nullable aware. Setting the
// > project-level nullable context to "enable" could result in many
// > warnings that a user is unable to fix. To support this scenario any syntax
// > tree that is determined to be generated will have its nullable state
// > implicitly set to "disable", regardless of the overall project state.
//
// Source: https://github.com/dotnet/roslyn/blob/70e158ba6c2c99bd3c3fc0754af0dbf82a6d353d/docs/features/nullable-reference-types.md#generated-code

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using Experimental;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns the Cartesian product of two sequences by enumerating all
        /// possible combinations of one item from each sequence, and applying
        /// a user-defined projection to the items in a given combination.
        /// </summary>
        /// <typeparam name="T1">
        /// The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="T2">
        /// The type of the elements of <paramref name="second"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements of the result sequence.</typeparam>
        /// <param name="first">The first sequence of elements.</param>
        /// <param name="second">The second sequence of elements.</param>
        /// <param name="resultSelector">A projection function that combines
        /// elements from all of the sequences.</param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// <para>
        /// The method returns items in the same order as a nested foreach
        /// loop, but all sequences except for <paramref name="first"/> are
        /// cached when iterated over. The cache is then re-used for any
        /// subsequent iterations.</para>
        /// <para>
        /// This method uses deferred execution and stream its results.</para>
        /// </remarks>

        public static IEnumerable<TResult> Cartesian<T1, T2, TResult>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            Func<T1, T2, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                IEnumerable<T2> secondMemo;

                using ((secondMemo = second.Memoize()) as IDisposable)
                {
                    foreach (var item1 in first)
                    foreach (var item2 in secondMemo)
                        yield return resultSelector(item1, item2);
                }
            }
        }

        /// <summary>
        /// Returns the Cartesian product of two sequences by enumerating tuples
        /// of all possible combinations of one item from each sequence.
        /// </summary>
        /// <typeparam name="T1">The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="T2">The type of the elements of <paramref name="second"/>.</typeparam>
        /// <param name="first">The first sequence of elements.</param>
        /// <param name="second">The second sequence of elements.</param>
        /// <returns>A sequence of tuples.</returns>
        /// <remarks>
        /// <para>
        /// The method returns items in the same order as a nested foreach
        /// loop, but all sequences except for <paramref name="first"/> are
        /// cached when iterated over. The cache is then re-used for any
        /// subsequent iterations.</para>
        /// <para>
        /// This method uses deferred execution and stream its results.</para>
        /// </remarks>

        public static IEnumerable<(T1 First, T2 Second)>
            Cartesian<T1, T2>(
                this IEnumerable<T1> first,
                IEnumerable<T2> second)
        {
            return Cartesian(first, second, ValueTuple.Create);
        }

        /// <summary>
        /// Returns the Cartesian product of three sequences by enumerating all
        /// possible combinations of one item from each sequence, and applying
        /// a user-defined projection to the items in a given combination.
        /// </summary>
        /// <typeparam name="T1">
        /// The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="T2">
        /// The type of the elements of <paramref name="second"/>.</typeparam>
        /// <typeparam name="T3">
        /// The type of the elements of <paramref name="third"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements of the result sequence.</typeparam>
        /// <param name="first">The first sequence of elements.</param>
        /// <param name="second">The second sequence of elements.</param>
        /// <param name="third">The third sequence of elements.</param>
        /// <param name="resultSelector">A projection function that combines
        /// elements from all of the sequences.</param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// <para>
        /// The method returns items in the same order as a nested foreach
        /// loop, but all sequences except for <paramref name="first"/> are
        /// cached when iterated over. The cache is then re-used for any
        /// subsequent iterations.</para>
        /// <para>
        /// This method uses deferred execution and stream its results.</para>
        /// </remarks>

        public static IEnumerable<TResult> Cartesian<T1, T2, T3, TResult>(
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
                IEnumerable<T2> secondMemo;
                IEnumerable<T3> thirdMemo;

                using ((secondMemo = second.Memoize()) as IDisposable)
                using ((thirdMemo = third.Memoize()) as IDisposable)
                {
                    foreach (var item1 in first)
                    foreach (var item2 in secondMemo)
                    foreach (var item3 in thirdMemo)
                        yield return resultSelector(item1, item2, item3);
                }
            }
        }

        /// <summary>
        /// Returns the Cartesian product of three sequences by enumerating tuples
        /// of all possible combinations of one item from each sequence.
        /// </summary>
        /// <typeparam name="T1">The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="T2">The type of the elements of <paramref name="second"/>.</typeparam>
        /// <typeparam name="T3">The type of the elements of <paramref name="third"/>.</typeparam>
        /// <param name="first">The first sequence of elements.</param>
        /// <param name="second">The second sequence of elements.</param>
        /// <param name="third">The third sequence of elements.</param>
        /// <returns>A sequence of tuples.</returns>
        /// <remarks>
        /// <para>
        /// The method returns items in the same order as a nested foreach
        /// loop, but all sequences except for <paramref name="first"/> are
        /// cached when iterated over. The cache is then re-used for any
        /// subsequent iterations.</para>
        /// <para>
        /// This method uses deferred execution and stream its results.</para>
        /// </remarks>

        public static IEnumerable<(T1 First, T2 Second, T3 Third)>
            Cartesian<T1, T2, T3>(
                this IEnumerable<T1> first,
                IEnumerable<T2> second,
                IEnumerable<T3> third)
        {
            return Cartesian(first, second, third, ValueTuple.Create);
        }

        /// <summary>
        /// Returns the Cartesian product of four sequences by enumerating all
        /// possible combinations of one item from each sequence, and applying
        /// a user-defined projection to the items in a given combination.
        /// </summary>
        /// <typeparam name="T1">
        /// The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="T2">
        /// The type of the elements of <paramref name="second"/>.</typeparam>
        /// <typeparam name="T3">
        /// The type of the elements of <paramref name="third"/>.</typeparam>
        /// <typeparam name="T4">
        /// The type of the elements of <paramref name="fourth"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements of the result sequence.</typeparam>
        /// <param name="first">The first sequence of elements.</param>
        /// <param name="second">The second sequence of elements.</param>
        /// <param name="third">The third sequence of elements.</param>
        /// <param name="fourth">The fourth sequence of elements.</param>
        /// <param name="resultSelector">A projection function that combines
        /// elements from all of the sequences.</param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// <para>
        /// The method returns items in the same order as a nested foreach
        /// loop, but all sequences except for <paramref name="first"/> are
        /// cached when iterated over. The cache is then re-used for any
        /// subsequent iterations.</para>
        /// <para>
        /// This method uses deferred execution and stream its results.</para>
        /// </remarks>

        public static IEnumerable<TResult> Cartesian<T1, T2, T3, T4, TResult>(
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
                IEnumerable<T2> secondMemo;
                IEnumerable<T3> thirdMemo;
                IEnumerable<T4> fourthMemo;

                using ((secondMemo = second.Memoize()) as IDisposable)
                using ((thirdMemo = third.Memoize()) as IDisposable)
                using ((fourthMemo = fourth.Memoize()) as IDisposable)
                {
                    foreach (var item1 in first)
                    foreach (var item2 in secondMemo)
                    foreach (var item3 in thirdMemo)
                    foreach (var item4 in fourthMemo)
                        yield return resultSelector(item1, item2, item3, item4);
                }
            }
        }

        /// <summary>
        /// Returns the Cartesian product of four sequences by enumerating tuples
        /// of all possible combinations of one item from each sequence.
        /// </summary>
        /// <typeparam name="T1">The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="T2">The type of the elements of <paramref name="second"/>.</typeparam>
        /// <typeparam name="T3">The type of the elements of <paramref name="third"/>.</typeparam>
        /// <typeparam name="T4">The type of the elements of <paramref name="fourth"/>.</typeparam>
        /// <param name="first">The first sequence of elements.</param>
        /// <param name="second">The second sequence of elements.</param>
        /// <param name="third">The third sequence of elements.</param>
        /// <param name="fourth">The fourth sequence of elements.</param>
        /// <returns>A sequence of tuples.</returns>
        /// <remarks>
        /// <para>
        /// The method returns items in the same order as a nested foreach
        /// loop, but all sequences except for <paramref name="first"/> are
        /// cached when iterated over. The cache is then re-used for any
        /// subsequent iterations.</para>
        /// <para>
        /// This method uses deferred execution and stream its results.</para>
        /// </remarks>

        public static IEnumerable<(T1 First, T2 Second, T3 Third, T4 Fourth)>
            Cartesian<T1, T2, T3, T4>(
                this IEnumerable<T1> first,
                IEnumerable<T2> second,
                IEnumerable<T3> third,
                IEnumerable<T4> fourth)
        {
            return Cartesian(first, second, third, fourth, ValueTuple.Create);
        }

        /// <summary>
        /// Returns the Cartesian product of five sequences by enumerating all
        /// possible combinations of one item from each sequence, and applying
        /// a user-defined projection to the items in a given combination.
        /// </summary>
        /// <typeparam name="T1">
        /// The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="T2">
        /// The type of the elements of <paramref name="second"/>.</typeparam>
        /// <typeparam name="T3">
        /// The type of the elements of <paramref name="third"/>.</typeparam>
        /// <typeparam name="T4">
        /// The type of the elements of <paramref name="fourth"/>.</typeparam>
        /// <typeparam name="T5">
        /// The type of the elements of <paramref name="fifth"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements of the result sequence.</typeparam>
        /// <param name="first">The first sequence of elements.</param>
        /// <param name="second">The second sequence of elements.</param>
        /// <param name="third">The third sequence of elements.</param>
        /// <param name="fourth">The fourth sequence of elements.</param>
        /// <param name="fifth">The fifth sequence of elements.</param>
        /// <param name="resultSelector">A projection function that combines
        /// elements from all of the sequences.</param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// <para>
        /// The method returns items in the same order as a nested foreach
        /// loop, but all sequences except for <paramref name="first"/> are
        /// cached when iterated over. The cache is then re-used for any
        /// subsequent iterations.</para>
        /// <para>
        /// This method uses deferred execution and stream its results.</para>
        /// </remarks>

        public static IEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, TResult>(
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
                IEnumerable<T2> secondMemo;
                IEnumerable<T3> thirdMemo;
                IEnumerable<T4> fourthMemo;
                IEnumerable<T5> fifthMemo;

                using ((secondMemo = second.Memoize()) as IDisposable)
                using ((thirdMemo = third.Memoize()) as IDisposable)
                using ((fourthMemo = fourth.Memoize()) as IDisposable)
                using ((fifthMemo = fifth.Memoize()) as IDisposable)
                {
                    foreach (var item1 in first)
                    foreach (var item2 in secondMemo)
                    foreach (var item3 in thirdMemo)
                    foreach (var item4 in fourthMemo)
                    foreach (var item5 in fifthMemo)
                        yield return resultSelector(item1, item2, item3, item4, item5);
                }
            }
        }

        /// <summary>
        /// Returns the Cartesian product of five sequences by enumerating tuples
        /// of all possible combinations of one item from each sequence.
        /// </summary>
        /// <typeparam name="T1">The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="T2">The type of the elements of <paramref name="second"/>.</typeparam>
        /// <typeparam name="T3">The type of the elements of <paramref name="third"/>.</typeparam>
        /// <typeparam name="T4">The type of the elements of <paramref name="fourth"/>.</typeparam>
        /// <typeparam name="T5">The type of the elements of <paramref name="fifth"/>.</typeparam>
        /// <param name="first">The first sequence of elements.</param>
        /// <param name="second">The second sequence of elements.</param>
        /// <param name="third">The third sequence of elements.</param>
        /// <param name="fourth">The fourth sequence of elements.</param>
        /// <param name="fifth">The fifth sequence of elements.</param>
        /// <returns>A sequence of tuples.</returns>
        /// <remarks>
        /// <para>
        /// The method returns items in the same order as a nested foreach
        /// loop, but all sequences except for <paramref name="first"/> are
        /// cached when iterated over. The cache is then re-used for any
        /// subsequent iterations.</para>
        /// <para>
        /// This method uses deferred execution and stream its results.</para>
        /// </remarks>

        public static IEnumerable<(T1 First, T2 Second, T3 Third, T4 Fourth, T5 Fifth)>
            Cartesian<T1, T2, T3, T4, T5>(
                this IEnumerable<T1> first,
                IEnumerable<T2> second,
                IEnumerable<T3> third,
                IEnumerable<T4> fourth,
                IEnumerable<T5> fifth)
        {
            return Cartesian(first, second, third, fourth, fifth, ValueTuple.Create);
        }

        /// <summary>
        /// Returns the Cartesian product of six sequences by enumerating all
        /// possible combinations of one item from each sequence, and applying
        /// a user-defined projection to the items in a given combination.
        /// </summary>
        /// <typeparam name="T1">
        /// The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="T2">
        /// The type of the elements of <paramref name="second"/>.</typeparam>
        /// <typeparam name="T3">
        /// The type of the elements of <paramref name="third"/>.</typeparam>
        /// <typeparam name="T4">
        /// The type of the elements of <paramref name="fourth"/>.</typeparam>
        /// <typeparam name="T5">
        /// The type of the elements of <paramref name="fifth"/>.</typeparam>
        /// <typeparam name="T6">
        /// The type of the elements of <paramref name="sixth"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements of the result sequence.</typeparam>
        /// <param name="first">The first sequence of elements.</param>
        /// <param name="second">The second sequence of elements.</param>
        /// <param name="third">The third sequence of elements.</param>
        /// <param name="fourth">The fourth sequence of elements.</param>
        /// <param name="fifth">The fifth sequence of elements.</param>
        /// <param name="sixth">The sixth sequence of elements.</param>
        /// <param name="resultSelector">A projection function that combines
        /// elements from all of the sequences.</param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// <para>
        /// The method returns items in the same order as a nested foreach
        /// loop, but all sequences except for <paramref name="first"/> are
        /// cached when iterated over. The cache is then re-used for any
        /// subsequent iterations.</para>
        /// <para>
        /// This method uses deferred execution and stream its results.</para>
        /// </remarks>

        public static IEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, T6, TResult>(
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
                IEnumerable<T2> secondMemo;
                IEnumerable<T3> thirdMemo;
                IEnumerable<T4> fourthMemo;
                IEnumerable<T5> fifthMemo;
                IEnumerable<T6> sixthMemo;

                using ((secondMemo = second.Memoize()) as IDisposable)
                using ((thirdMemo = third.Memoize()) as IDisposable)
                using ((fourthMemo = fourth.Memoize()) as IDisposable)
                using ((fifthMemo = fifth.Memoize()) as IDisposable)
                using ((sixthMemo = sixth.Memoize()) as IDisposable)
                {
                    foreach (var item1 in first)
                    foreach (var item2 in secondMemo)
                    foreach (var item3 in thirdMemo)
                    foreach (var item4 in fourthMemo)
                    foreach (var item5 in fifthMemo)
                    foreach (var item6 in sixthMemo)
                        yield return resultSelector(item1, item2, item3, item4, item5, item6);
                }
            }
        }

        /// <summary>
        /// Returns the Cartesian product of six sequences by enumerating tuples
        /// of all possible combinations of one item from each sequence.
        /// </summary>
        /// <typeparam name="T1">The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="T2">The type of the elements of <paramref name="second"/>.</typeparam>
        /// <typeparam name="T3">The type of the elements of <paramref name="third"/>.</typeparam>
        /// <typeparam name="T4">The type of the elements of <paramref name="fourth"/>.</typeparam>
        /// <typeparam name="T5">The type of the elements of <paramref name="fifth"/>.</typeparam>
        /// <typeparam name="T6">The type of the elements of <paramref name="sixth"/>.</typeparam>
        /// <param name="first">The first sequence of elements.</param>
        /// <param name="second">The second sequence of elements.</param>
        /// <param name="third">The third sequence of elements.</param>
        /// <param name="fourth">The fourth sequence of elements.</param>
        /// <param name="fifth">The fifth sequence of elements.</param>
        /// <param name="sixth">The sixth sequence of elements.</param>
        /// <returns>A sequence of tuples.</returns>
        /// <remarks>
        /// <para>
        /// The method returns items in the same order as a nested foreach
        /// loop, but all sequences except for <paramref name="first"/> are
        /// cached when iterated over. The cache is then re-used for any
        /// subsequent iterations.</para>
        /// <para>
        /// This method uses deferred execution and stream its results.</para>
        /// </remarks>

        public static IEnumerable<(T1 First, T2 Second, T3 Third, T4 Fourth, T5 Fifth, T6 Sixth)>
            Cartesian<T1, T2, T3, T4, T5, T6>(
                this IEnumerable<T1> first,
                IEnumerable<T2> second,
                IEnumerable<T3> third,
                IEnumerable<T4> fourth,
                IEnumerable<T5> fifth,
                IEnumerable<T6> sixth)
        {
            return Cartesian(first, second, third, fourth, fifth, sixth, ValueTuple.Create);
        }

        /// <summary>
        /// Returns the Cartesian product of seven sequences by enumerating all
        /// possible combinations of one item from each sequence, and applying
        /// a user-defined projection to the items in a given combination.
        /// </summary>
        /// <typeparam name="T1">
        /// The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="T2">
        /// The type of the elements of <paramref name="second"/>.</typeparam>
        /// <typeparam name="T3">
        /// The type of the elements of <paramref name="third"/>.</typeparam>
        /// <typeparam name="T4">
        /// The type of the elements of <paramref name="fourth"/>.</typeparam>
        /// <typeparam name="T5">
        /// The type of the elements of <paramref name="fifth"/>.</typeparam>
        /// <typeparam name="T6">
        /// The type of the elements of <paramref name="sixth"/>.</typeparam>
        /// <typeparam name="T7">
        /// The type of the elements of <paramref name="seventh"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements of the result sequence.</typeparam>
        /// <param name="first">The first sequence of elements.</param>
        /// <param name="second">The second sequence of elements.</param>
        /// <param name="third">The third sequence of elements.</param>
        /// <param name="fourth">The fourth sequence of elements.</param>
        /// <param name="fifth">The fifth sequence of elements.</param>
        /// <param name="sixth">The sixth sequence of elements.</param>
        /// <param name="seventh">The seventh sequence of elements.</param>
        /// <param name="resultSelector">A projection function that combines
        /// elements from all of the sequences.</param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// <para>
        /// The method returns items in the same order as a nested foreach
        /// loop, but all sequences except for <paramref name="first"/> are
        /// cached when iterated over. The cache is then re-used for any
        /// subsequent iterations.</para>
        /// <para>
        /// This method uses deferred execution and stream its results.</para>
        /// </remarks>

        public static IEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, T6, T7, TResult>(
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
                IEnumerable<T2> secondMemo;
                IEnumerable<T3> thirdMemo;
                IEnumerable<T4> fourthMemo;
                IEnumerable<T5> fifthMemo;
                IEnumerable<T6> sixthMemo;
                IEnumerable<T7> seventhMemo;

                using ((secondMemo = second.Memoize()) as IDisposable)
                using ((thirdMemo = third.Memoize()) as IDisposable)
                using ((fourthMemo = fourth.Memoize()) as IDisposable)
                using ((fifthMemo = fifth.Memoize()) as IDisposable)
                using ((sixthMemo = sixth.Memoize()) as IDisposable)
                using ((seventhMemo = seventh.Memoize()) as IDisposable)
                {
                    foreach (var item1 in first)
                    foreach (var item2 in secondMemo)
                    foreach (var item3 in thirdMemo)
                    foreach (var item4 in fourthMemo)
                    foreach (var item5 in fifthMemo)
                    foreach (var item6 in sixthMemo)
                    foreach (var item7 in seventhMemo)
                        yield return resultSelector(item1, item2, item3, item4, item5, item6, item7);
                }
            }
        }

        /// <summary>
        /// Returns the Cartesian product of seven sequences by enumerating tuples
        /// of all possible combinations of one item from each sequence.
        /// </summary>
        /// <typeparam name="T1">The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="T2">The type of the elements of <paramref name="second"/>.</typeparam>
        /// <typeparam name="T3">The type of the elements of <paramref name="third"/>.</typeparam>
        /// <typeparam name="T4">The type of the elements of <paramref name="fourth"/>.</typeparam>
        /// <typeparam name="T5">The type of the elements of <paramref name="fifth"/>.</typeparam>
        /// <typeparam name="T6">The type of the elements of <paramref name="sixth"/>.</typeparam>
        /// <typeparam name="T7">The type of the elements of <paramref name="seventh"/>.</typeparam>
        /// <param name="first">The first sequence of elements.</param>
        /// <param name="second">The second sequence of elements.</param>
        /// <param name="third">The third sequence of elements.</param>
        /// <param name="fourth">The fourth sequence of elements.</param>
        /// <param name="fifth">The fifth sequence of elements.</param>
        /// <param name="sixth">The sixth sequence of elements.</param>
        /// <param name="seventh">The seventh sequence of elements.</param>
        /// <returns>A sequence of tuples.</returns>
        /// <remarks>
        /// <para>
        /// The method returns items in the same order as a nested foreach
        /// loop, but all sequences except for <paramref name="first"/> are
        /// cached when iterated over. The cache is then re-used for any
        /// subsequent iterations.</para>
        /// <para>
        /// This method uses deferred execution and stream its results.</para>
        /// </remarks>

        public static IEnumerable<(T1 First, T2 Second, T3 Third, T4 Fourth, T5 Fifth, T6 Sixth, T7 Seventh)>
            Cartesian<T1, T2, T3, T4, T5, T6, T7>(
                this IEnumerable<T1> first,
                IEnumerable<T2> second,
                IEnumerable<T3> third,
                IEnumerable<T4> fourth,
                IEnumerable<T5> fifth,
                IEnumerable<T6> sixth,
                IEnumerable<T7> seventh)
        {
            return Cartesian(first, second, third, fourth, fifth, sixth, seventh, ValueTuple.Create);
        }

        /// <summary>
        /// Returns the Cartesian product of eight sequences by enumerating all
        /// possible combinations of one item from each sequence, and applying
        /// a user-defined projection to the items in a given combination.
        /// </summary>
        /// <typeparam name="T1">
        /// The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="T2">
        /// The type of the elements of <paramref name="second"/>.</typeparam>
        /// <typeparam name="T3">
        /// The type of the elements of <paramref name="third"/>.</typeparam>
        /// <typeparam name="T4">
        /// The type of the elements of <paramref name="fourth"/>.</typeparam>
        /// <typeparam name="T5">
        /// The type of the elements of <paramref name="fifth"/>.</typeparam>
        /// <typeparam name="T6">
        /// The type of the elements of <paramref name="sixth"/>.</typeparam>
        /// <typeparam name="T7">
        /// The type of the elements of <paramref name="seventh"/>.</typeparam>
        /// <typeparam name="T8">
        /// The type of the elements of <paramref name="eighth"/>.</typeparam>
        /// <typeparam name="TResult">
        /// The type of the elements of the result sequence.</typeparam>
        /// <param name="first">The first sequence of elements.</param>
        /// <param name="second">The second sequence of elements.</param>
        /// <param name="third">The third sequence of elements.</param>
        /// <param name="fourth">The fourth sequence of elements.</param>
        /// <param name="fifth">The fifth sequence of elements.</param>
        /// <param name="sixth">The sixth sequence of elements.</param>
        /// <param name="seventh">The seventh sequence of elements.</param>
        /// <param name="eighth">The eighth sequence of elements.</param>
        /// <param name="resultSelector">A projection function that combines
        /// elements from all of the sequences.</param>
        /// <returns>A sequence of elements returned by
        /// <paramref name="resultSelector"/>.</returns>
        /// <remarks>
        /// <para>
        /// The method returns items in the same order as a nested foreach
        /// loop, but all sequences except for <paramref name="first"/> are
        /// cached when iterated over. The cache is then re-used for any
        /// subsequent iterations.</para>
        /// <para>
        /// This method uses deferred execution and stream its results.</para>
        /// </remarks>

        public static IEnumerable<TResult> Cartesian<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
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
                IEnumerable<T2> secondMemo;
                IEnumerable<T3> thirdMemo;
                IEnumerable<T4> fourthMemo;
                IEnumerable<T5> fifthMemo;
                IEnumerable<T6> sixthMemo;
                IEnumerable<T7> seventhMemo;
                IEnumerable<T8> eighthMemo;

                using ((secondMemo = second.Memoize()) as IDisposable)
                using ((thirdMemo = third.Memoize()) as IDisposable)
                using ((fourthMemo = fourth.Memoize()) as IDisposable)
                using ((fifthMemo = fifth.Memoize()) as IDisposable)
                using ((sixthMemo = sixth.Memoize()) as IDisposable)
                using ((seventhMemo = seventh.Memoize()) as IDisposable)
                using ((eighthMemo = eighth.Memoize()) as IDisposable)
                {
                    foreach (var item1 in first)
                    foreach (var item2 in secondMemo)
                    foreach (var item3 in thirdMemo)
                    foreach (var item4 in fourthMemo)
                    foreach (var item5 in fifthMemo)
                    foreach (var item6 in sixthMemo)
                    foreach (var item7 in seventhMemo)
                    foreach (var item8 in eighthMemo)
                        yield return resultSelector(item1, item2, item3, item4, item5, item6, item7, item8);
                }
            }
        }

        /// <summary>
        /// Returns the Cartesian product of eight sequences by enumerating tuples
        /// of all possible combinations of one item from each sequence.
        /// </summary>
        /// <typeparam name="T1">The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="T2">The type of the elements of <paramref name="second"/>.</typeparam>
        /// <typeparam name="T3">The type of the elements of <paramref name="third"/>.</typeparam>
        /// <typeparam name="T4">The type of the elements of <paramref name="fourth"/>.</typeparam>
        /// <typeparam name="T5">The type of the elements of <paramref name="fifth"/>.</typeparam>
        /// <typeparam name="T6">The type of the elements of <paramref name="sixth"/>.</typeparam>
        /// <typeparam name="T7">The type of the elements of <paramref name="seventh"/>.</typeparam>
        /// <typeparam name="T8">The type of the elements of <paramref name="eighth"/>.</typeparam>
        /// <param name="first">The first sequence of elements.</param>
        /// <param name="second">The second sequence of elements.</param>
        /// <param name="third">The third sequence of elements.</param>
        /// <param name="fourth">The fourth sequence of elements.</param>
        /// <param name="fifth">The fifth sequence of elements.</param>
        /// <param name="sixth">The sixth sequence of elements.</param>
        /// <param name="seventh">The seventh sequence of elements.</param>
        /// <param name="eighth">The eighth sequence of elements.</param>
        /// <returns>A sequence of tuples.</returns>
        /// <remarks>
        /// <para>
        /// The method returns items in the same order as a nested foreach
        /// loop, but all sequences except for <paramref name="first"/> are
        /// cached when iterated over. The cache is then re-used for any
        /// subsequent iterations.</para>
        /// <para>
        /// This method uses deferred execution and stream its results.</para>
        /// </remarks>

        public static IEnumerable<(T1 First, T2 Second, T3 Third, T4 Fourth, T5 Fifth, T6 Sixth, T7 Seventh, T8 Eighth)>
            Cartesian<T1, T2, T3, T4, T5, T6, T7, T8>(
                this IEnumerable<T1> first,
                IEnumerable<T2> second,
                IEnumerable<T3> third,
                IEnumerable<T4> fourth,
                IEnumerable<T5> fifth,
                IEnumerable<T6> sixth,
                IEnumerable<T7> seventh,
                IEnumerable<T8> eighth)
        {
            return Cartesian(first, second, third, fourth, fifth, sixth, seventh, eighth, ValueTuple.Create);
        }
    }
}
