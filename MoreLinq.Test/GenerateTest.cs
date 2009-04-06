using System;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class GenerateTest
    {
        [Test]
        public void GenerateTerminatesWhenCheckReturnsFalse()
        {
            var result = MoreEnumerable.Generate(1, n => n + 2).TakeWhile(n => n < 10);

            result.AssertSequenceEqual(1, 3, 5, 7, 9);
        }

        [Test]
        public void GenerateProcessesNonNumerics()
        {
            var result = MoreEnumerable.Generate("", s => s + 'a').TakeWhile(s => s.Length < 5);

            result.AssertSequenceEqual("", "a", "aa", "aaa", "aaaa");
        }

        [Test]
        public void GenerateIsLazy()
        {
            var result = MoreEnumerable.Generate(0, BreakingFunc.Of<int, int>()).TakeWhile(n => false);

            result.Consume();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateWithNullGenerator()
        {
            MoreEnumerable.Generate(0, null);
        }

        [Test]
        public void GenerateByIndexIsLazy()
        {
            MoreEnumerable.GenerateByIndex(BreakingFunc.Of<int, int>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateByIndexWithNullGenerator()
        {
            MoreEnumerable.GenerateByIndex<int>(null);
        }

        [Test]
        public void GenerateByIndex()
        {
            var sequence = MoreEnumerable.GenerateByIndex(x => x.ToString()).Take(3);
            sequence.AssertSequenceEqual("0", "1", "2");
        }
    }
}
