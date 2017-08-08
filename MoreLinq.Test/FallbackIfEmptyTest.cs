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
    using System;
    using System.Linq;
    using NUnit.Framework;
    using LinqEnumerable = System.Linq.Enumerable;

    [TestFixture]
    public class FallbackIfEmptyTest
    {
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

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void FallbackIfEmptyPreservesSourceCollectionIfPossible(int length)
        {
            var source = new int[] { 1 };
            // ReSharper disable PossibleMultipleEnumeration
            switch (length) {
            case 1: Assert.AreSame(source.FallbackIfEmpty(12), source); break;
            case 2: Assert.AreSame(source.FallbackIfEmpty(12, 23), source); break;
            case 3: Assert.AreSame(source.FallbackIfEmpty(12, 23, 34), source); break;
            case 4: Assert.AreSame(source.FallbackIfEmpty(12, 23, 34, 45), source); break;
            case 5: Assert.AreSame(source.FallbackIfEmpty(12, 23, 34, 45, 56), source); break;
            case 6: Assert.AreSame(source.FallbackIfEmpty(12, 23, 34, 45, 56, 67), source); break;
            default:
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            // ReSharper restore PossibleMultipleEnumeration
        }

        [Test]
        public void FallbackIfEmptyPreservesFallbackCollectionIfPossible()
        {
            var source = new int[0];
            var fallback = new int[] { 1 };
            Assert.AreSame(source.FallbackIfEmpty(fallback), fallback);
            Assert.AreSame(source.FallbackIfEmpty(fallback.AsEnumerable()), fallback);
        }

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
