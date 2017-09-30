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
    using NUnit.Framework;

    [TestFixture]
    public class InsertAtTest
    {
        [Test]
        public void InsertAtWithNegativeIndex()
        {
            AssertThrowsArgument.OutOfRangeException("index", () =>
                 Enumerable.Range(1, 10).InsertAt(new[] { 97, 98, 99 }, -1));
        }

        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        public void InsertAtWithIndexGreaterThanSourceLengthMaterialized(int count)
        {
            var source = Enumerable.Range(0, count).ToList();
            var result = source.InsertAt(new[] { 97, 98, 99 }, count + 1);

            AssertThrowsArgument.OutOfRangeException("index", () =>
                result.ForEach((e, index) => 
                    Assert.That(e, Is.EqualTo(source[index]))));
        }

        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        public void InsertAtWithIndexGreaterThanSourceLengthLazy(int count)
        {
            var source = Enumerable.Range(0, count);

            var result = source.InsertAt(new[] { 97, 98, 99 }, count + 1)
                               .Take(count)
                               .ToList();

            Assert.That(source, Is.EqualTo(result));
        }

        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(3, 2)]
        [TestCase(3, 3)]
        public void InsertAt(int count, int index)
        {
            var source = Enumerable.Range(1, count);

            var second = new[] { 97, 98, 99 };

            var result = source.InsertAt(second, index);

            var expectations = source.Take(index).Concat(second).Concat(source.Skip(index));

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void InsertAtIsLazy()
        {
            new BreakingSequence<int>().InsertAt(new BreakingSequence<int>(), 0);
        }
    }
}
