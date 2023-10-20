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
    using NUnit.Framework;
    using static MoreLinq.Extensions.DistinctByExtension;

    [TestFixture]
    public class DistinctByTest
    {
        [Test]
        public void DistinctBy()
        {
            string[] source = { "first", "second", "third", "fourth", "fifth" };
            var distinct = source.DistinctBy(word => word.Length);
            distinct.AssertSequenceEqual("first", "second");
        }

        [Test]
        public void DistinctByIsLazy()
        {
            _ = new BreakingSequence<string>().DistinctBy(BreakingFunc.Of<string, int>());
        }

        [Test]
        public void DistinctByWithComparer()
        {
            string[] source = { "first", "FIRST", "second", "second", "third" };
            var distinct = source.DistinctBy(word => word, StringComparer.OrdinalIgnoreCase);
            distinct.AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        public void DistinctByNullComparer()
        {
            string[] source = { "first", "second", "third", "fourth", "fifth" };
            var distinct = source.DistinctBy(word => word.Length, null);
            distinct.AssertSequenceEqual("first", "second");
        }

        [Test]
        public void DistinctByIsLazyWithComparer()
        {
            var bs = new BreakingSequence<string>();
            _ = bs.DistinctBy(BreakingFunc.Of<string, string>(), StringComparer.Ordinal);
        }
    }
}
