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
    using System.Linq;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// Performs a scan (inclusive prefix sum) on a sequence of elements.
        /// </summary>
        /// <remarks>
        /// An inclusive prefix sum returns an equal-length sequence where the
        /// N-th element is the sum of the first N input elements. More
        /// generally, the scan allows any commutative binary operation, not
        /// just a sum.
        /// The exclusive version of Scan is <see cref="MoreEnumerable.PreScan{TSource}"/>.
        /// This operator uses deferred execution and streams its result.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// int[] values = { 1, 2, 3, 4 };
        /// var prescan = values.PreScan((a, b) => a + b, 0);
        /// var scan = values.Scan((a, b) => a + b);
        /// var result = values.EquiZip(scan, ValueTuple.Create);
        /// ]]></code>
        /// <c>prescan</c> will yield <c>{ 0, 1, 3, 6 }</c>, while <c>scan</c>
        /// and <c>result</c> will both yield <c>{ 1, 3, 6, 10 }</c>. This
        /// shows the relationship between the inclusive and exclusive prefix sum.
        /// </example>
        /// <typeparam name="TSource">Type of elements in source sequence</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="transformation">Transformation operation</param>
        /// <returns>The scanned sequence</returns>

        public static IEnumerable<TSource> Scan<TSource>(this IEnumerable<TSource> source,
            Func<TSource, TSource, TSource> transformation)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (transformation == null) throw new ArgumentNullException(nameof(transformation));

            return ScanImpl(source, transformation, e => e.MoveNext() ? (true, e.Current) : default);
        }

        /// <summary>
        /// Like <see cref="Enumerable.Aggregate{TSource}"/> except returns
        /// the sequence of intermediate results as well as the final one.
        /// An additional parameter specifies a seed.
        /// </summary>
        /// <remarks>
        /// This operator uses deferred execution and streams its result.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// var result = Enumerable.Range(1, 5).Scan(0, (a, b) => a + b);
        /// ]]></code>
        /// When iterated, <c>result</c> will yield <c>{ 0, 1, 3, 6, 10, 15 }</c>.
        /// </example>
        /// <typeparam name="TSource">Type of elements in source sequence</typeparam>
        /// <typeparam name="TState">Type of state</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="seed">Initial state to seed</param>
        /// <param name="transformation">Transformation operation</param>
        /// <returns>The scanned sequence</returns>

        public static IEnumerable<TState> Scan<TSource, TState>(this IEnumerable<TSource> source,
            TState seed, Func<TState, TSource, TState> transformation)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (transformation == null) throw new ArgumentNullException(nameof(transformation));

            return ScanImpl(source, transformation, _ => (true, seed));
        }

        static IEnumerable<TState> ScanImpl<TSource, TState>(IEnumerable<TSource> source,
            Func<TState, TSource, TState> transformation,
            Func<IEnumerator<TSource>, (bool, TState)> seeder)
        {
            using var e = source.GetEnumerator();

            var (seeded, aggregator) = seeder(e);

            if (!seeded)
                yield break;

            yield return aggregator;

            while (e.MoveNext())
            {
                aggregator = transformation(aggregator, e.Current);
                yield return aggregator;
            }
        }
    }
}
