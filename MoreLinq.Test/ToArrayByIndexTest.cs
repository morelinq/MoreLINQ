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

using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
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
            var inputs = indicies.Select(i => new { Index = i }).ToArray();
            var result = length < 0 ? inputs.ToArrayByIndex(e => e.Index)
                       : inputs.ToArrayByIndex(length, e => e.Index);
            var nils = result.ToList();

            var lastIndex = length < 0
                          ? inputs.Select(e => e.Index).DefaultIfEmpty(-1).Max()
                          : length - 1;
            var expectedLength = lastIndex + 1;
            Assert.That(result.Count, Is.EqualTo(expectedLength));

            foreach (var e in from e in inputs
                              orderby e.Index descending
                              select e)
            {
                Assert.That(result[e.Index], Is.SameAs(inputs.Single(inp => inp.Index == e.Index)));
                nils.RemoveAt(e.Index);
            }

            Assert.That(nils.Count, Is.EqualTo(expectedLength - inputs.Length));
            Assert.That(nils.All(e => e == null), Is.True);
        }
    }
}
