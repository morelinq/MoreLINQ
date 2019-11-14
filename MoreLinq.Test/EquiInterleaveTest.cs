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
    using System;
    using NUnit.Framework;

    /// <summary>
    /// Verify the behavior of the Interleave operator
    /// </summary>
    [TestFixture]
    public class EquiInterleaveTests
    {
        /// <summary>
        /// Verify that EquiInterleave behaves in a lazy manner
        /// </summary>
        [Test]
        public void TestEquiInterleaveIsLazy()
        {
            new BreakingSequence<int>().EquiInterleave(new BreakingSequence<int>());
        }

        /// <summary>
        /// Verify that EquiInterleave disposes those enumerators that it managed
        /// to open successfully
        /// </summary>
        [Test]
        public void TestEquiInterleaveDisposesOnError()
        {
            using (var sequenceA = TestingSequence.Of<int>())
            {
                Assert.Throws<InvalidOperationException>(() => // Expected and thrown by BreakingSequence
                    sequenceA.EquiInterleave(new BreakingSequence<int>()).Consume());
            }
        }

        /// <summary>
        /// Verify that two balanced sequences will EquiInterleave all of their elements
        /// </summary>
        [Test]
        public void TestEquiInterleaveTwoBalancedSequences()
        {
            const int count = 10;
            var sequenceA = Enumerable.Range(1, count);
            var sequenceB = Enumerable.Range(1, count);
            var result = sequenceA.EquiInterleave(sequenceB);

            Assert.That(result, Is.EqualTo(Enumerable.Range(1, count).Select(x => new[] { x, x }).SelectMany(z => z)));
        }

        /// <summary>
        /// Verify that EquiInterleave with two empty sequences results in an empty sequence
        /// </summary>
        [Test]
        public void TestEquiInterleaveTwoEmptySequences()
        {
            var sequenceA = Enumerable.Empty<int>();
            var sequenceB = Enumerable.Empty<int>();
            var result = sequenceA.EquiInterleave(sequenceB);

            Assert.That(result, Is.EqualTo(Enumerable.Empty<int>()));
        }

        /// <summary>
        /// Verify that EquiInterleave throw on two unbalanced sequences
        /// </summary>
        [Test]
        public void TestEquiInterleaveThrowOnUnbalanced()
        {
            void Code()
            {
                var sequenceA = new[] { 0, 0, 0, 0, 0, 0 };
                var sequenceB = new[] { 1, 1, 1, 1 };
                sequenceA.EquiInterleave(sequenceB).Consume();
            }

            Assert.Throws<InvalidOperationException>(Code);
        }

        /// <summary>
        /// Verify that EquiInterleave multiple empty sequences results in an empty sequence
        /// </summary>
        [Test]
        public void TestEquiInterleaveManyEmptySequences()
        {
            var sequenceA = Enumerable.Empty<int>();
            var sequenceB = Enumerable.Empty<int>();
            var sequenceC = Enumerable.Empty<int>();
            var sequenceD = Enumerable.Empty<int>();
            var sequenceE = Enumerable.Empty<int>();
            var result = sequenceA.EquiInterleave(sequenceB, sequenceC, sequenceD, sequenceE);

            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// Verify that EquiInterleave throw on multiple unbalanced sequences
        /// </summary>
        [Test]
        public void TestEquiInterleaveManyImbalanceStrategySkip()
        {
            void Code()
            {
                var sequenceA = new[] {1, 5, 8, 11, 14, 16,};
                var sequenceB = new[] {2, 6, 9, 12,};
                var sequenceC = new int[] { };
                var sequenceD = new[] {3};
                var sequenceE = new[] {4, 7, 10, 13, 15, 17,};
                sequenceA.EquiInterleave(sequenceB, sequenceC, sequenceD, sequenceE).Consume();
            }

            Assert.Throws<InvalidOperationException>(Code);
        }

        /// <summary>
        /// Verify that Interleave disposes of all iterators it creates.
        /// </summary>
        [Test]
        public void TestEquiInterleaveDisposesAllIterators()
        {
            const int count = 10;

            using (var sequenceA = Enumerable.Range(1, count).AsTestingSequence())
            using (var sequenceB = Enumerable.Range(1, count).AsTestingSequence())
            using (var sequenceC = Enumerable.Range(1, count).AsTestingSequence())
            using (var sequenceD = Enumerable.Range(1, count).AsTestingSequence())
            {
                sequenceA.EquiInterleave(sequenceB, sequenceC, sequenceD).Consume();
            }
        }
    }
}
