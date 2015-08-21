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
        /// Returns the single element in the given sequence, or the result
        /// of executing a fallback delegate if the sequence is empty.
        /// This method throws an exception if there is more than one element in the sequence.
        /// </summary>
        /// <remarks>
        /// The fallback delegate is not executed if the sequence is non-empty.
        /// This operator uses immediate execution and has optimizations for <see cref="IList{T}"/> sources.
        /// </remarks>
        /// <typeparam name="TSource">Element type of sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="fallback">The fallback delegate to execute if the sequence is empty</param>
        /// <exception cref="ArgumentNullException">source or fallback is null</exception>
        /// <exception cref="InvalidOperationException">The sequence has more than one element</exception>
        /// <returns>The single element in the sequence, or the result of calling the
        /// fallback delegate if the sequence is empty.</returns>
        /// <example>
        /// <code>
        /// var numbers = { 123, 456, 789 };
        /// var result = numbers.Where(x => x == 100).SingleOrFallback(() => -1);
        /// </code>
        /// The <c>result</c> variable will contain <c>-1</c>.
        /// </example>

        public static TSource SingleOrFallback<TSource>(this IEnumerable<TSource> source, Func<TSource> fallback)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (fallback == null) throw new ArgumentNullException("fallback");

            var list = source as IList<TSource>;
            if (list != null)
            {
                switch (list.Count)
                {
                    case 0:
                        return fallback();

                    case 1:
                        return list[0];

                    // anything but 0 and 1 is not handled
                }
            }
            else
            {
                using (var iterator = source.GetEnumerator())
                {
                    if (!iterator.MoveNext())
                    {
                        return fallback();
                    }
                    var first = iterator.Current;

                    // Return if there's no next element
                    if (!iterator.MoveNext())
                    {
                        return first;
                    }
                }
            }

            // We should have checked the sequence length and returned by now
            throw new InvalidOperationException("Sequence contains more than one element");
        }
    }
}
