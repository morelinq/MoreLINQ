namespace MoreLinq.Test
{
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class WindowRightTest
    {
        [Test]
        public void WindowRightIsLazy()
        {
            new BreakingSequence<int>().WindowRight(1);
        }

        [Test]
        public void WindowRightWithNegativeWindowSize()
        {
            AssertThrowsArgument.OutOfRangeException("size", () =>
                Enumerable.Repeat(1, 10).WindowRight(-5));
        }

        [Test]
        public void WindowRightWithEmptySequence()
        {
            using (var xs = Enumerable.Empty<int>().AsTestingSequence())
            {
                var result = xs.WindowRight(5);
                Assert.That(result, Is.Empty);
            }
        }

        [Test]
        public void WindowRightWithSingleElement()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count).ToArray();

            IList<int>[] result;
            using (var ts = sequence.AsTestingSequence())
                result = ts.WindowRight(1).ToArray();

            // number of windows should be equal to the source sequence length
            Assert.That(result.Length, Is.EqualTo(count));

            // each window should contain single item consistent of element at that offset
            foreach (var window in result.Index())
                Assert.That(sequence.ElementAt(window.Key), Is.EqualTo(window.Value.Single()));
        }

        [Test]
        public void WindowRightWithWindowSizeLargerThanSequence()
        {
            using (var sequence = Enumerable.Range(1, 5).AsTestingSequence())
            using (var reader = sequence.WindowRight(10).Read())
            {
                reader.Read().AssertSequenceEqual(            1);
                reader.Read().AssertSequenceEqual(         1, 2);
                reader.Read().AssertSequenceEqual(      1, 2, 3);
                reader.Read().AssertSequenceEqual(   1, 2, 3, 4);
                reader.Read().AssertSequenceEqual(1, 2, 3, 4, 5);
                reader.ReadEnd();
            }
        }

        [Test]
        public void WindowRightWithWindowSizeSmallerThanSequence()
        {
            using (var sequence = Enumerable.Range(1, 5).AsTestingSequence())
            using (var reader = sequence.WindowRight(3).Read())
            {
                reader.Read().AssertSequenceEqual(      1);
                reader.Read().AssertSequenceEqual(   1, 2);
                reader.Read().AssertSequenceEqual(1, 2, 3);
                reader.Read().AssertSequenceEqual(2, 3, 4);
                reader.Read().AssertSequenceEqual(3, 4, 5);
                reader.ReadEnd();
            }
        }
    }
}
