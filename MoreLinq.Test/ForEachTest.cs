using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class ForEachTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ForEachNullSequence()
        {
            MoreEnumerable.ForEach<int>(null, x => { throw new InvalidOperationException(); });
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ForEachNullAction()
        {
            MoreEnumerable.ForEach(new[] { 1, 2, 3 }, null);
        }

        [Test]
        public void ForEachWithSequence()
        {
            List<int> results = new List<int>();
            MoreEnumerable.ForEach(new[] { 1, 2, 3 }, results.Add);
            results.AssertSequenceEqual(1, 2, 3);
        }
    }
}
