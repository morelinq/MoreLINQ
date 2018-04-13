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

namespace MoreLinq.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class CountBetweenTest
    {
        [Test]
        public void CountBetweenWithNegativeMin()
        {
            AssertThrowsArgument.OutOfRangeException("min", () =>
                new[] { 1 }.CountBetween(-1, 0));
        }

        [Test]
        public void CountBetweenWithNegativeMax()
        {
            AssertThrowsArgument.OutOfRangeException("max", () =>
               new[] { 1 }.CountBetween(0, -1));
        }

        [Test]
        public void CountBetweenWithMaxLesserThanMin()
        {
            AssertThrowsArgument.OutOfRangeException("max", () =>
                new[] { 1 }.CountBetween(1, 0));
        }

        [Test]
        public void CountBetweenWithMaxEqualsMin()
        {
            foreach (var xs in new[] { 1 }.ArrangeCollectionTestCases())
                Assert.IsTrue(xs.CountBetween(1, 1));
        }

        [TestCase(1, 2, 4, false)]
        [TestCase(2, 2, 4, true)]
        [TestCase(3, 2, 4, true)]
        [TestCase(4, 2, 4, true)]
        [TestCase(5, 2, 4, false)]
        public void CountBetweenRangeTests(int count, int min, int max, bool expecting)
        {
            foreach (var xs in Enumerable.Range(1, count).ArrangeCollectionTestCases())
                Assert.That(xs.CountBetween(min, max), Is.EqualTo(expecting));
        }

        [Test]
        public void CountBetweenDoesNotIterateUnnecessaryElements()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => 2,
                                             () => 3,
                                             () => 4,
                                             () => throw new TestException());
            Assert.False(source.CountBetween(2, 3));
        }
    }
}
