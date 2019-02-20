#region License and Terms

// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 RyotaMurohoshi All rights reserved.
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
using System.Collections.Generic;

namespace MoreLinq.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class IsEmptyTest
    {
        [Test]
        public void IsEmptyTrueWithEmptySequence()
        {
            Assert.IsTrue(new int[0].IsEmpty());
        }

        [Test]
        public void IsEmptyFalseWithSingleElement()
        {
            Assert.IsFalse(new[] {1}.IsEmpty());
        }

        [Test]
        public void IsEmptyFalseWithSomeElements()
        {
            var sequence = new[] {0};
            var infiniteSequence = sequence.Repeat();
            Assert.IsFalse(infiniteSequence.IsEmpty());
        }

        [Test]
        public void IsEmptyThrowsException()
        {
            Assert.Throws<TestException>(() =>
            {
                var source = MoreEnumerable.From<int>(() => throw new TestException());
                source.IsEmpty();
                Assert.Fail();
            });
        }

        [Test]
        public void IsEmptyThrowsNullArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                IEnumerable<int> source = null;
                source.IsEmpty();
                Assert.Fail();
            });
        }

        [Test]
        public void IsEmptyFalseNoThrowsException()
        {
            var source = MoreEnumerable.From(
                () => 0,
                () => throw new TestException()
            );
            Assert.IsFalse(source.IsEmpty());
        }
    }
}
