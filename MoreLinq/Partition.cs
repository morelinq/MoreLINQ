using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MoreLinq
{
    /// <summary>
    /// Enum that indicates how Partition() handles cases where the source sequence is longer
    /// than the sum of all partitions requested.
    /// </summary>
    public enum PartitionUnderflowStrategy
    {
        /// <summary>
        /// Stops generating partitions after the last partition
        /// </summary>
        Stop = 1,
        /// <summary>
        /// Generates an additional partition from the remainder of the sequence
        /// </summary>
        Rest = 2,
    }

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Divides a sequence into an series of sequences whose number of elements is defined by <paramref name="partitions"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="sequence">The sequence to partition</param>
        /// <param name="partitions">A sequence of sizes, defining how many elements to place in each partition</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> sequence, IEnumerable<int> partitions)
        {
            return Partition(sequence, partitions, PartitionUnderflowStrategy.Stop);
        }

        /// <summary>
        /// Divides a sequence into a series of sequences whose number of elements is defined by <paramref name="partitions"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence</typeparam>
        /// <param name="sequence">The sequence to partition</param>
        /// <param name="partitions">A sequence of sizes, defining how many elements to place in each partition</param>
        /// <param name="underflowStrategy">Controls how <c>Partition()</c> behaves when there are elements left over in the source sequence after all explicit partitions have been evaluated</param>
        /// <returns>A sequence of partitioned lists whose elements correspond to the defined partitions sizes</returns>
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> sequence, IEnumerable<int> partitions, PartitionUnderflowStrategy underflowStrategy)
        {
            sequence.ThrowIfNull("sequence");
            partitions.ThrowIfNull("partitions");
            if( partitions.Any( x => x < 0 ) )
                throw new ArgumentException("Negative partition sizes are not allowed.");
            
            return PartitionImpl(sequence, partitions, underflowStrategy);
        }

        /// <summary>
        /// Divides a sequence into a series of sequences whose number of elements is defined by <paramref name="partitions"/>
        /// and then applies a user-defined projection function to each sequence to produce a result.
        /// </summary>
        /// <remarks>
        /// This overload uses the Stop underflow strategy.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
        /// <param name="sequence">The sequence to partition and project</param>
        /// <param name="partitions">A sequence of sizes, defining how many elements to place in each partition</param>
        /// <param name="resultSelector">A projection function which transforms a partitioned sub-sequence into a result</param>
        /// <returns>A sequence of result elements from the projected partitions</returns>
        public static IEnumerable<TResult> Partition<TSource, TResult>(this IEnumerable<TSource> sequence, IEnumerable<int> partitions, Func<IEnumerable<TSource>, TResult> resultSelector)
        {
            return Partition(sequence, partitions, resultSelector, PartitionUnderflowStrategy.Stop);
        }

        /// <summary>
        /// Divides a sequence into a series of sequences whose number of elements is defined by <paramref name="partitions"/>
        /// and then applies a user-defined projection function to each sequence to produce a result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence</typeparam>
        /// <param name="sequence">The sequence to partition and project</param>
        /// <param name="partitions">A sequence of sizes, defining how many elements to place in each partition</param>
        /// <param name="resultSelector">A projection function which transforms a partitioned sub-sequence into a result</param>
        /// <param name="underflowStrategy">Control how <c>Partition()</c> behaves when there are elements left over in the source sequence after all explicit partitions have been evaluated</param>
        /// <returns></returns>
        public static IEnumerable<TResult> Partition<TSource, TResult>(this IEnumerable<TSource> sequence, IEnumerable<int> partitions, Func<IEnumerable<TSource>, TResult> resultSelector, PartitionUnderflowStrategy underflowStrategy)
        {
            sequence.ThrowIfNull("sequence");
            partitions.ThrowIfNull("partitions");
            resultSelector.ThrowIfNull("resultSelector");
            if (partitions.Any(x => x < 0))
                throw new ArgumentException("Negative partition sizes are not allowed.");

            return PartitionImpl(sequence, partitions, resultSelector, underflowStrategy);
        }

        private static IEnumerable<TResult> PartitionImpl<TSource, TResult>(this IEnumerable<TSource> sequence, IEnumerable<int> partitions, Func<IEnumerable<TSource>, TResult> resultSelector, PartitionUnderflowStrategy underflowStrategy)
        {
            foreach (var partitionSequence in Partition(sequence, partitions, underflowStrategy))
                yield return resultSelector(partitionSequence);
        }

        /// <summary>
        /// Private implementation of the partition operation.
        /// </summary>
        private static IEnumerable<IEnumerable<T>> PartitionImpl<T>(IEnumerable<T> sequence, IEnumerable<int> partitions, PartitionUnderflowStrategy underflowStrategy)
        {
            var iter = sequence.GetEnumerator();
            try
            {
                ulong countConsumed = 0; // made this ulong rather than int to handle really, really long sequences
                // In theory, it's still not infinite, but in practice, you're unlikely to
                // have a program running long enough to exceed this variable's range.
                var sourceContinues = false;
                foreach (var partitionSize in partitions)
                {
                    var partition = new T[partitionSize]; // allocate temporary storage to retain partition (for idempotence)
                    var index = 0;
                    while (index < partitionSize && (sourceContinues = iter.MoveNext()))
                        partition[index++] = iter.Current;
                    // if last partition is empty, break out (don't yield anything)
                    if (index == 0)
                        break;
                    // if last partition is smaller than the requested size,
                    // resize the partition cache array to that size
                    if (index > 0 && index < partitionSize)
                        Array.Resize(ref partition, index);

                    // tracks how many items in the original sequence have been consumed
                    countConsumed += (ulong)partition.Length;

                    yield return partition;
                }
                
                // handle case where source sequence is not fully partitioned...
                if (sourceContinues && underflowStrategy == PartitionUnderflowStrategy.Rest)
                {
                    // prevent iterator from being disposed because it will
                    // be retained and managed by YieldRestEnumerable
                    var iterCopy = iter;
                    iter = null;
                    yield return new YieldRestEnumerable<T>(countConsumed, iterCopy);
                }

                //       The main reason this was added, was because it became too tempting to pass int.MaxValue
                //       as the size of the last partition in an attempt to retrieve the *rest* of a sequence
                //       being partitioned. That was a problem for two reasons:
                //          1. It would attempt to allocation an enormous (2gb) buffer for the last partition,
                //             which in most cases could not be fulfilled. and...
                //          2. It doesn't actually solve the problem of returning the "rest" of a sequence if 
                //             it is longer than int.MaxValue (a large file, network stream, or generated sequence).
                //       The first problem could be addressed by allocating a small array (initially), and then
                //       doubling it's size with each overflow. This has limitations, and is an expensive and
                //       incomplete way to deal with the problem.
                //       The previous solution of simply yielding the underlying iterator to the caller was
                //       undesirable because it broke the expectations of most consumers, namely:
                //          1. Enumerable sequences are idempotent, and can be iterated more than once
                //          2. Iterators can be reset, and then re-iterated mid-stream
                //          3. The iterator couldn't be gauranteed to be disposed by the caler
                //
                //       The best solution I could think of to deal with this, was to return a special version
                //       of IEnumerable<> that when iterated the first time, yields the rest of the sequence
                //       initially being iterated. And when iterated subsequent times, resets the iterator,
                //       skips the first N items (where N is the sum of the prior partition lengths), and then
                //       begins iterating again. This should correctly handle finite, in-memory sequences, as
                //       well as infinite sequence produced by generators. For external (I/O) sequences, the
                //       caller will tyically be aware that the unerlying iterator cannot be safely reset - 
                //       and (hopefully) won't attempt to do so.
                //
                //       This YieldRestEnumerable is a tricky class, and deserves additional testing and code
                //       review to ensure that it behaves correctly.
            }
            finally
            {
                // NOTE: We are using a try/block, rather than a using block, because using will result in
                //       the premature disposal of the iterator. There is no way around this, since assignment
                //       to using() variables is prohibited (they are readonly), and even declaring the used
                //       variable outside of the scope of the using() and then reassigning it inside, does not
                //       prevent disposal.
                if (iter != null)
                    iter.Dispose();
            }
        }

        /// <summary>
        /// Special <see cref="IEnumerable{T}"/> implementation that allows an existing, partially traversed
        /// iterator to be wrapped and returned as a sequence. Helps solve the special case of
        /// returning the rest of a partitioned sequence.
        /// </summary>
        /// <remarks>
        /// When reset, this enumerable switches is iteration strategy to use the original
        /// source sequence, skip the number of elements consumed be preceding partitions,
        /// and then yielding the remainder.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        private class YieldRestEnumerable<T> : IEnumerable<T>
        {
            #region Nested Classes
            private class NonDisposingEnumerator : IEnumerator<T>
            {
                private readonly IEnumerator<T> m_Iter;

                public NonDisposingEnumerator(IEnumerator<T> iter)
                {
                    m_Iter = iter;
                }

                public void Dispose()
                {
                    return; // does nothing by intent
                }

                public bool MoveNext()
                {
                    return m_Iter.MoveNext();
                }

                public void Reset()
                {
                    m_Iter.Reset();
                }

                public T Current
                {
                    get { return m_Iter.Current; }
                }

                object IEnumerator.Current
                {
                    get { return Current; }
                }
            }
            #endregion

            private readonly ulong m_ItemsToSkip;
            private readonly IEnumerator<T> m_Iterator;
            private bool m_IsInitialIteration = true;

            #region Constructor
            public YieldRestEnumerable(ulong itemsToSkip, IEnumerator<T> iter)
            {
                m_ItemsToSkip = itemsToSkip;
                m_Iterator = iter;
            }

            /// <summary>
            /// Disposes the underlying iterator
            /// </summary>
            ~YieldRestEnumerable()
            {
                m_Iterator.Dispose();
            }
            #endregion

            #region Implementation of IEnumerable
            public IEnumerator<T> GetEnumerator()
            {
                if (!m_IsInitialIteration)
                {
                    m_IsInitialIteration = false;
                    m_Iterator.Reset();
                    for (ulong i = 0; i < m_ItemsToSkip; i++)
                        m_Iterator.MoveNext();
                }
                return new NonDisposingEnumerator(m_Iterator);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            #endregion
        }
    }
}