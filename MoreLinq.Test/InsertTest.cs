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
    public class InsertTest
    {
        [Test]
        public void InsertWithNegativeIndex()
        {
            Assert.That(() => Enumerable.Range(1, 10).Insert(new[] { 97, 98, 99 }, -1),
                        Throws.ArgumentOutOfRangeException("index"));
        }

        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        public void InsertWithIndexGreaterThanSourceLengthMaterialized(int count)
        {
            var seq1 = Enumerable.Range(0, count).ToList();
            var seq2 = new[] { 97, 98, 99 };

            using var test1 = seq1.AsTestingSequence();
            using var test2 = seq2.AsTestingSequence();

            var result = test1.Insert(test2, count + 1);

            Assert.That(() => result.ForEach((e, index) => Assert.That(e, Is.EqualTo(seq1[index]))),
                        Throws.ArgumentOutOfRangeException("index"));
        }

        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        public void InsertWithIndexGreaterThanSourceLengthLazy(int count)
        {
            var seq1 = Enumerable.Range(0, count);
            var seq2 = new[] { 97, 98, 99 };

            using var test1 = seq1.AsTestingSequence();
            using var test2 = seq2.AsTestingSequence();

            var result = test1.Insert(test2, count + 1).Take(count);

            Assert.That(seq1, Is.EqualTo(result));
        }

        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(3, 2)]
        [TestCase(3, 3)]
        public void Insert(int count, int index)
        {
            var seq1 = Enumerable.Range(1, count);
            var seq2 = new[] { 97, 98, 99 };

            using var test1 = seq1.AsTestingSequence();
            using var test2 = seq2.AsTestingSequence();

            var result = test1.Insert(test2, index);

            var expectations = seq1.Take(index).Concat(seq2).Concat(seq1.Skip(index));
            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void InsertIsLazy()
        {
            _ = new BreakingSequence<int>().Insert(new BreakingSequence<int>(), 0);
        }
    }
}
