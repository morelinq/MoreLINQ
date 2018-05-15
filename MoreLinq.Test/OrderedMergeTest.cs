using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    class OrderedMergeTest {
        [Test]
        public void ShouldBeLazy() {
            var first = new BreakingSequence<object>();
            var second = new BreakingSequence<object>();
            Assert.DoesNotThrow(() => MoreEnumerable.OrderedMerge(first, second, id => id, id => id, id => id, id => id, (f, _) => f, null));
        }

        [Test]
        public void ShouldDisposeEnumerators() {
            var firstDisposed = false;
            var first = new int[] { }.AsVerifiable();
            first.WhenDisposed(_ => firstDisposed = true);

            var secondDisposed = false;
            var second = new int[] { }.AsVerifiable();
            second.WhenDisposed(_ => secondDisposed = true);

            MoreEnumerable.OrderedMerge(first, second, id => id, id => id, id => id, id => id, (f, _) => f, null).ToArray();

            Assert.IsTrue(firstDisposed, "First was not disposed");
            Assert.IsTrue(secondDisposed, "Second was not disposed");
        }

        [Test]
        public void IfThereAreNoMoreElementsToReturnFromTheFirstCollectionThenReturnTheRemainingSecondCollection() {
            var first = new int[] { };
            var second = new[] { 1, 2, 3 };

            var merged = MoreEnumerable.OrderedMerge(first, second, id => id, id => id, id => id, id => id, (f, _) => f, null);

            Assert.That(merged, Is.EqualTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void IfThereAreNoMoreElementsToReturnFromTheSecondCollectionThenReturnTheRemainingFirstCollection() {
            var first = new [] { 1, 2, 3 };
            var second = new int[] { };

            var merged = MoreEnumerable.OrderedMerge(first, second, id => id, id => id, id => id, id => id, (f, _) => f, null);

            Assert.That(merged, Is.EqualTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void TwoSequencesWithNoCollistionsShouldMergeUsingTheDefaultComparer() {
            var first = new[] { 1, 4, 5 };
            var second = new [] { 2, 3, 6 };

            var merged = MoreEnumerable.OrderedMerge(first, second, id => id, id => id, id => id, id => id, (f, _) => f, null);

            Assert.That(merged, Is.EqualTo(new[] { 1, 2, 3, 4, 5, 6 }));
        }

        [Test]
        public void WhenThereIsACollisionShouldUseBothSelectorToChooseFromFirstOfSecondCollection() {
            var firstElement = new Version(3, 0);
            var secondElement = new Version(3, 0);

            var first = new[] { firstElement };
            var second = new[] { secondElement };

            Assert.That(MoreEnumerable.OrderedMerge(first, second, id => id, id => id, id => id, id => id, (f, _) => f, null).First(), Is.SameAs(firstElement), "Should have returned First");
            Assert.That(MoreEnumerable.OrderedMerge(first, second, id => id, id => id, id => id, id => id, (_, s) => s, null).First(), Is.SameAs(secondElement), "Should have returned Second");
        }

        [Test]
        public void ShouldBeAbleToSelectKeyOfFirstAndSecondCollection() {
            var firstElement = new Version(2, 4);
            var secondElement = new Version(3, 0);

            var first = new[] { firstElement };
            var second = new[] { secondElement };
            
            var merged = MoreEnumerable.OrderedMerge(
                first,
                second,
                version => version.Major * 2,
                version => version.Major,
                id => id, id => id,
                (f, _) => f,
                null);

            Assert.That(merged, Is.EqualTo(new[] { secondElement, firstElement }));
        }

        [Test]
        public void FirstAndSecondCanBeOfDifferentInputTypesWithASharedOutputType() {
            int[] first = {1, 4, 5};
            string[] second = {"2", "3", "6"};
            
            var merged = MoreEnumerable.OrderedMerge(
                first,
                second,
                intValue => intValue,
                int.Parse,
                intValue => intValue,
                int.Parse,
                (f, _) => f,
                null);

            Assert.That(merged, Is.EqualTo(new[] { 1, 2, 3, 4, 5, 6 }));
        }

        [Test]
        public void ShouldBeAbleToUseNonDefaultComparer() {
            var first = new[] { 1, 4, 5 };
            var second = new[] { 2, 3, 6 };

            var merged = MoreEnumerable.OrderedMerge(first, second, id => id, id => id, id => id, id => id, (f, _) => f, new UpendedComparer());

            Assert.That(merged, Is.EqualTo(new[] { 2, 3, 6, 1, 4, 5 }));
        }

        class UpendedComparer : IComparer<int> {
            public int Compare(int x, int y) => y.CompareTo(x);
        }
    }
}
