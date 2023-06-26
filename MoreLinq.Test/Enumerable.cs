#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Atif Aziz. All rights reserved.
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

namespace MoreLinq.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using LinqEnumerable = System.Linq.Enumerable;

    [DebuggerStepThrough]
    static partial class Enumerable
    {
        public static TSource Aggregate<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func) =>
            LinqEnumerable.Aggregate(source, func);

        public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func) =>
            LinqEnumerable.Aggregate(source, seed, func);

        public static TResult Aggregate<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector) =>
            LinqEnumerable.Aggregate(source, seed, func, resultSelector);

        public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.All(source, predicate);

        public static bool Any<TSource>(this IEnumerable<TSource> source) =>
            LinqEnumerable.Any(source);

        public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.Any(source, predicate);

        public static IEnumerable<TSource> AsEnumerable<TSource>(this IEnumerable<TSource> source) =>
            LinqEnumerable.AsEnumerable(source);

        public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector) =>
            LinqEnumerable.Average(source, selector);

        public static decimal? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector) =>
            LinqEnumerable.Average(source, selector);

        public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector) =>
            LinqEnumerable.Average(source, selector);

        public static float Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector) =>
            LinqEnumerable.Average(source, selector);

        public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector) =>
            LinqEnumerable.Average(source, selector);

        public static float? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector) =>
            LinqEnumerable.Average(source, selector);

        public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector) =>
            LinqEnumerable.Average(source, selector);

        public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector) =>
            LinqEnumerable.Average(source, selector);

        public static decimal Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector) =>
            LinqEnumerable.Average(source, selector);

        public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector) =>
            LinqEnumerable.Average(source, selector);

        public static float? Average(this IEnumerable<float?> source) =>
            LinqEnumerable.Average(source);

        public static double? Average(this IEnumerable<long?> source) =>
            LinqEnumerable.Average(source);

        public static double? Average(this IEnumerable<int?> source) =>
            LinqEnumerable.Average(source);

        public static double? Average(this IEnumerable<double?> source) =>
            LinqEnumerable.Average(source);

        public static decimal? Average(this IEnumerable<decimal?> source) =>
            LinqEnumerable.Average(source);

        public static double Average(this IEnumerable<long> source) =>
            LinqEnumerable.Average(source);

        public static double Average(this IEnumerable<int> source) =>
            LinqEnumerable.Average(source);

        public static double Average(this IEnumerable<double> source) =>
            LinqEnumerable.Average(source);

        public static float Average(this IEnumerable<float> source) =>
            LinqEnumerable.Average(source);

        public static decimal Average(this IEnumerable<decimal> source) =>
            LinqEnumerable.Average(source);

        public static IEnumerable<TResult> Cast<TResult>(this IEnumerable source) =>
            LinqEnumerable.Cast<TResult>(source);

        public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second) =>
            LinqEnumerable.Concat(first, second);

        public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value) =>
            LinqEnumerable.Contains(source, value);

        public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer) =>
            LinqEnumerable.Contains(source, value, comparer);

        public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.Count(source, predicate);

        public static int Count<TSource>(this IEnumerable<TSource> source) =>
            LinqEnumerable.Count(source);

        public static IEnumerable<TSource?> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source) =>
            LinqEnumerable.DefaultIfEmpty(source);

        public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source, TSource defaultValue) =>
            LinqEnumerable.DefaultIfEmpty(source, defaultValue);

        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source) =>
            LinqEnumerable.Distinct(source);

        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer) =>
            LinqEnumerable.Distinct(source, comparer);

        public static TSource ElementAt<TSource>(this IEnumerable<TSource> source, int index) =>
            LinqEnumerable.ElementAt(source, index);

        public static TSource? ElementAtOrDefault<TSource>(this IEnumerable<TSource> source, int index) =>
            LinqEnumerable.ElementAtOrDefault(source, index);

        public static IEnumerable<TResult> Empty<TResult>() =>
            LinqEnumerable.Empty<TResult>();

        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second) =>
            LinqEnumerable.Except(first, second);

        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer) =>
            LinqEnumerable.Except(first, second, comparer);

        public static TSource First<TSource>(this IEnumerable<TSource> source) =>
            LinqEnumerable.First(source);

        public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.First(source, predicate);

        public static TSource? FirstOrDefault<TSource>(this IEnumerable<TSource> source) =>
            LinqEnumerable.FirstOrDefault(source);

        public static TSource? FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.FirstOrDefault(source, predicate);

        public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer) =>
            LinqEnumerable.GroupBy(source, keySelector, resultSelector, comparer);

        public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector) =>
            LinqEnumerable.GroupBy(source, keySelector, resultSelector);

        public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer) =>
            LinqEnumerable.GroupBy(source, keySelector, elementSelector, comparer);

        public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector) =>
            LinqEnumerable.GroupBy(source, keySelector, elementSelector, resultSelector);

        public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer) =>
            LinqEnumerable.GroupBy(source, keySelector, comparer);

        public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            LinqEnumerable.GroupBy(source, keySelector);

        public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) =>
            LinqEnumerable.GroupBy(source, keySelector, elementSelector);

        public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer) =>
            LinqEnumerable.GroupBy(source, keySelector, elementSelector, resultSelector, comparer);

        public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector) =>
            LinqEnumerable.GroupJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector);

        public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer) =>
            LinqEnumerable.GroupJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);

        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second) =>
            LinqEnumerable.Intersect(first, second);

        public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer) =>
            LinqEnumerable.Intersect(first, second, comparer);

        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector) =>
            LinqEnumerable.Join(outer, inner, outerKeySelector, innerKeySelector, resultSelector);

        public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer) =>
            LinqEnumerable.Join(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);

        public static TSource Last<TSource>(this IEnumerable<TSource> source) =>
            LinqEnumerable.Last(source);

        public static TSource Last<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.Last(source, predicate);

        public static TSource? LastOrDefault<TSource>(this IEnumerable<TSource> source) =>
            LinqEnumerable.LastOrDefault(source);

        public static TSource? LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.LastOrDefault(source, predicate);

        public static long LongCount<TSource>(this IEnumerable<TSource> source) =>
            LinqEnumerable.LongCount(source);

        public static long LongCount<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.LongCount(source, predicate);

        public static double Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector) =>
            LinqEnumerable.Max(source, selector);

        public static int Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector) =>
            LinqEnumerable.Max(source, selector);

        public static long Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector) =>
            LinqEnumerable.Max(source, selector);

        public static decimal? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector) =>
            LinqEnumerable.Max(source, selector);

        public static decimal Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector) =>
            LinqEnumerable.Max(source, selector);

        public static int? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector) =>
            LinqEnumerable.Max(source, selector);

        public static long? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector) =>
            LinqEnumerable.Max(source, selector);

        public static float? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector) =>
            LinqEnumerable.Max(source, selector);

        public static TResult? Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) =>
            LinqEnumerable.Max(source, selector);

        public static double? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector) =>
            LinqEnumerable.Max(source, selector);

        public static TSource? Max<TSource>(this IEnumerable<TSource> source) =>
            LinqEnumerable.Max(source);

        public static float Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector) =>
            LinqEnumerable.Max(source, selector);

        public static float? Max(this IEnumerable<float?> source) =>
            LinqEnumerable.Max(source);

        public static long? Max(this IEnumerable<long?> source) =>
            LinqEnumerable.Max(source);

        public static int? Max(this IEnumerable<int?> source) =>
            LinqEnumerable.Max(source);

        public static double? Max(this IEnumerable<double?> source) =>
            LinqEnumerable.Max(source);

        public static decimal? Max(this IEnumerable<decimal?> source) =>
            LinqEnumerable.Max(source);

        public static long Max(this IEnumerable<long> source) =>
            LinqEnumerable.Max(source);

        public static int Max(this IEnumerable<int> source) =>
            LinqEnumerable.Max(source);

        public static double Max(this IEnumerable<double> source) =>
            LinqEnumerable.Max(source);

        public static decimal Max(this IEnumerable<decimal> source) =>
            LinqEnumerable.Max(source);

        public static float Max(this IEnumerable<float> source) =>
            LinqEnumerable.Max(source);

        public static int Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector) =>
            LinqEnumerable.Min(source, selector);

        public static long Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector) =>
            LinqEnumerable.Min(source, selector);

        public static decimal? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector) =>
            LinqEnumerable.Min(source, selector);

        public static double? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector) =>
            LinqEnumerable.Min(source, selector);

        public static TResult? Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) =>
            LinqEnumerable.Min(source, selector);

        public static long? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector) =>
            LinqEnumerable.Min(source, selector);

        public static float? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector) =>
            LinqEnumerable.Min(source, selector);

        public static float Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector) =>
            LinqEnumerable.Min(source, selector);

        public static decimal Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector) =>
            LinqEnumerable.Min(source, selector);

        public static int? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector) =>
            LinqEnumerable.Min(source, selector);

        public static TSource? Min<TSource>(this IEnumerable<TSource> source) =>
            LinqEnumerable.Min(source);

        public static double Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector) =>
            LinqEnumerable.Min(source, selector);

        public static float? Min(this IEnumerable<float?> source) =>
            LinqEnumerable.Min(source);

        public static long? Min(this IEnumerable<long?> source) =>
            LinqEnumerable.Min(source);

        public static int? Min(this IEnumerable<int?> source) =>
            LinqEnumerable.Min(source);

        public static double? Min(this IEnumerable<double?> source) =>
            LinqEnumerable.Min(source);

        public static decimal? Min(this IEnumerable<decimal?> source) =>
            LinqEnumerable.Min(source);

        public static long Min(this IEnumerable<long> source) =>
            LinqEnumerable.Min(source);

        public static int Min(this IEnumerable<int> source) =>
            LinqEnumerable.Min(source);

        public static double Min(this IEnumerable<double> source) =>
            LinqEnumerable.Min(source);

        public static decimal Min(this IEnumerable<decimal> source) =>
            LinqEnumerable.Min(source);

        public static float Min(this IEnumerable<float> source) =>
            LinqEnumerable.Min(source);

        public static IEnumerable<TResult> OfType<TResult>(this IEnumerable source) =>
            LinqEnumerable.OfType<TResult>(source);

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) =>
            LinqEnumerable.OrderBy(source, keySelector, comparer);

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            LinqEnumerable.OrderBy(source, keySelector);

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            LinqEnumerable.OrderByDescending(source, keySelector);

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) =>
            LinqEnumerable.OrderByDescending(source, keySelector, comparer);

        public static IEnumerable<int> Range(int start, int count) =>
            LinqEnumerable.Range(start, count);

        public static IEnumerable<TResult> Repeat<TResult>(TResult element, int count) =>
            LinqEnumerable.Repeat(element, count);

        public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source) =>
            LinqEnumerable.Reverse(source);

        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) =>
            LinqEnumerable.Select(source, selector);

        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector) =>
            LinqEnumerable.Select(source, selector);

        public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) =>
            LinqEnumerable.SelectMany(source, collectionSelector, resultSelector);

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector) =>
            LinqEnumerable.SelectMany(source, selector);

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector) =>
            LinqEnumerable.SelectMany(source, selector);

        public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) =>
            LinqEnumerable.SelectMany(source, collectionSelector, resultSelector);

        public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer) =>
            LinqEnumerable.SequenceEqual(first, second, comparer);

        public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second) =>
            LinqEnumerable.SequenceEqual(first, second);

        public static TSource Single<TSource>(this IEnumerable<TSource> source) =>
            LinqEnumerable.Single(source);

        public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.Single(source, predicate);

        public static TSource? SingleOrDefault<TSource>(this IEnumerable<TSource> source) =>
            LinqEnumerable.SingleOrDefault(source);

        public static TSource? SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.SingleOrDefault(source, predicate);

        public static IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> source, int count) =>
            LinqEnumerable.Skip(source, count);

        public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.SkipWhile(source, predicate);

        public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            LinqEnumerable.SkipWhile(source, predicate);

        public static int? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector) =>
            LinqEnumerable.Sum(source, selector);

        public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector) =>
            LinqEnumerable.Sum(source, selector);

        public static long Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector) =>
            LinqEnumerable.Sum(source, selector);

        public static decimal? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector) =>
            LinqEnumerable.Sum(source, selector);

        public static float Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector) =>
            LinqEnumerable.Sum(source, selector);

        public static float? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector) =>
            LinqEnumerable.Sum(source, selector);

        public static double Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector) =>
            LinqEnumerable.Sum(source, selector);

        public static long? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector) =>
            LinqEnumerable.Sum(source, selector);

        public static decimal Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector) =>
            LinqEnumerable.Sum(source, selector);

        public static double? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector) =>
            LinqEnumerable.Sum(source, selector);

        public static float? Sum(this IEnumerable<float?> source) =>
            LinqEnumerable.Sum(source);

        public static long? Sum(this IEnumerable<long?> source) =>
            LinqEnumerable.Sum(source);

        public static int? Sum(this IEnumerable<int?> source) =>
            LinqEnumerable.Sum(source);

        public static double? Sum(this IEnumerable<double?> source) =>
            LinqEnumerable.Sum(source);

        public static decimal? Sum(this IEnumerable<decimal?> source) =>
            LinqEnumerable.Sum(source);

        public static float Sum(this IEnumerable<float> source) =>
            LinqEnumerable.Sum(source);

        public static long Sum(this IEnumerable<long> source) =>
            LinqEnumerable.Sum(source);

        public static int Sum(this IEnumerable<int> source) =>
            LinqEnumerable.Sum(source);

        public static double Sum(this IEnumerable<double> source) =>
            LinqEnumerable.Sum(source);

        public static decimal Sum(this IEnumerable<decimal> source) =>
            LinqEnumerable.Sum(source);

        public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count) =>
            LinqEnumerable.Take(source, count);

        public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.TakeWhile(source, predicate);

        public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            LinqEnumerable.TakeWhile(source, predicate);

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) =>
            LinqEnumerable.ThenBy(source, keySelector, comparer);

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            LinqEnumerable.ThenBy(source, keySelector);

        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            LinqEnumerable.ThenByDescending(source, keySelector);

        public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer) =>
            LinqEnumerable.ThenByDescending(source, keySelector, comparer);

        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source) =>
            LinqEnumerable.ToArray(source);

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
            where TKey : notnull =>
            LinqEnumerable.ToDictionary(source, keySelector);

        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
            where TKey : notnull =>
            LinqEnumerable.ToDictionary(source, keySelector, comparer);

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
            where TKey : notnull =>
            LinqEnumerable.ToDictionary(source, keySelector, elementSelector);

        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
            where TKey : notnull =>
            LinqEnumerable.ToDictionary(source, keySelector, elementSelector, comparer);

        public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source) =>
            LinqEnumerable.ToList(source);

        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            LinqEnumerable.ToLookup(source, keySelector);

        public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer) =>
            LinqEnumerable.ToLookup(source, keySelector, comparer);

        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) =>
            LinqEnumerable.ToLookup(source, keySelector, elementSelector);

        public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer) =>
            LinqEnumerable.ToLookup(source, keySelector, elementSelector, comparer);

        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second) =>
            LinqEnumerable.Union(first, second);

        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer) =>
            LinqEnumerable.Union(first, second, comparer);

        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            LinqEnumerable.Where(source, predicate);

        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            LinqEnumerable.Where(source, predicate);

        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector) =>
            LinqEnumerable.Zip(first, second, resultSelector);
    }
}
