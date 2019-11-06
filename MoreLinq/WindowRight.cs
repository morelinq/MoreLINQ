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
    using System.Linq;

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Creates a right-aligned sliding window over the source sequence
        /// of a given size.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">
        /// The sequence over which to create the sliding window.</param>
        /// <param name="size">Size of the sliding window.</param>
        /// <returns>A sequence representing each sliding window.</returns>
        /// <remarks>
        /// <para>
        /// A window can contain fewer elements than <paramref name="size"/>,
        /// especially as it slides over the start of the sequence.</para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// Console.WriteLine(
        ///     Enumerable
        ///         .Range(1, 5)
        ///         .WindowRight(3)
        ///         .Select(w => "AVG(" + w.ToDelimitedString(",") + ") = " + w.Average())
        ///         .ToDelimitedString(Environment.NewLine));
        ///
        /// // Output:
        /// // AVG(1) = 1
        /// // AVG(1,2) = 1.5
        /// // AVG(1,2,3) = 2
        /// // AVG(2,3,4) = 3
        /// // AVG(3,4,5) = 4
        /// ]]></code>
        /// </example>

        public static IEnumerable<IList<TSource>> WindowRight<TSource>(this IEnumerable<TSource> source, int size)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (size < 0) throw new ArgumentOutOfRangeException(nameof(size));

            return source.WindowRightWhile((_, i) => i < size);
        }

        /// <summary>
        /// Creates a right-aligned sliding window over the source sequence
        /// with a predicate function determining the window range.
        /// </summary>

        static IEnumerable<IList<TSource>> WindowRightWhile<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return _(); IEnumerable<IList<TSource>> _()
            {
                var window = new List<TSource>();
                foreach (var item in source)
                {
                    window.Add(item);

                    // prepare next window before exposing data
                    var nextWindow = new List<TSource>(predicate(item, window.Count) ? window : window.Skip(1));
                    yield return window;
                    window = nextWindow;
                }
            }
        }
    }
}
