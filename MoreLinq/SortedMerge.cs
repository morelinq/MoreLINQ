using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreLinq
{
    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Merges two or more sequences that are in a common order (either ascending or descending) into
        /// a single sequence that preserves that order.
        /// </summary>
        /// <remarks>
        /// Using SortedMerge on sequences that are not ordered or are not in the same order produces
        /// undefined results.<br/>
        /// <c>SortedMerge</c> uses performs the merge in a deferred, streaming manner. <br/>
        /// 
        /// Here is an example of a merge, as well as the produced result:
        /// <code>
        ///   var s1 = new[] { 3, 7, 11 };
        ///   var s2 = new[] { 2, 4, 20 };
        ///   var s3 = new[] { 17, 19, 25 };
        ///   var merged = s1.SortedMerge( OrderByDirection.Ascending, s2, s3 );
        ///   var result = merged.ToArray();
        ///   // result will be:
        ///   // { 2, 3, 4, 7, 11, 17, 19, 20, 25 }
        /// </code>
        /// </remarks>
        /// <typeparam name="T">The type of the elements of the sequence</typeparam>
        /// <param name="sequence">The primary sequence with which to merge</param>
        /// <param name="direction">The ordering that all sequences must already exhibit</param>
        /// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
        /// <returns>A merged, order-preserving sequence containing all of the elements of the original sequences</returns>
        public static IEnumerable<T> SortedMerge<T>(this IEnumerable<T> sequence, OrderByDirection direction, params IEnumerable<T>[] otherSequences)
        {
            return SortedMerge(sequence, direction, Comparer<T>.Default, otherSequences);
        }

        /// <summary>
        /// Merges two or more sequences that are in a common order (either ascending or descending) into
        /// a single sequence that preserves that order.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="sequence">The primary sequence with which to merge</param>
        /// <param name="direction">The ordering that all sequences must already exhibit</param>
        /// <param name="comparer">The comparer used to evaluate the relative order between elements</param>
        /// <param name="otherSequences">A variable argument array of zero or more other sequences to merge with</param>
        /// <returns>A merged, order-preserving sequence containing al of the elements of the original sequences</returns>
        public static IEnumerable<T> SortedMerge<T>(this IEnumerable<T> sequence, OrderByDirection direction, IComparer<T> comparer, params IEnumerable<T>[] otherSequences)
        {
            sequence.ThrowIfNull("sequence");
            otherSequences.ThrowIfNull("otherSequences");

            if (otherSequences.Length == 0)
                return sequence; // optimization for when otherSequences is empty

            // define an precedence function based on the comparer and direction
            // this is a function that will return True if (b) should precede (a)
            var precedenceFunc =
                direction == OrderByDirection.Ascending
                    ? (Func<T, T, bool>)((a, b) => comparer.Compare(b, a) < 0)
                    : (a, b) => comparer.Compare(b, a) > 0;

            // return the sorted merge result
            return SortedMergeImpl(precedenceFunc, new[] { sequence }.Concat(otherSequences));
        }

        /// <summary>
        /// Private implementation method that performs a merge of multiple, ordered sequences using
        /// a precedence function which encodes order-sensitive comparison logic based on the caller's arguments.
        /// </summary>
        /// <remarks>
        /// The algorithm employed in this implementation is not necessarily the most optimal way to merge
        /// two sequences. A swap-compare version would probably be somewhat more efficient - but at the
        /// expense of considerably more complexity. One possible optimization would be to detect that only
        /// a single sequence remains (all other being consumed) and break out of the main while-loop and
        /// simply yield the items that are part of the final sequence.
        /// 
        /// The algorithm used here will perform N*(K1+K2+...Kn-1) comparisons, where <c>N => otherSequences.Count()+1.</c>
        /// </remarks>
        private static IEnumerable<T> SortedMergeImpl<T>(Func<T, T, bool> precedenceFunc, IEnumerable<IEnumerable<T>> otherSequences)
        {
            using (var allIterators = new DisposableGroup<T>(otherSequences))
            {
                // prime all of the iterators by advancing them to their first element (if any)
                // NOTE: We start with the last index to simplify the removal of an iterator if
                //       it happens to be terminal (no items) before we start merging
                for (var i = allIterators.Iterators.Count - 1; i >= 0; i--)
                {
                    if (!allIterators.Iterators[i].MoveNext())
                        allIterators.Exclude(i);
                }

                // while all iterators have not yet been consumed...
                while (allIterators.Iterators.Count > 0)
                {
                    var nextIndex = 0;
                    var nextValue = allIterators[0].Current;

                    // find the next least element to return
                    for (var i = 1; i < allIterators.Iterators.Count; i++)
                    {
                        var anotherElement = allIterators[i].Current;
                        // determine which element follows based on ordering function
                        if (precedenceFunc(nextValue, anotherElement))
                        {
                            nextIndex = i;
                            nextValue = anotherElement;
                        }
                    }

                    yield return nextValue; // next value in precedence order

                    // advance iterator that yielded element, excluding it when consumed
                    if (!allIterators.Iterators[nextIndex].MoveNext())
                        allIterators.Exclude(nextIndex);
                }
            }
        }

        /// <summary>
        /// Class used to assist in ensuring that groups of disposable iterators
        /// are disposed - either when Excluded or when the DisposableGroup is disposed.
        /// </summary>
        private sealed class DisposableGroup<T> : IDisposable
        {
            public DisposableGroup(IEnumerable<IEnumerable<T>> sequences)
            {
                // TODO Review leaking of disposable enumerators when a GetEnumerator throws
                Iterators = sequences.Select(seq => seq.GetEnumerator()).ToList();
            }

            public List<IEnumerator<T>> Iterators { get; private set; }

            public IEnumerator<T> this[int index] { get { return Iterators[index]; } }

            public void Exclude(int index)
            {
                Iterators[index].Dispose();
                Iterators.RemoveAt(index);
            }

            public void Dispose()
            {
                Iterators.ForEach(iter => iter.Dispose());
            }
        }
    }
}