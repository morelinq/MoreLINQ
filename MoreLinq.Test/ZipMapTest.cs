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
    using System.Text.RegularExpressions;
    using NUnit.Framework;

    [TestFixture]
    public class ZipMapTest
    {
        [Test]
        public void ZipMapIsLazy()
        {
            _ = new BreakingSequence<object>().ZipMap(BreakingFunc.Of<object, object>());
        }

        [Test]
        public void ZipMapEmptySequence()
        {
            using var objects = Enumerable.Empty<object>().AsTestingSequence();
            var result = objects.ZipMap(BreakingFunc.Of<object, object>());
            result.AssertSequenceEqual();
        }

        [Test]
        public void ZipMapStrings()
        {
            using var strings = TestingSequence.Of("foo", "bar", "baz");
            var result = strings.ZipMap(s => Regex.IsMatch(s, @"^b"));
            result.AssertSequenceEqual(("foo", false), ("bar", true), ("baz", true));
        }

        [Test]
        public void ZipMapFromSequence()
        {
            var result = MoreEnumerable.Sequence(5, 8).ZipMap(i => i % 2 == 0);
            result.AssertSequenceEqual((5, false), (6, true), (7, false), (8, true));
        }
    }
}
