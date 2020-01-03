#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Leandro F. Vieira (leandromoh). All rights reserved.
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

    [TestFixture]
    public class SkipLastTest
    {
        [TestCase( 0)]
        [TestCase(-1)]
        public void SkipLastWithCountLesserThanOne(int skip)
        {
            var numbers = Enumerable.Range(1, 5);

            Assert.That(numbers.SkipLast(skip), Is.EqualTo(numbers));
        }

        [Test]
        public void SkipLastEnumerable()
        {
            const int take = 100;
            const int skip = 20;

            var sequence = Enumerable.Range(1, take);

            var expectations = sequence.Take(take - skip);

            Assert.That(expectations, Is.EqualTo(sequence.SkipLast(skip)));
        }

        [Test]
        public void SkipLastCollection()
        {
            const int take = 100;
            const int skip = 20;

            var sequence = Enumerable.Range(1, take).ToArray();

            var expectations = sequence.Take(take - skip);

            Assert.That(expectations, Is.EqualTo(sequence.SkipLast(skip)));
        }

        [TestCase(5)]
        [TestCase(6)]
        public void SkipLastWithSequenceShorterThanCount(int skip)
        {
            Assert.That(Enumerable.Range(1, 5).SkipLast(skip), Is.Empty);
        }

        [Test]
        public void SkipLastIsLazy()
        {
            new BreakingSequence<object>().SkipLast(1);
        }
    }
}
