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
    public class AssertTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AssertNullSequence()
        {
            MoreEnumerable.Assert<object>(null, delegate { return false; });
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AssertNullPredicate()
        {
            new object[0].Assert(null);
        }

        [Test]
        public void AssertIsLazy()
        {
            new BreakingSequence<object>().Assert(delegate { throw new NotImplementedException(); });
            new BreakingSequence<object>().Assert(delegate { throw new NotImplementedException(); }, delegate { throw new NotImplementedException(); });
        }

        [Test]
        public void AssertSequenceWithValidAllElements()
        {
            var source = new[] {2, 4, 6, 8};
            source.Assert(n => n % 2 == 0).AssertSequenceEqual(source);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AssertSequenceWithValidSomeInvalidElements()
        {
            new[] { 2, 4, 6, 7, 8, 9 }.Assert(n => n % 2 == 0).Consume();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AssertSequenceWithInvalidElementsAndCustomErrorReturningNull()
        {
            new[] { 2, 4, 6, 7, 8, 9 }.Assert(n => n % 2 == 0, _ => null).Consume();
        }

        [Test]
        public void AssertSequenceWithInvalidElementsAndCustomError()
        {
            try
            {
                new[] { 2, 4, 6, 7, 8, 9 }.Assert(n => n % 2 == 0, n => new ValueException(n)).Consume();
                Assert.Fail("{0} expected", typeof(ValueException));
            }
            catch (ValueException e)
            {
                Assert.AreEqual(7, e.Value);
            }
        }

        class ValueException : Exception
        {
            public object Value { get; private set; }
            public ValueException(object value) { Value = value; }
        }
    }
}
