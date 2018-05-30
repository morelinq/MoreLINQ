namespace MoreLinq.Test
{
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class WindowLeftTest
    {
        [Test]
        public void WindowLeftIsLazy()
        {
            new BreakingSequence<int>().WindowLeft(1);
        }

        [Test]
        public void WindowLeftWithNegativeWindowSize()
        {
            AssertThrowsArgument.OutOfRangeException("size", () =>
                Enumerable.Repeat(1, 10).WindowLeft(-5));
        }

        [Test]
        public void WindowLeftWithEmptySequence()
        {
            using (var xs = Enumerable.Empty<int>().AsTestingSequence())
            {
                var result = xs.WindowLeft(5);
                Assert.That(result, Is.Empty);
            }
        }

        [Test]
        public void WindowLeftWithSingleElement()
        {
            const int count = 100;
            var sequence = Enumerable.Range(1, count).ToArray();

            IList<int>[] result;
            using (var ts = sequence.AsTestingSequence())
                result = ts.WindowLeft(1).ToArray();

            // number of windows should be equal to the source sequence length
            Assert.That(result.Length, Is.EqualTo(count));

            // each window should contain single item consistent of element at that offset
            foreach (var window in result.Index())
                Assert.That(sequence.ElementAt(window.Key), Is.EqualTo(window.Value.Single()));
        }

        [Test]
        public void WindowLeftWithWindowSizeLargerThanSequence()
        {
            using (var sequence = Enumerable.Range(1, 5).AsTestingSequence())
            using (var reader = sequence.WindowLeft(10).Read())
            {
                Assert.That(reader.Read(), Is.EquivalentTo(new[] { 1, 2, 3, 4, 5 }));
                Assert.That(reader.Read(), Is.EquivalentTo(new[] { 2, 3, 4, 5 }));
                Assert.That(reader.Read(), Is.EquivalentTo(new[] { 3, 4, 5 }));
                Assert.That(reader.Read(), Is.EquivalentTo(new[] { 4, 5 }));
                Assert.That(reader.Read(), Is.EquivalentTo(new[] { 5 }));
                reader.ReadEnd();
            }
        }

        [Test]
        public void WindowLeftWithWindowSizeSmallerThanSequence()
        {
            using (var sequence = Enumerable.Range(1, 5).AsTestingSequence())
            using (var reader = sequence.WindowLeft(3).Read())
            {
                Assert.That(reader.Read(), Is.EquivalentTo(new[] { 1, 2, 3 }));
                Assert.That(reader.Read(), Is.EquivalentTo(new[] { 2, 3, 4 }));
                Assert.That(reader.Read(), Is.EquivalentTo(new[] { 3, 4, 5 }));
                Assert.That(reader.Read(), Is.EquivalentTo(new[] { 4, 5 }));
                Assert.That(reader.Read(), Is.EquivalentTo(new[] { 5 }));
                reader.ReadEnd();
            }
        }
    }
}
