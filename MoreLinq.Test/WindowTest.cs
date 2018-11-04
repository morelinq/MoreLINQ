namespace MoreLinq.Test
{
    using NUnit.Framework;

    /// <summary>
    /// Verify the behavior of the Window operator
    /// </summary>
    [TestFixture]
    public class WindowTests
    {
        /// <summary>
        /// Verify that Window behaves in a lazy manner
        /// </summary>
        [Test]
        public void TestWindowIsLazy()
        {
            new BreakingSequence<int>().Window(1);
        }

        /// <summary>
        /// Verify that a negative window size results in an exception
        /// </summary>
        [Test]
        public void TestWindowNegativeWindowSizeException()
        {
            var sequence = Enumerable.Repeat(1, 10);

            AssertThrowsArgument.OutOfRangeException("size",() =>
                sequence.Window(-5));
        }

        /// <summary>
        /// Verify that a sliding window of an any size over an empty sequence
        /// is an empty sequence
        /// </summary>
        [Test]
        public void TestWindowEmptySequence()
        {
            var sequence = Enumerable.Empty<int>();
            var result = sequence.Window(5);

            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// Verify that decomposing a sequence into windows of a single item
        /// degenerates to the original sequence.
        /// </summary>
        [Test]
        public void TestWindowOfSingleElement()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Window(1);

            // number of windows should be equal to the source sequence length
            Assert.AreEqual(count, result.Count());
            // each window should contain single item consistent of element at that offset
            var index = -1;
            foreach (var window in result)
                Assert.AreEqual(sequence.ElementAt(++index), window.Single());
        }

        /// <summary>
        /// Verify that asking for a window large than the source sequence results
        /// in a empty sequence.
        /// </summary>
        [Test]
        public void TestWindowLargerThanSequence()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Window(count + 1);

            // there should only be one window whose contents is the same
            // as the source sequence
            Assert.That(result, Is.Empty);
        }

        /// <summary>
        /// Verify that asking for a window smaller than the source sequence results
        /// in N sequences, where N = (source.Count() - windowSize) + 1.
        /// </summary>
        [Test]
        public void TestWindowSmallerThanSequence()
        {
            const int count = 100;
            const int windowSize = count / 3;
            var sequence = Enumerable.Range(1, count);
            var result = sequence.Window(windowSize);

            // ensure that the number of windows is correct
            Assert.AreEqual(count - windowSize + 1, result.Count());
            // ensure each window contains the correct set of items
            var index = -1;
            foreach (var window in result)
                Assert.That(window, Is.EqualTo(sequence.Skip(++index).Take(windowSize)));
        }

        /// <summary>
        /// Verify that later windows do not modify any of the previous ones.
        /// </summary>

        [Test]
        public void TestWindowWindowsImmutability()
        {
            using (var windows = Enumerable.Range(1, 5).Window(2).AsTestingSequence())
            using (var reader = windows.ToArray().Read())
            {
                reader.Read().AssertSequenceEqual(1, 2);
                reader.Read().AssertSequenceEqual(2, 3);
                reader.Read().AssertSequenceEqual(3, 4);
                reader.Read().AssertSequenceEqual(4, 5);
                reader.ReadEnd();
            }
        }
    }
}
