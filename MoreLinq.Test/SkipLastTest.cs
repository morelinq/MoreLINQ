#region License and Terms
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
using System;
using System.Collections.Generic;
using System.Linq;
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class SkipLastTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SkipLastWithNullSequence()
        {
            (null as IEnumerable<int>).SkipLast(1);
        }

        [Test]
        public void SkipLastWithCountLesserThanOne()
        {
            var numbers = Enumerable.Range(1, 5);

            Assert.IsTrue(numbers.SkipLast(-1).SequenceEqual(numbers));
            Assert.IsTrue(numbers.SkipLast(0).SequenceEqual(numbers));
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

        [Test]
        public void SkipLastWithSequenceShorterThanCount()
        {
            var numbers = Enumerable.Range(1, 5);

            Assert.IsFalse(numbers.SkipLast(5).Any());
            Assert.IsFalse(numbers.SkipLast(6).Any());
        }

        [Test]
        public void SkipLastIsLazy()
        {
            new BreakingSequence<object>().SkipLast(1);
        }
    }
}
