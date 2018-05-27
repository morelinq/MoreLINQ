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
        static Func<Exception, bool> defaultErrorPredicate = (Exception ex) => false;

        /// <summary>
        /// Applies a key-generating function to each element of a sequence and returns a sequence of
        /// unique keys and their number of occurrences in the original sequence.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TException">Type of the elements of the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="errorPredicate">Source sequence.</param>
        /// <returns>A sequence of unique keys and their number of occurrences in the original sequence.</returns>
        public static IEnumerable<T> IgnoreErrors<T, TException>(
            this IEnumerable<T> source,
            Func<TException, bool> errorPredicate)
            where TException : Exception
            => IgnoreErrors(source, errorPredicate ?? throw new ArgumentNullException(nameof(errorPredicate)),
                            defaultErrorPredicate, defaultErrorPredicate);

        /// <summary>
        /// Applies a key-generating function to each element of a sequence and returns a sequence of
        /// unique keys and their number of occurrences in the original sequence.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TException1">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TException2">Type of the elements of the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="error1Predicate">Source sequence.</param>
        /// <param name="error2Predicate">Source sequence.</param>
        /// <returns>A sequence of unique keys and their number of occurrences in the original sequence.</returns>
        public static IEnumerable<T> IgnoreErrors<T, TException1, TException2>(
            this IEnumerable<T> source,
            Func<TException1, bool> error1Predicate,
            Func<TException2, bool> error2Predicate)
            where TException1 : Exception
            where TException2 : Exception
            => IgnoreErrors(source, error1Predicate, error2Predicate, defaultErrorPredicate);

        /// <summary>
        /// Applies a key-generating function to each element of a sequence and returns a sequence of
        /// unique keys and their number of occurrences in the original sequence.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TException1">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TException2">Type of the elements of the source sequence.</typeparam>
        /// <typeparam name="TException3">Type of the elements of the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="error1Predicate">Source sequence.</param>
        /// <param name="error2Predicate">Source sequence.</param>
        /// <param name="error3Predicate">Source sequence.</param>
        /// <returns>A sequence of unique keys and their number of occurrences in the original sequence.</returns>
        public static IEnumerable<T> IgnoreErrors<T, TException1, TException2, TException3>(
            this IEnumerable<T> source,
            Func<TException1, bool> error1Predicate,
            Func<TException2, bool> error2Predicate,
            Func<TException3, bool> error3Predicate)
            where TException1 : Exception
            where TException2 : Exception
            where TException3 : Exception
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (error1Predicate == null) throw new ArgumentNullException(nameof(error1Predicate));
            if (error2Predicate == null) throw new ArgumentNullException(nameof(error2Predicate));
            if (error3Predicate == null) throw new ArgumentNullException(nameof(error3Predicate));

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
                        catch (TException1 ex)
                        {
                            if (!error1Predicate(ex)) throw;
                            continue;
                        }
                        catch (TException2 ex)
                        {
                            if (!error2Predicate(ex)) throw;
                            continue;
                        }
                        catch (TException3 ex)
                        {
                            if (!error3Predicate(ex)) throw;
                            continue;
                        }

                        yield return e.Current;
                    }
                }
            }
        }
    }
}
