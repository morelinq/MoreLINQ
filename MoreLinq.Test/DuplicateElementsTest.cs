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
    public class DuplicateElementsTest
    {
        [Test]
        public void SimpleDuplicateElements() {
            string[] seq = { "aaa", "aaa", "bb", "c", "dddd" };
            var result = seq.DuplicateElements();
            result.AssertSequenceEqual("aaa", "aaa");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DuplicateElementsNullSequence()
        {
            string[] seq = null;
            seq.DuplicateElements(x => x);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DuplicateElementsNullKeySelector()
        {
            string[] seq = { "aaa" };
            seq.DuplicateElements((Func<string, int>)null);
        }
        
        [Test]
        public void DuplicateElementsIsLazy()
        {
            new BreakingSequence<string>().DuplicateElements(x => x.Length);
        }
        
        [Test]
        public void DuplicateElementsWithComparer()
        {
            string[] seq = { "first", "second", "third", "THIRD", "fourth" };
            var result = seq.DuplicateElements(word => word, StringComparer.OrdinalIgnoreCase);
            result.AssertSequenceEqual("third", "THIRD");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DuplicateElementsNullSequenceWithComparer()
        {
            string[] seq = null;
            seq.DuplicateElements(x => x.Length, EqualityComparer<int>.Default);
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DuplicateElementsNullKeySelectorWithComparer()
        {
            string[] seq = { "aaa" };
            seq.DuplicateElements(null, EqualityComparer<string>.Default);
        }

        [Test]
        public void DuplicateElementsNullComparer()
        {
            string[] seq = { "aaa", "bb", "aa", "c", "dddd" };
            var result = seq.DuplicateElements(x => x.Length, null);
            result.AssertSequenceEqual("bb", "aa");
        }

        [Test]
        public void DuplicateElementsIsLazyWithComparer()
        {
            new BreakingSequence<string>().DuplicateElements(x => x, StringComparer.Ordinal);
        }
    }
}
