using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class CreateTest
    {
        [Test]
        public void CreateIsLazy()
        {
            Assert.That(MoreEnumerable.Create(BreakingFunc.Of<IEnumerator<object>>()), Is.Not.Null);
        }

        [Test]
        public void CreateYieldsEnumeratorElements()
        {
            var xs = Enumerable.Range(1, 10);
            var result = MoreEnumerable.Create(() => xs.GetEnumerator());
            Assert.That(result, Is.EquivalentTo(xs));
        }

        [Test]
        public void CreateDisposesEnumerator()
        {
            var item = new object();
            using (var items = Enumerable.Repeat(item, 1).AsTestingSequence())
            {
                var r = MoreEnumerable.Create(() => items.GetEnumerator()).Read();
                Assert.That(r.Read(), Is.EqualTo(item));
                r.ReadEnd();
            }
        }
    }
}
