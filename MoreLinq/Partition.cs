#region License and Terms
//
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008-9 Jonathan Skeet. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public static partial class MoreEnumerable
    {
        /// <summary>
        /// Indicates what to do with the current element when partitioning a sequence.
        /// </summary>
        private enum PartitionInstruction
        {
            /// <summary>
            /// Adds the item to the current partition and then yields the partition.
            /// A new partition is opened afterwards.
            /// </summary>
            Yield,
            /// <summary>
            /// Adds the item to the current partition.
            /// </summary>
            Fill
        }


        /// <summary>
        /// Partitions a sequence into equal-sized partitions.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence to partition.</param>
        /// <param name="size">Size of partitions.</param>
        /// <returns>A sequence of equal-sized partitions containing elements of the source collection.</returns>
        /// <remarks>
        /// the source sequence is exhausted before a complete partition could be filled, the partly filled partition is yielded.
        /// This operator uses deferred execution and streams its results (partitions and partition content). 
        /// Each partition is fully filled before it's yielded. 
        /// </remarks>
        
        public static IEnumerable<IEnumerable<TSource>> Partition<TSource>(this IEnumerable<TSource> source, int size)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (size <= 0) throw new ArgumentOutOfRangeException("size");

            var splitInstructions = GenerateByIndex(i => i % size == size - 1 ? PartitionInstruction.Yield : PartitionInstruction.Fill);
            return source.PartitionImpl(splitInstructions);
        }

        /// <summary>
        /// Partitions a sequence into a series of partitions. Their size is defined by <paramref name="partitions"/>.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
        /// <param name="source">The source sequence to partition.</param>
        /// <param name="partitions">A sequence of partition sizes, defining how many elements to place in each partition.</param>
        /// <returns>A sequence of sized partitions containing elements of the source collection.</returns>
        /// <remarks>
        /// If the source sequence is exhausted before a complete partition could be filled, the partly filled partition is yielded.
        /// This operator uses deferred execution and streams its results (partitions and partition content). 
        /// Each partition is fully filled before yielded.
        /// </remarks>      

        public static IEnumerable<IEnumerable<TSource>> Partition<TSource>(this IEnumerable<TSource> source,
            IEnumerable<int> partitions)
        {
            if (source == null) throw new ArgumentNullException("source");

            var splitInstructions = Enumerable.Empty<PartitionInstruction>();

            // Each partition shall be filled and then splitted
            foreach (var partitionSize in partitions)
            {
                if (partitionSize < 0)
                    throw new ArgumentException("Partition sizes may not be negative.");

                splitInstructions = splitInstructions.Concat(Enumerable.Repeat(PartitionInstruction.Fill, partitionSize - 1));
                splitInstructions = splitInstructions.Concat(PartitionInstruction.Yield);
            }

            return PartitionImpl(source, splitInstructions);
        }


        /// <summary>
        /// Zips the source and instruction sequence, partitioning the source sequence according to the corresponding instruction.
        /// A partition is buffered before it's yielded element by element.
        /// If either input sequence is exhausted and a partition has been partly filled, it is yielded too.
        /// </summary>
        
        private static IEnumerable<IEnumerable<TSource>> PartitionImpl<TSource>(this IEnumerable<TSource> source,
            IEnumerable<PartitionInstruction> splitInstructions)
        {
            Debug.Assert(source != null);
            Debug.Assert(splitInstructions != null);

            var collector = (IList<TSource>) new List<TSource>();
            var collectorFilled = false;

            // Zip shortest
            foreach (var itemInstructionPair in source.ZipShortest(splitInstructions, (x, y) => new { Item = x, Instruction = y }))
            {
                switch (itemInstructionPair.Instruction)
                {
                    case PartitionInstruction.Yield:
                        // add and yield afterwards
                        collector.Add(itemInstructionPair.Item);
                        // partition contents are streamed too
                        yield return collector.Select(x => x);

                        // advance to the next partition, reset the collector 
                        collector = new List<TSource>();
                        collectorFilled = false;
                        break;
                    case PartitionInstruction.Fill:
                        // add item to the collector and indicate the collector is filled
                        collectorFilled = true;
                        collector.Add(itemInstructionPair.Item);
                        break;
                }
            }

            // The source or instruction sequence is exhausted, yield the partly filled collector
            if (collectorFilled)
            {
                yield return collector.Select(x => x);
            }
        }
    }
}
