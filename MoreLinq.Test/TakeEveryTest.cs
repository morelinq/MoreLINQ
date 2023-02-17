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

namespace MoreLinq.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class TakeEveryTest
    {
        [Test]
        public void TakeEveryNegativeSkip()
        {
            Assert.That(() => new object[0].TakeEvery(-1),
                        Throws.ArgumentOutOfRangeException("step"));
        }

        [Test]
        public void TakeEveryOutOfRangeZeroStep()
        {
            Assert.That(() => new object[0].TakeEvery(0),
                        Throws.ArgumentOutOfRangeException("step"));
        }

        [Test]
        public void TakeEveryEmptySequence()
        {
            Assert.That(new object[0].TakeEvery(1), Is.Empty);
        }

        [Test]
        public void TakeEveryNonEmptySequence()
        {
            var result = new[] { 1, 2, 3, 4, 5 }.TakeEvery(1);
            result.AssertSequenceEqual(1, 2, 3, 4, 5);
        }

        [Test]
        public void TakeEveryOtherOnNonEmptySequence()
        {
            var result = new[] { 1, 2, 3, 4, 5 }.TakeEvery(2);
            result.AssertSequenceEqual(1, 3, 5);
        }

        [Test]
        public void TakeEveryThirdOnNonEmptySequence()
        {
            var result = new[] { 1, 2, 3, 4, 5 }.TakeEvery(3);
            result.AssertSequenceEqual(1, 4);
        }

        [Test]
        public void TakeEveryIsLazy()
        {
            _ = new BreakingSequence<object>().TakeEvery(1);
        }
    }
}
