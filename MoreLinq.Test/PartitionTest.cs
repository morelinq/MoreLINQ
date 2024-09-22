#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2010 Leopold Bushkin. All rights reserved.
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
    using Tuple = System.ValueTuple;

    [TestFixture]
    public class PartitionTest
    {
        [Test]
        public void Partition()
        {
            var (evens, odds) =
                Enumerable.Range(0, 10)
                          .Partition(x => x % 2 == 0);

            Assert.That(evens, Is.EqualTo(new[] { 0, 2, 4, 6, 8 }));
            Assert.That(odds,  Is.EqualTo(new[] { 1, 3, 5, 7, 9 }));
        }

        [Test]
        public void PartitionWithEmptySequence()
        {
            var (evens, odds) =
                Enumerable.Empty<int>()
                          .Partition(x => x % 2 == 0);

            Assert.That(evens, Is.Empty);
            Assert.That(odds,  Is.Empty);
        }

        [Test]
        public void PartitionWithResultSelector()
        {
            var (evens, odds) =
                Enumerable.Range(0, 10)
                          .Partition(x => x % 2 == 0, Tuple.Create);

            Assert.That(evens, Is.EqualTo(new[] { 0, 2, 4, 6, 8 }));
            Assert.That(odds,  Is.EqualTo(new[] { 1, 3, 5, 7, 9 }));
        }

        [Test]
        public void PartitionBooleanGrouping()
        {
            var (evens, odds) =
                Enumerable.Range(0, 10)
                          .GroupBy(x => x % 2 == 0)
                          .Partition((t, f) => Tuple.Create(t, f));

            Assert.That(evens, Is.EqualTo(new[] { 0, 2, 4, 6, 8 }));
            Assert.That(odds,  Is.EqualTo(new[] { 1, 3, 5, 7, 9 }));
        }

        [Test]
        public void PartitionNullableBooleanGrouping()
        {
            var xs = new int?[] { 1, 2, 3, null, 5, 6, 7, null, 9, 10 };

            var (lt5, gte5, nils) =
                xs.GroupBy(x => x != null ? x < 5 : (bool?)null)
                  .Partition((t, f, n) => Tuple.Create(t, f, n));

            Assert.That(lt5,  Is.EqualTo(new[] { 1, 2, 3 }));
            Assert.That(gte5, Is.EqualTo(new[] { 5, 6, 7, 9, 10 }));
            Assert.That(nils, Is.EqualTo(new int?[] { null, null }));
        }

        [Test]
        public void PartitionBooleanGroupingWithSingleKey()
        {
            var (m3, etc) =
                Enumerable.Range(0, 10)
                          .GroupBy(x => x % 3)
                          .Partition(0, Tuple.Create);

            Assert.That(m3, Is.EqualTo(new[] { 0, 3, 6, 9 }));

            using var r = etc.Read();
            var r1 = r.Read();
            Assert.That(r1.Key, Is.EqualTo(1));
            Assert.That(r1, Is.EqualTo(new[] { 1, 4, 7 }));

            var r2 = r.Read();
            Assert.That(r2.Key, Is.EqualTo(2));
            Assert.That(r2, Is.EqualTo(new[] { 2, 5, 8 }));

            r.ReadEnd();
        }

        [Test]
        public void PartitionBooleanGroupingWitTwoKeys()
        {
            var (ms, r1, etc) =
                Enumerable.Range(0, 10)
                          .GroupBy(x => x % 3)
                          .Partition(0, 1, Tuple.Create);

            Assert.That(ms, Is.EqualTo(new[] { 0, 3, 6, 9 }));
            Assert.That(r1, Is.EqualTo(new[] { 1, 4, 7 }));

            using var r = etc.Read();
            var r2 = r.Read();
            Assert.That(r2.Key, Is.EqualTo(2));
            Assert.That(r2, Is.EqualTo(new[] { 2, 5, 8 }));
            r.ReadEnd();
        }

        [Test]
        public void PartitionBooleanGroupingWitThreeKeys()
        {
            var (ms, r1, r2, etc) =
                Enumerable.Range(0, 10)
                    .GroupBy(x => x % 3)
                    .Partition(0, 1, 2, Tuple.Create);

            Assert.That(ms, Is.EqualTo(new[] { 0, 3, 6, 9 }));
            Assert.That(r1, Is.EqualTo(new[] { 1, 4, 7 }));
            Assert.That(r2, Is.EqualTo(new[] { 2, 5, 8 }));
            Assert.That(etc, Is.Empty);
        }

        [Test]
        public void PartitionBooleanGroupingWithSingleKeyWithComparer()
        {
            var words =
                new[] { "foo", "bar", "FOO", "Bar" };

            var (foo, etc) =
                words.GroupBy(s => s, StringComparer.OrdinalIgnoreCase)
                    .Partition("foo", StringComparer.OrdinalIgnoreCase, Tuple.Create);

            Assert.That(foo, Is.EqualTo(new[] { "foo", "FOO" }));

            using var r = etc.Read();
            var bar = r.Read();
            Assert.That(bar, Is.EqualTo(new[] { "bar", "Bar" }));
            r.ReadEnd();
        }

        [Test]
        public void PartitionBooleanGroupingWithTwoKeysWithComparer()
        {
            var words =
                new[] { "foo", "bar", "FOO", "Bar", "baz", "QUx", "bAz", "QuX" };

            var (foos, bar, etc) =
                words.GroupBy(s => s, StringComparer.OrdinalIgnoreCase)
                     .Partition("foo", "bar", StringComparer.OrdinalIgnoreCase, Tuple.Create);

            Assert.That(foos, Is.EqualTo(new[] { "foo", "FOO" }));
            Assert.That(bar, Is.EqualTo(new[] { "bar", "Bar" }));

            using var r = etc.Read();
            var baz = r.Read();
            Assert.That(baz.Key, Is.EqualTo("baz"));
            Assert.That(baz, Is.EqualTo(new[] { "baz", "bAz" }));

            var qux = r.Read();
            Assert.That(qux.Key, Is.EqualTo("QUx"));
            Assert.That(qux, Is.EqualTo(new[] { "QUx", "QuX" }));

            r.ReadEnd();
        }

        [Test]
        public void PartitionBooleanGroupingWithThreeKeysWithComparer()
        {
            var words =
                new[] { "foo", "bar", "FOO", "Bar", "baz", "QUx", "bAz", "QuX" };

            var (foos, bar, baz, etc) =
                words.GroupBy(s => s, StringComparer.OrdinalIgnoreCase)
                    .Partition("foo", "bar", "baz", StringComparer.OrdinalIgnoreCase, Tuple.Create);

            Assert.That(foos, Is.EqualTo(new[] { "foo", "FOO" }));
            Assert.That(bar, Is.EqualTo(new[] { "bar", "Bar" }));
            Assert.That(baz, Is.EqualTo(new[] { "baz", "bAz" }));

            using var r = etc.Read();
            var qux = r.Read();
            Assert.That(qux.Key, Is.EqualTo("QUx"));
            Assert.That(qux, Is.EqualTo(new[] { "QUx", "QuX" }));
            r.ReadEnd();
        }
    }
}
