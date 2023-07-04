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
    using NUnit.Framework;

    /// <summary>
    /// Verify the behavior of the Interleave operator
    /// </summary>
    [TestFixture]
    public class InterleaveTests
    {
        /// <summary>
        /// Verify that Interleave behaves in a lazy manner
        /// </summary>
        [Test]
        public void TestInterleaveIsLazy()
        {
            _ = new BreakingSequence<int>().Interleave(new BreakingSequence<int>());
        }

        /// <summary>
        /// Verify that interleaving disposes those enumerators that it managed
        /// to open successfully
        /// </summary>
        [Test]
        public void TestInterleaveDisposesOnErrorAtGetEnumerator()
        {
            using var sequenceA = TestingSequence.Of<int>();
            var sequenceB = new BreakingSequence<int>();

            // Expected and thrown by BreakingSequence
            Assert.That(() => sequenceA.Interleave(sequenceB).Consume(),
                        Throws.BreakException);
        }

        /// <summary>
        /// Verify that Interleave early throw ArgumentNullException when an element
        /// of otherSequences is null.
        /// </summary>
        [Test]
        public void TestInterleaveEarlyThrowOnNullElementInOtherSequences()
        {
            var sequenceA = Enumerable.Range(1, 1);
            var otherSequences = new IEnumerable<int>[] { null! };

            Assert.That(() => sequenceA.Interleave(otherSequences),
                        Throws.ArgumentNullException("otherSequences"));
        }

        /// <summary>
        /// Verify that interleaving disposes those enumerators that it managed
        /// to open successfully
        /// </summary>
        [Test]
        public void TestInterleaveDisposesOnErrorAtMoveNext()
        {
            using var sequenceA = TestingSequence.Of<int>();
            using var sequenceB = MoreEnumerable.From<int>(() => throw new TestException()).AsTestingSequence();

            // Expected and thrown by sequenceB
            Assert.That(() => sequenceA.Interleave(sequenceB).Consume(),
                        Throws.TypeOf<TestException>());
        }

        /// <summary>
        /// Verify that interleaving do not call enumerable GetEnumerator method eagerly
        /// </summary>
        [Test]
        public void TestInterleaveDoNotCallGetEnumeratorEagerly()
        {
            using var sequenceA = TestingSequence.Of(1);
            var sequenceB = new BreakingSequence<int>();

            sequenceA.Interleave(sequenceB).Take(1).Consume();
        }

        /// <summary>
        /// Verify that interleaving do not call enumerators MoveNext method eagerly
        /// </summary>
        [Test]
        public void TestInterleaveDoNoCallMoveNextEagerly()
        {
            using var sequenceA = TestingSequence.Of(1);
            using var sequenceB = MoreEnumerable.From<int>(() => throw new TestException())
                                                .AsTestingSequence();
            var result = sequenceA.Interleave(sequenceB).Take(1);

            Assert.That(() => result.Consume(), Throws.Nothing);
        }

        /// <summary>
        /// Verify that interleaving disposes those enumerators that it managed
        /// to open successfully
        /// </summary>
        [Test]
        public void TestInterleaveDisposesOnError()
        {
            using var sequenceA = TestingSequence.Of<int>();

            Assert.That(() => sequenceA.Interleave(new BreakingSequence<int>()).Consume(),
                        Throws.BreakException); // Expected and thrown by BreakingSequence
        }

        /// <summary>
        /// Verify that, in case of partial enumeration, interleaving disposes those
        /// enumerators that it managed to open successfully
        /// </summary>
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void TestInterleaveDisposesOnPartialEnumeration(int count)
        {
            using var sequenceA = TestingSequence.Of(1);
            using var sequenceB = TestingSequence.Of(2);
            using var sequenceC = TestingSequence.Of(3);

            sequenceA.Interleave(sequenceB, sequenceC).Take(count).Consume();
        }

        /// <summary>
        /// Verify that two balanced sequences will interleave all of their elements
        /// </summary>
        [Test]
        public void TestInterleaveTwoBalancedSequences()
        {
            const int count = 10;
            var sequenceA = Enumerable.Range(1, count);
            var sequenceB = Enumerable.Range(1, count);
            var result = sequenceA.Interleave(sequenceB);

            Assert.That(result, Is.EqualTo(Enumerable.Range(1, count).Select(x => new[] { x, x }).SelectMany(z => z)));
        }

        /// <summary>
        /// Verify that interleaving two empty sequences results in an empty sequence
        /// </summary>
        [Test]
        public void TestInterleaveTwoEmptySequences()
        {
            var sequenceA = Enumerable.Empty<int>();
            var sequenceB = Enumerable.Empty<int>();
            var result = sequenceA.Interleave(sequenceB);

            Assert.That(result, Is.EqualTo(Enumerable.Empty<int>()));
        }

        /// <summary>
        /// Verify that interleaving two unequal sequences with the Skip strategy results in
        /// the shorter sequence being omitted from the interleave operation when consumed
        /// </summary>
        [Test]
        public void TestInterleaveTwoImbalanceStrategySkip()
        {
            var sequenceA = new[] { 0, 0, 0, 0, 0, 0 };
            var sequenceB = new[] { 1, 1, 1, 1 };
            var result = sequenceA.Interleave(sequenceB);

            var expectedResult = new[] { 0, 1, 0, 1, 0, 1, 0, 1, 0, 0 };

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Verify that interleaving multiple empty sequences results in an empty sequence
        /// </summary>
        [Test]
        public void TestInterleaveManyEmptySequences()
        {
            var sequenceA = Enumerable.Empty<int>();
            var sequenceB = Enumerable.Empty<int>();
            var sequenceC = Enumerable.Empty<int>();
            var sequenceD = Enumerable.Empty<int>();
            var sequenceE = Enumerable.Empty<int>();
            var result = sequenceA.Interleave(sequenceB, sequenceC, sequenceD, sequenceE);

            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// Verify that interleaving multiple unequal sequences with the Skip strategy
        /// results in sequences being omitted form the interleave operation when consumed
        /// </summary>
        [Test]
        public void TestInterleaveManyImbalanceStrategySkip()
        {
            var sequenceA = new[] { 1, 5, 8, 11, 14, 16, };
            var sequenceB = new[] { 2, 6, 9, 12, };
            var sequenceC = new int[] { };
            var sequenceD = new[] { 3 };
            var sequenceE = new[] { 4, 7, 10, 13, 15, 17, };
            var result = sequenceA.Interleave(sequenceB, sequenceC, sequenceD, sequenceE);

            var expectedResult = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Verify that Interleave disposes of all iterators it creates regardless of which strategy
        /// is used to interleave the sequences
        /// </summary>
        [Test]
        public void TestInterleaveDisposesAllIterators()
        {
            const int count = 10;

            using var sequenceA = Enumerable.Range(1, count).AsTestingSequence();
            using var sequenceB = Enumerable.Range(1, count - 1).AsTestingSequence();
            using var sequenceC = Enumerable.Range(1, count - 5).AsTestingSequence();
            using var sequenceD = Enumerable.Range(1, 0).AsTestingSequence();

            sequenceA.Interleave(sequenceB, sequenceC, sequenceD)
                     .Consume();
        }
    }
}
