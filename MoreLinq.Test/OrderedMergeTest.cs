using System.Collections.Generic;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    class OrderedMergeTest {
        public static IEnumerable<TResult> TDDOrderedMerge<TResult>(IEnumerable<TResult> first, IEnumerable<TResult> second) {
            var comparer = Comparer<TResult>.Default;
            return _();
            IEnumerable<TResult> _() {
                using (var e1 = first.GetEnumerator())
                using (var e2 = second.GetEnumerator())
                {
                    var gotFirst = e1.MoveNext();
                    var gotSecond = e2.MoveNext();

                    while (gotFirst || gotSecond)
                    {
                        if (gotFirst && gotSecond) {
                            var element1 = e1.Current;
                            var element2 = e2.Current;
                            var comparison = comparer.Compare(element1, element2);

                            if (comparison < 0)
                            {
                                yield return element1;
                                gotFirst = e1.MoveNext();
                            }
                            else if (comparison > 0) {
                                yield return element2;
                                gotSecond = e2.MoveNext();
                            }
                        } else if (gotSecond)
                        {
                            yield return e2.Current;
                            gotSecond = e2.MoveNext();
                        } else if (gotFirst)
                        {
                            yield return e1.Current;
                            gotFirst = e1.MoveNext();
                        }
                    }
                }
			}
        }

        [Test]
        public void ShouldBeLazy() {
            var first = new BreakingSequence<object>();
            var second = new BreakingSequence<object>();
            Assert.DoesNotThrow(() => TDDOrderedMerge(first, second));
        }

        [Test]
        public void ShouldDisposeEnumerators() {
            var firstDisposed = false;
            var first = new int[] { }.AsVerifiable();
            first.WhenDisposed(_ => firstDisposed = true);

            var secondDisposed = false;
            var second = new int[] { }.AsVerifiable();
            second.WhenDisposed(_ => secondDisposed = true);

            TDDOrderedMerge(first, second).ToArray();

            Assert.IsTrue(firstDisposed, "First was not disposed");
            Assert.IsTrue(secondDisposed, "Second was not disposed");
        }

        [Test]
        public void IfThereAreNoMoreElementsToReturnFromTheFirstCollectionThenReturnTheRemainingSecondCollection()
        {
            Assert.That(TDDOrderedMerge(new int[] { }, new[] { 1, 2, 3 }), Is.EquivalentTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void IfThereAreNoMoreElementsToReturnFromTheSecondCollectionThenReturnTheRemainingFirstCollection()
        {
            Assert.That(TDDOrderedMerge(new [] { 1, 2, 3 }, new int[] { }), Is.EquivalentTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void TwoSequencesWithNoCollistionsShouldMergeUsingTheDefaultComparer() {
            Assert.That(TDDOrderedMerge(new[] { 1, 3, 5 }, new [] { 2, 4, 6 }), Is.EquivalentTo(new[] { 1, 2, 3, 4, 5, 6 }));
        }
    }
}
