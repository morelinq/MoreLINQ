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
    /// Tests that verify the Repeat() extension method.
    /// </summary>
    [TestFixture]
    public class RepeatTest
    {
        /// <summary>
        /// Verify that the repeat method returns results in a lazy manner.
        /// </summary>
        [Test]
        public void TestRepeatIsLazy()
        {
            _ = new BreakingSequence<int>().Repeat(5);
        }

        /// <summary>
        /// Verify that repeat will yield the expected number of items.
        /// </summary>
        [Test]
        public void TestRepeatBehavior()
        {
            const int count = 10;
            const int repeatCount = 3;
            var sequence = Enumerable.Range(1, 10);

            int[] result;
            using (var ts = sequence.AsTestingSequence())
                result = ts.Repeat(repeatCount).ToArray();

            var expectedResult = Enumerable.Empty<int>();
            for (var i = 0; i < repeatCount; i++)
                expectedResult = expectedResult.Concat(sequence);

            Assert.That(result.Length, Is.EqualTo(count * repeatCount));
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        /// <summary>
        /// Verify that repeat throws an exception when the repeat count is negative.
        /// </summary>
        [Test]
        public void TestNegativeRepeatCount()
        {
            Assert.That(() => Enumerable.Range(1, 10).Repeat(-3),
                        Throws.ArgumentOutOfRangeException("count"));
        }

        /// <summary>
        /// Verify applying Repeat without passing count produces a circular sequence
        /// </summary>
        [Test]
        public void TestRepeatForeverBehaviorSingleElementList()
        {
            const int value = 3;
            using var sequence = new[] { value }.AsTestingSequence();

            var result = sequence.Repeat();

            Assert.That(result.Take(100).All(x => x == value), Is.True);
        }

        /// <summary>
        /// Verify applying Repeat without passing count produces a circular sequence
        /// </summary>
        [Test]
        public void TestRepeatForeverBehaviorManyElementsList()
        {
            const int repeatCount = 30;
            const int rangeCount = 10;
            const int takeCount = repeatCount * rangeCount;

            var sequence = Enumerable.Range(1, rangeCount);

            int[] result;
            using (var ts = sequence.AsTestingSequence())
                result = ts.Repeat().Take(takeCount).ToArray();

            var expectedResult = Enumerable.Empty<int>();
            for (var i = 0; i < repeatCount; i++)
                expectedResult = expectedResult.Concat(sequence);

            Assert.That(expectedResult, Is.EqualTo(result));
        }

        /// <summary>
        /// Verify that the repeat method returns results in a lazy manner.
        /// </summary>
        [Test]
        public void TestRepeatForeverIsLazy()
        {
            _ = new BreakingSequence<int>().Repeat();
        }
    }
}
