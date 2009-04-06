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

        #region Default strategy (same tests as for ImbalancedZipStrategy.Truncate)
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
        #endregion

        #region Handling of ImbalancedZipStrategy.Truncate
        [Test]
        public void BothSequencesDisposedWithUnequalLengthsTruncateStrategy()
        {
            var longer = DisposeTestingSequence.Of(1, 2, 3);
            var shorter = DisposeTestingSequence.Of(1, 2);

            longer.Zip(shorter, (x, y) => x + y, ImbalancedZipStrategy.Truncate).Consume();
            longer.AssertDisposed();
            shorter.AssertDisposed();

            // Just in case it works one way but not the other...
            shorter.Zip(longer, (x, y) => x + y, ImbalancedZipStrategy.Truncate).Consume();
            longer.AssertDisposed();
            shorter.AssertDisposed();
        }

        [Test]
        public void ZipWithEqualLengthSequencesTruncateStrategy()
        {
            var zipped = MoreEnumerable.Zip(new[] { 1, 2, 3 }, new[] { 4, 5, 6 },
                (x, y) => Tuple(x, y), ImbalancedZipStrategy.Truncate);
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual(Tuple(1, 4), Tuple(2, 5), Tuple(3, 6));
        }

        [Test]
        public void ZipWithFirstSequenceShorterThanSecondTruncateStrategy()
        {
            var zipped = MoreEnumerable.Zip(new[] { 1, 2 }, new[] { 4, 5, 6 },
                (x, y) => Tuple(x, y), ImbalancedZipStrategy.Truncate);
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual(Tuple(1, 4), Tuple(2, 5));
        }

        [Test]
        public void ZipWithFirstSequnceLongerThanSecondTruncateStrategy()
        {
            var zipped = MoreEnumerable.Zip(new[] { 1, 2, 3 }, new[] { 4, 5 },
                (x, y) => Tuple(x, y), ImbalancedZipStrategy.Truncate);
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual(Tuple(1, 4), Tuple(2, 5));
        }
        #endregion

        #region Handling of ImbalancedZipStrategy.Pad
        [Test]
        public void BothSequencesDisposedWithUnequalLengthsPadStrategy()
        {
            var longer = DisposeTestingSequence.Of(1, 2, 3);
            var shorter = DisposeTestingSequence.Of(1, 2);

            longer.Zip(shorter, (x, y) => x + y, ImbalancedZipStrategy.Pad).Consume();
            longer.AssertDisposed();
            shorter.AssertDisposed();

            // Just in case it works one way but not the other...
            shorter.Zip(longer, (x, y) => x + y, ImbalancedZipStrategy.Pad).Consume();
            longer.AssertDisposed();
            shorter.AssertDisposed();
        }

        [Test]
        public void ZipWithEqualLengthSequencesPadStrategy()
        {
            var zipped = MoreEnumerable.Zip(new[] { 1, 2, 3 }, new[] { 4, 5, 6 },
                (x, y) => Tuple(x, y), ImbalancedZipStrategy.Pad);
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual(Tuple(1, 4), Tuple(2, 5), Tuple(3, 6));
        }

        [Test]
        public void ZipWithFirstSequenceShorterThanSecondPadStrategy()
        {
            var zipped = MoreEnumerable.Zip(new[] { 1, 2 }, new[] { 4, 5, 6 },
                (x, y) => Tuple(x, y), ImbalancedZipStrategy.Pad);
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual(Tuple(1, 4), Tuple(2, 5), Tuple(0, 6));
        }

        [Test]
        public void ZipWithFirstSequnceLongerThanSecondPadStrategy()
        {
            var zipped = MoreEnumerable.Zip(new[] { 1, 2, 3 }, new[] { 4, 5 },
                (x, y) => Tuple(x, y), ImbalancedZipStrategy.Pad);
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual(Tuple(1, 4), Tuple(2, 5), Tuple(3, 0));
        }
        #endregion

        #region Handling of ImbalancedZipStrategy.Fail
        [Test]
        public void BothSequencesDisposedWithUnequalLengthsFailStrategy()
        {
            var longer = DisposeTestingSequence.Of(1, 2, 3);
            var shorter = DisposeTestingSequence.Of(1, 2);

            // Yes, this will throw... but then we should still have disposed both sequences
            try
            {
                longer.Zip(shorter, (x, y) => x + y, ImbalancedZipStrategy.Fail).Consume();
            }
            catch (InvalidOperationException)
            {
                // Expected
            }
            longer.AssertDisposed();
            shorter.AssertDisposed();

            // Just in case it works one way but not the other...
            try
            {
                shorter.Zip(longer, (x, y) => x + y, ImbalancedZipStrategy.Fail).Consume();
            }
            catch (InvalidOperationException)
            {
                // Expected
            }
            longer.AssertDisposed();
            shorter.AssertDisposed();
        }

        [Test]
        public void ZipWithEqualLengthSequencesFailStrategy()
        {
            var zipped = MoreEnumerable.Zip(new[] { 1, 2, 3 }, new[] { 4, 5, 6 },
                (x, y) => Tuple(x, y), ImbalancedZipStrategy.Fail);
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual(Tuple(1, 4), Tuple(2, 5), Tuple(3, 6));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ZipWithFirstSequenceShorterThanSecondFailStrategy()
        {
            var zipped = MoreEnumerable.Zip(new[] { 1, 2 }, new[] { 4, 5, 6 },
                (x, y) => Tuple(x, y), ImbalancedZipStrategy.Fail);
            Assert.That(zipped, Is.Not.Null);
            zipped.Consume();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ZipWithFirstSequnceLongerThanSecondFailStrategy()
        {
            var zipped = MoreEnumerable.Zip(new[] { 1, 2, 3 }, new[] { 4, 5 },
                (x, y) => Tuple(x, y), ImbalancedZipStrategy.Fail);
            Assert.That(zipped, Is.Not.Null);
            zipped.Consume();
            zipped.AssertSequenceEqual(Tuple(1, 4), Tuple(2, 5));
        }
        #endregion

        #region Invalid arguments
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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ZipWithInvalidStrategy()
        {
            MoreEnumerable.Zip(new[] { 1, 2, 3 }, new[] { 4, 5, 6 },
                (x, y) => Tuple(x, y), (ImbalancedZipStrategy)10);
        }
        #endregion

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
