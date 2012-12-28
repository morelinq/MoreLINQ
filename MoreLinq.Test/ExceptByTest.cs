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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExceptByNullFirstSequence()
        {
            string[] first = null;
            string[] second = { "aaa" };
            first.ExceptBy(second, x => x.Length);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExceptByNullSecondSequence()
        {
            string[] first = { "aaa" };
            string[] second = null;
            first.ExceptBy(second, x => x.Length);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExceptByNullKeySelector()
        {
            string[] first = { "aaa" };
            string[] second = { "aaa" };
            first.ExceptBy(second, (Func<string, string>)null);
        }
        
        [Test]
        public void ExceptByIsLazy()
        {
            new BreakingSequence<string>().ExceptBy(new string[0], x => x.Length);
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
            string[] second = { "FIRST" , "thiRD", "FIFTH" };
            var result = first.ExceptBy(second, word => word, StringComparer.OrdinalIgnoreCase);
            result.AssertSequenceEqual("second", "fourth");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExceptByNullFirstSequenceWithComparer()
        {
            string[] first = null;
            string[] second = { "aaa" };
            first.ExceptBy(second, x => x.Length, EqualityComparer<int>.Default);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExceptByNullSecondSequenceWithComparer()
        {
            string[] first = { "aaa" };
            string[] second = null;
            first.ExceptBy(second, x => x.Length, EqualityComparer<int>.Default);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExceptByNullKeySelectorWithComparer()
        {
            string[] first = { "aaa" };
            string[] second = { "aaa" };
            first.ExceptBy(second, null, EqualityComparer<string>.Default);
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
            new BreakingSequence<string>().ExceptBy(new string[0], x => x, StringComparer.Ordinal);
        }
    }
}
