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
using System.Linq;
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class CountBetweenTest
    {
        [Test]
        public void CountBetweenWithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source",
                () => MoreEnumerable.CountBetween<int>(null, 1, 2));
        }

        [Test]
        public void CountBetweenWithNegativeMin()
        {
            Assert.ThrowsArgumentOutOfRangeException("min", () =>
                new[] { 1 }.CountBetween(-1, 0));
        }

        [Test]
        public void CountBetweenWithNegativeMax()
        {
            Assert.ThrowsArgumentOutOfRangeException("max", () =>
               new[] { 1 }.CountBetween(0, -1));
        }

        [Test]
        public void CountBetweenWithMaxLesserThanMin()
        {
            Assert.ThrowsArgumentOutOfRangeException("max", () =>
                new[] { 1 }.CountBetween(1, 0));
        }

        [Test]
        public void CountBetweenWithMaxEqualsMin()
        {
            Assert.IsTrue(new[] { 1 }.CountBetween(1, 1));
        }

        [TestCase(1, 1, 2, 4, false)]
        [TestCase(1, 2, 2, 4, true)]
        [TestCase(1, 3, 2, 4, true)]
        [TestCase(1, 4, 2, 4, true)]
        [TestCase(1, 5, 2, 4, false)]
        public void CountBetweenRangeTests(int start, int count, int min, int max, bool expecting)
        {
            Assert.That(Enumerable.Range(start, count).CountBetween(min, max), Is.EqualTo(expecting));
        }
    }
}
