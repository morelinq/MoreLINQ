#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2009 Atif Aziz. All rights reserved.
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
        /// Asserts that a source sequence contains a given count of elements.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Count to assert.</param>
        /// <returns>
        /// Returns the original sequence as long it is contains the
        /// number of elements specified by <paramref name="count"/>.
        /// Otherwise it throws <see cref="Exception" />.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TSource> AssertCount<TSource>(this IEnumerable<TSource> source, int count) =>
            AssertCount(source, count, static (cmp, count) => new SequenceException(FormatSequenceLengthErrorMessage(cmp, count)));

        /// <summary>
        /// Asserts that a source sequence contains a given count of elements.
        /// A parameter specifies the exception to be thrown.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Count to assert.</param>
        /// <param name="errorSelector">
        /// Function that receives a comparison (a negative integer if actual
        /// count is less than <paramref name="count"/> and a positive integer
        /// if actual count is greater than <paramref name="count"/>) and
        /// <paramref name="count"/> as arguments and which returns the
        /// <see cref="Exception"/> object to throw.</param>
        /// <returns>
        /// Returns the original sequence as long it is contains the
        /// number of elements specified by <paramref name="count"/>.
        /// Otherwise it throws the <see cref="Exception" /> object
        /// returned by calling <paramref name="errorSelector"/>.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>

        public static IEnumerable<TSource> AssertCount<TSource>(this IEnumerable<TSource> source,
            int count, Func<int, int, Exception> errorSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));

            return _(); IEnumerable<TSource> _()
            {
                if (source.TryAsCollectionLike() is { Count: var collectionCount }
                    && collectionCount.CompareTo(count) is var comparison && comparison != 0)
                {
                    throw errorSelector(comparison, count);
                }

                var iterations = 0;
                foreach (var element in source)
                {
                    iterations++;
                    if (iterations > count)
                        throw errorSelector(1, count);
                    yield return element;
                }
                if (iterations != count)
                    throw errorSelector(-1, count);
            }
        }

        internal static string FormatSequenceLengthErrorMessage(int cmp, int count) =>
            $"Sequence contains too {(cmp < 0 ? "few" : "many")} elements when exactly {count:N0} {(count == 1 ? "was" : "were")} expected.";
    }
}
