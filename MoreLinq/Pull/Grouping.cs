using System;
using System.Collections.Generic;

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
    }
}
