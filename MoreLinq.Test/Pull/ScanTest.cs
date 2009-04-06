namespace MoreLinq.Test.Pull
{
    using System;
    using System.Linq;
    using MoreLinq.Pull;
    using NUnit.Framework;

    partial class EnumerableTest
    {
        [Test, Category("Transformation")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ScanEmpty()
        {
            (new int[] { }).Scan(Plus).Count();
        }

        [Test, Category("Transformation")]
        public void ScanSum()
        {
            var result = values.Scan(Plus);
            var gold = values.PreScan(Plus, 0).Zip(values, Plus);
            result.AssertSequenceEqual(gold);
        }
    }
}
