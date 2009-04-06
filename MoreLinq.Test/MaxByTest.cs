using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class MaxByTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MaxByNullSequence()
        {
            ((IEnumerable<string>)null).MaxBy(x => x.Length);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MaxByNullSelector()
        {
            SampleData.Strings.MaxBy<string, int>(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MaxByNullComparer()
        {
            SampleData.Strings.MaxBy(x => x.Length, null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MaxByEmptySequence()
        {
            new string[0].MaxBy(x => x.Length);
        }

        [Test]
        public void MaxByWithNaturalComparer()
        {
            Assert.AreEqual("az", SampleData.Strings.MaxBy(x => x[1]));
        }

        [Test]
        public void MaxByWithComparer()
        {
            Assert.AreEqual("aa", SampleData.Strings.MaxBy(x => x[1], SampleData.ReverseCharComparer));
        }

        [Test]
        public void MaxByReturnsFirstOfEquals()
        {
            Assert.AreEqual("hello", SampleData.Strings.MaxBy(x => x.Length));
        }
    }
}
