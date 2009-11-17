namespace MoreLinq.Test
{
    using System;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;

    [TestFixture]
    public class EquiZipTest
    {
        private static Tuple<TFirst, TSecond> Tuple<TFirst, TSecond>(TFirst a, TSecond b)
        {
            return new Tuple<TFirst, TSecond>(a, b);
        }

        [Test]
        public void BothSequencesDisposedWithUnequalLengthsAndLongerFirst()
        {
            using (var longer = TestingSequence.Of(1, 2, 3))
            using (var shorter = TestingSequence.Of(1, 2))
            {
                // Yes, this will throw... but then we should still have disposed both sequences
                try
                {
                    longer.EquiZip(shorter, (x, y) => x + y).Consume();
                }
                catch (InvalidOperationException)
                {
                    // Expected
                }
            }
        }

        [Test]
        public void BothSequencesDisposedWithUnequalLengthsAndShorterFirst()
        {
            using (var longer = TestingSequence.Of(1, 2, 3))
            using (var shorter = TestingSequence.Of(1, 2))
            {
                // Yes, this will throw... but then we should still have disposed both sequences
                try
                {
                    shorter.EquiZip(longer, (x, y) => x + y).Consume();
                }
                catch (InvalidOperationException)
                {
                    // Expected
                }
            }
        }

        [Test]
        public void ZipWithEqualLengthSequencesFailStrategy()
        {
            var zipped = MoreEnumerable.EquiZip(new[] { 1, 2, 3 }, new[] { 4, 5, 6 },
                (x, y) => Tuple(x, y));
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual(Tuple(1, 4), Tuple(2, 5), Tuple(3, 6));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ZipWithFirstSequenceShorterThanSecondFailStrategy()
        {
            var zipped = MoreEnumerable.EquiZip(new[] { 1, 2 }, new[] { 4, 5, 6 },
                (x, y) => Tuple(x, y));
            Assert.That(zipped, Is.Not.Null);
            zipped.Consume();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ZipWithFirstSequnceLongerThanSecondFailStrategy()
        {
            var zipped = MoreEnumerable.EquiZip(new[] { 1, 2, 3 }, new[] { 4, 5 },
                (x, y) => Tuple(x, y));
            Assert.That(zipped, Is.Not.Null);
            zipped.Consume();
            zipped.AssertSequenceEqual(Tuple(1, 4), Tuple(2, 5));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ZipWithNullFirstSequence()
        {
            MoreEnumerable.EquiZip(null, new[] { 4, 5, 6 }, BreakingFunc.Of<int, int, int>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ZipWithNullSecondSequence()
        {
            MoreEnumerable.EquiZip(new[] { 1, 2, 3 }, null, BreakingFunc.Of<int, int, int>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ZipWithNullResultSelector()
        {
            MoreEnumerable.EquiZip<int, int, int>(new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, null);
        }

        [Test]
        public void ZipIsLazy()
        {
            MoreEnumerable.EquiZip<int, int, int>(
                new BreakingSequence<int>(),
                new BreakingSequence<int>(),
                delegate { throw new NotImplementedException(); });
        }
    }
}
