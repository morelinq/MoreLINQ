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
            Assert.That(() => source.AssertCount(-1),
                        Throws.ArgumentOutOfRangeException("count"));
            Assert.That(() => source.AssertCount(-1, BreakingFunc.Of<int, int, Exception>()),
                        Throws.ArgumentOutOfRangeException("count"));
        }

        [Test]
        public void AssertCountSequenceWithMatchingLength()
        {
            "foo,bar,baz".GenerateSplits(',').AssertCount(3).Consume();
        }

        [Test]
        public void AssertCountShortSequence()
        {
            Assert.That(() => "foo,bar,baz".GenerateSplits(',').AssertCount(4).Consume(),
                        Throws.TypeOf<SequenceException>());
        }

        [Test]
        public void AssertCountLongSequence()
        {
            Assert.That(() => "foo,bar,baz".GenerateSplits(',').AssertCount(2).Consume(),
                        Throws.TypeOf<SequenceException>());
        }

        [TestCase(4, "Sequence contains too few elements when exactly 4 were expected.")]
        [TestCase(2, "Sequence contains too many elements when exactly 2 were expected.")]
        public void AssertCountDefaultExceptionMessageVariesWithCase(int count, string expectedMessage)
        {
            var tokens = "foo,bar,baz".GenerateSplits(',');

            Assert.That(() => tokens.AssertCount(count).Consume(),
                        Throws.TypeOf<SequenceException>().With.Message.EqualTo(expectedMessage));
        }

        [Test]
        public void AssertCountLongSequenceWithErrorSelector()
        {
            Assert.That(() =>
                "foo,bar,baz".GenerateSplits(',').AssertCount(2, (cmp, count) => new TestException(cmp, count))
                             .Consume(),
                Throws.TypeOf<TestException>()
                      .With.Property(nameof(TestException.Cmp)).GreaterThan(0)
                      .And.Count.EqualTo(2));
        }

        [Test]
        public void AssertCountShortSequenceWithErrorSelector()
        {
            Assert.That(() =>
                "foo,bar,baz".GenerateSplits(',').AssertCount(4, (cmp, count) => new TestException(cmp, count))
                             .Consume(),
                Throws.TypeOf<TestException>()
                      .With.Property(nameof(TestException.Cmp)).LessThan(0)
                      .And.Count.EqualTo(4));
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
            Assert.That(xs, Is.SameAs(xs.AssertCount(3)));
        }

        [TestCase(3, 2, "Sequence contains too many elements when exactly 2 were expected.")]
        [TestCase(3, 4, "Sequence contains too few elements when exactly 4 were expected.")]
        public void AssertCountWithMismatchingCollectionCount(int sourceCount, int count, string message)
        {
            var xs = new int[sourceCount];
            using var enumerator = xs.AssertCount(count).GetEnumerator();
            Assert.That(enumerator.MoveNext,
                        Throws.TypeOf<SequenceException>().With.Message.EqualTo(message));
        }

        [Test]
        public void AssertCountWithReadOnlyCollectionIsLazy()
        {
            new BreakingReadOnlyCollection<object>(5).AssertCount(0);
        }
    }
}
