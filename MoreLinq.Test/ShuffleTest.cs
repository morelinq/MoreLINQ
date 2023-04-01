#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Leandro F. Vieira (leandromoh). All rights reserved.
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

    [TestFixture]
    public class ShuffleTest
    {
        static readonly Random Seed = new(12345);

        [Test]
        public void ShuffleIsLazy()
        {
            _ = new BreakingSequence<int>().Shuffle();
        }

        [Test]
        public void Shuffle()
        {
            var source = Enumerable.Range(1, 100);
            var result = source.Shuffle();

            Assert.That(result.OrderBy(x => x), Is.EqualTo(source));
        }

        [Test]
        public void ShuffleWithEmptySequence()
        {
            var source = Enumerable.Empty<int>();
            var result = source.Shuffle();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ShuffleIsIdempotent()
        {
            var sequence = Enumerable.Range(1, 100).ToArray();
            var sequenceClone = sequence.ToArray();

            // force complete enumeration of random subsets
            sequence.Shuffle().Consume();

            // verify the original sequence is untouched
            Assert.That(sequence, Is.EqualTo(sequenceClone));
        }

        [Test]
        public void ShuffleSeedIsLazy()
        {
            _ = new BreakingSequence<int>().Shuffle(Seed);
        }

        [Test]
        public void ShuffleSeed()
        {
            var source = Enumerable.Range(1, 100);
            var result = source.Shuffle(Seed);

            Assert.That(result, Is.Not.EqualTo(source));
            Assert.That(result.OrderBy(x => x), Is.EqualTo(source));
        }

        [Test]
        public void ShuffleSeedWithEmptySequence()
        {
            var source = Enumerable.Empty<int>();
            var result = source.Shuffle(Seed);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ShuffleSeedIsIdempotent()
        {
            var sequence = Enumerable.Range(1, 100).ToArray();
            var sequenceClone = sequence.ToArray();

            // force complete enumeration of random subsets
            sequence.Shuffle(Seed).Consume();

            // verify the original sequence is untouched
            Assert.That(sequence, Is.EqualTo(sequenceClone));
        }
    }
}
