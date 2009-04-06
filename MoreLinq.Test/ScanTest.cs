using System;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class ScanTest
    {
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ScanEmpty()
        {
            (new int[] { }).Scan(SampleData.Plus).Count();
        }

        [Test]
        public void ScanSum()
        {
            var result = SampleData.Values.Scan(SampleData.Plus);
            var gold = SampleData.Values.PreScan(SampleData.Plus, 0).Zip(SampleData.Values, SampleData.Plus);
            result.AssertSequenceEqual(gold);
        }
    }
}
