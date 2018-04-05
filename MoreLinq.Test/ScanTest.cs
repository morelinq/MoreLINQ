#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
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
    public class ScanTest
    {
        [Test]
        public void ScanEmpty()
        {
            Assert.False(new int[0].Scan(SampleData.Plus).GetEnumerator().MoveNext());
        }

        [Test]
        public void ScanSum()
        {
            var result = Enumerable.Range(1, 10).Scan(SampleData.Plus);
            var gold = new[] { 1, 3, 6, 10, 15, 21, 28, 36, 45, 55 };
            result.AssertSequenceEqual(gold);
        }

        [Test]
        public void ScanIsLazy()
        {
            new BreakingSequence<object>().Scan(delegate { throw new NotImplementedException(); });
        }

        [Test]
        public void ScanDoesNotIterateExtra()
        {
            var sequence = Enumerable.Range(1, 3).Concat(new BreakingSequence<int>()).Scan(SampleData.Plus);
            var gold = new[] {1, 3, 6};
            Assert.Throws<InvalidOperationException>(sequence.Consume);
            sequence.Take(3).AssertSequenceEqual(gold);
        }

        [Test]
        public void SeededScanEmpty()
        {
            Assert.AreEqual(-1, new int[0].Scan(-1, SampleData.Plus).Single());
        }

        [Test]
        public void SeededScanSum()
        {
            var result = Enumerable.Range(1, 10).Scan(0, SampleData.Plus);
            var gold = new[] { 0, 1, 3, 6, 10, 15, 21, 28, 36, 45, 55 };
            result.AssertSequenceEqual(gold);
        }

        [Test]
        public void SeededScanIsLazy()
        {
            new BreakingSequence<object>().Scan<object, object>(null, delegate { throw new NotImplementedException(); });
        }

        [Test]
        public void SeededScanDoesNotIterateExtra()
        {
            var sequence = Enumerable.Range(1, 3).Concat(new BreakingSequence<int>()).Scan(0, SampleData.Plus);
            var gold = new[] { 0, 1, 3, 6 };
            Assert.Throws<InvalidOperationException>(sequence.Consume);
            sequence.Take(4).AssertSequenceEqual(gold);
        }

    }
}
