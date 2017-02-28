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
    public class ExceptAllKeysTest
    {
        [Test]
        public void SimpleExceptAllKeys()
        {
            string[] first = { "aaa", "bb", "c", "dddd" };
            int[] second = { 1, 2 };
            var result = first.ExceptAllKeys(second, x => x.Length);
            result.AssertSequenceEqual("aaa", "dddd");
        }

        [Test]
        public void ExceptAllKeysNullFirstSequence()
        {
            string[] first = null;
            int[] second = { 1 };
            Assert.ThrowsArgumentNullException("first", () =>
             first.ExceptAllKeys(second, x => x.Length));
        }

        [Test]
        public void ExceptAllKeysNullSecondSequence()
        {
            string[] first = { "aaa" };
            int[] second = null;
            Assert.ThrowsArgumentNullException("second", () =>
             first.ExceptAllKeys(second, x => x.Length));
        }

        [Test]
        public void ExceptAllKeysNullKeySelector()
        {
            string[] first = { "aaa" };
            int[] second = { 1 };
            Assert.ThrowsArgumentNullException("keySelector", () =>
             first.ExceptAllKeys(second, (Func<string, int>)null));
        }

        [Test]
        public void ExceptAllKeysIsLazy()
        {
            new BreakingSequence<string>().ExceptAllKeys(new int[0], x => x.Length);
        }

        [Test]
        public void ExceptAllKeysRepeatsSourceElementsWithDuplicateKeys()
        {
            string[] first = { "aaa", "bb", "c", "a", "b", "c", "dddd" };
            int[] second = { 2 };
            var result = first.ExceptAllKeys(second, x => x.Length);
            result.AssertSequenceEqual("aaa", "c", "a", "b", "c", "dddd");
        }

        [Test]
        public void ExceptAllKeysWithComparer()
        {
            string[] first = { "first", "second", "third", "fourth" };
            string[] second = { "FIRST", "thiRD", "FIFTH" };
            var result = first.ExceptAllKeys(second, word => word, StringComparer.OrdinalIgnoreCase);
            result.AssertSequenceEqual("second", "fourth");
        }

        [Test]
        public void ExceptAllKeysNullFirstSequenceWithComparer()
        {
            string[] first = null;
            int[] second = { 1 };
            Assert.ThrowsArgumentNullException("first", () =>
             first.ExceptAllKeys(second, x => x.Length, EqualityComparer<int>.Default));
        }

        [Test]
        public void ExceptAllKeysNullSecondSequenceWithComparer()
        {
            string[] first = { "aaa" };
            int[] second = null;
            Assert.ThrowsArgumentNullException("second", () =>
             first.ExceptAllKeys(second, x => x.Length, EqualityComparer<int>.Default));
        }

        [Test]
        public void ExceptAllKeysNullKeySelectorWithComparer()
        {
            string[] first = { "aaa" };
            int[] second = { 1 };
            Assert.ThrowsArgumentNullException("keySelector", () =>
             first.ExceptAllKeys(second, null, EqualityComparer<int>.Default));
        }

        [Test]
        public void ExceptAllKeysNullComparer()
        {
            string[] first = { "aaa", "aaa", "bb", "c", "dddd" };
            int[] second = { 1, 2 };
            var result = first.ExceptAllKeys(second, x => x.Length, null);
            result.AssertSequenceEqual("aaa", "aaa", "dddd");
        }

        [Test]
        public void ExceptAllKeysIsLazyWithComparer()
        {
            new BreakingSequence<string>().ExceptAllKeys(new string[0], x => x, StringComparer.Ordinal);
        }
    }
}
