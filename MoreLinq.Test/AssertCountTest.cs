#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
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

using System;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class AssertCountTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AssertCountNullSequence()
        {
            MoreEnumerable.AssertCount<object>(null, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AssertCountNegativeCount()
        {
            MoreEnumerable.AssertCount(new object[0], -1);
        }

        [Test]
        public void AssertCountSequenceWithMatchingLength()
        {
            "foo,bar,baz".GenerateSplits(',').AssertCount(3).Consume();
        }

        [Test]
        [ExpectedException(typeof(SequenceException))]
        public void AssertCountShortSequence()
        {
            "foo,bar,baz".GenerateSplits(',').AssertCount(4).Consume();
        }

        [Test]
        [ExpectedException(typeof(SequenceException))]
        public void AssertCountLongSequence()
        {
            "foo,bar,baz".GenerateSplits(',').AssertCount(2).Consume();
        }

        [Test]
        public void AssertCountDefaultExceptionMessageVariesWithCase()
        {
            var tokens = "foo,bar,baz".GenerateSplits(',');
            Exception e1 = null, e2 = null;
            try
            {
                tokens.AssertCount(4).Consume();
                Assert.Fail("Exception expected.");
            }
            catch (Exception e)
            {
                e1 = e;
            }
            try
            {
                tokens.AssertCount(2).Consume();
                Assert.Fail("Exception expected.");
            }
            catch (Exception e)
            {
                e2 = e;
            }
            Assert.That(e1.Message, Is.Not.EqualTo(e2.Message));
        }

        [Test]
        public void AssertCountLongSequenceWithErrorSelector()
        {
            try
            {
                "foo,bar,baz".GenerateSplits(',').AssertCount(2, (cmp, count) => new TestException(cmp, count)).Consume();
                Assert.Fail("Exception expected.");
            }
            catch (TestException e)
            {
                Assert.That(e.Cmp, Is.GreaterThan(0));
                Assert.That(e.Count, Is.EqualTo(2));
            }
        }

        [Test]
        public void AssertCountShortSequenceWithErrorSelector()
        {
            try
            {
                "foo,bar,baz".GenerateSplits(',').AssertCount(4, (cmp, count) => new TestException(cmp, count)).Consume();
                Assert.Fail("Exception expected.");
            }
            catch (TestException e)
            {
                Assert.That(e.Cmp, Is.LessThan(0));
                Assert.That(e.Count, Is.EqualTo(4));
            }
        }
        
        private sealed class TestException : Exception
        {
            public int Cmp { get; private set; }
            public int Count { get; private set; }

            public TestException(int cmp, int count)
            {
                Cmp = cmp;
                Count = count;
            }
        }

        [Test]
        public void AssertCountIsLazy()
        {
            MoreEnumerable.AssertCount(new BreakingSequence<object>(), 0);
        }
    }
}