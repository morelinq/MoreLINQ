using System.Linq;
using NUnit.Framework;
using System;
using MoreLinq.Pull;

namespace MoreLinq.Test.Pull
{
    [TestFixture]
    public class MiscellaneousTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConsumeWithNullSource()
        {
            Miscellaneous.Consume<int>(null);
        }

        [Test]
        public void ConsumeReallyConsumes()
        {
            int counter = 0;
            var sequence = Enumerable.Range(0, 10).Pipe(x => counter++);
            sequence.Consume();
            Assert.AreEqual(10, counter);
        }
    }
}
