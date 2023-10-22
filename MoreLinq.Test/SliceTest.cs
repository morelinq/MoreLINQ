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
    using System.Collections.Generic;

    /// <summary>
    /// Verify the behavior of the Slice operator
    /// </summary>
    [TestFixture]
    public class SliceTests
    {
        /// <summary>
        /// Verify that Slice evaluates in a lazy manner.
        /// </summary>
        [Test]
        public void TestSliceIsLazy()
        {
            _ = new BreakingSequence<int>().Slice(10, 10);
        }

        /// <summary>
        /// Verify that a slice from 0 to sequence.Count() yields the source sequence.
        /// NOTE: Since there are two different implementations of Slice() - one optimized that's
        ///       for lists and one that operates on any <c>IEnumerable{T}</c>, this test examines both.
        /// </summary>
        [Test]
        public void TestSliceIdentity()
        {
            const int count = 100;
            var sequenceA = Enumerable.Range(1, count);
            var sequenceB = sequenceA.ToList();

            var resultA = sequenceA.Slice(0, count);
            var resultB = sequenceB.Slice(0, count);

            Assert.That(resultA, Is.EqualTo(sequenceA));
            Assert.That(resultB, Is.EqualTo(sequenceB));
        }

        /// <summary>
        /// Verify that Slice can yield just the first item.
        /// NOTE: Since there are two different implementations of Slice() - one optimized that's
        ///       for lists and one that operates on any <c>IEnumerable{T}</c>, this test examines both.
        /// </summary>
        [Test]
        public void TestSliceFirstItem()
        {
            const int count = 10;
            var sequenceA = Enumerable.Range(1, count);
            var sequenceB = sequenceA.ToList();
            var resultA = sequenceA.Slice(0, 1);
            var resultB = sequenceB.Slice(0, 1);

            Assert.That(resultA, Is.EqualTo(sequenceA.Take(1)));
            Assert.That(resultB, Is.EqualTo(sequenceB.Take(1)));
        }

        /// <summary>
        /// Verify that Slice can yield just the last item.
        /// NOTE: Since there are two different implementations of Slice() - one optimized that's
        ///       for lists and one that operates on any <c>IEnumerable{T}</c>, this test examines both.
        /// </summary>
        [Test]
        public void TestSliceLastItem()
        {
            const int count = 10;
            var sequenceA = Enumerable.Range(1, count);
            var sequenceB = sequenceA.ToList();
            var resultA = sequenceA.Slice(count - 1, 1);
            var resultB = sequenceB.Slice(count - 1, 1);

            Assert.That(resultA, Is.EqualTo(sequenceA.Skip(9).Take(1)));
            Assert.That(resultB, Is.EqualTo(sequenceB.Skip(9).Take(1)));
        }

        /// <summary>
        /// Verify that slice yields the correct set of items when it
        /// is completely contained (start,end) within the source sequence.
        /// NOTE: Since there are two different implementations of Slice() - one optimized that's
        ///       for lists and one that operates on any <c>IEnumerable{T}</c>, this test examines both.
        /// </summary>
        [Test]
        public void TestSliceSmallerThanSequence()
        {
            const int count = 10;
            var sequenceA = Enumerable.Range(1, count);
            var sequenceB = sequenceA.ToList();
            var resultA = sequenceA.Slice(4, 5);
            var resultB = sequenceB.Slice(4, 5);

            Assert.That(resultA, Is.EqualTo(sequenceA.Skip(4).Take(5)));
            Assert.That(resultB, Is.EqualTo(sequenceB.Skip(4).Take(5)));
        }

        /// <summary>
        /// Verify that Slice yields the correct set of items when it
        /// extends past the end of the source sequence.
        /// NOTE: Since there are two different implementations of Slice() - one optimized that's
        ///       for lists and one that operates on any <c>IEnumerable{T}</c>, this test examines both.
        /// </summary>
        [Test]
        public void TestSliceLongerThanSequence()
        {
            const int count = 100;
            var sequenceA = Enumerable.Range(1, count);
            var sequenceB = sequenceA.ToList();
            var resultA = sequenceA.Slice(count / 2, count);
            var resultB = sequenceB.Slice(count / 2, count);

            Assert.That(resultA, Is.EqualTo(sequenceA.Skip(count / 2).Take(count)));
            Assert.That(resultB, Is.EqualTo(sequenceB.Skip(count / 2).Take(count)));
        }

        /// <summary>
        /// Verify that slice is optimized for <see cref="IList{T}"/> and <see cref="IReadOnlyList{T}"/> implementations and does not
        /// unnecessarily traverse items outside of the slice region.
        /// </summary>
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        public void TestSliceOptimization(SourceKind sourceKind)
        {
            const int sliceStart = 4;
            const int sliceCount = 3;
            var sequence = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.ToSourceKind(sourceKind);

            var result = sequence.Slice(sliceStart, sliceCount);

            Assert.That(result.Count(), Is.EqualTo(sliceCount));
            Assert.That(Enumerable.Range(5, sliceCount), Is.EqualTo(result));
        }
    }
}
