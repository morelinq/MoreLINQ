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
        /// Returns a projection of tuples, where the N-th tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to elements combined from each sequence.</param>
        /// <returns>
        /// A projection of tuples, where the N-th tuple contains the N-th element
        /// from each of the argument sequences.</returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TResult> ZipLongest<T1, T2, TResult>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            Func<T1, T2, TResult> resultSelector)
        {
            if (first == null) throw new ArgumentNullException(nameof(first));
            if (second == null) throw new ArgumentNullException(nameof(second));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                IEnumerator<T1> e1 = null;
                IEnumerator<T2> e2 = null;

                try
                {
                    e1 = first.GetEnumerator();
                    e2 = second.GetEnumerator();

                    var v1 = default(T1);
                    var v2 = default(T2);

                    // | is used instead of || in purpose. All operands have to be evaluated.
                    while (
                        Enumerator.Read(ref e1, ref v1) |
                        Enumerator.Read(ref e2, ref v2))
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
        /// Returns a projection of tuples, where the N-th tuple contains the N-th
        /// element from each of the input sequences. The resulting sequence
        /// will always be as long as the longest of input sequences where the
        /// default value of each of the shorter sequence element types is used
        /// for padding.
        /// </summary>
        /// <typeparam name="T1">Type of elements in first input sequence.</typeparam>
        /// <typeparam name="T2">Type of elements in second input sequence.</typeparam>
        /// <typeparam name="T3">Type of elements in third input sequence.</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence.</typeparam>
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="third">The third source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to elements combined from each sequence.</param>
        /// <returns>
        /// A projection of tuples, where the N-th tuple contains the N-th element
        /// from each of the argument sequences.</returns>
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

            return _(); IEnumerable<TResult> _()
            {
                IEnumerator<T1> e1 = null;
                IEnumerator<T2> e2 = null;
                IEnumerator<T3> e3 = null;

                try
                {
                    e1 = first.GetEnumerator();
                    e2 = second.GetEnumerator();
                    e3 = third.GetEnumerator();

                    var v1 = default(T1);
                    var v2 = default(T2);
                    var v3 = default(T3);

                    // | is used instead of || in purpose. All operands have to be evaluated.
                    while (
                        Enumerator.Read(ref e1, ref v1) |
                        Enumerator.Read(ref e2, ref v2) |
                        Enumerator.Read(ref e3, ref v3))
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
        /// Returns a projection of tuples, where the N-th tuple contains the N-th
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
        /// <param name="first">The first source sequence.</param>
        /// <param name="second">The second source sequence.</param>
        /// <param name="third">The third source sequence.</param>
        /// <param name="fourth">The fourth source sequence.</param>
        /// <param name="resultSelector">
        /// Function to apply to elements combined from each sequence.</param>
        /// <returns>
        /// A projection of tuples, where the N-th tuple contains the N-th element
        /// from each of the argument sequences.</returns>
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

            return _(); IEnumerable<TResult> _()
            {
                IEnumerator<T1> e1 = null;
                IEnumerator<T2> e2 = null;
                IEnumerator<T3> e3 = null;
                IEnumerator<T4> e4 = null;

                try
                {
                    e1 = first.GetEnumerator();
                    e2 = second.GetEnumerator();
                    e3 = third.GetEnumerator();
                    e4 = fourth.GetEnumerator();

                    var v1 = default(T1);
                    var v2 = default(T2);
                    var v3 = default(T3);
                    var v4 = default(T4);

                    // | is used instead of || in purpose. All operands have to be evaluated.
                    while (
                        Enumerator.Read(ref e1, ref v1) |
                        Enumerator.Read(ref e2, ref v2) |
                        Enumerator.Read(ref e3, ref v3) |
                        Enumerator.Read(ref e4, ref v4))
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

    }
}
