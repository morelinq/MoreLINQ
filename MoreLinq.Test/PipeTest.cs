using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MoreLinq.Test
{
    [TestFixture]
    public class PipeTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeNullSequence()
        {
            Enumerable.Pipe<int>(null, x => { throw new InvalidOperationException(); });
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeNullAction()
        {
            Enumerable.Pipe(new[] { 1, 2, 3 }, null);
        }

        [Test]
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

        [Test]
        public void PipeIsLazy()
        {
            new BreakingSequence<int>().Pipe(x => { });
        }

        [Test]
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
