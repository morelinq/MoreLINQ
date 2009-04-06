namespace MoreLinq.Test
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using System.Collections.ObjectModel;

    [TestFixture]
    public class PreScanTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PreScanNullSequence()
        {
            Enumerable.PreScan(null, SampleData.Plus, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PreScanNullOperation()
        {
            SampleData.Values.PreScan(null, 0);
        }

        [Test]
        public void PreScanSum()
        {
            var result = SampleData.Values.PreScan(SampleData.Plus, 0);
            var gold = new[] { 0, 1, 3, 6, 10, 15, 21, 28, 36, 45 };
            result.AssertSequenceEqual(gold);
        }

        [Test]
        public void PreScanMul()
        {
            var seq = new[] { 1, 2, 3 };
            var gold = new[] { 1, 1, 2 };
            var result = seq.PreScan(SampleData.Mul, 1);
            result.AssertSequenceEqual(gold);
        }
    }
}
