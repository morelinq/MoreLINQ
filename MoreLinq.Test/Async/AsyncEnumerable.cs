#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2021 Atif Aziz. All rights reserved.
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

namespace MoreLinq.Test.Async
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using LinqEnumerable = System.Linq.AsyncEnumerable;

    [DebuggerStepThrough]
    static partial class AsyncEnumerable
    {
        public static ValueTask<TSource> AggregateAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> func) =>
            LinqEnumerable.AggregateAsync(source, func);

        public static ValueTask<TAccumulate> AggregateAsync<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func) =>
            LinqEnumerable.AggregateAsync(source, seed, func);

        public static ValueTask<TResult> AggregateAsync<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector) =>
            LinqEnumerable.AggregateAsync(source, seed, func, resultSelector);

        public static ValueTask<bool> AllAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.AllAsync(source, predicate);

        public static ValueTask<bool> AnyAsync<TSource>(this IAsyncEnumerable<TSource> source) =>
            LinqEnumerable.AnyAsync(source);

        public static ValueTask<bool> AnyAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.AnyAsync(source, predicate);

        public static IAsyncEnumerable<TSource> AsEnumerable<TSource>(this IAsyncEnumerable<TSource> source) =>
            LinqEnumerable.AsAsyncEnumerable(source);

        public static ValueTask<double> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector) =>
            LinqEnumerable.AverageAsync(source, selector);

        public static ValueTask<decimal?> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector) =>
            LinqEnumerable.AverageAsync(source, selector);

        public static ValueTask<double?> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector) =>
            LinqEnumerable.AverageAsync(source, selector);

        public static ValueTask<float> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector) =>
            LinqEnumerable.AverageAsync(source, selector);

        public static ValueTask<double?> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector) =>
            LinqEnumerable.AverageAsync(source, selector);

        public static ValueTask<float?> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector) =>
            LinqEnumerable.AverageAsync(source, selector);

        public static ValueTask<double> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector) =>
            LinqEnumerable.AverageAsync(source, selector);

        public static ValueTask<double?> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector) =>
            LinqEnumerable.AverageAsync(source, selector);

        public static ValueTask<decimal> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector) =>
            LinqEnumerable.AverageAsync(source, selector);

        public static ValueTask<double> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector) =>
            LinqEnumerable.AverageAsync(source, selector);

        public static ValueTask<float?> AverageAsync(this IAsyncEnumerable<float?> source) =>
            LinqEnumerable.AverageAsync(source);

        public static ValueTask<double?> AverageAsync(this IAsyncEnumerable<long?> source) =>
            LinqEnumerable.AverageAsync(source);

        public static ValueTask<double?> AverageAsync(this IAsyncEnumerable<int?> source) =>
            LinqEnumerable.AverageAsync(source);

        public static ValueTask<double?> AverageAsync(this IAsyncEnumerable<double?> source) =>
            LinqEnumerable.AverageAsync(source);

        public static ValueTask<decimal?> AverageAsync(this IAsyncEnumerable<decimal?> source) =>
            LinqEnumerable.AverageAsync(source);

        public static ValueTask<double> AverageAsync(this IAsyncEnumerable<long> source) =>
            LinqEnumerable.AverageAsync(source);

        public static ValueTask<double> AverageAsync(this IAsyncEnumerable<int> source) =>
            LinqEnumerable.AverageAsync(source);

        public static ValueTask<double> AverageAsync(this IAsyncEnumerable<double> source) =>
            LinqEnumerable.AverageAsync(source);

        public static ValueTask<float> AverageAsync(this IAsyncEnumerable<float> source) =>
            LinqEnumerable.AverageAsync(source);

        public static ValueTask<decimal> AverageAsync(this IAsyncEnumerable<decimal> source) =>
            LinqEnumerable.AverageAsync(source);

        public static IAsyncEnumerable<TResult> Cast<TResult>(this IAsyncEnumerable<object> source) =>
            LinqEnumerable.Cast<TResult>(source);

        public static IAsyncEnumerable<TSource> Concat<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second) =>
            LinqEnumerable.Concat(first, second);

        public static ValueTask<bool> ContainsAsync<TSource>(this IAsyncEnumerable<TSource> source, TSource value) =>
            LinqEnumerable.ContainsAsync(source, value);

        public static ValueTask<bool> ContainsAsync<TSource>(this IAsyncEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer) =>
            LinqEnumerable.ContainsAsync(source, value, comparer);

        public static ValueTask<int> CountAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.CountAsync(source, predicate);

        public static ValueTask<int> CountAsync<TSource>(this IAsyncEnumerable<TSource> source) =>
            LinqEnumerable.CountAsync(source);

        public static IAsyncEnumerable<TSource> DefaultIfEmpty<TSource>(this IAsyncEnumerable<TSource> source) =>
            LinqEnumerable.DefaultIfEmpty(source);

        public static IAsyncEnumerable<TSource> DefaultIfEmpty<TSource>(this IAsyncEnumerable<TSource> source, TSource defaultValue) =>
            LinqEnumerable.DefaultIfEmpty(source, defaultValue);

        public static IAsyncEnumerable<TSource> Distinct<TSource>(this IAsyncEnumerable<TSource> source) =>
            LinqEnumerable.Distinct(source);

        public static IAsyncEnumerable<TSource> Distinct<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer) =>
            LinqEnumerable.Distinct(source, comparer);

        public static ValueTask<TSource> ElementAtAsync<TSource>(this IAsyncEnumerable<TSource> source, int index) =>
            LinqEnumerable.ElementAtAsync(source, index);

        public static ValueTask<TSource?> ElementAtOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, int index) =>
            LinqEnumerable.ElementAtOrDefaultAsync(source, index);

        public static IAsyncEnumerable<TResult> Empty<TResult>() =>
            LinqEnumerable.Empty<TResult>();

        public static IAsyncEnumerable<TSource> Except<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second) =>
            LinqEnumerable.Except(first, second);

        public static IAsyncEnumerable<TSource> Except<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer) =>
            LinqEnumerable.Except(first, second, comparer);

        public static ValueTask<TSource> FirstAsync<TSource>(this IAsyncEnumerable<TSource> source) =>
            LinqEnumerable.FirstAsync(source);

        public static ValueTask<TSource> FirstAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.FirstAsync(source, predicate);

        public static ValueTask<TSource?> FirstOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source) =>
            LinqEnumerable.FirstOrDefaultAsync(source);

        public static ValueTask<TSource?> FirstOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.FirstOrDefaultAsync(source, predicate);

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IAsyncEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer) =>
            LinqEnumerable.GroupBy(source, keySelector, resultSelector, comparer);

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IAsyncEnumerable<TSource>, TResult> resultSelector) =>
            LinqEnumerable.GroupBy(source, keySelector, resultSelector);

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer) =>
            LinqEnumerable.GroupBy(source, keySelector, elementSelector, comparer);

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IAsyncEnumerable<TElement>, TResult> resultSelector) =>
            LinqEnumerable.GroupBy(source, keySelector, elementSelector, resultSelector);

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer) =>
            LinqEnumerable.GroupBy(source, keySelector, comparer);

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            LinqEnumerable.GroupBy(source, keySelector);

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) =>
            LinqEnumerable.GroupBy(source, keySelector, elementSelector);

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IAsyncEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer) =>
            LinqEnumerable.GroupBy(source, keySelector, elementSelector, resultSelector, comparer);

        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, TResult> resultSelector) =>
            LinqEnumerable.GroupJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector);

        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer) =>
            LinqEnumerable.GroupJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);

        public static IAsyncEnumerable<TSource> Intersect<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second) =>
            LinqEnumerable.Intersect(first, second);

        public static IAsyncEnumerable<TSource> Intersect<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer) =>
            LinqEnumerable.Intersect(first, second, comparer);

        public static IAsyncEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector) =>
            LinqEnumerable.Join(outer, inner, outerKeySelector, innerKeySelector, resultSelector);

        public static IAsyncEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer) =>
            LinqEnumerable.Join(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);

        public static ValueTask<TSource> LastAsync<TSource>(this IAsyncEnumerable<TSource> source) =>
            LinqEnumerable.LastAsync(source);

        public static ValueTask<TSource> LastAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.LastAsync(source, predicate);

        public static ValueTask<TSource?> LastOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source) =>
            LinqEnumerable.LastOrDefaultAsync(source);

        public static ValueTask<TSource?> LastOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.LastOrDefaultAsync(source, predicate);

        public static ValueTask<long> LongCountAsync<TSource>(this IAsyncEnumerable<TSource> source) =>
            LinqEnumerable.LongCountAsync(source);

        public static ValueTask<long> LongCountAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.LongCountAsync(source, predicate);

        public static ValueTask<double> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector) =>
            LinqEnumerable.MaxAsync(source, selector);

        public static ValueTask<int> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector) =>
            LinqEnumerable.MaxAsync(source, selector);

        public static ValueTask<long> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector) =>
            LinqEnumerable.MaxAsync(source, selector);

        public static ValueTask<decimal?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector) =>
            LinqEnumerable.MaxAsync(source, selector);

        public static ValueTask<decimal> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector) =>
            LinqEnumerable.MaxAsync(source, selector);

        public static ValueTask<int?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector) =>
            LinqEnumerable.MaxAsync(source, selector);

        public static ValueTask<long?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector) =>
            LinqEnumerable.MaxAsync(source, selector);

        public static ValueTask<float?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector) =>
            LinqEnumerable.MaxAsync(source, selector);

        public static ValueTask<TResult> MaxAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector) =>
            LinqEnumerable.MaxAsync(source, selector);

        public static ValueTask<double?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector) =>
            LinqEnumerable.MaxAsync(source, selector);

        public static ValueTask<TSource> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source) =>
            LinqEnumerable.MaxAsync(source);

        public static ValueTask<float> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector) =>
            LinqEnumerable.MaxAsync(source, selector);

        public static ValueTask<float?> MaxAsync(this IAsyncEnumerable<float?> source) =>
            LinqEnumerable.MaxAsync(source);

        public static ValueTask<long?> MaxAsync(this IAsyncEnumerable<long?> source) =>
            LinqEnumerable.MaxAsync(source);

        public static ValueTask<int?> MaxAsync(this IAsyncEnumerable<int?> source) =>
            LinqEnumerable.MaxAsync(source);

        public static ValueTask<double?> MaxAsync(this IAsyncEnumerable<double?> source) =>
            LinqEnumerable.MaxAsync(source);

        public static ValueTask<decimal?> MaxAsync(this IAsyncEnumerable<decimal?> source) =>
            LinqEnumerable.MaxAsync(source);

        public static ValueTask<long> MaxAsync(this IAsyncEnumerable<long> source) =>
            LinqEnumerable.MaxAsync(source);

        public static ValueTask<int> MaxAsync(this IAsyncEnumerable<int> source) =>
            LinqEnumerable.MaxAsync(source);

        public static ValueTask<double> MaxAsync(this IAsyncEnumerable<double> source) =>
            LinqEnumerable.MaxAsync(source);

        public static ValueTask<decimal> MaxAsync(this IAsyncEnumerable<decimal> source) =>
            LinqEnumerable.MaxAsync(source);

        public static ValueTask<float> MaxAsync(this IAsyncEnumerable<float> source) =>
            LinqEnumerable.MaxAsync(source);

        public static ValueTask<int> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector) =>
            LinqEnumerable.MinAsync(source, selector);

        public static ValueTask<long> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector) =>
            LinqEnumerable.MinAsync(source, selector);

        public static ValueTask<decimal?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector) =>
            LinqEnumerable.MinAsync(source, selector);

        public static ValueTask<double?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector) =>
            LinqEnumerable.MinAsync(source, selector);

        public static ValueTask<TResult> MinAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector) =>
            LinqEnumerable.MinAsync(source, selector);

        public static ValueTask<long?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector) =>
            LinqEnumerable.MinAsync(source, selector);

        public static ValueTask<float?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector) =>
            LinqEnumerable.MinAsync(source, selector);

        public static ValueTask<float> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector) =>
            LinqEnumerable.MinAsync(source, selector);

        public static ValueTask<decimal> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector) =>
            LinqEnumerable.MinAsync(source, selector);

        public static ValueTask<int?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector) =>
            LinqEnumerable.MinAsync(source, selector);

        public static ValueTask<TSource> MinAsync<TSource>(this IAsyncEnumerable<TSource> source) =>
            LinqEnumerable.MinAsync(source);

        public static ValueTask<double> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector) =>
            LinqEnumerable.MinAsync(source, selector);

        public static ValueTask<float?> MinAsync(this IAsyncEnumerable<float?> source) =>
            LinqEnumerable.MinAsync(source);

        public static ValueTask<long?> MinAsync(this IAsyncEnumerable<long?> source) =>
            LinqEnumerable.MinAsync(source);

        public static ValueTask<int?> MinAsync(this IAsyncEnumerable<int?> source) =>
            LinqEnumerable.MinAsync(source);

        public static ValueTask<double?> MinAsync(this IAsyncEnumerable<double?> source) =>
            LinqEnumerable.MinAsync(source);

        public static ValueTask<decimal?> MinAsync(this IAsyncEnumerable<decimal?> source) =>
            LinqEnumerable.MinAsync(source);

        public static ValueTask<long> MinAsync(this IAsyncEnumerable<long> source) =>
            LinqEnumerable.MinAsync(source);

        public static ValueTask<int> MinAsync(this IAsyncEnumerable<int> source) =>
            LinqEnumerable.MinAsync(source);

        public static ValueTask<double> MinAsync(this IAsyncEnumerable<double> source) =>
            LinqEnumerable.MinAsync(source);

        public static ValueTask<decimal> MinAsync(this IAsyncEnumerable<decimal> source) =>
            LinqEnumerable.MinAsync(source);

        public static ValueTask<float> MinAsync(this IAsyncEnumerable<float> source) =>
            LinqEnumerable.MinAsync(source);

        public static IAsyncEnumerable<TResult> OfType<TResult>(this IAsyncEnumerable<object> source) =>
            LinqEnumerable.OfType<TResult>(source);

        public static IOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) =>
            LinqEnumerable.OrderBy(source, keySelector, comparer);

        public static IOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            LinqEnumerable.OrderBy(source, keySelector);

        public static IOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            LinqEnumerable.OrderByDescending(source, keySelector);

        public static IOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) =>
            LinqEnumerable.OrderByDescending(source, keySelector, comparer);

        public static IAsyncEnumerable<int> Range(int start, int count) =>
            LinqEnumerable.Range(start, count);

        public static IAsyncEnumerable<TResult> Repeat<TResult>(TResult element, int count) =>
            LinqEnumerable.Repeat(element, count);

        public static IAsyncEnumerable<TSource> Reverse<TSource>(this IAsyncEnumerable<TSource> source) =>
            LinqEnumerable.Reverse(source);

        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector) =>
            LinqEnumerable.Select(source, selector);

        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, TResult> selector) =>
            LinqEnumerable.Select(source, selector);

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) =>
            LinqEnumerable.SelectMany(source, collectionSelector, resultSelector);

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TResult>> selector) =>
            LinqEnumerable.SelectMany(source, selector);

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TResult>> selector) =>
            LinqEnumerable.SelectMany(source, selector);

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) =>
            LinqEnumerable.SelectMany(source, collectionSelector, resultSelector);

        public static ValueTask<bool> SequenceEqualAsync<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer) =>
            LinqEnumerable.SequenceEqualAsync(first, second, comparer);

        public static ValueTask<bool> SequenceEqualAsync<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second) =>
            LinqEnumerable.SequenceEqualAsync(first, second);

        public static ValueTask<TSource> SingleAsync<TSource>(this IAsyncEnumerable<TSource> source) =>
            LinqEnumerable.SingleAsync(source);

        public static ValueTask<TSource> SingleAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.SingleAsync(source, predicate);

        public static ValueTask<TSource?> SingleOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source) =>
            LinqEnumerable.SingleOrDefaultAsync(source);

        public static ValueTask<TSource?> SingleOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.SingleOrDefaultAsync(source, predicate);

        public static IAsyncEnumerable<TSource> Skip<TSource>(this IAsyncEnumerable<TSource> source, int count) =>
            LinqEnumerable.Skip(source, count);

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.SkipWhile(source, predicate);

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            LinqEnumerable.SkipWhile(source, predicate);

        public static ValueTask<int?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector) =>
            LinqEnumerable.SumAsync(source, selector);

        public static ValueTask<int> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector) =>
            LinqEnumerable.SumAsync(source, selector);

        public static ValueTask<long> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector) =>
            LinqEnumerable.SumAsync(source, selector);

        public static ValueTask<decimal?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector) =>
            LinqEnumerable.SumAsync(source, selector);

        public static ValueTask<float> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector) =>
            LinqEnumerable.SumAsync(source, selector);

        public static ValueTask<float?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector) =>
            LinqEnumerable.SumAsync(source, selector);

        public static ValueTask<double> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector) =>
            LinqEnumerable.SumAsync(source, selector);

        public static ValueTask<long?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector) =>
            LinqEnumerable.SumAsync(source, selector);

        public static ValueTask<decimal> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector) =>
            LinqEnumerable.SumAsync(source, selector);

        public static ValueTask<double?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector) =>
            LinqEnumerable.SumAsync(source, selector);

        public static ValueTask<float?> SumAsync(this IAsyncEnumerable<float?> source) =>
            LinqEnumerable.SumAsync(source);

        public static ValueTask<long?> SumAsync(this IAsyncEnumerable<long?> source) =>
            LinqEnumerable.SumAsync(source);

        public static ValueTask<int?> SumAsync(this IAsyncEnumerable<int?> source) =>
            LinqEnumerable.SumAsync(source);

        public static ValueTask<double?> SumAsync(this IAsyncEnumerable<double?> source) =>
            LinqEnumerable.SumAsync(source);

        public static ValueTask<decimal?> SumAsync(this IAsyncEnumerable<decimal?> source) =>
            LinqEnumerable.SumAsync(source);

        public static ValueTask<float> SumAsync(this IAsyncEnumerable<float> source) =>
            LinqEnumerable.SumAsync(source);

        public static ValueTask<long> SumAsync(this IAsyncEnumerable<long> source) =>
            LinqEnumerable.SumAsync(source);

        public static ValueTask<int> SumAsync(this IAsyncEnumerable<int> source) =>
            LinqEnumerable.SumAsync(source);

        public static ValueTask<double> SumAsync(this IAsyncEnumerable<double> source) =>
            LinqEnumerable.SumAsync(source);

        public static ValueTask<decimal> SumAsync(this IAsyncEnumerable<decimal> source) =>
            LinqEnumerable.SumAsync(source);

        public static IAsyncEnumerable<TSource> Take<TSource>(this IAsyncEnumerable<TSource> source, int count) =>
            LinqEnumerable.Take(source, count);

        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.TakeWhile(source, predicate);

        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            LinqEnumerable.TakeWhile(source, predicate);

        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) =>
            LinqEnumerable.ThenBy(source, keySelector, comparer);

        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            LinqEnumerable.ThenBy(source, keySelector);

        public static IOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            LinqEnumerable.ThenByDescending(source, keySelector);

        public static IOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) =>
            LinqEnumerable.ThenByDescending(source, keySelector, comparer);

        public static ValueTask<TSource[]> ToArrayAsync<TSource>(this IAsyncEnumerable<TSource> source) =>
            LinqEnumerable.ToArrayAsync(source);

        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
            where TKey : notnull =>
            LinqEnumerable.ToDictionaryAsync(source, keySelector);

        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
            where TKey : notnull =>
            LinqEnumerable.ToDictionaryAsync(source, keySelector, comparer);

        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
            where TKey : notnull =>
            LinqEnumerable.ToDictionaryAsync(source, keySelector, elementSelector);

        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
            where TKey : notnull =>
            LinqEnumerable.ToDictionaryAsync(source, keySelector, elementSelector, comparer);

        public static ValueTask<List<TSource>> ToListAsync<TSource>(this IAsyncEnumerable<TSource> source) =>
            LinqEnumerable.ToListAsync(source);

        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            LinqEnumerable.ToLookupAsync(source, keySelector);

        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer) =>
            LinqEnumerable.ToLookupAsync(source, keySelector, comparer);

        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) =>
            LinqEnumerable.ToLookupAsync(source, keySelector, elementSelector);

        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer) =>
            LinqEnumerable.ToLookupAsync(source, keySelector, elementSelector, comparer);

        public static IAsyncEnumerable<TSource> Union<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second) =>
            LinqEnumerable.Union(first, second);

        public static IAsyncEnumerable<TSource> Union<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer) =>
            LinqEnumerable.Union(first, second, comparer);

        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            LinqEnumerable.Where(source, predicate);

        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.Where(source, predicate);

        public static IAsyncEnumerable<TResult> Zip<TFirstAsync, TSecond, TResult>(this IAsyncEnumerable<TFirstAsync> first, IAsyncEnumerable<TSecond> second, Func<TFirstAsync, TSecond, TResult> resultSelector) =>
            LinqEnumerable.Zip(first, second, resultSelector);
    }
}
