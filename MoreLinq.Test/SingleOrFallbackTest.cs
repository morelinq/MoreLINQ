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

#pragma warning disable 618 // TODO SingleOrFallback is obsolete

namespace MoreLinq.Test
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class SingleOrFallbackTest
    {
        [Test]
        public void SingleOrFallbackWithEmptySequence()
        {
            Assert.AreEqual(5, Enumerable.Empty<int>().Select(x => x).SingleOrFallback(() => 5));
        }
        [Test]
        public void SingleOrFallbackWithEmptySequenceIListOptimized()
        {
            Assert.AreEqual(5, Enumerable.Empty<int>().SingleOrFallback(() => 5));
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
        public void SingleOrFallbackWithLongSequence()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new[] { 10, 20, 30 }.Select(x => x).SingleOrFallback(BreakingFunc.Of<int>()));
        }

        [Test]
        public void SingleOrFallbackWithLongSequenceIListOptimized()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new[] { 10, 20, 30 }.SingleOrFallback(BreakingFunc.Of<int>()));
        }
    }
}
