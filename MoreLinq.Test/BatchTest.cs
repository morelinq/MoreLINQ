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
    using System.Collections.Generic;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BatchTest
    {
        [Test]
        public void BatchZeroSize()
        {
            AssertThrowsArgument.OutOfRangeException("size",() =>
                new object[0].Batch(0));
        }

        [Test]
        public void BatchNegativeSize()
        {
            AssertThrowsArgument.OutOfRangeException("size",() =>
                new object[0].Batch(-1));
        }

        [Test]
        public void BatchEvenlyDivisibleSequence()
        {
            var result = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Batch(3);
            using (var reader = result.Read())
            {
                reader.Read().AssertSequenceEqual(1, 2, 3);
                reader.Read().AssertSequenceEqual(4, 5, 6);
                reader.Read().AssertSequenceEqual(7, 8, 9);
                reader.ReadEnd();
            }
        }

        [Test]
        public void BatchUnevenlyDivisbleSequence()
        {
            var result = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Batch(4);
            using (var reader = result.Read())
            {
                reader.Read().AssertSequenceEqual(1, 2, 3, 4);
                reader.Read().AssertSequenceEqual(5, 6, 7, 8);
                reader.Read().AssertSequenceEqual(9);
                reader.ReadEnd();
            }
        }

        [Test]
        public void BatchSequenceTransformingResult()
        {
            var result = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Batch(4, batch => batch.Sum());
            result.AssertSequenceEqual(10, 26, 9);
        }

        [Test]
        public void BatchSequenceYieldsIEnumerablesOfBatches()
        {
            var result = new[] { 1, 2, 3 }.Batch(2);
            using (var reader = result.Read())
            {
                Assert.That(reader.Read(), Is.InstanceOf(typeof(IEnumerable<int>)));
                Assert.That(reader.Read(), Is.InstanceOf(typeof(IEnumerable<int>)));
                reader.ReadEnd();
            }
        }

        [Test]
        public void BatchIsLazy()
        {
            new BreakingSequence<object>().Batch(1);
        }

        [Test]
        public void BatchHasLazyEnumerables()
        {
            var seq1 = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var seq2 = MoreEnumerable.From<int>(() => throw new InvalidOperationException());
            var result = seq1.Concat(seq2).Batch(4);

            using (var reader = result.Read())
            {
                reader.Read().AssertSequenceEqual(1, 2, 3, 4);
                reader.Read().AssertSequenceEqual(5, 6, 7, 8);
                reader.Read().Take(1).AssertSequenceEqual(9);
            }
        }

        [Test]
        public void BatchesCanBePartialIterated()
        {
            var result = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }.Batch(3);
            IEnumerable<int> batch1, batch2, batch3, batch4;

            using (var reader = result.Read())
            {
                batch1 = reader.Read();
                batch1.AssertSequenceEqual(1, 2, 3);

                batch2 = reader.Read();
                batch2.Take(1).AssertSequenceEqual(4);

                batch3 = reader.Read();
                batch3.Take(2).AssertSequenceEqual(7, 8);

                batch4 = reader.Read();
                batch4.Take(3).AssertSequenceEqual(10, 11);

                reader.ReadEnd();
            }

            batch1.AssertSequenceEqual( 1, 2, 3);
            batch2.AssertSequenceEqual( 4, 5, 6);
            batch3.AssertSequenceEqual( 7, 8, 9);
            batch4.AssertSequenceEqual(10, 11);
        }
    }
}
