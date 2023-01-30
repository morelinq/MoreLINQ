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
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class BacksertTest
    {
        [Test]
        public void BacksertIsLazy()
        {
            _ = new BreakingSequence<int>().Backsert(new BreakingSequence<int>(), 0);
        }

        [Test]
        public void BacksertWithNegativeIndex()
        {
            Assert.That(() => Enumerable.Range(1, 10).Backsert(new[] { 97, 98, 99 }, -1),
                        Throws.ArgumentOutOfRangeException("index"));
        }

        [TestCase(new[] { 1, 2, 3 }, 4, new[] { 9 })]
        public void BacksertWithIndexGreaterThanSourceLength(int[] seq1, int index, int[] seq2)
        {
            using var test1 = seq1.AsTestingSequence();
            using var test2 = seq2.AsTestingSequence();

            var result = test1.Backsert(test2, index);

            Assert.That(() => result.ElementAt(0),
                        Throws.ArgumentOutOfRangeException());
        }

        [TestCase(new[] { 1, 2, 3 }, 0, new[] { 8, 9 }, ExpectedResult = new[] { 1, 2, 3, 8, 9 })]
        [TestCase(new[] { 1, 2, 3 }, 1, new[] { 8, 9 }, ExpectedResult = new[] { 1, 2, 8, 9, 3 })]
        [TestCase(new[] { 1, 2, 3 }, 2, new[] { 8, 9 }, ExpectedResult = new[] { 1, 8, 9, 2, 3 })]
        [TestCase(new[] { 1, 2, 3 }, 3, new[] { 8, 9 }, ExpectedResult = new[] { 8, 9, 1, 2, 3 })]
        public IEnumerable<int> Backsert(int[] seq1, int index, int[] seq2)
        {
            using var test1 = seq1.AsTestingSequence();
            using var test2 = seq2.AsTestingSequence();

            return test1.Backsert(test2, index).ToArray();
        }
    }
}
