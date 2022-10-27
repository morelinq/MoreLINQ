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

            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(1, 2, 3);
            reader.Read().AssertSequenceEqual(4, 5, 6);
            reader.Read().AssertSequenceEqual(7, 8, 9);
            reader.ReadEnd();
        }

        [Test]
        public void BatchUnevenlyDivisibleSequence()
        {
            var result = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Batch(4);

            using var reader = result.Read();
            reader.Read().AssertSequenceEqual(1, 2, 3, 4);
            reader.Read().AssertSequenceEqual(5, 6, 7, 8);
            reader.Read().AssertSequenceEqual(9);
            reader.ReadEnd();
        }

        [Test]
        public void BatchSequenceTransformingResult()
        {
            var result = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Batch(4, batch => batch.Sum());
            result.AssertSequenceEqual(10, 26, 9);
        }

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
        public void BatchSequencesAreIndependentInstances()
        {
            var result = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Batch(4);

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
    public abstract class BatchPoolTest
    {
        protected abstract IListView<T> Batch<T>(IEnumerable<T> source, int size);

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

        void AssertNext<T>(IListView<T> bucket, params T[] items)
        {
            Assert.That(bucket.MoveNext(), Is.True);

            Assert.That(bucket.Count, Is.EqualTo(items.Length));

            foreach (var (i, item) in items.Index())
            {
                Assert.That(bucket.Contains(item));
                Assert.That(bucket.IndexOf(item), Is.EqualTo(i));
            }

            bucket.AssertSequenceEqual(items);
            bucket.AsSpan().ToArray().SequenceEqual(items);
        }

        [Test]
        public void BatchEvenlyDivisibleSequence()
        {
            using var result = Batch(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 3);

            AssertNext(result, 1, 2, 3);
            AssertNext(result, 4, 5, 6);
            AssertNext(result, 7, 8, 9);
            Assert.That(result.MoveNext(), Is.False);
        }

        [Test]
        public void BatchUnevenlyDivisibleSequence()
        {
            using var result = Batch(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 4);

            AssertNext(result, 1, 2, 3, 4);
            AssertNext(result, 5, 6, 7, 8);
            AssertNext(result, 9);
            Assert.That(result.MoveNext(), Is.False);
        }

        [Test]
        public void BatchIsLazy()
        {
            new BreakingSequence<object>().Batch(1);
        }

        [TestCase(SourceKind.BreakingList        , 0)]
        [TestCase(SourceKind.BreakingReadOnlyList, 0)]
        [TestCase(SourceKind.BreakingList        , 1)]
        [TestCase(SourceKind.BreakingReadOnlyList, 1)]
        [TestCase(SourceKind.BreakingList        , 2)]
        [TestCase(SourceKind.BreakingReadOnlyList, 2)]
        public void BatchCollectionSmallerThanSize(SourceKind kind, int oversize)
        {
            var xs = new[] { 1, 2, 3, 4, 5 };
            using var result = Batch(xs.ToSourceKind(kind), xs.Length + oversize);

            AssertNext(result, 1, 2, 3, 4, 5);
            Assert.That(result.MoveNext(), Is.False);
        }

        [Test]
        public void BatchReadOnlyCollectionSmallerThanSize()
        {
            var collection = ReadOnlyCollection.From(1, 2, 3, 4, 5);
            using var result = Batch(collection, collection.Count * 2);
            Assert.That(result.MoveNext(), Is.True);
            Assert.That(result.Count, Is.EqualTo(5));
            result.AssertSequenceEqual(1, 2, 3, 4, 5);
            Assert.That(result.MoveNext(), Is.False);
        }

        [TestCase(SourceKind.Sequence)]
        [TestCase(SourceKind.BreakingList)]
        [TestCase(SourceKind.BreakingReadOnlyList)]
        [TestCase(SourceKind.BreakingReadOnlyCollection)]
        [TestCase(SourceKind.BreakingCollection)]
        public void BatchEmptySource(SourceKind kind)
        {
            using var result = Batch(Enumerable.Empty<int>().ToSourceKind(kind), 100);
            Assert.That(result.MoveNext(), Is.False);
        }
    }

    public class BatchPooledArrayTest : BatchPoolTest
    {
        protected override IListView<T> Batch<T>(IEnumerable<T> source, int size) =>
            source.Batch(size, ArrayPool<T>.Create());
    }

    public class BatchPooledMemoryTest : BatchPoolTest
    {
        protected override IListView<T> Batch<T>(IEnumerable<T> source, int size) =>
            source.Batch(size, MemoryPool<T>.Shared);
    }
}

#endif // NETCOREAPP3_1_OR_GREATER
