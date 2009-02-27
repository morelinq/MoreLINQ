namespace MoreLinq.Test.Pull
{
    using System;
    using MoreLinq.Pull;
    using NUnit.Framework;

    using LinqEnumerable = System.Linq.Enumerable;

    partial class EnumerableTest
    {
        [Test, Category("Miscellaneous")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConsumeWithNullSource()
        {
            Enumerable.Consume<int>(null);
        }

        [Test, Category("Miscellaneous")]
        public void ConsumeReallyConsumes()
        {
            int counter = 0;
            var sequence = LinqEnumerable.Range(0, 10).Pipe(x => counter++);
            sequence.Consume();
            Assert.AreEqual(10, counter);
        }
    }
}
