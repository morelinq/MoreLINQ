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
    public class IntersectByTest {
        [Test]
        public void SimpleIntersectBy() {
            string[] first = { "aaa", "bb", "c", "dddd" };
            string[] second = { "bb", "c" };
            var result = first.IntersectBy(second, x => x.Length);
            result.AssertSequenceEqual("bb", "c");
        }

        [Test]
        public void IntersectByNullFirstSequence() {
            string[] first = null;
            string[] second = { "aaa" };
            Assert.ThrowsArgumentNullException("first", () =>
             first.IntersectBy(second, x => x.Length));
        }

        [Test]
        public void IntersectByNullSecondSequence() {
            string[] first = { "aaa" };
            string[] second = null;
            Assert.ThrowsArgumentNullException("second", () =>
             first.IntersectBy(second, x => x.Length));
        }

        [Test]
        public void IntersectByNullKeySelector() {
            string[] first = { "aaa" };
            string[] second = { "aaa" };
            Assert.ThrowsArgumentNullException("keySelector", () =>
             first.IntersectBy<string, string>(second, (Func<string, string>)null));
        }

        [Test]
        public void IntersectByIsLazy() {
            new BreakingSequence<string>().IntersectBy(new string[0], x => x.Length);
        }

        [Test]
        public void IntersectByDoesNotRepeatSourceElementsWithDuplicateKeys() {
            string[] first = { "aaa", "bb", "c", "a", "b", "c", "dddd" };
            string[] second = { "c" };
            var result = first.IntersectBy(second, x => x.Length);
            result.AssertSequenceEqual("c");
        }

        [Test]
        public void IntersectByWithComparer() {
            string[] first = { "first", "second", "third", "fourth" };
            string[] second = { "FIRST", "thiRD", "FIFTH" };
            var result = first.IntersectBy(second, word => word, StringComparer.OrdinalIgnoreCase);
            result.AssertSequenceEqual("first", "third");
        }

        [Test]
        public void IntersectByNullFirstSequenceWithComparer() {
            string[] first = null;
            string[] second = { "aaa" };
            Assert.ThrowsArgumentNullException("first", () => 
            first.IntersectBy(second, x => x.Length, EqualityComparer<int>.Default));
        }

        [Test]
        public void IntersectByNullSecondSequenceWithComparer() {
            string[] first = { "aaa" };
            string[] second = null;
            Assert.ThrowsArgumentNullException("second", () =>
             first.IntersectBy(second, x => x.Length, EqualityComparer<int>.Default));
        }

        [Test]
        public void IntersectByNullKeySelectorWithComparer() {
            string[] first = { "aaa" };
            string[] second = { "aaa" };
            Assert.ThrowsArgumentNullException("keySelector", () =>
             first.IntersectBy(second, null, EqualityComparer<string>.Default));
        }

        [Test]
        public void IntersectByNullComparer() {
            string[] first = { "aaa", "bb", "c", "dddd" };
            string[] second = { "xx", "y" };
            var result = first.IntersectBy(second, x => x.Length, null);
            result.AssertSequenceEqual("bb", "c");
        }

        [Test]
        public void IntersectByIsLazyWithComparer() {
            new BreakingSequence<string>().IntersectBy(new string[0], x => x, StringComparer.Ordinal);
        }
    }
}
