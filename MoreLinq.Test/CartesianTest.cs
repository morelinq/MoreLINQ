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
    using NUnit.Framework;

    /// <summary>
    /// Verify the behavior of the Cartesian operator
    /// </summary>
    [TestFixture]
    public class CartesianTests
    {
        /// <summary>
        /// Verify that the Cartesian product is evaluated in a lazy fashion on demand.
        /// </summary>
        [Test]
        public void TestCartesianIsLazy()
        {
            var bs = new BreakingSequence<string>();
            _ = bs.Cartesian(new BreakingSequence<int>(), BreakingFunc.Of<string, int, bool>());
        }

        /// <summary>
        /// Verify that the Cartesian product of two empty sequences is an empty sequence
        /// </summary>
        [Test]
        public void TestCartesianOfEmptySequences()
        {
            using var sequenceA = Enumerable.Empty<int>().AsTestingSequence();
            using var sequenceB = Enumerable.Empty<int>().AsTestingSequence();

            var result = sequenceA.Cartesian(sequenceB, (a, b) => a + b);

            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// Verify that the Cartesian product of an empty and non-empty sequence is an empty sequence
        /// </summary>
        [Test]
        public void TestCartesianOfEmptyAndNonEmpty()
        {
            var sequenceA = Enumerable.Empty<int>();
            var sequenceB = Enumerable.Repeat(1, 10);

            using (var tsA = sequenceA.AsTestingSequence())
            using (var tsB = sequenceB.AsTestingSequence())
            {
                var result = tsA.Cartesian(tsB, (a, b) => a + b);
                Assert.That(result, Is.EqualTo(sequenceA));
            }

            using (var tsA = sequenceA.AsTestingSequence())
            using (var tsB = sequenceB.AsTestingSequence())
            {
                var result = tsB.Cartesian(tsA, (a, b) => a + b);
                Assert.That(result, Is.EqualTo(sequenceA));
            }
        }

        /// <summary>
        /// Verify that the number of elements in a Cartesian product is the product of the number of elements of each sequence
        /// </summary>
        [Test]
        public void TestCartesianProductCount()
        {
            const int countA = 100;
            const int countB = 75;
            const int expectedCount = countA * countB;
            using var sequenceA = Enumerable.Range(1, countA).AsTestingSequence();
            using var sequenceB = Enumerable.Range(1, countB).AsTestingSequence();

            var result = sequenceA.Cartesian(sequenceB, (a, b) => a + b);

            Assert.That(result.Count(), Is.EqualTo(expectedCount));
        }

        /// <summary>
        /// Verify that the number of elements in a Cartesian product is the product of the number of elements of each sequence,
        /// even when there are more than two sequences
        /// </summary>
        [Test]
        public void TestCartesianProductCount_Multidimensional()
        {
            const int countA = 10;
            const int countB = 9;
            const int countC = 8;
            const int countD = 7;

            using var sequenceA = Enumerable.Range(1, countA).AsTestingSequence();
            using var sequenceB = Enumerable.Range(1, countB).AsTestingSequence();
            using var sequenceC = Enumerable.Range(1, countC).AsTestingSequence();
            using var sequenceD = Enumerable.Range(1, countD).AsTestingSequence();

            var result = sequenceA.Cartesian(sequenceB, sequenceC, sequenceD, (a, b, c, d) => a + b + c + d);

            const int expectedCount = countA * countB * countC * countD;
            Assert.That(result.Count(), Is.EqualTo(expectedCount));
        }

        /// <summary>
        /// Verify that each combination is produced in the Cartesian product
        /// </summary>
        [Test]
        public void TestCartesianProductCombinations()
        {
            var sequenceA = Enumerable.Range(0, 5);
            var sequenceB = Enumerable.Range(0, 5);

            var expectedSet = new[]
            {
                Enumerable.Repeat(false, 5).ToArray(),
                Enumerable.Repeat(false, 5).ToArray(),
                Enumerable.Repeat(false, 5).ToArray(),
                Enumerable.Repeat(false, 5).ToArray(),
                Enumerable.Repeat(false, 5).ToArray()
            };

            using var tsA = sequenceA.AsTestingSequence();
            using var tsB = sequenceB.AsTestingSequence();

            var result = tsA.Cartesian(tsB, (a, b) => new { A = a, B = b })
                            .ToArray();

            // verify that the expected number of results is correct
            Assert.That(result.Count(), Is.EqualTo(sequenceA.Count() * sequenceB.Count()));

            // ensure that all "cells" were visited by the cartesian product
            foreach (var coord in result)
                expectedSet[coord.A][coord.B] = true;
            Assert.That(expectedSet.SelectMany(x => x).All(z => z), Is.True);
        }

        /// <summary>
        /// Verify that if either sequence passed to Cartesian is empty, the result
        /// is an empty sequence.
        /// </summary>
        [Test]
        public void TestEmptyCartesianEvaluation()
        {
            using var sequence = Enumerable.Range(0, 5).AsTestingSequence();

            var resultA = sequence.Cartesian(Enumerable.Empty<int>(), (a, b) => new { A = a, B = b });
            var resultB = Enumerable.Empty<int>().Cartesian(sequence, (a, b) => new { A = a, B = b });
            var resultC = Enumerable.Empty<int>().Cartesian(Enumerable.Empty<int>(), (a, b) => new { A = a, B = b });

            Assert.That(resultA.Count(), Is.Zero);
            Assert.That(resultB.Count(), Is.Zero);
            Assert.That(resultC.Count(), Is.Zero);
        }
    }
}
