#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2013 Atif Aziz. All rights reserved.
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

    partial class MoreEnumerable
    {
        /// <summary>
        /// Returns a sequence resulting from applying a function to each
        /// element in the source sequence with additional parameters
        /// indicating whether the element is the first and/or last of the
        /// sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the element of the returned sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="resultSelector">A function that determines how to
        /// project the each element along with its first or last tag.</param>
        /// <returns>
        /// Returns the resulting sequence.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// var numbers = new[] { 123, 456, 789 };
        /// var result = numbers.TagFirstLast((num, fst, lst) => new
        ///              {
        ///                  Number = num,
        ///                  IsFirst = fst, IsLast = lst
        ///              });
        /// ]]></code>
        /// The <c>result</c> variable, when iterated over, will yield
        /// <c>{ Number = 123, IsFirst = True, IsLast = False }</c>,
        /// <c>{ Number = 456, IsFirst = False, IsLast = False }</c> and
        /// <c>{ Number = 789, IsFirst = False, IsLast = True }</c> in turn.
        /// </example>

        public static IEnumerable<TResult> TagFirstLast<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, bool, bool, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            return _(); IEnumerable<TResult> _()
            {
                using var enumerator = source.GetEnumerator();

                if (!enumerator.MoveNext())
                    yield break;

                var current = enumerator.Current;
                var hasNext = enumerator.MoveNext();
                yield return resultSelector(current, true, !hasNext);

                while (hasNext)
                {
                    current = enumerator.Current;
                    hasNext = enumerator.MoveNext();
                    yield return resultSelector(current, false, !hasNext);
                }
            }
        }

        /// <summary>
        /// Returns a sequence of tuples, where the N-th tuple contains the N-th
        /// element of the source sequence and two booleans indicating whether the
        /// element is the first and/or last.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>
        /// Returns the resulting sequence.
        /// </returns>
        /// <remarks>
        /// This operator uses deferred execution and streams its results.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// var numbers = new[] { 123, 456, 789 };
        /// var result = numbers.TagFirstLast();
        /// ]]></code>
        /// The <c>result</c> variable, when iterated over, will yield
        /// <c>(123, True, False)</c>, <c>(456, False, False)</c> and
        /// <c>(789, False, True)</c> in turn.
        /// </example>

        public static IEnumerable<(TSource Item, bool IsFirst, bool IsLast)> TagFirstLast<TSource>(this IEnumerable<TSource> source)
        {
            return TagFirstLast(source, ValueTuple.Create);
        }
    }
}
