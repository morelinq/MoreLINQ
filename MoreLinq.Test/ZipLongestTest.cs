#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
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
    using Tuple = System.ValueTuple;

    [TestFixture]
    public class ZipLongestTest
    {
        [Test]
        public void BothSequencesDisposedWithUnequalLengthsAndLongerFirst()
        {
            using (var longer = TestingSequence.Of(1, 2, 3))
            using (var shorter = TestingSequence.Of(1, 2))
            {
                longer.ZipLongest(shorter, (x, y) => x + y).Consume();
            }
        }

        [Test]
        public void BothSequencesDisposedWithUnequalLengthsAndShorterFirst()
        {
            using (var longer = TestingSequence.Of(1, 2, 3))
            using (var shorter = TestingSequence.Of(1, 2))
            {
                shorter.ZipLongest(longer, (x, y) => x + y).Consume();
            }
        }

        [Test]
        public void ZipWithEqualLengthSequences()
        {
            var zipped = new[] { 1, 2, 3 }.ZipLongest(new[] { 4, 5, 6 }, Tuple.Create);
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual((1, 4), (2, 5), (3, 6));
        }

        [Test]
        public void ZipWithFirstSequenceShorterThanSecond()
        {
            var zipped = new[] { 1, 2 }.ZipLongest(new[] { 4, 5, 6 }, Tuple.Create);
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual((1, 4), (2, 5), (0, 6));
        }

        [Test]
        public void ZipWithFirstSequnceLongerThanSecond()
        {
            var zipped = new[] { 1, 2, 3 }.ZipLongest(new[] { 4, 5 }, Tuple.Create);
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual((1, 4), (2, 5), (3, 0));
        }

        [Test]
        public void ZipLongestIsLazy()
        {
            var bs = new BreakingSequence<int>();
            bs.ZipLongest<int, int, int>(bs, delegate { throw new NotImplementedException(); });
        }

        [Test]
        public void ZipLongestDisposeSequencesEagerly()
        {
            var shorter = TestingSequence.Of(1, 2, 3);
            var longer = MoreEnumerable.Generate(1, x => x + 1);
            var zipped = shorter.ZipLongest(longer, Tuple.Create);

            var count = 0;
            foreach(var _ in zipped.Take(10))
            {
                if (++count == 4)
                    ((IDisposable)shorter).Dispose();
            }
        }
    }
}
