#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2009 Atif Aziz. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

namespace MoreLinq.Test
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class AssertCountTest
    {
        [Test]
        public void AssertCountNegativeCount()
        {
            var source = new object[0];
            AssertThrowsArgument.OutOfRangeException("count", () =>
                source.AssertCount(-1));
            AssertThrowsArgument.OutOfRangeException("count", () =>
                source.AssertCount(-1, BreakingFunc.Of<int, int, Exception>()));
        }

        [Test]
        public void AssertCountSequenceWithMatchingLength()
        {
            "foo,bar,baz".GenerateSplits(',').AssertCount(3).Consume();
        }

        [Test]
        public void AssertCountShortSequence()
        {
            Assert.Throws<SequenceException>(() =>
                "foo,bar,baz".GenerateSplits(',').AssertCount(4).Consume());
        }

        [Test]
        public void AssertCountLongSequence()
        {
            Assert.Throws<SequenceException>(() =>
                "foo,bar,baz".GenerateSplits(',').AssertCount(2).Consume());
        }

        [Test]
        public void AssertCountDefaultExceptionMessageVariesWithCase()
        {
            var tokens = "foo,bar,baz".GenerateSplits(',');
            var e1 = Assert.Throws<SequenceException>(() => tokens.AssertCount(4).Consume());
            var e2 = Assert.Throws<SequenceException>(() => tokens.AssertCount(2).Consume());
            Assert.That(e1.Message, Is.Not.EqualTo(e2.Message));
        }

        [Test]
        public void AssertCountLongSequenceWithErrorSelector()
        {
            var e =
                Assert.Throws<TestException>(() =>
                    "foo,bar,baz".GenerateSplits(',').AssertCount(2, (cmp, count) => new TestException(cmp, count))
                                 .Consume());
            Assert.That(e.Cmp, Is.GreaterThan(0));
            Assert.That(e.Count, Is.EqualTo(2));
        }

        [Test]
        public void AssertCountShortSequenceWithErrorSelector()
        {
            var e =
                Assert.Throws<TestException>(() =>
                    "foo,bar,baz".GenerateSplits(',').AssertCount(4, (cmp, count) => new TestException(cmp, count))
                                 .Consume());
            Assert.That(e.Cmp, Is.LessThan(0));
            Assert.That(e.Count, Is.EqualTo(4));
        }

        sealed class TestException : Exception
        {
            public int Cmp { get; }
            public int Count { get; }

            public TestException(int cmp, int count)
            {
                Cmp = cmp;
                Count = count;
            }
        }

        [Test]
        public void AssertCountIsLazy()
        {
            new BreakingSequence<object>().AssertCount(0);
        }

        [Test]
        public void AssertCountWithCollectionIsLazy()
        {
            new BreakingCollection<object>(5).AssertCount(0);
        }

        [Test]
        public void AssertCountWithMatchingCollectionCount()
        {
            var xs = new[] { 123, 456, 789 };
            Assert.AreSame(xs, xs.AssertCount(3));
        }

        [TestCase(3, 2, "Sequence contains too many elements when exactly 2 were expected.")]
        [TestCase(3, 4, "Sequence contains too few elements when exactly 4 were expected.")]
        public void AssertCountWithMismatchingCollectionCount(int sourceCount, int count, string message)
        {
            var xs = new int[sourceCount];
            using var enumerator = xs.AssertCount(count).GetEnumerator();
            var e = Assert.Throws<SequenceException>(() => enumerator.MoveNext());
            Assert.AreEqual(e.Message, message);
        }

        [Test]
        public void AssertCountWithReadOnlyCollectionIsLazy()
        {
            new BreakingReadOnlyCollection<object>(5).AssertCount(0);
        }
    }
}
