using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    class OrderedMergeTest {
        public static IEnumerable<TResult> TDDOrderedMerge<TResult, TKey>(
            IEnumerable<TResult> first,
            IEnumerable<TResult> second,
            Func<TResult, TKey> firstKeySelector,
            Func<TResult, TKey> secondKeySelector,
            Func<TResult, TResult, TResult> bothSelector)
        {
            var comparer = Comparer<TKey>.Default;
            return _(); IEnumerable<TResult> _() {
                using (var e1 = first.GetEnumerator())
                using (var e2 = second.GetEnumerator())
                {
                    var gotFirst = e1.MoveNext();
                    var gotSecond = e2.MoveNext();

                    while (gotFirst || gotSecond)
                    {
                        if (gotFirst && gotSecond) {
                            var element1 = e1.Current;
                            var key1 = firstKeySelector(element1);
                            var element2 = e2.Current;
                            var key2 = secondKeySelector(element2);
                            var comparison = comparer.Compare(key1, key2);

                            if (comparison < 0)
                            {
                                yield return element1;
                                gotFirst = e1.MoveNext();
                            }
                            else if (comparison > 0) {
                                yield return element2;
                                gotSecond = e2.MoveNext();
                            } else {
                                yield return bothSelector(element1, element2);
                                gotFirst = e1.MoveNext();
                                gotSecond = e2.MoveNext();
                            }
                        } else if (gotSecond)
                        {
                            yield return e2.Current;
                            gotSecond = e2.MoveNext();
                        } else {
                            yield return e1.Current;
                            gotFirst = e1.MoveNext();
                        }
                    }
                }
			}
        }

        static Func<TResult, TResult, TResult> ChooseFirst<TResult>()
            => (first, _) => first;
        static Func<TResult, TResult, TResult> ChooseSecond<TResult>()
            => (_, second) => second;

        [Test]
        public void ShouldBeLazy() {
            var first = new BreakingSequence<object>();
            var second = new BreakingSequence<object>();
            Assert.DoesNotThrow(() => TDDOrderedMerge(first, second, id => id, id => id, ChooseFirst<object>()));
        }

        [Test]
        public void ShouldDisposeEnumerators() {
            var firstDisposed = false;
            var first = new int[] { }.AsVerifiable();
            first.WhenDisposed(_ => firstDisposed = true);

            var secondDisposed = false;
            var second = new int[] { }.AsVerifiable();
            second.WhenDisposed(_ => secondDisposed = true);

            TDDOrderedMerge(first, second, id => id, id => id, ChooseFirst<int>()).ToArray();

            Assert.IsTrue(firstDisposed, "First was not disposed");
            Assert.IsTrue(secondDisposed, "Second was not disposed");
        }

        [Test]
        public void IfThereAreNoMoreElementsToReturnFromTheFirstCollectionThenReturnTheRemainingSecondCollection() {
            var first = new int[] { };
            var second = new[] { 1, 2, 3 };

            var merged = TDDOrderedMerge(first, second, id => id, id => id, ChooseFirst<int>());

            Assert.That(merged, Is.EquivalentTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void IfThereAreNoMoreElementsToReturnFromTheSecondCollectionThenReturnTheRemainingFirstCollection() {
            var first = new [] { 1, 2, 3 };
            var second = new int[] { };

            var merged = TDDOrderedMerge(first, second, id => id, id => id, ChooseFirst<int>());

            Assert.That(merged, Is.EquivalentTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void TwoSequencesWithNoCollistionsShouldMergeUsingTheDefaultComparer() {
            var first = new[] { 1, 4, 5 };
            var second = new [] { 2, 3, 6 };

            var merged = TDDOrderedMerge(first, second, id => id, id => id, ChooseFirst<int>());

            Assert.That(merged, Is.EquivalentTo(new[] { 1, 2, 3, 4, 5, 6 }));
        }

        [Test]
        public void WhenThereIsACollisionShouldUseBothSelectorToChooseFromFirstOfSecondCollection() {
            var firstElement = new Version(3, 0);
            var secondElement = new Version(3, 0);

            var first = new[] { firstElement };
            var second = new[] { secondElement };

            Assert.That(TDDOrderedMerge(first, second, id => id, id => id, ChooseFirst<Version>()).First(), Is.SameAs(firstElement), "Should have returned First");
            Assert.That(TDDOrderedMerge(first, second, id => id, id => id, ChooseSecond<Version>()).First(), Is.SameAs(secondElement), "Should have returned Second");
        }

        [Test]
        public void ShouldBeAbleToSelectKeyOfFirstAndSecondCollection() {
            var firstElement = new Version(2, 4);
            var secondElement = new Version(3, 0);

            var first = new[] { firstElement };
            var second = new[] { secondElement };
            
            var merged = TDDOrderedMerge(
                first,
                second,
                version =>
                    version.Major * 2,
                version =>
                    version.Major,
                ChooseFirst<Version>());

            Assert.That(merged, Is.EqualTo(new[] { secondElement, firstElement }));
        }
    }
}
