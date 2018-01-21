using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    class OrderedMergeTest {
        public static IEnumerable<TResult> TDDOrderedMerge<TResult>(IEnumerable<TResult> first) {
            return _();

            IEnumerable<TResult> _() {
                var e1 = first.GetEnumerator();
                yield break;
            }
        }

        [Test]
        public void ShouldBeLazy() {
            var first = new BreakingSequence<object>();
            Assert.DoesNotThrow(() => TDDOrderedMerge(first));
        }
    }
}
