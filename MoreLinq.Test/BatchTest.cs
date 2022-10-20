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

    [TestFixture]
    public class BatchTest : BatchBaseTest
    {
        protected override IEnumerable<IEnumerable<T>> Batch<T>(IEnumerable<T> source, int size) =>
            source.Batch(size);

        [Test]
        public void BatchSequenceYieldsListsOfBatches()
        {
            var result = new[] { 1, 2, 3 }.Batch(2);

            using var reader = result.Read();
            Assert.That(reader.Read(), Is.InstanceOf(typeof(IList<int>)));
            Assert.That(reader.Read(), Is.InstanceOf(typeof(IList<int>)));
            reader.ReadEnd();
        }

        [Test]
        public void BatchSequenceTransformingResult()
        {
            var result = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Batch(4, batch => batch.Sum());
            result.AssertSequenceEqual(10, 26, 9);
        }
    }

    public abstract class BatchBaseTest
    {
        protected abstract IEnumerable<IEnumerable<T>> Batch<T>(IEnumerable<T> source, int size);

        [Test]
        public void BatchZeroSize()
        {
            AssertThrowsArgument.OutOfRangeException("size",() =>
                Batch(new object[0], 0));
        }

        [Test]
        public void BatchNegativeSize()
        {
            AssertThrowsArgument.OutOfRangeException("size",() =>
                Batch(new object[0], -1));
        }

        [Test]
        public void BatchEvenlyDivisibleSequence()
        {
            var result = Batch(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 3);

            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(1, 2, 3);
            reader.Read().AssertSequenceEqual(4, 5, 6);
            reader.Read().AssertSequenceEqual(7, 8, 9);
            reader.ReadEnd();
        }

        [Test]
        public void BatchUnevenlyDivisibleSequence()
        {
            var result = Batch(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 4);

            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(1, 2, 3, 4);
            reader.Read().AssertSequenceEqual(5, 6, 7, 8);
            reader.Read().AssertSequenceEqual(9);
            reader.ReadEnd();
        }

        [Test]
        public void BatchSequencesAreIndependentInstances()
        {
            var result = Batch(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 4);

            using var reader = result.Read();
            var first = reader.Read();
            var second = reader.Read();
            var third = reader.Read();
            reader.ReadEnd();

            first.AssertSequenceEqual(1, 2, 3, 4);
            second.AssertSequenceEqual(5, 6, 7, 8);
            third.AssertSequenceEqual(9);
        }

        [Test]
        public void BatchIsLazy()
        {
            new BreakingSequence<object>().Batch(1);
        }

        [TestCase(SourceKind.BreakingCollection  , 0)]
        [TestCase(SourceKind.BreakingList        , 0)]
        [TestCase(SourceKind.BreakingReadOnlyList, 0)]
        [TestCase(SourceKind.BreakingCollection  , 1)]
        [TestCase(SourceKind.BreakingList        , 1)]
        [TestCase(SourceKind.BreakingReadOnlyList, 1)]
        [TestCase(SourceKind.BreakingCollection  , 2)]
        [TestCase(SourceKind.BreakingList        , 2)]
        [TestCase(SourceKind.BreakingReadOnlyList, 2)]
        public void BatchCollectionSmallerThanSize(SourceKind kind, int oversize)
        {
            var xs = new[] { 1, 2, 3, 4, 5 };
            var result = xs.ToSourceKind(kind).Batch(xs.Length + oversize);
            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(1, 2, 3, 4, 5);
            reader.ReadEnd();
        }

        [Test]
        public void BatchReadOnlyCollectionSmallerThanSize()
        {
            var collection = ReadOnlyCollection.From(1, 2, 3, 4, 5);
            var result = collection.Batch(collection.Count * 2);
            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(1, 2, 3, 4, 5);
            reader.ReadEnd();
        }

        [TestCase(SourceKind.Sequence)]
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        [TestCase(SourceKind.BreakingReadOnlyCollection)]
        [TestCase(SourceKind.BreakingCollection)]
        public void BatchEmptySource(SourceKind kind)
        {
            var batches = Enumerable.Empty<int>().ToSourceKind(kind).Batch(100);
            Assert.That(batches, Is.Empty);
        }
    }
}

#if NETCOREAPP3_1_OR_GREATER

namespace MoreLinq.Test
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using MoreLinq.Experimental;
    using NUnit.Framework;

    [TestFixture]
    public class BatchArrayPoolTest : BatchBaseTest
    {
        protected override IEnumerable<IEnumerable<T>> Batch<T>(IEnumerable<T> source, int size) =>
            from b in source.Batch(size, ArrayPool<T>.Shared)
            select b.Bucket.Take(b.Length);

        [Test]
        public void BatchReturnsNewArraysWhenUnreturnedToPool()
        {
            var pairs = Enumerable.Range(1, 100)
                                  .Batch(13, ArrayPool<int>.Shared)
                                  .Pairwise(ValueTuple.Create);

            foreach (var ((prev, _), (curr, _)) in pairs)
                Assert.That(curr, Is.Not.SameAs(prev));
        }

        [Test]
        public void BatchReusesReturnedArraysFromPool()
        {
            int[] previousBucket = null;
            var pool = ArrayPool<int>.Shared;

            foreach (var (bucket, _) in Enumerable.Range(1, 100)
                                                  .Batch(13, pool))
            {
                if (previousBucket is { } somePreviousBucket)
                    Assert.That(bucket, Is.SameAs(somePreviousBucket));

                previousBucket = bucket;

                pool.Return(bucket);
            }
        }
    }

    public class BatchMemoryPoolTest : BatchBaseTest
    {
        protected override IEnumerable<IEnumerable<T>> Batch<T>(IEnumerable<T> source, int size) =>
            from b in source.Batch(size, MemoryPool<T>.Shared)
            select Elements(b.Bucket.Memory).Take(b.Length);

        static IEnumerable<T> Elements<T>(Memory<T> memory)
        {
            for (var i = 0; i < memory.Length; i++)
                yield return memory.Span[i];
        }

        [TestCase(false)]
        [TestCase(true)]
        public void BatchMemoryReuseFromPool(bool disposeBucket)
        {
            foreach (var (i, (bucket, _)) in Enumerable.Range(1, 100)
                                                       .Batch(13, MemoryPool<int>.Shared)
                                                       .Index())
            {
                var memory = bucket.Memory;

                Assert.That(memory.Length, Is.GreaterThan(13));

                if (i is 0)
                    memory.Span.Fill(42);
                else
                    Assert.That(memory.Span[^1], Is.EqualTo(disposeBucket ? 42 : 0));

                if (disposeBucket)
                    bucket.Dispose();
            }
        }
    }
}

#endif
