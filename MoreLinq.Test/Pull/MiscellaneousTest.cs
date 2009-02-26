namespace MoreLinq.Test.Pull
{
    using System;
    using MoreLinq.Pull;
    using NUnit.Framework;

    using LinqEnumerable = System.Linq.Enumerable;

    [TestFixture]
    public class MiscellaneousTest
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
