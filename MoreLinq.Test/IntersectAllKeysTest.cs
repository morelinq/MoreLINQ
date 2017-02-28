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
    public class IntersectAllKeysTest {
        [Test]
        public void SimpleIntersectAllKeys() {
            string[] first = { "aaa", "bb", "c", "dddd" };
            int[] second = { 1, 2 };
            var result = first.IntersectAllKeys(second, x => x.Length);
            result.AssertSequenceEqual("bb", "c");
        }

        [Test]
        public void IntersectAllKeysNullFirstSequence() {
            string[] first = null;
            int[] second = { 1 };
            Assert.ThrowsArgumentNullException("first", () =>
             first.IntersectAllKeys(second, x => x.Length));
        }

        [Test]
        public void IntersectAllKeysNullSecondSequence() {
            string[] first = { "aaa" };
            int[] second = null;
            Assert.ThrowsArgumentNullException("second", () =>
             first.IntersectAllKeys(second, x => x.Length));
        }

        [Test]
        public void IntersectAllKeysNullKeySelector() {
            string[] first = { "aaa" };
            int[] second = { 3 };
            Assert.ThrowsArgumentNullException("keySelector", () =>
             first.IntersectAllKeys<string, int>(second, (Func<string, int>)null));
        }

        [Test]
        public void IntersectAllKeysIsLazy() {
            new BreakingSequence<string>().IntersectAllKeys(new int[0], x => x.Length);
        }

        [Test]
        public void IntersectAllKeysRepeatsSourceElementsWithDuplicateKeys() {
            string[] first = { "aaa", "bb", "c", "a", "b", "c", "dddd" };
            int[] second = { 1 };
            var result = first.IntersectAllKeys(second, x => x.Length);
            result.AssertSequenceEqual("c", "a", "b", "c");
        }

        [Test]
        public void IntersectAllKeysWithComparer() {
            string[] first = { "first", "second", "third", "fourth" };
            string[] second = { "FIRST", "thiRD", "FIFTH" };
            var result = first.IntersectAllKeys(second, word => word, StringComparer.OrdinalIgnoreCase);
            result.AssertSequenceEqual("first", "third");
        }

        [Test]
        public void IntersectAllKeysNullFirstSequenceWithComparer() {
            string[] first = null;
            int[] second = { 1 };
            Assert.ThrowsArgumentNullException("first", () =>
             first.IntersectAllKeys(second, x => x.Length, EqualityComparer<int>.Default));
        }

        [Test]
        public void IntersectAllKeysNullSecondSequenceWithComparer() {
            string[] first = { "aaa" };
            int[] second = null;
            Assert.ThrowsArgumentNullException("second", () =>
             first.IntersectAllKeys(second, x => x.Length, EqualityComparer<int>.Default));
        }

        [Test]
        public void IntersectAllKeysNullKeySelectorWithComparer() {
            string[] first = { "aaa" };
            int[] second = { 1 };
            Assert.ThrowsArgumentNullException("keySelector", () =>
             first.IntersectAllKeys(second, null, EqualityComparer<int>.Default));
        }

        [Test]
        public void IntersectAllKeysNullComparer() {
            string[] first = { "aaa", "bb", "bb", "c", "dddd" };
            int[] second = { 1, 2 };
            var result = first.IntersectAllKeys(second, x => x.Length, null);
            result.AssertSequenceEqual("bb", "bb", "c");
        }

        [Test]
        public void IntersectAllKeysIsLazyWithComparer() {
            new BreakingSequence<string>().IntersectAllKeys(new string[0], x => x, StringComparer.Ordinal);
        }
    }
}
