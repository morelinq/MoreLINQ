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
    using System.Linq;

    static partial class MoreEnumerable
    {
   //   static Func<Exception, bool> defaultErrorPredicate = (Exception ex) => false;

        /// <summary>
        /// Ignore some exceptions that can occurs during the iteration of the sequence.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TException">Type of the exception that can be ignored.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>
        /// A sequence of elements <paramref name="source"/> that ignore the exceptions of type
        /// <typeparamref name="TException"/>.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<T> IgnoreErrors<T, TException>(
            this IEnumerable<T> source)
            where TException : Exception
            => IgnoreErrorsImpl<T, TException, TException, TException>(source, null, null, null);

        /// <summary>
        /// Ignore some exceptions that can occurs during the iteration of the sequence.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TException1">First type of the exception that can be ignored.</typeparam>
        /// <typeparam name="TException2">Second type of the exception that can be ignored.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>
        /// A sequence of elements <paramref name="source"/> that ignore the exceptions of type
        /// <typeparamref name="TException1"/> or <typeparamref name="TException2"/>.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// Exceptions are caught in order which were passed, as well in a catch block.
        /// </remarks>

        public static IEnumerable<T> IgnoreErrors<T, TException1, TException2>(
            this IEnumerable<T> source)
            where TException1 : Exception
            where TException2 : Exception
            => IgnoreErrorsImpl<T, TException1, TException2, TException2>(source, null, null, null);

        /// <summary>
        /// Ignore some exceptions that can occurs during the iteration of the sequence.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TException1">First type of the exception that can be ignored.</typeparam>
        /// <typeparam name="TException2">Second type of the exception that can be ignored.</typeparam>
        /// <typeparam name="TException3">Third type of the exception that can be ignored.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>
        /// A sequence of elements <paramref name="source"/> that ignore the exceptions of type
        /// <typeparamref name="TException1"/>, <typeparamref name="TException2"/> or <typeparamref name="TException3"/>.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// Exceptions are caught in order which were passed, as well in a catch block.
        /// </remarks>

        public static IEnumerable<T> IgnoreErrors<T, TException1, TException2, TException3>(
            this IEnumerable<T> source)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
            => IgnoreErrorsImpl<T, TException1, TException2, TException3>(source, null, null, null);

        /// <summary>
        /// Ignore some exceptions that can occurs during the iteration of the sequence.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TException">Type of the exception that can be ignored.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="errorPredicate">
        /// A function that receives the exceptions of type <typeparamref name="TException"/>
        /// that occurs during the iteration and indicates if it should be ignored or not.
        /// </param>
        /// <returns>
        /// A sequence of elements <paramref name="source"/> that ignore the exceptions of type
        /// <typeparamref name="TException"/> in accordance with <paramref name="errorPredicate"/>.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<T> IgnoreErrors<T, TException>(
            this IEnumerable<T> source,
            Func<TException, bool> errorPredicate)
            where TException : Exception
        {
            if (errorPredicate == null) throw new ArgumentNullException(nameof(errorPredicate));

            return IgnoreErrorsImpl(source, errorPredicate, errorPredicate, errorPredicate);
        }

        /// <summary>
        /// Ignore some exceptions that can occurs during the iteration of the sequence.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TException1">First type of the exception that can be ignored.</typeparam>
        /// <typeparam name="TException2">Second type of the exception that can be ignored.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="error1Predicate">
        /// A function that receives the exceptions of type <typeparamref name="TException1"/>
        /// that occurs during the iteration and indicates if it should be ignored or not.
        /// </param>
        /// <param name="error2Predicate">
        /// A function that receives the exceptions of type <typeparamref name="TException2"/>
        /// that occurs during the iteration and indicates if it should be ignored or not.
        /// </param>
        /// <returns>
        /// A sequence of elements <paramref name="source"/> that ignore the exceptions of type
        /// <typeparamref name="TException1"/> or <typeparamref name="TException2"/>
        /// in accordance with predicates.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// Exceptions are caught in order which predicates were passed, as well in a catch block.
        /// </remarks>

        public static IEnumerable<T> IgnoreErrors<T, TException1, TException2>(
            this IEnumerable<T> source,
            Func<TException1, bool> error1Predicate,
            Func<TException2, bool> error2Predicate)
            where TException1 : Exception
            where TException2 : Exception
        {
            if (error1Predicate == null) throw new ArgumentNullException(nameof(error1Predicate));
            if (error2Predicate == null) throw new ArgumentNullException(nameof(error2Predicate));

            return IgnoreErrorsImpl(source, error1Predicate, error2Predicate, error2Predicate);
        }

        /// <summary>
        /// Ignore some exceptions that can occurs during the iteration of the sequence.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TException1">First type of the exception that can be ignored.</typeparam>
        /// <typeparam name="TException2">Second type of the exception that can be ignored.</typeparam>
        /// <typeparam name="TException3">Third type of the exception that can be ignored.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="error1Predicate">
        /// A function that receives the exceptions of type <typeparamref name="TException1"/>
        /// that occurs during the iteration and indicates if it should be ignored or not.
        /// </param>
        /// <param name="error2Predicate">
        /// A function that receives the exceptions of type <typeparamref name="TException2"/>
        /// that occurs during the iteration and indicates if it should be ignored or not.
        /// </param>
        /// <param name="error3Predicate">
        /// A function that receives the exceptions of type <typeparamref name="TException3"/>
        /// that occurs during the iteration and indicates if it should be ignored or not.
        /// </param>
        /// <returns>
        /// A sequence of elements <paramref name="source"/> that ignore the exceptions of type
        /// <typeparamref name="TException1"/>, <typeparamref name="TException2"/> or <typeparamref name="TException3"/>
        /// in accordance with predicates.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// Exceptions are caught in order which predicates were passed, as well in a catch block.
        /// </remarks>

        public static IEnumerable<T> IgnoreErrors<T, TException1, TException2, TException3>(
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

            return IgnoreErrorsImpl(source, error1Predicate, error2Predicate, error3Predicate);
        }

        static IEnumerable<T> IgnoreErrorsImpl<T, TException1, TException2, TException3>(
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
                            if (!e.MoveNext()) break;
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
