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
    public class UniqueKeysTest
    {
        [Test]
        public void SimpleUniqueKeys() {
            string[] seq = { "aaa", "bbb", "bb", "c", "dddd" };
            var result = seq.UniqueKeys(x => x.Length)
                .OrderBy(x => x, OrderByDirection.Ascending);
            result.AssertSequenceEqual(1, 2, 4);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UniqueKeysNullSequence()
        {
            string[] seq = null;
            seq.UniqueKeys(x => x);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UniqueKeysNullKeySelector()
        {
            string[] seq = { "aaa" };
            seq.UniqueKeys((Func<string, int>)null);
        }
        
        [Test]
        public void UniqueKeysIsLazy()
        {
            new BreakingSequence<string>().UniqueKeys(x => x.Length);
        }
        
        [Test]
        public void UniqueKeysWithComparer()
        {
            string[] seq = { "first", "second", "third", "THIRD", "fourth" };
            var result = seq.UniqueKeys(word => word, StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x, OrderByDirection.Ascending);
            result.AssertSequenceEqual("first", "fourth", "second");
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UniqueKeysNullSequenceWithComparer()
        {
            string[] seq = null;
            seq.UniqueKeys(x => x.Length, EqualityComparer<int>.Default);
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UniqueKeysNullKeySelectorWithComparer()
        {
            string[] seq = { "aaa" };
            seq.UniqueKeys(null, EqualityComparer<string>.Default);
        }

        [Test]
        public void UniqueKeysNullComparer()
        {
            string[] seq = { "aaa", "bb", "aa", "c", "dddd" };
            var result = seq.UniqueKeys(x => x.Length, null)
                .OrderBy(x => x, OrderByDirection.Ascending);
            result.AssertSequenceEqual(1, 3, 4);
        }

        [Test]
        public void UniqueKeysIsLazyWithComparer()
        {
            new BreakingSequence<string>().UniqueKeys(x => x, StringComparer.Ordinal);
        }
    }
}
