namespace MoreLinq.Test
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class ShuffleTest
    {
        static Random seed = new Random(12345);

        [Test]
        public void ShuffleIsLazy()
        {
            new BreakingSequence<int>().Shuffle();
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
            new BreakingSequence<int>().Shuffle(seed);
        }

        [Test]
        public void ShuffleSeed()
        {
            var source = Enumerable.Range(1, 100);
            var result = source.Shuffle(seed);

            Assert.That(result, Is.Not.EqualTo(source));
            Assert.That(result.OrderBy(x => x), Is.EqualTo(source));
        }

        [Test]
        public void ShuffleSeedWithEmptySequence()
        {
            var source = Enumerable.Empty<int>();
            var result = source.Shuffle(seed);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ShuffleSeedIsIdempotent()
        {
            var sequence = Enumerable.Range(1, 100).ToArray();
            var sequenceClone = sequence.ToArray();

            // force complete enumeration of random subsets
            sequence.Shuffle(seed).Consume();

            // verify the original sequence is untouched
            Assert.That(sequence, Is.EqualTo(sequenceClone));
        }
    }
}
