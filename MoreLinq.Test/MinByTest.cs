using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class MinByTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MinByNullSequence()
        {
            ((IEnumerable<string>)null).MinBy(x => x.Length);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MinByNullSelector()
        {
            SampleData.Strings.MinBy<string, int>(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MinByNullComparer()
        {
            SampleData.Strings.MinBy(x => x.Length, null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MinByEmptySequence()
        {
            new string[0].MinBy(x => x.Length);
        }

        [Test]
        public void MinByWithNaturalComparer()
        {
            Assert.AreEqual("aa", SampleData.Strings.MinBy(x => x[1]));
        }

        [Test]
        public void MinByWithComparer()
        {
            Assert.AreEqual("az", SampleData.Strings.MinBy(x => x[1], SampleData.ReverseCharComparer));
        }

        [Test]
        public void MinByReturnsFirstOfEquals()
        {
            Assert.AreEqual("ax", SampleData.Strings.MinBy(x => x.Length));
        }
    }
}
