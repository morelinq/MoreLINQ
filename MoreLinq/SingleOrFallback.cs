#region License and Terms
//
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008-9 Jonathan Skeet. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using System.Collections.Generic;

namespace MoreLinq
{
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Returns the single element in the given sequence, or the result
        /// of executing a fallback delegate if the sequence is empty.
        /// </summary>
        /// <remarks>
        /// The fallback delegate is not executed if the sequence is non-empty.
        /// </remarks>
        /// <typeparam name="T">Element type of sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="fallback">The fallback delegate to execute if the sequence is empty</param>
        /// <exception cref="ArgumentNullException">source or fallback is null</exception>
        /// <exception cref="InvalidOperationException">The sequence has more than one element</exception>
        /// <returns>The single element in the sequence, or the result of calling the
        /// fallback delegate if the sequence is empty.</returns>

        public static T SingleOrFallback<T>(this IEnumerable<T> source, Func<T> fallback)
        {
            source.ThrowIfNull("source");
            fallback.ThrowIfNull("fallback");
            using (IEnumerator<T> iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                {
                    return fallback();
                }
                T first = iterator.Current;
                if (iterator.MoveNext())
                {
                    throw new InvalidOperationException();
                }
                return first;
            }
        }
    }
}
