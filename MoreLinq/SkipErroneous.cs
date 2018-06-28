#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Leandro F. Vieira (leandromoh). All rights reserved.
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
        /// Skips elements of a sequence that cause a type of exception during
        /// iteration.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">
        /// Type or sub-type of exception to tolerate.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>
        /// A sequence of elements from <paramref name="source"/> except those
        /// that caused an exception of type or sub-type of
        /// <typeparamref name="TException"/>.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<T> SkipErroneous<T, TException>(
            this IEnumerable<T> source)
            where TException : Exception
            => SkipErroneousImpl<T, TException, TException, TException>(source, null, null, null);

        /// <summary>
        /// Skips elements of a sequence that cause one of two types of
        /// exceptions during iteration.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException1">
        /// First type or sub-type of exception to tolerate.</typeparam>
        /// <typeparam name="TException2">
        /// Second type or sub-type of exception to tolerate.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>
        /// A sequence of elements from <paramref name="source"/> except those
        /// that caused an exception of type or sub-type of
        /// either <typeparamref name="TException1"/> or
        /// <typeparamref name="TException2"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// <para>
        /// Exceptions are caught in the order in which they are specified
        /// as type parameters, from most specific
        /// (<typeparamref name="TException1"/>) to most general
        /// (<typeparamref name="TException2"/>).</para>
        /// </remarks>

        public static IEnumerable<T> SkipErroneous<T, TException1, TException2>(
            this IEnumerable<T> source)
            where TException1 : Exception
            where TException2 : Exception
            => SkipErroneousImpl<T, TException1, TException2, TException2>(source, null, null, null);

        /// <summary>
        /// Skips elements of a sequence that cause one of three types of
        /// exceptions during iteration.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException1">
        /// First type or sub-typeof exception to tolerate.</typeparam>
        /// <typeparam name="TException2">
        /// Second type or sub-typeof exception to tolerate.</typeparam>
        /// <typeparam name="TException3">
        /// Third type or sub-type of exception to tolerate.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>
        /// A sequence of elements from <paramref name="source"/> except those
        /// that caused an exception of type or sub-type of
        /// either <typeparamref name="TException1"/>,
        /// <typeparamref name="TException2"/> or
        /// <typeparamref name="TException3"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// <para>
        /// Exceptions are caught in the order in which they are specified
        /// as type parameters, from most specific
        /// (<typeparamref name="TException1"/>) to most general
        /// (<typeparamref name="TException3"/>).</para>
        /// </remarks>

        public static IEnumerable<T> SkipErroneous<T, TException1, TException2, TException3>(
            this IEnumerable<T> source)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
            => SkipErroneousImpl<T, TException1, TException2, TException3>(source, null, null, null);

        /// <summary>
        /// Skips elements of a sequence that cause a type of exception of some
        /// condition during iteration.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException">
        /// Type or sub-type of exception to tolerate.</typeparam>
        /// <param name="errorPredicate">
        /// A function that receives exceptions of type or sub-type of
        /// <typeparamref name="TException"/> and determines if it should be
        /// tolerated or not.</param>
        /// <param name="source">The source sequence.</param>
        /// <returns>
        /// A sequence of elements from <paramref name="source"/> except those
        /// that conditionally caused an exception of type or sub-type of
        /// <typeparamref name="TException"/>.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<T> SkipErroneous<T, TException>(
            this IEnumerable<T> source,
            Func<TException, bool> errorPredicate)
            where TException : Exception
        {
            if (errorPredicate == null) throw new ArgumentNullException(nameof(errorPredicate));

            return SkipErroneousImpl(source, errorPredicate, errorPredicate, errorPredicate);
        }

        /// <summary>
        /// Skips elements of a sequence that cause one of two types of
        /// exceptions of some condition during iteration.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException1">
        /// First type or sub-typeof exception to tolerate.</typeparam>
        /// <typeparam name="TException2">
        /// Second type or sub-typeof exception to tolerate.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="error1Predicate">
        /// A function that receives exceptions of type or sub-type of
        /// <typeparamref name="TException1"/> and determines if it should be
        /// tolerated or not.</param>
        /// <param name="error2Predicate">
        /// A function that receives exceptions of type or sub-type of
        /// <typeparamref name="TException2"/> and determines if it should be
        /// tolerated or not.</param>
        /// <returns>
        /// A sequence of elements from <paramref name="source"/> except those
        /// that conditionally caused an exception of type or sub-type of
        /// either <typeparamref name="TException1"/> or
        /// <typeparamref name="TException2"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// <para>
        /// Exceptions are caught in the order in which they are specified
        /// as type parameters, from most specific
        /// (<typeparamref name="TException1"/>) to most general
        /// (<typeparamref name="TException2"/>).</para>
        /// </remarks>

        public static IEnumerable<T> SkipErroneous<T, TException1, TException2>(
            this IEnumerable<T> source,
            Func<TException1, bool> error1Predicate,
            Func<TException2, bool> error2Predicate)
            where TException1 : Exception
            where TException2 : Exception
        {
            if (error1Predicate == null) throw new ArgumentNullException(nameof(error1Predicate));
            if (error2Predicate == null) throw new ArgumentNullException(nameof(error2Predicate));

            return SkipErroneousImpl(source, error1Predicate, error2Predicate, error2Predicate);
        }

        /// <summary>
        /// Skips elements of a sequence that cause one of three types of
        /// exceptions of some condition during iteration.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TException1">
        /// First type or sub-typeof exception to tolerate.</typeparam>
        /// <typeparam name="TException2">
        /// Second type or sub-typeof exception to tolerate.</typeparam>
        /// <typeparam name="TException3">
        /// Second type or sub-typeof exception to tolerate.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="error1Predicate">
        /// A function that receives exceptions of type or sub-type of
        /// <typeparamref name="TException1"/> and determines if it should be
        /// tolerated or not.</param>
        /// <param name="error2Predicate">
        /// A function that receives exceptions of type or sub-type of
        /// <typeparamref name="TException2"/> and determines if it should be
        /// tolerated or not.</param>
        /// <param name="error3Predicate">
        /// A function that receives exceptions of type or sub-type of
        /// <typeparamref name="TException3"/> and determines if it should be
        /// tolerated or not.</param>
        /// <returns>
        /// A sequence of elements from <paramref name="source"/> except those
        /// that conditionally caused an exception of type or sub-type of
        /// either <typeparamref name="TException1"/>,
        /// <typeparamref name="TException2"/> or
        /// <typeparamref name="TException3"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// <para>
        /// Exceptions are caught in the order in which they are specified
        /// as type parameters, from most specific
        /// (<typeparamref name="TException1"/>) to most general
        /// (<typeparamref name="TException3"/>).</para>
        /// </remarks>

        public static IEnumerable<T> SkipErroneous<T, TException1, TException2, TException3>(
            this IEnumerable<T> source,
            Func<TException1, bool> error1Predicate,
            Func<TException2, bool> error2Predicate,
            Func<TException3, bool> error3Predicate)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
        {
            if (error1Predicate == null) throw new ArgumentNullException(nameof(error1Predicate));
            if (error2Predicate == null) throw new ArgumentNullException(nameof(error2Predicate));
            if (error3Predicate == null) throw new ArgumentNullException(nameof(error3Predicate));

            return SkipErroneousImpl(source, error1Predicate, error2Predicate, error3Predicate);
        }

        static IEnumerable<T> SkipErroneousImpl<T, TException1, TException2, TException3>(
            this IEnumerable<T> source,
            Func<TException1, bool> error1Predicate,
            Func<TException2, bool> error2Predicate,
            Func<TException3, bool> error3Predicate)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return _(); IEnumerable<T> _()
            {
                using (var e = source.GetEnumerator())
                {
                    while (true)
                    {
                        try
                        {
                            if (!e.MoveNext())
                            {
                                break;
                            }
                        }
                        catch (TException1 ex) when (error1Predicate?.Invoke(ex) ?? true)
                        {
                            continue;
                        }
                        catch (TException2 ex) when (error2Predicate?.Invoke(ex) ?? true)
                        {
                            continue;
                        }
                        catch (TException3 ex) when (error3Predicate?.Invoke(ex) ?? true)
                        {
                            continue;
                        }

                        yield return e.Current;
                    }
                }
            }
        }
    }
}
