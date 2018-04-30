#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Leandro F. Vieira (leandromoh). All rights reserved.
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
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class BacksertTest
    {
        [Test]
        public void BacksertWithNegativeIndex()
        {
            AssertThrowsArgument.OutOfRangeException("index", () =>
                 Enumerable.Range(1, 10).Backsert(new[] { 97, 98, 99 }, -1));
        }

        [Test]
        public void BacksertWithIndexGreaterThanSourceLength()
        {
            const int count = 5;
            var source = Enumerable.Range(0, count);
            var result = source.Backsert(new[] { 97, 98, 99 }, count + 1);

            Assert.Throws<ArgumentOutOfRangeException>(() => result.ElementAt(0));
        }

        [Test]
        public void BacksertWithIndexEqualsSourceLength()
        {
            const int count = 5;
            var source = Enumerable.Range(1, count);
            var second = new[] { 9 };
            var result = source.Backsert(second, count);
            var expectations = second.Concat(source);

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void BacksertWithIndexZero()
        {
            var source = Enumerable.Range(1, 5);
            var second = new[] { 9 };
            var result = source.Backsert(second, 0);
            var expectations = source.Concat(second);

            Assert.That(result, Is.EqualTo(expectations));
        }

        [TestCase(3, 1)]
        [TestCase(3, 2)]
        public void Backsert(int count, int index)
        {
            var first = Enumerable.Range(1, count);
            var second = new[] { 97, 98, 99 };
            var result = first.Backsert(second, index);
            var expectations = first.SkipLast(index).Concat(second).Concat(first.TakeLast(index));

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void BacksertIsLazy()
        {
            new BreakingSequence<int>().Backsert(new BreakingSequence<int>(), 0);
        }
    }
}
