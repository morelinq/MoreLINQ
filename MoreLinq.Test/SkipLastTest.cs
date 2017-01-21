﻿#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Leandro F. Vieira (leandromoh). All rights reserved.
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

using NUnit.Framework;
using System.Linq;

namespace MoreLinq.Test
{
    [TestFixture]
    public class SkipLastTest
    {
        [Test]
        public void SkipLastWithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source", () => MoreEnumerable.SkipLast<int>(null, 1));
        }

        [TestCase(1, 5, 0)]
        [TestCase(1, 5, -1)]
        public void SkipLastWithCountLesserThanOne(int start, int count, int skip)
        {
            var numbers = Enumerable.Range(start, count);

            Assert.IsTrue(numbers.SkipLast(skip).SequenceEqual(numbers));
        }

        [Test]
        public void SkipLastSimpleTest()
        {
            int take = 100;
            int skip = 20;

            var randomSequence = MoreEnumerable.Random(0, 100).Take(take).ToArray();

            var expectations = randomSequence.Take(take - skip);

            Assert.IsTrue(expectations.SequenceEqual(randomSequence.SkipLast(skip)));
        }

        [TestCase(1, 5, 5)]
        [TestCase(1, 5, 6)]
        public void SkipLastWithSequenceShorterThanCount(int start, int count, int skip)
        {
            Assert.IsFalse(Enumerable.Range(start, count).SkipLast(skip).Any());
        }

        [Test]
        public void SkipLastIsLazy()
        {
            new BreakingSequence<object>().SkipLast(1);
        }
    }
}
