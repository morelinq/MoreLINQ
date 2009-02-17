using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MoreLinq.Pull
{
    /// <summary>
    /// Grouping operators.
    /// </summary>
    public static class Grouping
    {
        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th element 
        /// from each of the argument sequences.
        /// </summary>
        /// <remarks>
        /// This is equivalent to <see cref="Zip{T1,T2,TResult}(IEnumerable{T1},IEnumerable{T2},Func{T1,T2,TResult},ImbalancedZipStrategy)" />
        /// with a stategy of <see cref="ImbalancedZipStrategy.Truncate" />: if the two input sequences are of different lengths,
        /// the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// 
        /// IEnumerable&lt;string&gt; zipped = numbers.Zip(letters, (n, l) => n + l);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", in turn.
        /// </example>
        /// <typeparam name="TFirst">Type of elements in first sequence</typeparam>
        /// <typeparam name="TSecond">Type of elements in second sequence</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence</typeparam>
        /// <param name="first">First sequence</param>
        /// <param name="second">Second sequence</param>
        /// <param name="resultSelector">Function to apply to each pair of elements</param>
        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, 
            IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            first.ThrowIfNull("first");
            second.ThrowIfNull("second");
            resultSelector.ThrowIfNull("resultSelector");

            return ZipImpl(first, second, resultSelector, ImbalancedZipStrategy.Truncate);
        }

        /// <summary>
        /// Returns a projection of tuples, where each tuple contains the N-th element 
        /// from each of the argument sequences.
        /// </summary>
        /// <remarks>
        /// This is equivalent to <see cref="Zip{T1,T2,TResult}(IEnumerable{T1},IEnumerable{T2},Func{T1,T2,TResult},ImbalancedZipStrategy)" />
        /// with a stategy of <see cref="ImbalancedZipStrategy.Truncate" />: if the two input sequences are of different lengths,
        /// the result sequence is terminated as soon as the shortest input sequence is exhausted.
        /// </remarks>
        /// <example>
        /// <code>
        /// int[] numbers = { 1, 2, 3 };
        /// string[] letters = { "A", "B", "C", "D" };
        /// 
        /// IEnumerable&lt;string&gt; zipped = numbers.Zip(letters, (n, l) => n + l, ImbalancedZipStrategy.Pad);
        /// </code>
        /// The <c>zipped</c> variable, when iterated over, will yield "1A", "2B", "3C", "0D" in turn.
        /// </example>
        /// <typeparam name="TFirst">Type of elements in first sequence</typeparam>
        /// <typeparam name="TSecond">Type of elements in second sequence</typeparam>
        /// <typeparam name="TResult">Type of elements in result sequence</typeparam>
        /// <param name="first">First sequence</param>
        /// <param name="second">Second sequence</param>
        /// <param name="resultSelector">Function to apply to each pair of elements</param>
        /// <param name="imbalanceStrategy">Strategy to apply if the two input sequences differ in length.</param>
        public static IEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first,
             IEnumerable<TSecond> second, 
             Func<TFirst, TSecond, TResult> resultSelector,
             ImbalancedZipStrategy imbalanceStrategy)
        {
            first.ThrowIfNull("first");
            second.ThrowIfNull("second");
            resultSelector.ThrowIfNull("resultSelector");
            if (!Enum.IsDefined(typeof(ImbalancedZipStrategy), imbalanceStrategy))
            {
                throw new ArgumentOutOfRangeException("Unknown imbalanced zip strategy: " + imbalanceStrategy);
            }

            return ZipImpl(first, second, resultSelector, imbalanceStrategy);
        }

        private static IEnumerable<TResult> ZipImpl<TFirst, TSecond, TResult>(
            IEnumerable<TFirst> first, 
            IEnumerable<TSecond> second, 
            Func<TFirst, TSecond, TResult> resultSelector,
            ImbalancedZipStrategy imbalanceStrategy)
        {
            using (var e1 = first.GetEnumerator())
            {
                using (var e2 = second.GetEnumerator())
                {
                    while (e1.MoveNext())
                    {
                        if (e2.MoveNext())
                        {
                            yield return resultSelector(e1.Current, e2.Current);
                        }
                        else
                        {
                            switch(imbalanceStrategy)
                            {
                                case ImbalancedZipStrategy.Fail:
                                    throw new InvalidOperationException("Second sequence ran out before first");
                                case ImbalancedZipStrategy.Truncate:
                                    yield break;
                                case ImbalancedZipStrategy.Pad:
                                    do
                                    {
                                        yield return resultSelector(e1.Current, default(TSecond));
                                    } while (e1.MoveNext());
                                    yield break;
                            }
                        }
                    }
                    if (e2.MoveNext())
                    {
                        switch (imbalanceStrategy)
                        {
                            case ImbalancedZipStrategy.Fail:
                                throw new InvalidOperationException("First sequence ran out before second");
                            case ImbalancedZipStrategy.Truncate:
                                yield break;
                            case ImbalancedZipStrategy.Pad:
                                do
                                {
                                    yield return resultSelector(default(TFirst), e2.Current);
                                } while (e2.MoveNext());
                                yield break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Batches the source sequence into sized buckets.
        /// </summary>

        public static IEnumerable<IEnumerable<TSource>> Batch<TSource>(this IEnumerable<TSource> source, int size)
        {
            return Batch(source, size, IdentityFunc<IEnumerable<TSource>>.Value);
        }

        /// <summary>
        /// Batches the source sequence into sized buckets that are then
        /// transformed into results.
        /// </summary>

        public static IEnumerable<TResult> Batch<TSource, TResult>(this IEnumerable<TSource> source, int size,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            source.ThrowIfNull("source");
            size.ThrowIfNonPositive("size");
            resultSelector.ThrowIfNull("resultSelector");
            return BatchImpl(source, size, resultSelector);
        }

        private static IEnumerable<TResult> BatchImpl<TSource, TResult>(this IEnumerable<TSource> source, int size,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            Debug.Assert(source != null);
            Debug.Assert(size > 0);
            Debug.Assert(resultSelector != null);

            TSource[] items = null;
            var count = 0;

            foreach (var item in source)
            {
                if (items == null)
                    items = new TSource[size];

                items[count++] = item;

                if (count != size) 
                    continue;

                yield return resultSelector(items.Select(IdentityFunc<TSource>.Value));
                items = null;
                count = 0;
            }

            if (items != null && count > 0)
                yield return resultSelector(items.Take(count));
        }

        /// <summary>
        /// Splits the source sequence by a separator.
        /// </summary>

        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source,
            TSource separator)
        {
            return Split(source, separator, int.MaxValue);
        }

        /// <summary>
        /// Splits the source sequence by a separator given a maximum count of splits.
        /// </summary>

        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source,
            TSource separator, int count)
        {
            return Split(source, separator, count, IdentityFunc<IEnumerable<TSource>>.Value);
        }

        /// <summary>
        /// Splits the source sequence by a separator and then transforms 
        /// the splits into results.
        /// </summary>

        public static IEnumerable<TResult> Split<TSource, TResult>(this IEnumerable<TSource> source,
            TSource separator,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            return Split(source, separator, int.MaxValue, resultSelector);
        }

        /// <summary>
        /// Splits the source sequence by a separator, given a maximum count
        /// of splits, and then transforms the splits into results.
        /// </summary>

        public static IEnumerable<TResult> Split<TSource, TResult>(this IEnumerable<TSource> source,
            TSource separator, int count,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            return Split(source, separator, null, count, resultSelector);
        }

        /// <summary>
        /// Splits the source sequence by a separator and then transforms the 
        /// splits into results.
        /// </summary>

        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source,
            TSource separator, IEqualityComparer<TSource> comparer)
        {
            return Split(source, separator, comparer, int.MaxValue);
        }

        /// <summary>
        /// Splits the source sequence by a separator, given a maximum count
        /// of splits. A parameter specifies how the separator is compared 
        /// for equality.
        /// </summary>

        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source,
            TSource separator, IEqualityComparer<TSource> comparer, int count)
        {
            return Split(source, separator, comparer, count, IdentityFunc<IEnumerable<TSource>>.Value);
        }

        /// <summary>
        /// Splits the source sequence by a separator and then transforms the 
        /// splits into results. A parameter specifies how the separator is 
        /// compared for equality.
        /// </summary>

        public static IEnumerable<TResult> Split<TSource, TResult>(this IEnumerable<TSource> source,
            TSource separator, IEqualityComparer<TSource> comparer,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            return Split(source, separator, comparer, int.MaxValue, resultSelector);
        }

        /// <summary>
        /// Splits the source sequence by a separator, given a maximum count
        /// of splits, and then transforms the splits into results. A
        /// parameter specifies how the separator is compared for equality.
        /// </summary>

        public static IEnumerable<TResult> Split<TSource, TResult>(this IEnumerable<TSource> source,
            TSource separator, IEqualityComparer<TSource> comparer, int count,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            source.ThrowIfNull("source");
            count.ThrowIfNonPositive("count");
            resultSelector.ThrowIfNull("resultSelector");
            return SplitImpl(source, separator, comparer ?? EqualityComparer<TSource>.Default, count, resultSelector);
        }

        private static IEnumerable<TResult> SplitImpl<TSource, TResult>(IEnumerable<TSource> source,
            TSource separator, IEqualityComparer<TSource> comparer, int count,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            Debug.Assert(source != null);
            Debug.Assert(comparer != null);
            Debug.Assert(count >= 0);
            Debug.Assert(resultSelector != null);

            return Split(source, item => comparer.Equals(item, separator), count, resultSelector);
        }

        /// <summary>
        /// Splits the source sequence by a separator function.
        /// </summary>

        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> separatorFunc)
        {
            return Split(source, separatorFunc, int.MaxValue);
        }

        /// <summary>
        /// Splits the source sequence by a separator function, given a
        /// maximum count of splits.
        /// </summary>

        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> separatorFunc, int count)
        {
            return Split(source, separatorFunc, count, IdentityFunc<IEnumerable<TSource>>.Value);
        }

        /// <summary>
        /// Splits the source sequence by a separator function and then
        /// transforms the splits into results.
        /// </summary>

        public static IEnumerable<TResult> Split<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, bool> separatorFunc,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            return Split(source, separatorFunc, int.MaxValue, resultSelector);
        }

        /// <summary>
        /// Splits the source sequence by a separator function, given a 
        /// maximum count of splits, and then transforms the splits into results.
        /// </summary>

        public static IEnumerable<TResult> Split<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, bool> separatorFunc, int count,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            source.ThrowIfNull("source");
            source.ThrowIfNull("separatorFunc");
            count.ThrowIfNonPositive("count");
            resultSelector.ThrowIfNull("resultSelector");
            return SplitImpl(source, separatorFunc, count, resultSelector);
        }

        private static IEnumerable<TResult> SplitImpl<TSource, TResult>(IEnumerable<TSource> source,
            Func<TSource, bool> separatorFunc, int count,
            Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            Debug.Assert(source != null);
            Debug.Assert(separatorFunc != null);
            Debug.Assert(count >= 0);
            Debug.Assert(resultSelector != null);

            if (count == 0) // No splits?
            {
                yield return resultSelector(source);
            }
            else
            {
                List<TSource> items = null;
                var index = 0;

                foreach (var item in source)
                {
                    index++;
                    if (count > 0 && separatorFunc(item))
                    {
                        yield return resultSelector(items ?? Enumerable.Empty<TSource>());
                        count--;
                        items = null;
                    }
                    else
                    {
                        if (items == null)
                            items = new List<TSource>();

                        items.Add(item);
                    }
                }

                if (items != null && items.Count > 0)
                    yield return resultSelector(items);
            }
        }
    }
}
