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
    using NUnit.Framework;

    [TestFixture]
    public class AtLeastTest
    {
        [Test]
        public void AtLeastWithNegativeCount()
        {
            AssertThrowsArgument.OutOfRangeException("count", () =>
                new[] { 1 }.AtLeast(-1));
        }

        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastZeroElements()
        {
            foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionTestCases())
                Assert.IsTrue(xs.AtLeast(0));
        }

        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastOneElement()
        {
            foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionTestCases())
                Assert.IsFalse(xs.AtLeast(1));
        }

        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastManyElements()
        {
            foreach (var xs in Enumerable.Empty<int>().ArrangeCollectionTestCases())
                Assert.IsFalse(xs.AtLeast(2));
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastZeroElements()
        {
            foreach (var xs in new[] { 1 }.ArrangeCollectionTestCases())
                Assert.IsTrue(xs.AtLeast(0));
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastOneElement()
        {
            foreach (var xs in new[] { 1 }.ArrangeCollectionTestCases())
                Assert.IsTrue(xs.AtLeast(1));
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastManyElements()
        {
            foreach (var xs in new[] { 1 }.ArrangeCollectionTestCases())
                Assert.IsFalse(xs.AtLeast(2));
        }

        [Test]
        public void AtLeastWithManyElementsHasAtLeastZeroElements()
        {
            foreach (var xs in new[] { 1, 2, 3 }.ArrangeCollectionTestCases())
                Assert.IsTrue(xs.AtLeast(0));
        }

        [Test]
        public void AtLeastWithManyElementsHasAtLeastOneElement()
        {
            foreach (var xs in new[] { 1, 2, 3 }.ArrangeCollectionTestCases())
                Assert.IsTrue(xs.AtLeast(1));
        }

        [Test]
        public void AtLeastWithManyElementsHasAtLeastManyElements()
        {
            foreach (var xs in new[] { 1, 2, 3 }.ArrangeCollectionTestCases())
                Assert.IsTrue(xs.AtLeast(2));
        }

        [Test]
        public void AtLeastDoesNotIterateUnnecessaryElements()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => 2,
                                             () => throw new TestException());
            Assert.IsTrue(source.AtLeast(2));
        }
    }
}
