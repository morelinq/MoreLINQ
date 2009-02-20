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
        public void ExpandTerminatesWhenCheckReturnsFalse()
        {
            var result = Sequence.Generate(1, n => n + 2).TakeWhile(n => n < 10);

            result.AssertSequenceEqual(1, 3, 5, 7, 9);
        }

        [Test]
        public void ExpandProcessesNonNumerics()
        {
            var result = Sequence.Generate("", s => s + 'a').TakeWhile(s => s.Length < 5);

            result.AssertSequenceEqual("", "a", "aa", "aaa", "aaaa");
        }

        [Test]
        public void ExpandIsLazy()
        {
            Func<int, int> generateFail =
                n =>
                {
                  throw new InvalidOperationException();
                };

            var result = Sequence.Generate(0, generateFail).TakeWhile(n => false);

            result.Exhaust();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExpandWithNullSequencer()
        {
            Sequence.Generate(0, null);
        }
    }
}
