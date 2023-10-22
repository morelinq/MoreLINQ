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
    using System.Linq;

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Creates a left-aligned sliding window of a given size over the
        /// source sequence.
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
        /// especially as it slides over the end of the sequence.</para>
        /// <para>
        /// This operator uses deferred execution and streams its results.</para>
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// Console.WriteLine(
        ///     Enumerable
        ///         .Range(1, 5)
        ///         .WindowLeft(3)
        ///         .Select(w => "AVG(" + w.ToDelimitedString(",") + ") = " + w.Average())
        ///         .ToDelimitedString(Environment.NewLine));
        ///
        /// // Output:
        /// // AVG(1,2,3) = 2
        /// // AVG(2,3,4) = 3
        /// // AVG(3,4,5) = 4
        /// // AVG(4,5) = 4.5
        /// // AVG(5) = 5
        /// ]]></code>
        /// </example>

        public static IEnumerable<IList<TSource>> WindowLeft<TSource>(this IEnumerable<TSource> source, int size)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

            return _(); IEnumerable<IList<TSource>> _()
            {
                var window = new List<TSource>();
                foreach (var item in source)
                {
                    window.Add(item);
                    if (window.Count < size)
                        continue;

                    // prepare next window before exposing data
                    var nextWindow = new List<TSource>(window.Skip(1));
                    yield return window;
                    window = nextWindow;
                }
                while (window.Count > 0)
                {
                    // prepare next window before exposing data
                    var nextWindow = new List<TSource>(window.Skip(1));
                    yield return window;
                    window = nextWindow;
                }
            }
        }
    }
}
