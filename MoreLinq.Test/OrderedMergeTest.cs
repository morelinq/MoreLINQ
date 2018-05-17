namespace MoreLinq.Test
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    class OrderedMergeTest
    {
        [Test]
        public void ShouldBeLazy()
        {
            var first = new BreakingSequence<object>();
            var second = new BreakingSequence<object>();
            Assert.DoesNotThrow(() => first.OrderedMerge(second, id => id, id => id, id => id, id => id, (f, _) => f, null));
        }

        [Test]
        public void ShouldDisposeEnumerators()
        {
            using (var first = TestingSequence.Of<int>())
            using (var second = TestingSequence.Of<int>())
                first.OrderedMerge(second, id => id, id => id, id => id, id => id, (f, _) => f, null).ToArray();
        }

        [Test]
        public void WhenEndOfFirstIsReachedThenReturnTheRemainingFromSecond()
        {
            var first = new[] { 1 };
            var second = new[] { 2, 3 };

            var merged = first.OrderedMerge(second,
                firstKeySelector: value => value,
                secondKeySelector: value => value,
                firstSelector: value => value,
                secondSelector: value => value,
                bothSelector: (f, _) => f,
                comparer: null);

            Assert.That(merged, Is.EqualTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void WhenEndOfSecondIsReachedThenReturnTheRemainingFromFirst()
        {
            var first = new [] { 2, 3 };
            var second = new[] { 1 };

            var merged = first.OrderedMerge(second,
                firstKeySelector: value => value,
                secondKeySelector: value => value,
                firstSelector: value => value,
                secondSelector: value => value,
                bothSelector: (f, _) => f,
                comparer: null);

            Assert.That(merged, Is.EqualTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void ShouldMapResultUsingFirstAndSecondSelectors()
        {
            var first = new[] { 1 };
            var second = new [] { 2 };

            var merged = first.OrderedMerge(second,
                firstKeySelector: value => value,
                secondKeySelector: value => value,
                firstSelector: value => value - 1,
                secondSelector: value => value + 1,
                bothSelector: (f, _) => f,
                comparer: null);

            Assert.That(merged, Is.EqualTo(new[] { 0, 3 }));
        }

        [Test]
        public void ShouldMapResultUsingBothSelectorsWhenThereIsACollision()
        {
            var firstElement = "mergeConflict";
            var secondElement = "mergeConflict";

            var first = new[] { firstElement };
            var second = new[] { secondElement };

            var firstMerge = first.OrderedMerge(second,
                firstKeySelector: value => value,
                secondKeySelector: value => value,
                firstSelector: value => value,
                secondSelector: value => value,
                bothSelector: (f, _) => f,
                comparer: null);
            Assert.That(firstMerge.First(), Is.SameAs(firstElement), "Should have returned First");

            var secondMerge = first.OrderedMerge(second,
                firstKeySelector: value => value,
                secondKeySelector: value => value,
                firstSelector: value => value,
                secondSelector: value => value,
                bothSelector: (_, s) => s,
                comparer: null);
            Assert.That(secondMerge.First(), Is.SameAs(secondElement), "Should have returned Second");
        }

        [Test]
        public void ShouldMapKeyUsingFirstAndSecondKeySelectors()
        {
            var first = new[] { 2 };
            var second = new[] { 3 };

            var merged = first.OrderedMerge(second,
                firstKeySelector: value => 2 * value,
                secondKeySelector: value => value,
                firstSelector: value => value,
                secondSelector: value => value,
                bothSelector: (f, _) => f,
                comparer: null);

            Assert.That(merged, Is.EqualTo(new[] { 3, 2 }));
        }

        [Test]
        public void ShouldBeAbleToUseNonDefaultComparer()
        {
            var first = new[] { 1 };
            var second = new[] { 2, 3 };

            var merged = first.OrderedMerge(second,
                firstKeySelector: value => value,
                secondKeySelector: value => value,
                firstSelector: value => value,
                secondSelector: value => value,
                bothSelector: (f, _) => f,
                comparer: Comparer.Create<int>((x, y) => y.CompareTo(x)));

            Assert.That(merged, Is.EqualTo(new[] { 2, 3, 1 }));
        }

        [TestFixture]
        public class OverloadCorrectness
        {
            [Test]
            public void first_second()
            {
                var first = new[] { 1, 4 };
                var second = new[] { 2, 3 };

                var overload = first.OrderedMerge(second);

                var fullOrderedMerge = first.OrderedMerge(second,
                    firstKeySelector: value => value,
                    secondKeySelector: value => value,
                    firstSelector: value => value,
                    secondSelector: value => value,
                    bothSelector: (f, _) => f,
                    comparer: null);

                Assert.That(overload, Is.EqualTo(fullOrderedMerge));
                Assert.That(fullOrderedMerge, Is.EqualTo(new [] { 1, 2, 3, 4 }));
            }

            [Test]
            public void first_second_comparer()
            {
                var first = new[] { 1, 4 };
                var second = new[] { 2, 3 };
                var comparer = Comparer.Create<int>((x, y) => y.CompareTo(x));

                var overload = first.OrderedMerge(second,
                    comparer);

                var fullOrderedMerge = first.OrderedMerge(second,
                    firstKeySelector: value => value,
                    secondKeySelector: value => value,
                    firstSelector: value => value,
                    secondSelector: value => value,
                    bothSelector: (f, _) => f,
                    comparer: comparer);

                Assert.That(overload, Is.EqualTo(fullOrderedMerge));
                Assert.That(fullOrderedMerge, Is.EqualTo(new[] { 2, 3, 1, 4 }));
            }

            [Test]
            public void first_second_keySelector()
            {
                var first = new[] { 4, 1 }; // 2, 8,
                var second = new[] { 8, 2 }; // 1, 4
                int KeySelector(int key) => 8 / key;

                var overload = first.OrderedMerge(second,
                    KeySelector);

                var fullOrderedMerge = first.OrderedMerge(second,
                    firstKeySelector: KeySelector,
                    secondKeySelector: KeySelector,
                    firstSelector: value => value,
                    secondSelector: value => value,
                    bothSelector: (f, _) => f,
                    comparer: null);

                Assert.That(overload, Is.EqualTo(fullOrderedMerge));
                Assert.That(fullOrderedMerge, Is.EqualTo(new[] { 8, 4, 2, 1 }));
            }

            [Test]
            public void first_second_keySelector_firstSelector_secondSelector_bothSelector()
            {
                var first = new[] { 1, 4 };
                var second = new[] { 2, 3 };
                int KeySelector(int id) => id;
                int FirstSelector(int value) => value;
                int SecondSelector(int value) => value;
                int BothSelector(int f, int _) => f;

                var overload = first.OrderedMerge(second,
                    keySelector: KeySelector,
                    firstSelector: FirstSelector,
                    secondSelector: SecondSelector,
                    bothSelector: BothSelector);

                var fullOrderedMerge = first.OrderedMerge(second,
                    firstKeySelector: KeySelector,
                    secondKeySelector: KeySelector,
                    firstSelector: FirstSelector,
                    secondSelector: SecondSelector,
                    bothSelector: BothSelector,
                    comparer: null);

                Assert.That(overload, Is.EqualTo(fullOrderedMerge));
                Assert.That(fullOrderedMerge, Is.EqualTo(new[] { 1, 2, 3, 4 }));
            }

            [Test]
            public void first_second_keySelector_firstSelector_secondSelector_bothSelector_comparer()
            {
                var first = new[] { 1, 4 };
                var second = new[] { 2, 3 };
                int KeySelector(int value) => value;
                int FirstSelector(int value) => value;
                int SecondSelector(int value) => value;
                int BothSelector(int f, int _) => f;
                var comparer = Comparer.Create<int>((x, y) => y.CompareTo(x));

                var overload = first.OrderedMerge(second,
                    keySelector: KeySelector,
                    firstSelector: FirstSelector,
                    secondSelector: SecondSelector,
                    bothSelector: BothSelector,
                    comparer: comparer);

                var fullOrderedMerge = first.OrderedMerge(second,
                    firstKeySelector: KeySelector,
                    secondKeySelector: KeySelector,
                    firstSelector: FirstSelector,
                    secondSelector: SecondSelector,
                    bothSelector: BothSelector,
                    comparer: comparer);

                Assert.That(overload, Is.EqualTo(fullOrderedMerge));
                Assert.That(fullOrderedMerge, Is.EqualTo(new[] { 2, 3, 1, 4 }));
            }

            [Test]
            public void first_second_firstKeySelector_secondKeySelector_firstSelector_secondSelector_bothSelector()
            {
                var first = new[] { 1, 4 };
                var second = new[] { 2, 3 };
                int FirstKeySelector(int value) => value;
                int SecondKeySelector(int value) => value;
                int FirstSelector(int value) => value;
                int SecondSelector(int value) => value;
                int BothSelector(int f, int _) => f;

                var overload = first.OrderedMerge(second,
                    firstKeySelector: FirstKeySelector,
                    secondKeySelector: SecondKeySelector,
                    firstSelector: FirstSelector,
                    secondSelector: SecondSelector,
                    bothSelector: BothSelector);

                var fullOrderedMerge = first.OrderedMerge(second,
                    firstKeySelector: FirstKeySelector,
                    secondKeySelector: SecondKeySelector,
                    firstSelector: FirstSelector,
                    secondSelector: SecondSelector,
                    bothSelector: BothSelector,
                    comparer: null);

                Assert.That(overload, Is.EqualTo(fullOrderedMerge));
                Assert.That(fullOrderedMerge, Is.EqualTo(new[] { 1, 2, 3, 4 }));
            }
        }
    }
}
