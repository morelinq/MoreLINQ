using System;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class UnionByTest
    {
        [Test]
        public void UnionBy()
        {
            string[] first = { "one", "two", "three", "five" };
            string[] second = { "alice", "tom", "joe" };

            var result = first.UnionBy(second, s => s.First());
            result.AssertSequenceEqual("one", "two", "five", "alice", "joe");
        }

        [Test]
        public void UnionByIsLazy()
        {
            new BreakingSequence<string>().UnionBy(new BreakingSequence<string>(), BreakingFunc.Of<string, int>());
        }
    }
}
