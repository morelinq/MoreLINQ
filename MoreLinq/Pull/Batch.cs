namespace MoreLinq.Pull
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public static partial class Enumerable
    {
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
    }
}
