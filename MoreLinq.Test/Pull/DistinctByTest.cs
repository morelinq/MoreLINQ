namespace MoreLinq.Test.Pull
{
    using System;
    using MoreLinq.Pull;
    using NUnit.Framework;

    partial class EnumerableTest
    {
        [Test, Category("SetOperations")]
        public void DistinctBy()
        {
            string[] source = { "first", "second", "third", "fourth", "fifth" };
            var distinct = source.DistinctBy(word => word.Length);
            distinct.AssertSequenceEqual("first", "second");
        }

        [Test, Category("SetOperations")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DistinctByNullSequence()
        {
            string[] source = null;
            source.DistinctBy(x => x.Length);
        }

        [Test, Category("SetOperations")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DistinctByNullKeySelector()
        {
            string[] source = {};
            source.DistinctBy((Func<string,string>) null);
        }

        [Test, Category("SetOperations")]
        public void DistinctByIsLazy()
        {
            new BreakingSequence<string>().DistinctBy(x => x.Length);
        }

        [Test, Category("SetOperations")]
        public void DistinctByWithComparer()
        {
            string[] source = { "first", "FIRST", "second", "second", "third" };
            var distinct = source.DistinctBy(word => word, StringComparer.OrdinalIgnoreCase);
            distinct.AssertSequenceEqual("first", "second", "third");
        }

        [Test, Category("SetOperations")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DistinctByNullSequenceWithComparer()
        {
            string[] source = null;
            source.DistinctBy(x => x, StringComparer.Ordinal);
        }

        [Test, Category("SetOperations")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DistinctByNullKeySelectorWithComparer()
        {
            string[] source = { };
            source.DistinctBy(null, StringComparer.Ordinal);
        }

        [Test, Category("SetOperations")]
        public void DistinctByNullComparer()
        {
            string[] source = { "first", "second", "third", "fourth", "fifth" };
            var distinct = source.DistinctBy(word => word.Length, null);
            distinct.AssertSequenceEqual("first", "second");
        }

        [Test, Category("SetOperations")]
        public void DistinctByIsLazyWithComparer()
        {
            new BreakingSequence<string>().DistinctBy(x => x, StringComparer.Ordinal);
        }
    }
}
