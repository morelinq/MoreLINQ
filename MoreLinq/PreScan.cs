#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2009 Konrad Rudolph. All rights reserved.
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
        /// Performs a pre-scan (exclusive prefix sum) on a sequence of elements.
        /// </summary>
        /// <remarks>
        /// An exclusive prefix sum returns an equal-length sequence where the
        /// N-th element is the sum of the first N-1 input elements (the first
        /// element is a special case, it is set to the identity). More
        /// generally, the pre-scan allows any commutative binary operation,
        /// not just a sum.
        /// The inclusive version of PreScan is <see cref="MoreEnumerable.Scan{TSource}"/>.
        /// This operator uses deferred execution and streams its result.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// int[] values = { 1, 2, 3, 4 };
        /// var prescan = values.PreScan((a, b) => a + b, 0);
        /// var scan = values.Scan((a, b) => a + b);
        /// var result = values.EquiZip(prescan, ValueTuple.Create);
        /// ]]></code>
        /// <c>prescan</c> will yield <c>{ 0, 1, 3, 6 }</c>, while <c>scan</c>
        /// and <c>result</c> will both yield <c>{ 1, 3, 6, 10 }</c>. This
        /// shows the relationship between the inclusive and exclusive prefix sum.
        /// </example>
        /// <typeparam name="TSource">Type of elements in source sequence</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="transformation">Transformation operation</param>
        /// <param name="identity">Identity element (see remarks)</param>
        /// <returns>The scanned sequence</returns>

        public static IEnumerable<TSource> PreScan<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, TSource> transformation,
            TSource identity)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (transformation == null) throw new ArgumentNullException(nameof(transformation));

            return _(); IEnumerable<TSource> _()
            {
                var aggregator = identity;

                using (var e = source.GetEnumerator())
                {
                    if (e.MoveNext())
                    {
                        yield return aggregator;
                        var current = e.Current;

                        while (e.MoveNext())
                        {
                            aggregator = transformation(aggregator, current);
                            yield return aggregator;
                            current = e.Current;
                        }
                    }
                }
            }
        }
    }
}
