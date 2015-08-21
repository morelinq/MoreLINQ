using System;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    /// <summary>
    /// Verify the behavior of the Partition family of operators
    /// </summary>
    [TestFixture]
    public class PartitionTests
    {
        /// <summary>
        /// Verify that partitioning a sequence into a single partition works
        /// </summary>
        [Test]
        public void TestIdentityPartition()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Partition(new[] { count });

            Assert.IsTrue(result.Single().SequenceEqual(sequence));
        }

        /// <summary>
        /// Verify that requesting a single partition shorter than the sequence results
        /// in just that sub sequence. This must be equivalent to Take()
        /// </summary>
        [Test]
        public void TestSinglePartitionShorterThanSequence()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Partition(new[] { count / 2 });

            Assert.IsTrue(result.Single().SequenceEqual(sequence.Take(count / 2)));
        }

        /// <summary>
        /// Verify that requesting a single partition longer than the sequence results
        /// in a valid partition that is as long as the source sequence can yield.
        /// </summary>
        [Test]
        public void TestSinglePartitionLongerThanSequence()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Partition(new[] { count * 2 });
            Assert.IsTrue(result.Single().SequenceEqual(sequence));
        }

        /// <summary>
        /// Verify that requesting multiple partitions whose combined lengths are shorter
        /// than the original sequence, results in the appropriate subset of the sequence.
        /// This must be equivalent to repeated calls to Take().
        /// </summary>
        [Test]
        public void TestMultiplePartitionsShorterThanSequence()
        {
            const int count = 100;
            var parSizes = new[] { 10, 20, 30, 40 };
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Partition(parSizes);

            var index = 0;
            foreach (var resultSequence in result)
            {
                Assert.AreEqual(parSizes[index], resultSequence.Count());
                Assert.IsTrue(resultSequence.SequenceEqual(sequence.Skip(parSizes.Take(index).Sum())
                                                                   .Take(parSizes[index])));
                ++index;
            }
        }

        /// <summary>
        /// Verify that requesting multiple partitions whose combined length is longer
        /// than the source sequence, results in fewer partitions whose contents is
        /// a complete partition of the original sequence.
        /// </summary>
        [Test]
        public void TestMultiplePartitionsLongerThanSequence()
        {
            const int count = 40;
            var parSizes = new[] { 10, 20, 30, 40 };
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Partition(parSizes);

            // compute the number of expected partitions...
            var index = 0;
            var expectedPartitions = 0;
            var remainingCount = count;
            var lastPartitionSize = 0;
            while (remainingCount > 0)
            {
                ++expectedPartitions;
                remainingCount -= parSizes[index++];
                lastPartitionSize = remainingCount > 0 ? remainingCount : lastPartitionSize;
            }

            // verify that the right number of partitions was produced
            Assert.AreEqual(expectedPartitions, result.Count());
            // verify the last partition contains the correct items
            Assert.IsTrue(sequence.Skip(count - lastPartitionSize).SequenceEqual(result.Last()));
        }

        /// <summary>
        /// Verify that iterating the contents of each partition is a stable, idempotent operation
        /// that does not consume additional memory.
        /// </summary>
        [Test]
        public void TestPartitioningIsIdempotent()
        {
            var sequence = Enumerable.Range(1, 100);

            var result = sequence.Partition(Enumerable.Repeat(10, 10));
            var index = 0;
            foreach (var partition in result)
            {
                for (var i = 0; i < 5; i++)
                    Assert.IsTrue(partition.SequenceEqual(sequence.Skip(index * 10).Take(10)));
                ++index;
            }

        }

        /// <summary>
        /// Verify that accessing a negative sized partition results in an exception
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPartitionNegativeValues()
        {
            var sequence = Enumerable.Range(1, 100);
            var result = sequence.Partition(new[] { 10, -10 });

            result.Skip(1).Count(); // force second partition to be evaluated
        }

        /// <summary>
        /// Verify that generating partitions is a lazy operation. We ensure this be asking for a
        /// partition that would result in an exception if yielded.
        /// </summary>
        [Test]
        public void TestPartitionEvaluationIsLazy()
        {
            const int count = 10;
            var sequence = Enumerable.Range(1, count).Concat(new BreakingSequence<int>());

            var result = sequence.Partition(new[] { count, count });
            Assert.IsTrue(result.First().Count() == count);
            // We specifically don't iterate to the next partition, to ensure it is not evaluated
        }

        /// <summary>
        /// Verify that partitioning all elements of a sequence and then recombining
        /// them results in the original sequence. Ensures that the partition operation
        /// is reflexive over the original sequence domain.
        /// and 
        /// </summary>
        [Test]
        public void TestPartitionCompleteness()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Partition(new[] { count / 2, count / 2 });
            Assert.IsTrue(result.SelectMany(x => x).SequenceEqual(sequence));
        }

        /// <summary>
        /// Verify that the version of partition that also performs projection correctly
        /// operates on the partitioned sequences.
        /// </summary>
        [Test]
        public void TestPartitionWithProjection()
        {
            var sequence = Enumerable.Repeat(1, 100);
            var result = sequence.Partition(Enumerable.Repeat(10, 10)).Select(seq => seq.Sum());

            Assert.IsTrue(result.All(x => x == 10));
        }
    }
}