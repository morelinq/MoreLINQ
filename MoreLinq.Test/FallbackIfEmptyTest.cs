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
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class FallbackIfEmptyTest
    {
        [Test]
        public void FallbackIfEmptyWithEmptySequence()
        {
            var source = Enumerable.Empty<int>().Select(x => x);
            // ReSharper disable PossibleMultipleEnumeration
            source.FallbackIfEmpty(12).AssertSequenceEqual(12);
            source.FallbackIfEmpty(12, 23).AssertSequenceEqual(12, 23);
            source.FallbackIfEmpty(12, 23, 34).AssertSequenceEqual(12, 23, 34);
            source.FallbackIfEmpty(12, 23, 34, 45).AssertSequenceEqual(12, 23, 34, 45);
            source.FallbackIfEmpty(12, 23, 34, 45, 56).AssertSequenceEqual(12, 23, 34, 45, 56);
            source.FallbackIfEmpty(12, 23, 34, 45, 56, 67).AssertSequenceEqual(12, 23, 34, 45, 56, 67);
            // ReSharper restore PossibleMultipleEnumeration
        }

        [TestCase(false)]
        [TestCase(true)]
        public void FallbackIfEmptyPreservesSourceCollectionIfPossible(bool readOnly)
        {
            var source = readOnly
                       ? new UnenumerableReadOnlyList<int>(new[] { 1 })
                       : (IEnumerable<int>)new UnenumerableList<int> { 1 };
            // ReSharper disable PossibleMultipleEnumeration
            Assert.AreSame(source.FallbackIfEmpty(12), source);
            Assert.AreSame(source.FallbackIfEmpty(12, 23), source);
            Assert.AreSame(source.FallbackIfEmpty(12, 23, 34), source);
            Assert.AreSame(source.FallbackIfEmpty(12, 23, 34, 45), source);
            Assert.AreSame(source.FallbackIfEmpty(12, 23, 34, 45, 56), source);
            Assert.AreSame(source.FallbackIfEmpty(12, 23, 34, 45, 56, 67), source);
            // ReSharper restore PossibleMultipleEnumeration
        }

        [TestCase(false)]
        [TestCase(true)]
        public void FallbackIfEmptyPreservesFallbackCollectionIfPossible(bool readOnly)
        {
            var source = readOnly
                       ? new UnenumerableReadOnlyList<int>(new int[0])
                       : (IEnumerable<int>)new UnenumerableList<int>();
            var fallback = new[] { 1 };
            Assert.AreSame(source.FallbackIfEmpty(fallback), fallback);
            Assert.AreSame(source.FallbackIfEmpty(fallback.AsEnumerable()), fallback);
        }
    }
}
