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
    public class ExceptKeysTest
    {
        [Test]
        public void SimpleExceptKeys()
        {
            string[] first = { "aaa", "bb", "c", "dddd" };
            int[] second = { 1, 2 };
            var result = first.ExceptKeys(second, x => x.Length);
            result.AssertSequenceEqual("aaa", "dddd");
        }

        [Test]
        public void ExceptKeysNullFirstSequence()
        {
            string[] first = null;
            int[] second = { 1 };
            Assert.ThrowsArgumentNullException("first", () =>
             first.ExceptKeys(second, x => x.Length));
        }

        [Test]
        public void ExceptKeysNullSecondSequence()
        {
            string[] first = { "aaa" };
            int[] second = null;
            Assert.ThrowsArgumentNullException("second", () =>
             first.ExceptKeys(second, x => x.Length));
        }

        [Test]
        public void ExceptKeysNullKeySelector()
        {
            string[] first = { "aaa" };
            int[] second = { 1 };
            Assert.ThrowsArgumentNullException("keySelector", () =>
             first.ExceptKeys(second, (Func<string, int>)null));
        }
        
        [Test]
        public void ExceptKeysIsLazy()
        {
            new BreakingSequence<string>().ExceptKeys(new int[0], x => x.Length);
        }

        [Test]
        public void ExceptKeysDoesNotRepeatSourceElementsWithDuplicateKeys()
        {
            string[] first = { "aaa", "bb", "c", "a", "b", "c", "dddd" };
            int[] second = { 2 };
            var result = first.ExceptKeys(second, x => x.Length);
            result.AssertSequenceEqual("aaa", "c", "dddd");
        }

        [Test]
        public void ExceptKeysWithComparer()
        {
            string[] first = { "first", "second", "third", "fourth" };
            string[] second = { "FIRST" , "thiRD", "FIFTH" };
            var result = first.ExceptKeys(second, word => word, StringComparer.OrdinalIgnoreCase);
            result.AssertSequenceEqual("second", "fourth");
        }

        [Test]
        public void ExceptKeysNullFirstSequenceWithComparer()
        {
            string[] first = null;
            int[] second = { 1 };
            Assert.ThrowsArgumentNullException("first", () =>
             first.ExceptKeys(second, x => x.Length, EqualityComparer<int>.Default));
        }

        [Test]
        public void ExceptKeysNullSecondSequenceWithComparer()
        {
            string[] first = { "aaa" };
            int[] second = null;
            Assert.ThrowsArgumentNullException("second", () =>
             first.ExceptKeys(second, x => x.Length, EqualityComparer<int>.Default));
        }

        [Test]
        public void ExceptKeysNullKeySelectorWithComparer()
        {
            string[] first = { "aaa" };
            int[] second = { 1 };
            Assert.ThrowsArgumentNullException("keySelector", () =>
             first.ExceptKeys(second, null, EqualityComparer<int>.Default));
        }

        [Test]
        public void ExceptKeysNullComparer()
        {
            string[] first = { "aaa", "bb", "c", "dddd" };
            int[] second = { 1, 2 };
            var result = first.ExceptKeys(second, x => x.Length, null);
            result.AssertSequenceEqual("aaa", "dddd");
        }

        [Test]
        public void ExceptKeysIsLazyWithComparer()
        {
            new BreakingSequence<string>().ExceptKeys(new string[0], x => x, StringComparer.Ordinal);
        }
    }
}
