namespace MoreLinq.Test.Pull
{
    using System;
    using System.Collections.Generic;
    using MoreLinq.Pull;
    using NUnit.Framework;

    partial class EnumerableTest
    {
        private readonly IEnumerable<int> values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private readonly Func<int, int, int> Plus = (a, b) => a + b;
        private readonly Func<int, int, int> Mul = (a, b) => a * b;

        [Test, Category("Transformation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PreScanNullSequence()
        {
            Enumerable.PreScan(null, Plus, 0);
        }

        [Test, Category("Transformation")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PreScanNullOperation()
        {
            values.PreScan(null, 0);
        }

        [Test, Category("Transformation")]
        public void PreScanSum()
        {
            var result = values.PreScan(Plus, 0);
            var gold = new[] { 0, 1, 3, 6, 10, 15, 21, 28, 36, 45 };
            result.AssertSequenceEqual(gold);
        }

        [Test, Category("Transformation")]
        public void PreScanMul()
        {
            var seq = new[] { 1, 2, 3 };
            var gold = new[] { 1, 1, 2 };
            var result = seq.PreScan(Mul, 1);
            result.AssertSequenceEqual(gold);
        }
    }
}
