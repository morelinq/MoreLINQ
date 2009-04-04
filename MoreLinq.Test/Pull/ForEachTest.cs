namespace MoreLinq.Test.Pull
{
    using System;
    using System.Collections.Generic;
    using MoreLinq.Pull;
    using NUnit.Framework;

    partial class EnumerableTest
    {
        [Test, Category("SideEffects")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ForEachNullSequence()
        {
            Enumerable.ForEach<int>(null, x => { throw new InvalidOperationException(); });
        }

        [Test, Category("SideEffects")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ForEachNullAction()
        {
            Enumerable.ForEach(new[] { 1, 2, 3 }, null);
        }

        [Test, Category("SideEffects")]
        public void ForEachWithSequence()
        {
            List<int> results = new List<int>();
            Enumerable.ForEach(new[] { 1, 2, 3 }, results.Add);
            results.AssertSequenceEqual(1, 2, 3);
        }
    }
}
