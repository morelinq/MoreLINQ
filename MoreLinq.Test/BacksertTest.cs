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

        [TestCase(new[] { 1, 2, 3 }, 4, new[] { 9 })]
        public void BacksertWithIndexGreaterThanSourceLength(int[] seq1, int index, int[] seq2)
        {
            using (var test1 = seq1.AsTestingSequence())
            using (var test2 = seq2.AsTestingSequence())
            {
                var result = test1.Backsert(test2, index);

                Assert.Throws<ArgumentOutOfRangeException>(() => result.ElementAt(0));
            }
        }

        [TestCase(new[] { 1, 2, 3 }, 3, new[] { 9 })]
        public void BacksertWithIndexEqualsSourceLength(int[] seq1, int index, int[] seq2)
        {
            using (var test1 = seq1.AsTestingSequence())
            using (var test2 = seq2.AsTestingSequence())
            {
                var result = test1.Backsert(test2, index);
                var expectations = seq2.Concat(seq1);

                Assert.That(result, Is.EqualTo(expectations));
            }
        }

        [TestCase(new[] { 1, 2, 3 }, 0, new[] { 9 })]
        public void BacksertWithIndexZero(int[] seq1, int index, int[] seq2)
        {
            using (var test1 = seq1.AsTestingSequence())
            using (var test2 = seq2.AsTestingSequence())
            {
                var result = test1.Backsert(test2, 0);
                var expectations = seq1.Concat(seq2);

                Assert.That(result, Is.EqualTo(expectations));
            }
        }

        [TestCase(new[] { 1, 2, 3 }, 1, new[] { 9 })]
        [TestCase(new[] { 1, 2, 3 }, 2, new[] { 9 })]
        public void Backsert(int[] seq1, int index, int[] seq2)
        {
            using (var test1 = seq1.AsTestingSequence())
            using (var test2 = seq2.AsTestingSequence())
            {
                var result = test1.Backsert(test2, index);
                var expectations = seq1.SkipLast(index).Concat(seq2).Concat(seq1.TakeLast(index));

                Assert.That(result, Is.EqualTo(expectations));
            }
        }

        [Test]
        public void BacksertIsLazy()
        {
            new BreakingSequence<int>().Backsert(new BreakingSequence<int>(), 0);
        }
    }
}
