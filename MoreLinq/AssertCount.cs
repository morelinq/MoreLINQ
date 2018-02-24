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
        #if MORELINQ

        static readonly Func<int, int, Exception> DefaultErrorSelector = OnAssertCountFailure;

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
            AssertCountImpl(source, count, DefaultErrorSelector);

        /// <summary>
        /// Asserts that a source sequence contains a given count of elements.
        /// A parameter specifies the exception to be thrown.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Count to assert.</param>
        /// <param name="errorSelector">Function that returns the <see cref="Exception"/> object to throw.</param>
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
            int count, Func<int, int, Exception> errorSelector) =>
            AssertCountImpl(source, count, errorSelector);

        static Exception OnAssertCountFailure(int cmp, int count)
        {
            var message = cmp < 0 
                        ? "Sequence contains too few elements when exactly {0} were expected."
                        : "Sequence contains too many elements when exactly {0} were expected.";
            return new SequenceException(string.Format(message, count.ToString("N0")));
        }

        #endif

        static IEnumerable<TSource> AssertCountImpl<TSource>(IEnumerable<TSource> source,
            int count, Func<int, int, Exception> errorSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));

            return
                source.TryGetCollectionCount() is int collectionCount
                ? collectionCount == count
                  ? source
                  : From<TSource>(() => throw errorSelector(collectionCount.CompareTo(count), count))
                : _(); IEnumerable<TSource> _()
                {
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
    }
}
