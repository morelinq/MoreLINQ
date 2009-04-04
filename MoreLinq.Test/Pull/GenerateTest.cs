namespace MoreLinq.Test.Pull
{
    using System;
    using System.Linq;
    using MoreLinq.Pull;
    using NUnit.Framework;
    using Enumerable = MoreLinq.Pull.Enumerable;

    partial class EnumerableTest
    {
        [Test, Category("Sequence")]
        public void GenerateTerminatesWhenCheckReturnsFalse()
        {
            var result = Enumerable.Generate(1, n => n + 2).TakeWhile(n => n < 10);

            result.AssertSequenceEqual(1, 3, 5, 7, 9);
        }

        [Test, Category("Sequence")]
        public void GenerateProcessesNonNumerics()
        {
            var result = Enumerable.Generate("", s => s + 'a').TakeWhile(s => s.Length < 5);

            result.AssertSequenceEqual("", "a", "aa", "aaa", "aaaa");
        }

        [Test, Category("Sequence")]
        public void GenerateIsLazy()
        {
            var result = Enumerable.Generate(0, BreakingFunc.Of<int, int>()).TakeWhile(n => false);

            result.Consume();
        }

        [Test, Category("Sequence")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateWithNullGenerator()
        {
            Enumerable.Generate(0, null);
        }

        [Test, Category("Sequence")]
        public void GenerateByIndexIsLazy()
        {
            Enumerable.GenerateByIndex(BreakingFunc.Of<int, int>());
        }

        [Test, Category("Sequence")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateByIndexWithNullGenerator()
        {
            Enumerable.GenerateByIndex<int>(null);
        }

        [Test, Category("Sequence")]
        public void GenerateByIndex()
        {
            var sequence = Enumerable.GenerateByIndex(x => x.ToString()).Take(3);
            sequence.AssertSequenceEqual("0", "1", "2");
        }
    }
}
