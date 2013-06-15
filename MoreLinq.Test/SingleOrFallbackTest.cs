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
using System.Linq;
using NUnit.Framework;
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class SingleOrFallbackTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleOrFallbackWithNullSequence()
        {
            MoreEnumerable.SingleOrFallback(null, BreakingFunc.Of<int>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SingleOrFallbackWithNullFallback()
        {
            new[] {1}.SingleOrFallback(null);
        }

        [Test]
        public void SingleOrFallbackWithEmptySequence()
        {
            Assert.AreEqual(5, LinqEnumerable.Empty<int>().Select(x => x).SingleOrFallback(() => 5));
        }
        [Test]
        public void SingleOrFallbackWithEmptySequenceIListOptimized()
        {
            Assert.AreEqual(5, LinqEnumerable.Empty<int>().SingleOrFallback(() => 5));
        }

        [Test]
        public void SingleOrFallbackWithSingleElementSequence()
        {
            Assert.AreEqual(10, new[]{10}.Select(x => x).SingleOrFallback(BreakingFunc.Of<int>()));
        }
        [Test]
        public void SingleOrFallbackWithSingleElementSequenceIListOptimized()
        {
            Assert.AreEqual(10, new[] { 10 }.SingleOrFallback(BreakingFunc.Of<int>()));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SingleOrFallbackWithLongSequence()
        {
            new[] { 10, 20, 30 }.Select(x => x).SingleOrFallback(BreakingFunc.Of<int>());
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SingleOrFallbackWithLongSequenceIListOptimized()
        {
            new[] { 10, 20, 30 }.SingleOrFallback(BreakingFunc.Of<int>());
        }
    }
}
