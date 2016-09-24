#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Leandro F. Vieira (leandromoh). All rights reserved.
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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class CountBetweenTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CountBetweenWithNullSequence()
        {
            IEnumerable<int> sequence = null;
            sequence.CountBetween(1, 2);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CountBetweenWithNegativeMin()
        {
            new[] { 1 }.CountBetween(-1, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CountBetweenWithNegativeMax()
        {
            new[] { 1 }.CountBetween(0, -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CountBetweenWithMaxLesserThanMin()
        {
            new[] { 1 }.CountBetween(1, 0);
        }

        [Test]
        public void CountBetweenWithMaxEqualsMin()
        {
            Assert.IsTrue(new[] { 1 }.CountBetween(1, 1));
        }

        [Test]
        public void CountBetweenRangeTests()
        {
            Assert.IsFalse(Enumerable.Range(1, 1).CountBetween(2, 4));

            Assert.IsTrue(Enumerable.Range(1, 2).CountBetween(2, 4));
            Assert.IsTrue(Enumerable.Range(1, 3).CountBetween(2, 4));
            Assert.IsTrue(Enumerable.Range(1, 4).CountBetween(2, 4));

            Assert.IsFalse(Enumerable.Range(1, 5).CountBetween(2, 4));
        }

    }
}
