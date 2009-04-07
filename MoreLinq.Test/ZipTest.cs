namespace MoreLinq.Test
{
    using System;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;

    [TestFixture]
    public class ZipTest
    {
        private static Tuple<TFirst, TSecond> Tuple<TFirst, TSecond>(TFirst a, TSecond b)
        {
            return new Tuple<TFirst, TSecond>(a, b);
        }

        [Test]
        public void BothSequencesDisposedWithUnequalLengths()
        {
            var longer = DisposeTestingSequence.Of(1, 2, 3);
            var shorter = DisposeTestingSequence.Of(1, 2);

            longer.Zip(shorter, (x, y) => x + y).Consume();
            longer.AssertDisposed();
            shorter.AssertDisposed();

            // Just in case it works one way but not the other...
            shorter.Zip(longer, (x, y) => x + y).Consume();
            longer.AssertDisposed();
            shorter.AssertDisposed();
        }

        [Test]
        public void ZipWithEqualLengthSequences()
        {
            var zipped = MoreEnumerable.Zip(new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, (x, y) => Tuple(x, y));
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual(Tuple(1, 4), Tuple(2, 5), Tuple(3, 6));
        }

        [Test]
        public void ZipWithFirstSequenceShorterThanSecond()
        {
            var zipped = MoreEnumerable.Zip(new[] { 1, 2 }, new[] { 4, 5, 6 }, (x, y) => Tuple(x, y));
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual(Tuple(1, 4), Tuple(2, 5));
        }

        [Test]
        public void ZipWithFirstSequnceLongerThanSecond()
        {
            var zipped = MoreEnumerable.Zip(new[] { 1, 2, 3 }, new[] { 4, 5 }, (x, y) => Tuple(x, y));
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual(Tuple(1, 4), Tuple(2, 5));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ZipWithNullFirstSequence()
        {
            MoreEnumerable.Zip(null, new[] { 4, 5, 6 }, BreakingFunc.Of<int, int, int>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ZipWithNullSecondSequence()
        {
            MoreEnumerable.Zip(new[] { 1, 2, 3 }, null, BreakingFunc.Of<int, int, int>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ZipWithNullResultSelector()
        {
            MoreEnumerable.Zip<int, int, int>(new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, null);
        }

        [Test]
        public void ZipIsLazy()
        {
            MoreEnumerable.Zip<int, int, int>(
                new BreakingSequence<int>(),
                new BreakingSequence<int>(),
                delegate { throw new NotImplementedException(); });
        }
    }
}
