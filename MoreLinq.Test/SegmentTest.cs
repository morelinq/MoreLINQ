#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2010 Leopold Bushkin. All rights reserved.
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
    using System.Collections.Generic;
    using NUnit.Framework.Interfaces;
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

        static IEnumerable<T> Seq<T>(params T[] values) => values;

        public static readonly IEnumerable<ITestCaseData> TestData =
            from e in new[]
            {
                // input sequence is empty
                new { Source = Seq<int>(),            Expected = Seq<IEnumerable<int>>()         },
                // input sequence contains only new segment start
                new { Source = Seq(0, 3, 6),          Expected = Seq(Seq(0), Seq(3), Seq(6))     },
                // input sequence do not contains new segment start
                new { Source = Seq(1, 2, 4, 5),       Expected = Seq(Seq(1, 2, 4, 5))            },
                // input sequence start with a segment start
                new { Source = Seq(0, 1, 2, 3, 4, 5), Expected = Seq(Seq(0, 1, 2), Seq(3, 4, 5)) },
                // input sequence do not start with a segment start
                new { Source = Seq(1, 2, 3, 4, 5),    Expected = Seq(Seq(1, 2), Seq(3, 4, 5))    }
            }
            select new TestCaseData(e.Source).Returns(e.Expected);

        [Test, TestCaseSource(nameof(TestData))]
        public IEnumerable<IEnumerable<int>> TestSegment(IEnumerable<int> source)
        {
            return source.AsTestingSequence().Segment(v => v % 3 == 0);
        }
    }
}
