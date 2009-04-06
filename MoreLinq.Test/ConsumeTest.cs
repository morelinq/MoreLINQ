using System;
using NUnit.Framework;

using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class ConsumeTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConsumeWithNullSource()
        {
            Enumerable.Consume<int>(null);
        }

        [Test]
        public void ConsumeReallyConsumes()
        {
            int counter = 0;
            var sequence = LinqEnumerable.Range(0, 10).Pipe(x => counter++);
            sequence.Consume();
            Assert.AreEqual(10, counter);
        }
    }
}
