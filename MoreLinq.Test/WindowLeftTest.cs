using NUnit.Framework.Interfaces;

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
        public void ModifyWindowBeforeMoveNextDoNotAffectNextWindow()
        {
            var sequence = Enumerable.Range(0, 3);
            using var e = sequence.WindowLeft(2).GetEnumerator();

            e.MoveNext();
            var window1 = e.Current;
            window1[1] = -1;
            e.MoveNext();
            var window2 = e.Current;

            Assert.That(window2[0], Is.EqualTo(1));
        }

        [Test]
        public void ModifyWindowAfterMoveNextDoNotAffectNextWindow()
        {
            var sequence = Enumerable.Range(0, 3);
            using var e = sequence.WindowLeft(2).GetEnumerator();

            e.MoveNext();
            var window1 = e.Current;
            e.MoveNext();
            window1[1] = -1;
            var window2 = e.Current;

            Assert.That(window2[0], Is.EqualTo(1));
        }

        [Test]
        public void ModifyWindowDoNotAffectPrevWindow()
        {
            var sequence = Enumerable.Range(0, 3);
            using var e = sequence.WindowLeft(2).GetEnumerator();

            e.MoveNext();
            var window1 = e.Current;
            e.MoveNext();
            var window2 = e.Current;
            window2[0] = -1;

            Assert.That(window1[1], Is.EqualTo(1));
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
                reader.Read().AssertSequenceEqual(1, 2, 3, 4, 5);
                reader.Read().AssertSequenceEqual(2, 3, 4, 5);
                reader.Read().AssertSequenceEqual(3, 4, 5);
                reader.Read().AssertSequenceEqual(4, 5);
                reader.Read().AssertSequenceEqual(5);
                reader.ReadEnd();
            }
        }

        [Test]
        public void WindowLeftWithWindowSizeSmallerThanSequence()
        {
            using (var sequence = Enumerable.Range(1, 5).AsTestingSequence())
            using (var reader = sequence.WindowLeft(3).Read())
            {
                reader.Read().AssertSequenceEqual(1, 2, 3);
                reader.Read().AssertSequenceEqual(2, 3, 4);
                reader.Read().AssertSequenceEqual(3, 4, 5);
                reader.Read().AssertSequenceEqual(4, 5);
                reader.Read().AssertSequenceEqual(5);
                reader.ReadEnd();
            }
        }

        static IEnumerable<T> Seq<T>(params T[] values) => values;

        public static readonly IEnumerable<ITestCaseData> TestData =
            from e in new[]
            {
                new {Source = Enumerable.Range(0, 4), Size = 1, Result = new[] {Seq(0), Seq(1), Seq(2), Seq(3)}},
                new {Source = Enumerable.Range(0, 4), Size = 2, Result = new[] {Seq(0, 1), Seq(1, 2), Seq(2, 3), Seq(3)}},
                new {Source = Enumerable.Range(0, 4), Size = 3, Result = new[] {Seq(0, 1, 2), Seq(1, 2, 3), Seq(2, 3), Seq(3)}},
                new {Source = Enumerable.Range(0, 4), Size = 4, Result = new[] {Seq(0, 1, 2, 3), Seq(1, 2, 3), Seq(2, 3), Seq(3)}}
            }
            select new TestCaseData(e.Source, e.Size).Returns(e.Result);

        [Test, TestCaseSource(nameof(TestData))]
        public IEnumerable<IEnumerable<int>> TestWindowLeftOnKnownResults(IEnumerable<int> sequence, int sizes)
        {
            using var testingSequence = sequence.AsTestingSequence();
            var r = testingSequence.WindowLeft(sizes).ToList();
            return r;
        }
    }
}
