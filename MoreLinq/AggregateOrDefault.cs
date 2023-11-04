namespace MoreLinq
{
    using System;
    using System.Collections.Generic;

    static partial class MoreEnumerable
    {
        /// <summary>Applies an accumulator function over a sequence, or a default value if sequence contains no elements.</summary>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to aggregate over.</param>
        /// <param name="func">An accumulator function to be invoked on each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> or <paramref name="func" /> is <see langword="null" />.</exception>
        /// <returns><see langword="default" />(<typeparamref name="TSource" />) if <paramref name="source" /> is empty; otherwise, the final accumulator value.</returns>
        public static TSource? AggregateOrDefault<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, TSource> func)
        {
            return TryAggregate(source, func, out _);
        }

        /// <summary>Applies an accumulator function over a sequence, or a specified default value if sequence contains no elements.</summary>
        /// <param name="source">An <see cref="IEnumerable{T}" /> to aggregate over.</param>
        /// <param name="func">An accumulator function to be invoked on each element.</param>
        /// <param name="defaultValue">The default value to return if the sequence is empty.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> or <paramref name="func" /> is <see langword="null" />.</exception>
        /// <returns><paramref name="defaultValue"/> if <paramref name="source" /> is empty; otherwise, the final accumulator value.</returns>
        public static TSource AggregateOrDefault<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, TSource> func,
            TSource defaultValue)
        {
            var value = TryAggregate(source, func, out var success);
            return success ? value! : defaultValue;
        }

        static TSource? TryAggregate<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, TSource> func,
            out bool success)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));

            using var e = source.GetEnumerator();
            if (!e.MoveNext())
            {
                success = false;
                return default;
            }

            var result = e.Current;
            while (e.MoveNext())
            {
                result = func(result, e.Current);
            }

            success = true;
            return result;
        }
    }
}
