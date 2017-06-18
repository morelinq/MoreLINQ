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

        [TestCase(10, 5, 3)]
        [TestCase(10, 0, 5)]
        public void SwapRange(int length, int index, int count)
        {
            var source = Enumerable.Range(0, length);

            Enumerable.Range(0, length + 3).ForEach(putAt => 
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
    }
}
