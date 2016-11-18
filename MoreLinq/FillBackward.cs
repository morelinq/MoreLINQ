#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Atif Aziz. All rights reserved.
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
        /// Returns a sequence with each null reference or value in the source
        /// replaced with the following non-null reference or value in
        /// that sequence.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> with null references or values
        /// replaced.
        /// </returns>
        /// <remarks>
        /// This method uses deferred execution semantics and streams its
        /// results. If references or values are null at the end of the
        /// sequence then they remain null.
        /// </remarks>

        public static IEnumerable<T> FillBackward<T>(this IEnumerable<T> source)
        {
            return source.FillBackward(e => e == null);
        }

        /// <summary>
        /// Returns a sequence with each missing element in the source replaced
        /// with the following non-missing element in that sequence. An
        /// additional parameter specified a function used to determine if an
        /// element is considered missing or not.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="predicate">The function used to determine if
        /// an element in the sequence is considered missing.</param>
        /// <typeparam name="T">Type of the elements in the source sequence.</typeparam>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> with missing values replaced.
        /// </returns>
        /// <remarks>
        /// This method uses deferred execution semantics and streams its
        /// results. If elements are missing at the end of the sequence then
        /// they remain missing.
        /// </remarks>

        public static IEnumerable<T> FillBackward<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");

            return FillBackwardImpl(source, predicate);
        }

        static IEnumerable<T> FillBackwardImpl<T>(IEnumerable<T> source, Func<T, bool> predicate)
        {
            List<T> blanks = null;

            foreach (var item in source)
            {
                var blank = predicate(item);
                if (blank)
                {
                    (blanks ?? (blanks = new List<T>())).Add(item);
                }
                else
                {
                    var count = blanks?.Count ?? 0;
                    blanks?.Clear();
                    while (count-- >= 0)
                        yield return item;
                }
            }

            if (blanks?.Count > 0)
            {
                foreach (var blank in blanks)
                    yield return blank;
            }
        }
    }
}
