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

namespace MoreLinq.Test
{
    [TestFixture]
    public class ExceptAllByTest
    {
        [Test]
        public void SimpleExceptAllBy()
        {
            string[] first = { "aaa", "bb", "c", "dddd" };
            string[] second = { "xx", "y" };
            var result = first.ExceptAllBy(second, x => x.Length);
            result.AssertSequenceEqual("aaa", "dddd");
        }

        [Test]
        public void ExceptAllByNullFirstSequence()
        {
            string[] first = null;
            string[] second = { "aaa" };
            Assert.ThrowsArgumentNullException("first", () =>
             first.ExceptAllBy(second, x => x.Length));
        }

        [Test]
        public void ExceptAllByNullSecondSequence()
        {
            string[] first = { "aaa" };
            string[] second = null;
            Assert.ThrowsArgumentNullException("second", () =>
             first.ExceptAllBy(second, x => x.Length));
        }

        [Test]
        public void ExceptAllByNullKeySelector()
        {
            string[] first = { "aaa" };
            string[] second = { "aaa" };
            Assert.ThrowsArgumentNullException("keySelector", () =>
             first.ExceptAllBy(second, (Func<string, string>)null));
        }
        
        [Test]
        public void ExceptAllByIsLazy()
        {
            new BreakingSequence<string>().ExceptAllBy(new string[0], x => x.Length);
        }

        [Test]
        public void ExceptAllByRepeatsSourceElementsWithDuplicateKeys()
        {
            string[] first = { "aaa", "bb", "c", "a", "b", "c", "dddd" };
            string[] second = { "xx" };
            var result = first.ExceptAllBy(second, x => x.Length);
            result.AssertSequenceEqual("aaa", "c", "a", "b", "c", "dddd");
        }

        [Test]
        public void ExceptAllByWithComparer()
        {
            string[] first = { "first", "second", "third", "fourth" };
            string[] second = { "FIRST", "thiRD", "FIFTH" };
            var result = first.ExceptBy(second, word => word, StringComparer.OrdinalIgnoreCase);
            result.AssertSequenceEqual("second", "fourth");
        }

        [Test]
        public void ExceptAllByNullFirstSequenceWithComparer()
        {
            string[] first = null;
            string[] second = { "aaa" };
            Assert.ThrowsArgumentNullException("first", () =>
             first.ExceptAllBy(second, x => x.Length, EqualityComparer<int>.Default));
        }

        [Test]
        public void ExceptAllByNullSecondSequenceWithComparer()
        {
            string[] first = { "aaa" };
            string[] second = null;
            Assert.ThrowsArgumentNullException("second", () =>
             first.ExceptAllBy(second, x => x.Length, EqualityComparer<int>.Default));
        }

        [Test]
        public void ExceptAllByNullKeySelectorWithComparer()
        {
            string[] first = { "aaa" };
            string[] second = { "aaa" };
            Assert.ThrowsArgumentNullException("keySelector", () =>
             first.ExceptAllBy(second, null, EqualityComparer<string>.Default));
        }

        [Test]
        public void ExceptAllByNullComparer()
        {
            string[] first = { "aaa", "bb", "bb", "c", "dddd", "dddd" };
            string[] second = { "xx", "y" };
            var result = first.ExceptAllBy(second, x => x.Length, null);
            result.AssertSequenceEqual("aaa", "dddd", "dddd");
        }

        [Test]
        public void ExceptAllByIsLazyWithComparer()
        {
            new BreakingSequence<string>().ExceptAllBy(new string[0], x => x, StringComparer.Ordinal);
        }
    }
}
