namespace MoreLinq.Test
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// Verify the behavior of the Segment operator
    /// </summary>
    [TestFixture]
    public class SegmentTests
    {
        /// <summary>
        /// Verify that the Segment operator behaves in a lazy manner
        /// </summary>
        [Test]
        public void TestSegmentIsLazy()
        {
            new BreakingSequence<int>().Segment(BreakingFunc.Of<int, bool>());
            new BreakingSequence<int>().Segment(BreakingFunc.Of<int, int, bool>());
            new BreakingSequence<int>().Segment(BreakingFunc.Of<int, int, int, bool>());
        }

        /// <summary>
        /// Verify that segmenting a sequence into a single sequence results in the original sequence.
        /// </summary>
        [Test]
        public void TestIdentitySegment()
        {
            const int count = 5;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Segment(x => false);

            Assert.That(result.Single(), Is.EqualTo(sequence));
        }

        /// <summary>
        /// Verify that segmenting an empty sequence results in an empty sequence of segments.
        /// </summary>
        [Test]
        public void TestEmptySequence()
        {
            var sequence = Enumerable.Repeat(-1, 0);
            var result = sequence.Segment(x => true);
            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// Verify that the segments returned can be enumerated more than once.
        /// </summary>
        [Test]
        public void TestSegmentIsIdempotent()
        {
            const int value = -1;
            var sequence = Enumerable.Repeat(value, 10);
            var result = sequence.Segment(x => true);

            foreach (var segment in result)
            {
                for (var i = 0; i < 2; i++)
                {
                    Assert.IsTrue(segment.Any());
                    Assert.AreEqual(value, segment.Single());
                }
            }
        }

        /// <summary>
        /// Verify that the first segment is never empty. By definition, segmentation
        /// begins with the second element in the source sequence.
        /// </summary>
        [Test]
        public void TestFirstSegmentNeverEmpty()
        {
            var sequence = Enumerable.Repeat(-1, 10);
            var resultA = sequence.Segment(x => true);
            var resultB = sequence.Segment((x, index) => true);
            var resultC = sequence.Segment((x, prevX, index) => true);

            Assert.IsTrue(resultA.First().Any());
            Assert.IsTrue(resultB.First().Any());
            Assert.IsTrue(resultC.First().Any());
        }

        /// <summary>
        /// Verify invariant that segmentation begins with second element of source sequence.
        /// </summary>
        [Test]
        public void TestSegmentationStartsWithSecondItem()
        {
            var sequence = new[] { 0 };
            var resultA = sequence.Segment(BreakingFunc.Of<int, bool>());
            var resultB = sequence.Segment(BreakingFunc.Of<int, int, bool>());
            var resultC = sequence.Segment(BreakingFunc.Of<int, int, int, bool>());

            Assert.IsTrue(resultA.Any());
            Assert.IsTrue(resultB.Any());
            Assert.IsTrue(resultC.Any());
        }

        /// <summary>
        /// Verify we can segment a source sequence by it's zero-based index
        /// </summary>
        [Test]
        public void VerifyCanSegmentByIndex()
        {
            const int count = 100;
            const int segmentSize = 2;

            var sequence = Enumerable.Repeat(1, count);
            var result = sequence.Segment((x, i) => i % segmentSize == 0);

            Assert.AreEqual(count / segmentSize, result.Count());
            foreach (var segment in result)
            {
                Assert.AreEqual(segmentSize, segment.Count());
            }
        }

        /// <summary>
        /// Verify that we can segment a source sequence by the change in adjacent items
        /// </summary>
        [Test]
        public void VerifyCanSegmentByPrevious()
        {
            const int repCount = 5;
            var sequence = Enumerable.Range(1, 3)
                                     .SelectMany(x => Enumerable.Repeat(x, repCount));
            var result = sequence.Segment((curr, prev, i) => curr != prev);

            Assert.AreEqual(sequence.Distinct().Count(), result.Count());
            Assert.IsTrue(result.All(s => s.Count() == repCount));
        }
    }
}
