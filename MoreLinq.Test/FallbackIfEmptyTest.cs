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

using System.Linq;
using NUnit.Framework;
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class FallbackIfEmptyTest
    {
        [Test]
        public void FallbackIfEmptyWithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source", () => MoreEnumerable.FallbackIfEmpty(null, 1));
            Assert.ThrowsArgumentNullException("source", () => MoreEnumerable.FallbackIfEmpty(null, 1, 2));
            Assert.ThrowsArgumentNullException("source", () => MoreEnumerable.FallbackIfEmpty(null, 1, 2, 3));
            Assert.ThrowsArgumentNullException("source", () => MoreEnumerable.FallbackIfEmpty(null, 1, 2, 3, 4));
            Assert.ThrowsArgumentNullException("source", () => MoreEnumerable.FallbackIfEmpty(null, 1, 2, 3, 4, 5));
        }

        [Test]
        public void FallbackIfEmptyWithNullFallbackParams()
        {
           Assert.ThrowsArgumentNullException("fallback", () => new[] { 1 }.FallbackIfEmpty(null));
        }

        [Test]
        public void FallbackIfEmptyWithEmptySequence()
        {
            var source = LinqEnumerable.Empty<int>().Select(x => x);
            // ReSharper disable PossibleMultipleEnumeration
            source.FallbackIfEmpty(12).AssertSequenceEqual(12);
            source.FallbackIfEmpty(12, 23).AssertSequenceEqual(12, 23);
            source.FallbackIfEmpty(12, 23, 34).AssertSequenceEqual(12, 23, 34);
            source.FallbackIfEmpty(12, 23, 34, 45).AssertSequenceEqual(12, 23, 34, 45);
            source.FallbackIfEmpty(12, 23, 34, 45, 56).AssertSequenceEqual(12, 23, 34, 45, 56);
            source.FallbackIfEmpty(12, 23, 34, 45, 56, 67).AssertSequenceEqual(12, 23, 34, 45, 56, 67);
            // ReSharper restore PossibleMultipleEnumeration
        }

        [Test]
        public void FallbackIfEmptyWithEmptySequenceCollectionOptimized()
        {
            var source = LinqEnumerable.Empty<int>();
            // ReSharper disable PossibleMultipleEnumeration
            source.FallbackIfEmpty(12).AssertSequenceEqual(12);
            source.FallbackIfEmpty(12, 23).AssertSequenceEqual(12, 23);
            source.FallbackIfEmpty(12, 23, 34).AssertSequenceEqual(12, 23, 34);
            source.FallbackIfEmpty(12, 23, 34, 45).AssertSequenceEqual(12, 23, 34, 45);
            source.FallbackIfEmpty(12, 23, 34, 45, 56).AssertSequenceEqual(12, 23, 34, 45, 56);
            source.FallbackIfEmpty(12, 23, 34, 45, 56, 67).AssertSequenceEqual(12, 23, 34, 45, 56, 67);
            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}
