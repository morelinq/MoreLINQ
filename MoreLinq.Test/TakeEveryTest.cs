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
    public class TakeEveryTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TakeEveryNullSequence()
        {
            MoreEnumerable.TakeEvery<object>(null, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TakeEveryNegativeSkip()
        {
            MoreEnumerable.TakeEvery(new object[0], -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TakeEveryOutOfRangeZeroStep()
        {
            MoreEnumerable.TakeEvery(new object[0], 0);
        }

        [Test]
        public void TakeEveryEmptySequence()
        {
            Assert.That(MoreEnumerable.TakeEvery(new object[0], 1).GetEnumerator().MoveNext(), Is.False);
        }

        [Test]
        public void TakeEveryNonEmptySequence()
        {
            var result = MoreEnumerable.TakeEvery(new[] { 1, 2, 3, 4, 5 }, 1);
            result.AssertSequenceEqual(1, 2, 3, 4, 5);
        }

        [Test]
        public void TakeEveryOtherOnNonEmptySequence()
        {
            var result = MoreEnumerable.TakeEvery(new[] { 1, 2, 3, 4, 5 }, 2);
            result.AssertSequenceEqual(1, 3, 5);
        }

        [Test]
        public void TakeEveryThirdOnNonEmptySequence()
        {
            var result = MoreEnumerable.TakeEvery(new[] { 1, 2, 3, 4, 5 }, 3);
            result.AssertSequenceEqual(1, 4);
        }

        [Test]
        public void TakeEveryIsLazy()
        {
            new BreakingSequence<object>().TakeEvery(1);
        }
    }
}