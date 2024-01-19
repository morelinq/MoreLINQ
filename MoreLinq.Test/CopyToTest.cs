#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2009 Atif Aziz. All rights reserved.
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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using NUnit.Framework;

    [TestFixture]
    public class CopyToTest
    {
        [Test]
        public void ThrowsOnNegativeIndex()
        {
            Assert.That(
                () => new BreakingSequence<int>().CopyTo(Array.Empty<int>(), -1),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ThrowsOnTooMuchDataForArray()
        {
            Assert.That(
                () => MoreEnumerable.From(() => 1).CopyTo(Array.Empty<int>()),
                Throws.TypeOf<ArgumentException>());
            Assert.That(
                () => Enumerable.Range(1, 1).CopyTo(Array.Empty<int>()),
                Throws.TypeOf<ArgumentException>());
            Assert.That(
                () => new List<int> { 1 }.AsEnumerable().CopyTo(Array.Empty<int>()),
                Throws.TypeOf<ArgumentException>());
            Assert.That(
                () => new List<int> { 1 }.AsReadOnly().AsEnumerable().CopyTo(Array.Empty<int>()),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ThrowsOnTooMuchDataForIListArray()
        {
            Assert.That(
                () => MoreEnumerable.From(() => 1).CopyTo((IList<int>)Array.Empty<int>()),
                Throws.TypeOf<ArgumentException>());
            Assert.That(
                () => Enumerable.Range(1, 1).CopyTo((IList<int>)Array.Empty<int>()),
                Throws.TypeOf<ArgumentException>());
            Assert.That(
                () => new List<int> { 1 }.AsEnumerable().CopyTo((IList<int>)Array.Empty<int>()),
                Throws.TypeOf<ArgumentException>());
            Assert.That(
                () => new List<int> { 1 }.AsReadOnly().AsEnumerable().CopyTo((IList<int>)Array.Empty<int>()),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void CopiesDataToArray()
        {
            var array = new int[4];

            using var xs = TestingSequence.Of(1);
            var cnt = xs.CopyTo(array);
            array.AssertSequenceEqual(1, 0, 0, 0);
            Assert.That(cnt, Is.EqualTo(1));

            cnt = new List<int> { 2 }.AsEnumerable().CopyTo(array, 1);
            array.AssertSequenceEqual(1, 2, 0, 0);
            Assert.That(cnt, Is.EqualTo(1));

            cnt = Enumerable.Range(3, 1).CopyTo(array, 2);
            array.AssertSequenceEqual(1, 2, 3, 0);
            Assert.That(cnt, Is.EqualTo(1));

            cnt = new[] { 4, }.AsEnumerable().CopyTo(array, 3);
            array.AssertSequenceEqual(1, 2, 3, 4);
            Assert.That(cnt, Is.EqualTo(1));
        }

        [Test]
        public void CopiesDataToList()
        {
            var list = new List<int>();

            var cnt = new[] { 1, }.AsEnumerable().CopyTo(list);
            list.AssertSequenceEqual(1);
            Assert.That(cnt, Is.EqualTo(1));

            using (var xs = TestingSequence.Of(2))
            {
                cnt = xs.CopyTo(list, 1);
                list.AssertSequenceEqual(1, 2);
                Assert.That(cnt, Is.EqualTo(1));
            }

            using (var xs = TestingSequence.Of(3, 4))
            {
                cnt = xs.AsEnumerable().CopyTo(list, 1);
                list.AssertSequenceEqual(1, 3, 4);
                Assert.That(cnt, Is.EqualTo(2));
            }
        }

        [Test]
        public void CopiesDataToIList()
        {
            var list = new Collection<int>();

            using (var xs = TestingSequence.Of(1))
            {
                var cnt = xs.CopyTo(list);
                list.AssertSequenceEqual(1);
                Assert.That(cnt, Is.EqualTo(1));
            }

            using (var xs = TestingSequence.Of(2))
            {
                var cnt = xs.CopyTo(list, 1);
                list.AssertSequenceEqual(1, 2);
                Assert.That(cnt, Is.EqualTo(1));
            }

            using (var xs = TestingSequence.Of(3, 4))
            {
                var cnt = xs.CopyTo(list, 1);
                list.AssertSequenceEqual(1, 3, 4);
                Assert.That(cnt, Is.EqualTo(2));
            }
        }
    }
}
