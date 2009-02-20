using System;
using System.Linq;
using MoreLinq.Pull;
using NUnit.Framework;

namespace MoreLinq.Test.Pull
{
    [TestFixture]
    public class SequenceTest
    {
        [Test]
        public void GenerateTerminatesWhenCheckReturnsFalse()
        {
            var result = Sequence.Generate(1, n => n + 2).TakeWhile(n => n < 10);

            result.AssertSequenceEqual(1, 3, 5, 7, 9);
        }

        [Test]
        public void GenerateProcessesNonNumerics()
        {
            var result = Sequence.Generate("", s => s + 'a').TakeWhile(s => s.Length < 5);

            result.AssertSequenceEqual("", "a", "aa", "aaa", "aaaa");
        }

        [Test]
        public void GenerateIsLazy()
        {
            var result = Sequence.Generate(0, BreakingFunc.Of<int,int>()).TakeWhile(n => false);

            result.Consume();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateWithNullGenerator()
        {
            Sequence.Generate(0, null);
        }

        [Test]
        public void GenerateByIndexIsLazy()
        {
            Sequence.GenerateByIndex(BreakingFunc.Of<int, int>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateByIndexWithNullGenerator()
        {
            Sequence.GenerateByIndex<int>(null);
        }

        [Test]
        public void GenerateByIndex()
        {
            var sequence = Sequence.GenerateByIndex(x => x.ToString()).Take(3);
            sequence.AssertSequenceEqual("0", "1", "2");
        }
    }
}
