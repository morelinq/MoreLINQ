using System.Collections.Generic;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    class OrderedMergeTest {
        public static IEnumerable<TResult> TDDOrderedMerge<TResult>(IEnumerable<TResult> first, IEnumerable<TResult> second) {
            return _();

            IEnumerable<TResult> _() {
                var e1 = first.GetEnumerator();
				var e2 = second.GetEnumerator();                
				yield break;
			}
        }

        [Test]
        public void ShouldBeLazy() {
            var first = new BreakingSequence<object>();
            var second = new BreakingSequence<object>();
            Assert.DoesNotThrow(() => TDDOrderedMerge(first, second));
        }
    }
}
