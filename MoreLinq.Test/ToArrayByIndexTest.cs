#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Atif Aziz. All rights reserved.
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
    using NUnit.Framework.Constraints;

    [TestFixture]
    public class ToArrayByIndexTest
    {
        [TestCase(-1, new int[0])]
        [TestCase(-1, new[] { 5 })]
        [TestCase(-1, new[] { 1, 5 })]
        [TestCase(-1, new[] { 0, 9 })]
        [TestCase(-1, new[] { 0, 5, 9 })]
        [TestCase(-1, new[] { 2, 3, 5, 9 })]
        [TestCase(-1, new[] { 5, 2, 9, 3 })]
        [TestCase(10, new int[0])]
        [TestCase(10, new[] { 5 })]
        [TestCase(10, new[] { 1, 5 })]
        [TestCase(10, new[] { 0, 9 })]
        [TestCase(10, new[] { 0, 5, 9 })]
        [TestCase(10, new[] { 2, 3, 5, 9 })]
        [TestCase(10, new[] { 5, 2, 9, 3 })]
        public void ToArrayByIndex(int length, int[] indicies)
        {
            var input = indicies.Select(i => new { Index = i }).ToArray();
            var result = length < 0 ? input.ToArrayByIndex(e => e.Index)
                       : input.ToArrayByIndex(length, e => e.Index);
            var nils = result.ToList();

            var lastIndex = length < 0
                          ? input.Select(e => e.Index).DefaultIfEmpty(-1).Max()
                          : length - 1;
            var expectedLength = lastIndex + 1;
            Assert.That(result.Count, Is.EqualTo(expectedLength));

            foreach (var e in from e in input
                              orderby e.Index descending
                              select e)
            {
                Assert.That(result[e.Index], Is.SameAs(input.Single(inp => inp.Index == e.Index)));
                nils.RemoveAt(e.Index);
            }

            Assert.That(nils.Count, Is.EqualTo(expectedLength - input.Length));
            Assert.That(nils.All(e => e == null), Is.True);
        }

        [Test]
        public void ToArrayByIndexWithBadIndexSelectorThrows()
        {
            var input = new[] { 42 };

            Assert.That(() => input.ToArrayByIndex(_ => -1),
                        Throws.TypeOf<IndexOutOfRangeException>());

            Assert.That(() => input.ToArrayByIndex(_ => -1, BreakingFunc.Of<int, object>()),
                        Throws.TypeOf<IndexOutOfRangeException>());
        }

        [TestCase(10, -1)]
        [TestCase(10, 10)]
        public void ToArrayByIndexWithLengthWithBadIndexSelectorThrows(int length, int badIndex)
        {
            var input = new[] { 42 };
            Assert.That(() => input.ToArrayByIndex(length, _ => badIndex),
                        Throws.TypeOf<IndexOutOfRangeException>());

            Assert.That(() => input.ToArrayByIndex(length, _ => badIndex, BreakingFunc.Of<int, object>()),
                        Throws.TypeOf<IndexOutOfRangeException>());
        }

        [Test]
        public void ToArrayByIndexOverwritesAtSameIndex()
        {
            var a = new { Index = 2 };
            var b = new { Index = 2 };
            var input = new[] { a, b };

            {
                var expectations = new IResolveConstraint[]
                {
                    Is.Null, Is.Null, Is.SameAs(b)
                };

                input.ToArrayByIndex(e => e.Index).AssertSequence(expectations);
                input.ToArrayByIndex(e => e.Index, e => e).AssertSequence(expectations);
            }

            {
                var expectations = new IResolveConstraint[]
                {
                    Is.Null, Is.Null, Is.SameAs(b), Is.Null
                };

                const int length = 4;
                input.ToArrayByIndex(length, e => e.Index).AssertSequence(expectations);
                input.ToArrayByIndex(length, e => e.Index, e => e).AssertSequence(expectations);
                input.ToArrayByIndex(length, e => e.Index, (e, _) => e).AssertSequence(expectations);
            }
        }
    }
}
