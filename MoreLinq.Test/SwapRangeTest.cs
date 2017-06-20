#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Leandro F. Vieira (leandromoh). All rights reserved.
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
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class SwapRangeTest
    {
        [Test]
        public void SwapRangeWithNegativeIndex()
        {
            Assert.ThrowsArgumentOutOfRangeException("index", () =>
                new[] { 1 }.SwapRange(-1, 0, 0));
        }

        [Test]
        public void SwapRangeWithNegativeCount()
        {
            Assert.ThrowsArgumentOutOfRangeException("count", () =>
                new[] { 1 }.SwapRange(0, -1, 0));
        }

        [Test]
        public void SwapRangeWithNegativePutAt()
        {
            Assert.ThrowsArgumentOutOfRangeException("putAt", () =>
                new[] { 1 }.SwapRange(0, 0, -1));
        }

        [Test]
        public void SwapRangeIsLazy()
        {
            new BreakingSequence<int>().SwapRange(0, 0, 0);
        }

        [TestCase(10, 5,  3)]
        [TestCase(10, 3,  5)]
        [TestCase(10, 0,  5)]
        [TestCase(10, 5,  0)]
        [TestCase(10, 6,  1)]
        [TestCase(10, 1,  6)]
        [TestCase(10, 0, 10)]
        [TestCase(10, 10, 0)]
        [TestCase(10, 3, 10)]
        [TestCase(10, 10, 3)]
        [TestCase(10, 99, 2)]
        public void SwapRange(int length, int index, int count)
        {
            var source = Enumerable.Range(0, length);

            source.ForEach(putAt => 
            {
                var exclude = source.Exclude(index, count);
                var slice = source.Slice(index, count);
                var expectations = exclude.Take(putAt).Concat(slice).Concat(exclude.Skip(putAt));

                using (var test = source.AsTestingSequence())
                {
                    var result = test.SwapRange(index, count, putAt);
                    Assert.That(result, Is.EquivalentTo(expectations));
                }
            });
        }

        public void SwapRangeWithSequenceShorterThanPutAt()
        {
            const int length = 10;
            const int index = 5;
            const int count = 2;

            var source = Enumerable.Range(0, length);

            Enumerable.Range(length, length + 5).ForEach(putAt => 
            {
                var expectations = source.Exclude(index, count).Concat(source.Slice(index, count));

                using (var test = source.AsTestingSequence())
                {
                    var result = test.SwapRange(index, count, putAt);
                    Assert.That(result, Is.EquivalentTo(expectations));
                }
            });
        }

        [Test]
        public void SwapRangeWithIndexSameAsPutAt()
        {
            var source = Enumerable.Range(0, 10);
            var result = source.SwapRange(5, 3, 5);
            
            Assert.That(source, Is.SameAs(result));
        }

        [Test]
        public void SwapRangeWithCountEqualsZero()
        {
            var source = Enumerable.Range(0, 10);
            var result = source.SwapRange(5, 0, 5);
            
            Assert.That(source, Is.SameAs(result));
        }
		
    }
}
