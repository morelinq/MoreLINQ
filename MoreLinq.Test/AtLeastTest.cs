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
    using System.Collections.Generic;
    using System;

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
            Enumerable.Empty<int>().AssertOptimizedForCollections(xs =>
                Assert.IsTrue(xs.AtLeast(0)));
        }

        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastOneElement()
        {
            Enumerable.Empty<int>().AssertOptimizedForCollections(xs =>
                Assert.IsFalse(xs.AtLeast(1)));
        }

        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastManyElements()
        {
            Enumerable.Empty<int>().AssertOptimizedForCollections(xs =>
                Assert.IsFalse(xs.AtLeast(2)));
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastZeroElements()
        {
            new[] { 1 }.AssertOptimizedForCollections(xs =>
                Assert.IsTrue(xs.AtLeast(0)));
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastOneElement()
        {
            new[] { 1 }.AssertOptimizedForCollections(xs =>
                Assert.IsTrue(xs.AtLeast(1)));
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastManyElements()
        {
            new[] { 1 }.AssertOptimizedForCollections(xs =>
                Assert.IsFalse(xs.AtLeast(2)));
        }

        [Test]
        public void AtLeastWithManyElementsHasAtLeastZeroElements()
        {
            new[] { 1, 2, 3 }.AssertOptimizedForCollections(xs =>
                Assert.IsTrue(xs.AtLeast(0)));
        }

        [Test]
        public void AtLeastWithManyElementsHasAtLeastOneElement()
        {
            new[] { 1, 2, 3 }.AssertOptimizedForCollections(xs =>
                Assert.IsTrue(xs.AtLeast(1)));
        }

        [Test]
        public void AtLeastWithManyElementsHasAtLeastManyElements()
        {
            new[] { 1, 2, 3 }.AssertOptimizedForCollections(xs =>
                Assert.IsTrue(xs.AtLeast(2)));
        }
    }
}
