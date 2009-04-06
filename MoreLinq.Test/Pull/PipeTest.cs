namespace MoreLinq.Test.Pull
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MoreLinq.Pull;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;
    using Enumerable = MoreLinq.Pull.Enumerable;

    partial class EnumerableTest
    {
        [Test, Category("SideEffects")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeNullSequence()
        {
            Enumerable.Pipe<int>(null, x => { throw new InvalidOperationException(); });
        }

        [Test, Category("SideEffects")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeNullAction()
        {
            Enumerable.Pipe(new[] { 1, 2, 3 }, null);
        }

        [Test, Category("SideEffects")]
        public void PipeWithSequence()
        {
            List<int> results = new List<int>();
            var returned = Enumerable.Pipe(new[] { 1, 2, 3 }, results.Add);
            // Lazy - nothing has executed yet
            Assert.That(results, Is.Empty);
            returned.AssertSequenceEqual(1, 2, 3);
            // Now it has...
            results.AssertSequenceEqual(1, 2, 3);
        }

        [Test, Category("SideEffects")]
        public void PipeIsLazy()
        {
            new BreakingSequence<int>().Pipe(x => { });
        }

        [Test, Category("SideEffects")]
        public void PipeActionOccursBeforeYield()
        {
            var source = new[] { new StringBuilder(), new StringBuilder() };
            // The action will occur "in" the pipe, so by the time Where gets it, the
            // sequence will be empty.
            Assert.That(source.Pipe(sb => sb.Append("x"))
                              .Where(x => x.Length == 0),
                        Is.Empty);
        }
    }
}
