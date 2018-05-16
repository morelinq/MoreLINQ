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

        [TestFixture]
        public class AllParametersEquivalent { 

            [Test]
            public void first_second() {
                var first = new[] { 1, 4, 5 };
                var second = new int[] { 2, 3, 6 };

                var exampleMerged = MoreEnumerable.OrderedMerge(
                    first,
                    second);

                int Id(int id) => id;
                int First(int f, int _) => f;

                var allParametersMerged = MoreEnumerable.OrderedMerge(
                    first: first,
                    second: second,
                    firstKeySelector: Id,
                    secondKeySelector: Id,
                    firstSelector: Id,
                    secondSelector: Id,
                    bothSelector: First,
                    comparer: null);

                Assert.That(exampleMerged, Is.EqualTo(allParametersMerged));
                Assert.That(allParametersMerged, Is.EqualTo(new [] { 1, 2, 3, 4, 5, 6 }));
            }

            [Test]
            public void first_second_comparer()
            {
                var first = new[] { 1, 4, 5 };
                var second = new int[] { 2, 3, 6 };
                var comparer = Comparer.Create<int>((x, y) => y.CompareTo(x));

                var exampleMerged = MoreEnumerable.OrderedMerge(
                    first,
                    second,
                    comparer);

                int Id(int id) => id;
                int First(int f, int _) => f;

                var allParametersMerged = MoreEnumerable.OrderedMerge(
                    first: first,
                    second: second,
                    comparer: comparer,
                    firstKeySelector: Id,
                    secondKeySelector: Id,
                    firstSelector: Id,
                    secondSelector: Id,
                    bothSelector: First);

                Assert.That(exampleMerged, Is.EqualTo(allParametersMerged));
                Assert.That(allParametersMerged, Is.EqualTo(new[] { 2, 3, 6, 1, 4, 5 }));
            }

            [Test]
            public void first_second_keySelector()
            {
                var first = new[] { 16, 4, 1 }; // 16, 64, 256
                var second = new int[] { 32, 8, 2 }; // 8, 32, 128
                int KeySelector(int key) => 256 / key;

                var exampleMerged = MoreEnumerable.OrderedMerge(
                    first,
                    second,
                    KeySelector);

                int Id(int id) => id;
                int First(int f, int _) => f;

                var allParametersMerged = MoreEnumerable.OrderedMerge(
                    first: first,
                    second: second,
                    firstKeySelector: KeySelector,
                    secondKeySelector: KeySelector,
                    firstSelector: Id,
                    secondSelector: Id,
                    bothSelector: First,
                    comparer: null);

                Assert.That(exampleMerged, Is.EqualTo(allParametersMerged));
                Assert.That(allParametersMerged, Is.EqualTo(new[] { 32, 16, 8, 4, 2, 1 }));
            }

            [Test]
            public void first_second_keySelector_firstSelector_secondSelector_bothSelector()
            {
                var first = new[] { 1, 4, 5 };
                var second = new int[] { 2, 3, 6 };
                int KeySelector(int id) => id;
                int FirstSelector(int id) => id;
                int SecondSelector(int id) => id;
                int BothSelector(int f, int _) => f;

                var exampleMerged = MoreEnumerable.OrderedMerge(first: first,
                    second: second,
                    keySelector: KeySelector,
                    firstSelector: FirstSelector,
                    secondSelector: SecondSelector,
                    bothSelector: BothSelector);

                var allParametersMerged = MoreEnumerable.OrderedMerge(
                    first: first,
                    second: second,
                    firstKeySelector: KeySelector,
                    secondKeySelector: KeySelector,
                    firstSelector: FirstSelector,
                    secondSelector: SecondSelector,
                    bothSelector: BothSelector,
                    comparer: null);

                Assert.That(exampleMerged, Is.EqualTo(allParametersMerged));
                Assert.That(allParametersMerged, Is.EqualTo(new[] { 1, 2, 3, 4, 5, 6 }));
            }

            [Test]
            public void first_second_keySelector_firstSelector_secondSelector_bothSelector_comparer()
            {
                var first = new[] { 1, 4, 5 };
                var second = new int[] { 2, 3, 6 };
                int KeySelector(int id) => id;
                int FirstSelector(int id) => id;
                int SecondSelector(int id) => id;
                int BothSelector(int f, int _) => f;
                var comparer = Comparer.Create<int>((x, y) => y.CompareTo(x));

                var exampleMerged = MoreEnumerable.OrderedMerge(
                    first: first,
                    second: second,
                    keySelector: KeySelector,
                    firstSelector: FirstSelector,
                    secondSelector: SecondSelector,
                    bothSelector: BothSelector,
                    comparer: comparer);

                var allParametersMerged = MoreEnumerable.OrderedMerge(
                    first: first,
                    second: second,
                    firstKeySelector: KeySelector,
                    secondKeySelector: KeySelector,
                    firstSelector: FirstSelector,
                    secondSelector: SecondSelector,
                    bothSelector: BothSelector,
                    comparer: comparer);

                Assert.That(exampleMerged, Is.EqualTo(allParametersMerged));
                Assert.That(allParametersMerged, Is.EqualTo(new[] { 2, 3, 6, 1, 4, 5 }));
            }

            [Test]
            public void first_second_firstKeySelector_secondKeySelector_firstSelector_secondSelector_bothSelector()
            {
                var first = new[] { 1, 4, 5 };
                var second = new int[] { 2, 3, 6 };
                int FirstKeySelector(int id) => id;
                int SecondKeySelector(int id) => id;
                int FirstSelector(int id) => id;
                int SecondSelector(int id) => id;
                int BothSelector(int f, int _) => f;

                var exampleMerged = MoreEnumerable.OrderedMerge(
                    first: first,
                    second: second,
                    firstKeySelector: FirstKeySelector,
                    secondKeySelector: SecondKeySelector,
                    firstSelector: FirstSelector,
                    secondSelector: SecondSelector,
                    bothSelector: BothSelector);

                var allParametersMerged = MoreEnumerable.OrderedMerge(
                    first: first,
                    second: second,
                    firstKeySelector: FirstKeySelector,
                    secondKeySelector: SecondKeySelector,
                    firstSelector: FirstSelector,
                    secondSelector: SecondSelector,
                    bothSelector: BothSelector,
                    comparer: null);

                Assert.That(exampleMerged, Is.EqualTo(allParametersMerged));
                Assert.That(allParametersMerged, Is.EqualTo(new[] { 1, 2, 3, 4, 5, 6 }));
            }



        }

    }
}
