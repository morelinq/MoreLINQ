using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    class OrderedMergeTest
    {
        [Test]
        public void ShouldBeLazy()
        {
            var first = new BreakingSequence<object>();
            var second = new BreakingSequence<object>();
            Assert.DoesNotThrow(() => MoreEnumerable.OrderedMerge(first, second, id => id, id => id, id => id, id => id, (f, _) => f, null));
        }

        [Test]
        public void ShouldDisposeEnumerators()
        {
            using (var first = TestingSequence.Of(new int[] { }))
            using (var second = TestingSequence.Of(new int[] { }))
                MoreEnumerable.OrderedMerge(first, second, id => id, id => id, id => id, id => id, (f, _) => f, null).ToArray();
        }

        [Test]
        public void IfThereAreNoMoreElementsToReturnFromTheFirstCollectionThenReturnTheRemainingSecondCollection()
        {
            var first = new int[] { };
            var second = new[] { 1, 2, 3 };

            int Id(int id) => id;
            int First(int f, int _) => f;

            var merged = MoreEnumerable.OrderedMerge(
                first: first,
                second: second,
                firstKeySelector: Id,
                secondKeySelector: Id,
                firstSelector: Id,
                secondSelector: Id,
                bothSelector: First,
                comparer: null);

            Assert.That(merged, Is.EqualTo(second));
        }

        [Test]
        public void IfThereAreNoMoreElementsToReturnFromTheSecondCollectionThenReturnTheRemainingFirstCollection()
        {
            var first = new[] { 1, 2, 3 };
            var second = new int[] { };

            int Id(int id) => id;
            int First(int f, int _) => f;

            var merged = MoreEnumerable.OrderedMerge(
                first: first,
                second: second,
                firstKeySelector: Id,
                secondKeySelector: Id,
                firstSelector: Id,
                secondSelector: Id,
                bothSelector: First,
                comparer: null);

            Assert.That(merged, Is.EqualTo(first));
        }

        [Test]
        public void TwoSequencesWithNoCollisionsShouldMergeUsingTheDefaultComparer()
        {
            var first = new[] { 1, 4, 5 };
            var second = new [] { 2, 3, 6 };

            int Id(int id) => id;
            int First(int f, int _) => f;

            var merged = MoreEnumerable.OrderedMerge(
                first: first,
                second: second,
                firstKeySelector: Id,
                secondKeySelector: Id,
                firstSelector: Id,
                secondSelector: Id,
                bothSelector: First,
                comparer: null);

            Assert.That(merged, Is.EqualTo(new[] { 1, 2, 3, 4, 5, 6 }));
        }

        [Test]
        public void WhenThereIsACollisionShouldUseBothSelectorToChooseFromFirstOfSecondCollection()
        {
            var firstElement = new Version(3, 0);
            var secondElement = new Version(3, 0);

            var first = new[] { firstElement };
            var second = new[] { secondElement };
            Version First(Version f, Version _) => f;
            Version Second(Version _, Version s) => s;

            Version Id(Version id) => id;

            var firstMerge = MoreEnumerable.OrderedMerge(
                first: first,
                second: second,
                firstKeySelector: Id,
                secondKeySelector: Id,
                firstSelector: Id,
                secondSelector: Id,
                bothSelector: First,
                comparer: null);
            Assert.That(firstMerge.First(), Is.SameAs(firstElement), "Should have returned First");

            var secondMerge = MoreEnumerable.OrderedMerge(
                first: first,
                second: second,
                firstKeySelector: Id,
                secondKeySelector: Id,
                firstSelector: Id,
                secondSelector: Id,
                bothSelector: Second,
                comparer: null);
            Assert.That(secondMerge.First(), Is.SameAs(secondElement), "Should have returned Second");
        }

        [Test]
        public void ShouldBeAbleToSelectKeyOfFirstAndSecondCollection()
        {
            var firstElement = new Version(2, 0);
            var secondElement = new Version(3, 0);

            var first = new[] { firstElement };
            var second = new[] { secondElement };
            int FirstKeySelector(Version version) => 2 * version.Major;
            int SecondKeySelector(Version version) => version.Major;

            Version Id(Version id) => id;
            Version First(Version f, Version _) => f;

            var merged = MoreEnumerable.OrderedMerge(
                first: first,
                second: second,
                firstKeySelector: FirstKeySelector,
                secondKeySelector: SecondKeySelector,
                firstSelector: Id,
                secondSelector: Id,
                bothSelector: First,
                comparer: null);

            Assert.That(merged, Is.EqualTo(new[] { secondElement, firstElement }));
        }

        [Test]
        public void FirstAndSecondCanBeOfDifferentInputTypesWithASharedOutputType()
        {
            int[] first = {1, 4, 5};
            string[] second = {"2", "3", "6"};

            int FirstKeySelector(int value) => value;
            int SecondKeySelector(string value) => int.Parse(value);
            int FirstSelector(int value) => value;
            int SecondSelector(string value) => int.Parse(value);

            int First(int f, string _) => f;

            var merged = MoreEnumerable.OrderedMerge(
                first: first,
                second: second,
                firstKeySelector: FirstKeySelector,
                secondKeySelector: SecondKeySelector,
                firstSelector: FirstSelector,
                secondSelector: SecondSelector,
                bothSelector: First,
                comparer: null);

            Assert.That(merged, Is.EqualTo(new[] { 1, 2, 3, 4, 5, 6 }));
        }

        [Test]
        public void ShouldBeAbleToUseNonDefaultComparer()
        {
            var first = new[] { 1, 4, 5 };
            var second = new[] { 2, 3, 6 };

            var comparer = Comparer.Create<int>((x, y) => y.CompareTo(x));

            int Id(int id) => id;
            int First(int f, int _) => f;

            var merged = MoreEnumerable.OrderedMerge(
                first: first,
                second: second,
                firstKeySelector: Id,
                secondKeySelector: Id,
                firstSelector: Id,
                secondSelector: Id,
                bothSelector: First,
                comparer: comparer);

            Assert.That(merged, Is.EqualTo(new[] { 2, 3, 6, 1, 4, 5 }));
        }

        [TestFixture]
        public class AllParametersEquivalent
        {

            [Test]
            public void first_second()
            {
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
