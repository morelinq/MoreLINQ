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
    /// Verify the behavior of the Exclude operator
    /// </summary>
    [TestFixture]
    public class ExcludeTests
    {
        /// <summary>
        /// Verify that Exclude behaves in a lazy manner
        /// </summary>
        [Test]
        public void TestExcludeIsLazy()
        {
            _ = new BreakingSequence<int>().Exclude(0, 10);
        }

        /// <summary>
        /// Verify that a negative startIndex parameter results in an exception
        /// </summary>
        [Test]
        public void TestExcludeNegativeStartIndexException()
        {
            Assert.That(() => Enumerable.Range(1, 10).Exclude(-10, 10),
                        Throws.ArgumentOutOfRangeException("startIndex"));
        }

        /// <summary>
        /// Verify that a negative count parameter results in an exception
        /// </summary>
        [Test]
        public void TestExcludeNegativeCountException()
        {
            Assert.That(() => Enumerable.Range(1, 10).Exclude(0, -5),
                        Throws.ArgumentOutOfRangeException("count"));
        }

        /// <summary>
        /// Verify that excluding with count equals zero returns the original source
        /// </summary>
        [Test]
        public void TestExcludeWithCountEqualsZero()
        {
            using var sequence = Enumerable.Range(1, 10).AsTestingSequence();
            var resultA = sequence.Exclude(5, 0);

            Assert.That(resultA, Is.SameAs(sequence));
        }

        /// <summary>
        /// Verify that excluding from an empty sequence results in an empty sequence
        /// </summary>
        [Test]
        public void TestExcludeEmptySequence()
        {
            var sequence = Enumerable.Empty<int>();
            var resultA = sequence.Exclude(0, 0);
            var resultB = sequence.Exclude(0, 10); // shouldn't matter how many we ask for past end
            var resultC = sequence.Exclude(5, 5);  // shouldn't matter where we start
            Assert.That(resultA, Is.EqualTo(sequence));
            Assert.That(resultB, Is.EqualTo(sequence));
            Assert.That(resultC, Is.EqualTo(sequence));
        }

        /// <summary>
        /// Verify we can exclude the beginning portion of a sequence
        /// </summary>
        [Test]
        public void TestExcludeSequenceHead()
        {
            const int count = 10;
            var xs = Enumerable.Range(1, count);
            using var sequence = xs.AsTestingSequence();
            var result = sequence.Exclude(0, count / 2);

            Assert.That(result, Is.EqualTo(xs.Skip(count / 2)));
        }

        /// <summary>
        /// Verify we can exclude the tail portion of a sequence
        /// </summary>
        [Test]
        public void TestExcludeSequenceTail()
        {
            const int count = 10;
            var xs = Enumerable.Range(1, count);
            using var sequence = xs.AsTestingSequence();
            var result = sequence.Exclude(count / 2, count);

            Assert.That(result, Is.EqualTo(xs.Take(count / 2)));
        }

        /// <summary>
        /// Verify we can exclude the middle portion of a sequence
        /// </summary>
        [Test]
        public void TestExcludeSequenceMiddle()
        {
            const int count = 10;
            const int startIndex = 3;
            const int excludeCount = 5;
            var xs = Enumerable.Range(1, count);
            using var sequence = xs.AsTestingSequence();
            var result = sequence.Exclude(startIndex, excludeCount);

            Assert.That(result, Is.EqualTo(xs.Take(startIndex).Concat(xs.Skip(startIndex + excludeCount))));
        }

        /// <summary>
        /// Verify that excluding the entire sequence results in an empty sequence
        /// </summary>
        [Test]
        public void TestExcludeEntireSequence()
        {
            const int count = 10;
            using var sequence = Enumerable.Range(1, count).AsTestingSequence();
            var result = sequence.Exclude(0, count);

            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// Verify that excluding past the end on a sequence excludes the appropriate elements
        /// </summary>
        [Test]
        public void TestExcludeCountGreaterThanSequenceLength()
        {
            const int count = 10;
            var xs = Enumerable.Range(1, count);
            using var sequence = xs.AsTestingSequence();
            var result = sequence.Exclude(1, count * 10);

            Assert.That(result, Is.EqualTo(xs.Take(1)));
        }

        /// <summary>
        /// Verify that beginning exclusion past the end of a sequence has no effect
        /// </summary>
        [Test]
        public void TestExcludeStartIndexGreaterThanSequenceLength()
        {
            const int count = 10;
            var xs = Enumerable.Range(1, count);
            using var sequence = xs.AsTestingSequence();
            var result = sequence.Exclude(count + 5, count);

            Assert.That(result, Is.EqualTo(xs));
        }
    }
}
