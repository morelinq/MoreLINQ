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

using System;
using System.Collections.Generic;
using NUnit.Framework;
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class AtLeastTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AtLeastWithNullSequence()
        {
            IEnumerable<int> sequence = null;
            MoreEnumerable.AtLeast(sequence, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AtLeastWithNegativeCount()
        {
            new[] { 1 }.AtLeast(-1);
        }

        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastZeroElements()
        {
            Assert.That(LinqEnumerable.Empty<int>().AtLeast(0));
        }
        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastOneElement()
        {
            Assert.That(!LinqEnumerable.Empty<int>().AtLeast(1));
        }
        [Test]
        public void AtLeastWithEmptySequenceHasAtLeastManyElements()
        {
            Assert.That(!LinqEnumerable.Empty<int>().AtLeast(2));
        }

        [Test]
        public void AtLeastWithSingleElementHasAtLeastZeroElements()
        {
            Assert.That(new[] { 1 }.AtLeast(0));
        }
        [Test]
        public void AtLeastWithSingleElementHasAtLeastOneElement()
        {
            Assert.That(new [] { 1 }.AtLeast(1));
        }
        [Test]
        public void AtLeastWithSingleElementHasAtLeastManyElements()
        {
            Assert.That(!new [] { 1 }.AtLeast(2));
        }

        [Test]
        public void AtLeastWithManyElementsHasAtLeastZeroElements()
        {
            Assert.That(new [] { 10, 20, 30 }.AtLeast(0));
        }
        [Test]
        public void AtLeastWithManyElementsHasAtLeastOneElement()
        {
            Assert.That(new [] { 10, 20, 30 }.AtLeast(1));
        }
        [Test]
        public void AtLeastWithManyElementsHasAtLeastManyElements()
        {
            Assert.That(new [] { 10, 20, 30 }.AtLeast(2));
        }
    }
}
