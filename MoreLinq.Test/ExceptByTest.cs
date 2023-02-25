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

    [TestFixture]
    public class ExceptByTest
    {
        [Test]
        public void SimpleExceptBy()
        {
            string[] first = { "aaa", "bb", "c", "dddd" };
            string[] second = { "xx", "y" };
            var result = first.ExceptBy(second, x => x.Length);
            result.AssertSequenceEqual("aaa", "dddd");
        }

        [Test]
        public void ExceptByIsLazy()
        {
            var bs = new BreakingSequence<string>();
            _ = bs.ExceptBy(bs, BreakingFunc.Of<string, int>());
        }

        [Test]
        public void ExceptByDoesNotRepeatSourceElementsWithDuplicateKeys()
        {
            string[] first = { "aaa", "bb", "c", "a", "b", "c", "dddd" };
            string[] second = { "xx" };
            var result = first.ExceptBy(second, x => x.Length);
            result.AssertSequenceEqual("aaa", "c", "dddd");
        }

        [Test]
        public void ExceptByWithComparer()
        {
            string[] first = { "first", "second", "third", "fourth" };
            string[] second = { "FIRST", "thiRD", "FIFTH" };
            var result = first.ExceptBy(second, word => word, StringComparer.OrdinalIgnoreCase);
            result.AssertSequenceEqual("second", "fourth");
        }

        [Test]
        public void ExceptByNullComparer()
        {
            string[] first = { "aaa", "bb", "c", "dddd" };
            string[] second = { "xx", "y" };
            var result = first.ExceptBy(second, x => x.Length, null);
            result.AssertSequenceEqual("aaa", "dddd");
        }

        [Test]
        public void ExceptByIsLazyWithComparer()
        {
            var bs = new BreakingSequence<string>();
            _ = bs.ExceptBy(bs, BreakingFunc.Of<string, string>(), StringComparer.Ordinal);
        }
    }
}
