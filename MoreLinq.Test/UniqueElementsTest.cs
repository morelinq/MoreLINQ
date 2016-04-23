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
    public class UniqueElementsTest
    {
        [Test]
        public void SimpleUniqueElements() {
            string[] seq = { "aaa", "aaa", "bb", "c", "dddd" };
            var result = seq.UniqueElements();
            result.AssertSequenceEqual("bb", "c", "dddd");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UniqueElementsNullSequence()
        {
            string[] seq = null;
            seq.UniqueElements(x => x);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UniqueElementsNullKeySelector()
        {
            string[] seq = { "aaa" };
            seq.UniqueElements((Func<string, int>)null);
        }
        
        [Test]
        public void UniqueElementsIsLazy()
        {
            new BreakingSequence<string>().UniqueElements(x => x.Length);
        }
        
        [Test]
        public void UniqueElementsWithComparer()
        {
            string[] seq = { "first", "second", "third", "THIRD", "fourth" };
            var result = seq.UniqueElements(word => word, StringComparer.OrdinalIgnoreCase);
            result.AssertSequenceEqual("first", "second", "fourth");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UniqueElementsNullSequenceWithComparer()
        {
            string[] seq = null;
            seq.UniqueElements(x => x.Length, EqualityComparer<int>.Default);
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UniqueElementsNullKeySelectorWithComparer()
        {
            string[] seq = { "aaa" };
            seq.UniqueElements(null, EqualityComparer<string>.Default);
        }

        [Test]
        public void UniqueElementsNullComparer()
        {
            string[] seq = { "aaa", "bb", "aa", "c", "dddd" };
            var result = seq.UniqueElements(x => x.Length, null);
            result.AssertSequenceEqual("aaa", "c", "dddd");
        }

        [Test]
        public void UniqueElementsIsLazyWithComparer()
        {
            new BreakingSequence<string>().UniqueElements(x => x, StringComparer.Ordinal);
        }
    }
}
