using System.Collections.Generic;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    class OrderedMergeTest {
        public static IEnumerable<TResult> TDDOrderedMerge<TResult>(IEnumerable<TResult> first, IEnumerable<TResult> second) {
            return _();

            IEnumerable<TResult> _() {
                using (var e1 = first.GetEnumerator())
                using (var e2 = second.GetEnumerator())
                {
                    yield break;
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
    }
}
