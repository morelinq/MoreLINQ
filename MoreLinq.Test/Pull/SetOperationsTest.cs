using System;
using MoreLinq.Pull;
using NUnit.Framework;

namespace MoreLinq.Test.Pull
{
    [TestFixture]
    public class SetOperationsTest
    {
        #region DistinctBy
        [Test]
        public void DistinctBy()
        {
            string[] source = { "first", "second", "third", "fourth", "fifth" };
            var distinct = source.DistinctBy(word => word.Length);
            distinct.AssertSequenceEqual("first", "second");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DistinctByNullSequence()
        {
            string[] source = null;
            source.DistinctBy(x => x.Length);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DistinctByNullKeySelector()
        {
            string[] source = {};
            source.DistinctBy((Func<string,string>) null);
        }

        [Test]
        public void DistinctByIsLazy()
        {
            new BreakingSequence<string>().DistinctBy(x => x.Length);
        }

        [Test]
        public void DistinctByWithComparer()
        {
            string[] source = { "first", "FIRST", "second", "second", "third" };
            var distinct = source.DistinctBy(word => word, StringComparer.OrdinalIgnoreCase);
            distinct.AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DistinctByNullSequenceWithComparer()
        {
            string[] source = null;
            source.DistinctBy(x => x, StringComparer.Ordinal);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DistinctByNullKeySelectorWithComparer()
        {
            string[] source = { };
            source.DistinctBy(null, StringComparer.Ordinal);
        }

        [Test]
        public void DistinctByNullComparer()
        {
            string[] source = { "first", "second", "third", "fourth", "fifth" };
            var distinct = source.DistinctBy(word => word.Length, null);
            distinct.AssertSequenceEqual("first", "second");
        }

        [Test]
        public void DistinctByIsLazyWithComparer()
        {
            new BreakingSequence<string>().DistinctBy(x => x, StringComparer.Ordinal);
        }
        #endregion
    }
}
