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
    public class IntersectKeysTest {
        [Test]
        public void SimpleIntersectKeys() {
            string[] first = { "aaa", "bb", "c", "dddd" };
            int[] second = { 1, 2 };
            var result = first.IntersectKeys(second, x => x.Length);
            result.AssertSequenceEqual("bb", "c");
        }

        [Test]
        public void IntersectKeysNullFirstSequence() {
            string[] first = null;
            int[] second = { 1 };
            Assert.ThrowsArgumentNullException("first", () =>
             first.IntersectKeys(second, x => x.Length));
        }

        [Test]
        public void IntersectKeysNullSecondSequence() {
            string[] first = { "aaa" };
            int[] second = null;
            Assert.ThrowsArgumentNullException("second", () =>
             first.IntersectKeys(second, x => x.Length));
        }

        [Test]
        public void IntersectKeysNullKeySelector() {
            string[] first = { "aaa" };
            int[] second = { 3 };
            Assert.ThrowsArgumentNullException("keySelector", () =>
             first.IntersectKeys<string, int>(second, (Func<string, int>)null));
        }

        [Test]
        public void IntersectKeysIsLazy() {
            new BreakingSequence<string>().IntersectKeys(new int[0], x => x.Length);
        }

        [Test]
        public void IntersectKeysDoesNotRepeatSourceElementsWithDuplicateKeys() {
            string[] first = { "aaa", "bb", "c", "a", "b", "c", "dddd" };
            int[] second = { 1 };
            var result = first.IntersectKeys(second, x => x.Length);
            result.AssertSequenceEqual("c");
        }

        [Test]
        public void IntersectKeysWithComparer() {
            string[] first = { "first", "second", "third", "fourth" };
            string[] second = { "FIRST", "thiRD", "FIFTH" };
            var result = first.IntersectKeys(second, word => word, StringComparer.OrdinalIgnoreCase);
            result.AssertSequenceEqual("first", "third");
        }

        [Test]
        public void IntersectKeysNullFirstSequenceWithComparer() {
            string[] first = null;
            int[] second = { 1 };
            Assert.ThrowsArgumentNullException("first", () =>
             first.IntersectKeys(second, x => x.Length, EqualityComparer<int>.Default));
        }

        [Test]
        public void IntersectKeysNullSecondSequenceWithComparer() {
            string[] first = { "aaa" };
            int[] second = null;
            Assert.ThrowsArgumentNullException("second", () =>
             first.IntersectKeys(second, x => x.Length, EqualityComparer<int>.Default));
        }

        [Test]
        public void IntersectKeysNullKeySelectorWithComparer() {
            string[] first = { "aaa" };
            int[] second = { 1 };
            Assert.ThrowsArgumentNullException("keySelector", () =>
             first.IntersectKeys(second, null, EqualityComparer<int>.Default));
        }

        [Test]
        public void IntersectKeysNullComparer() {
            string[] first = { "aaa", "bb", "c", "dddd" };
            int[] second = { 1, 2 };
            var result = first.IntersectKeys(second, x => x.Length, null);
            result.AssertSequenceEqual("bb", "c");
        }

        [Test]
        public void IntersectKeysIsLazyWithComparer() {
            new BreakingSequence<string>().IntersectKeys(new string[0], x => x, StringComparer.Ordinal);
        }
    }
}
