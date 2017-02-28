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

namespace MoreLinq.Test {
    [TestFixture]
    public class IntersectAllByTest {
        [Test]
        public void SimpleIntersectAllBy() {
            string[] first = { "aaa", "bb", "c", "dddd" };
            string[] second = { "bb", "c" };
            var result = first.IntersectAllBy(second, x => x.Length);
            result.AssertSequenceEqual("bb", "c");
        }

        [Test]
        public void IntersectAllByNullFirstSequence() {
            string[] first = null;
            string[] second = { "aaa" };
            Assert.ThrowsArgumentNullException("first", () =>
             first.IntersectAllBy(second, x => x.Length));
        }

        [Test]
        public void IntersectAllByNullSecondSequence() {
            string[] first = { "aaa" };
            string[] second = null;
            Assert.ThrowsArgumentNullException("second", () =>
             first.IntersectAllBy(second, x => x.Length));
        }

        [Test]
        public void IntersectAllByNullKeySelector() {
            string[] first = { "aaa" };
            string[] second = { "aaa" };
            Assert.ThrowsArgumentNullException("keySelector", () => 
             first.IntersectAllBy<string, string>(second, (Func<string, string>)null));
        }

        [Test]
        public void IntersectAllByIsLazy() {
            new BreakingSequence<string>().IntersectAllBy(new string[0], x => x.Length);
        }

        [Test]
        public void IntersectAllByRepeatsSourceElementsWithDuplicateKeys() {
            string[] first = { "aaa", "bb", "c", "a", "b", "c", "dddd" };
            string[] second = { "c" };
            var result = first.IntersectAllBy(second, x => x.Length);
            result.AssertSequenceEqual("c", "a", "b", "c");
        }

        [Test]
        public void IntersectAllByWithComparer() {
            string[] first = { "first", "second", "third", "fourth" };
            string[] second = { "FIRST", "thiRD", "FIFTH" };
            var result = first.IntersectAllBy(second, word => word, StringComparer.OrdinalIgnoreCase);
            result.AssertSequenceEqual("first", "third");
        }

        [Test]
        public void IntersectAllByNullFirstSequenceWithComparer() {
            string[] first = null;
            string[] second = { "aaa" };
            Assert.ThrowsArgumentNullException("first", () =>
             first.IntersectAllBy(second, x => x.Length, EqualityComparer<int>.Default));
        }

        [Test]
        public void IntersectAllByNullSecondSequenceWithComparer() {
            string[] first = { "aaa" };
            string[] second = null;
            Assert.ThrowsArgumentNullException("second", () =>
             first.IntersectAllBy(second, x => x.Length, EqualityComparer<int>.Default));
        }

        [Test]
        public void IntersectAllByNullKeySelectorWithComparer() {
            string[] first = { "aaa" };
            string[] second = { "aaa" };
            Assert.ThrowsArgumentNullException("keySelector", () =>
             first.IntersectAllBy(second, null, EqualityComparer<string>.Default));
        }

        [Test]
        public void IntersectAllByNullComparer() {
            string[] first = { "aaa", "bb", "bb", "c", "dddd" };
            string[] second = { "xx", "y" };
            var result = first.IntersectAllBy(second, x => x.Length, null);
            result.AssertSequenceEqual("bb","bb", "c");
        }

        [Test]
        public void IntersectAllByIsLazyWithComparer() {
            new BreakingSequence<string>().IntersectAllBy(new string[0], x => x, StringComparer.Ordinal);
        }
    }
}
