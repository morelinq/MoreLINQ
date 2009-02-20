using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq.Pull;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MoreLinq.Test.Pull
{
    [TestFixture]
    public class TransformationTest
    {
        private readonly IEnumerable<int> Values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private readonly Func<int, int, int> Plus = (a, b) => a + b;
        private readonly Func<int, int, int> Mul = (a, b) => a * b;

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullSequence()
        {
            Transformation.PreScan<int>(null, Plus, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullOperation()
        {
            Values.PreScan(null, 0);
        }

        [Test]
        public void PreSum()
        {
            var result = Values.PreScan(Plus, 0);
            var gold = new[] { 0, 1, 3, 6, 10, 15, 21, 28, 36, 45 };
            result.AssertSequenceEqual(gold);
        }

        [Test]
        public void PreMul()
        {
            var seq = new[] { 1, 2, 3 };
            var gold = new[] { 1, 1, 2 };
            var result = seq.PreScan(Mul, 1);
            result.AssertSequenceEqual(gold);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EmptyScan()
        {
            (new int[] { }).Scan(Plus).Count();
        }

        [Test]
        public void Sum()
        {
            var result = Values.Scan(Plus);
            var gold = Values.PreScan(Plus, 0).Zip(Values, Plus);
            result.AssertSequenceEqual(gold);
        }
    }
}
