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
    public class DuplicateKeysTest
    {
        [Test]
        public void SimpleDuplicateKeys() {
            string[] seq = { "aaa", "bbb", "bb", "c", "dddd" };
            var result = seq.DuplicateKeys(x => x.Length);
            result.AssertSequenceEqual(3);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DuplicateKeysNullSequence()
        {
            string[] seq = null;
            seq.DuplicateKeys(x => x);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DuplicateKeysNullKeySelector()
        {
            string[] seq = { "aaa" };
            seq.DuplicateKeys((Func<string, int>)null);
        }
        
        [Test]
        public void DuplicateKeysIsLazy()
        {
            new BreakingSequence<string>().DuplicateKeys(x => x.Length);
        }
        
        [Test]
        public void DuplicateKeysWithComparer()
        {
            string[] seq = { "first", "second", "third", "THIRD", "fourth" };
            var result = seq.DuplicateKeys(word => word, StringComparer.OrdinalIgnoreCase);
            result.AssertSequenceEqual("third");
        }

        [Test]
        public void DuplicateKeysDoesNotRepeatKeys() {
            string[] first = { "aaa", "bb", "bb", "c", "a", "b", "c", "dddd" };
            string[] second = { "xx" };
            var result = first.DuplicateKeys(x => x.Length);
            result.AssertSequenceEqual(2, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DuplicateKeysNullSequenceWithComparer()
        {
            string[] seq = null;
            seq.DuplicateKeys(x => x.Length, EqualityComparer<int>.Default);
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DuplicateKeysNullKeySelectorWithComparer()
        {
            string[] seq = { "aaa" };
            seq.DuplicateKeys(null, EqualityComparer<string>.Default);
        }

        [Test]
        public void DuplicateKeysNullComparer()
        {
            string[] seq = { "aaa", "bb", "aa", "c", "dddd" };
            var result = seq.DuplicateKeys(x => x.Length, null);
            result.AssertSequenceEqual(2);
        }

        [Test]
        public void DuplicateKeysIsLazyWithComparer()
        {
            new BreakingSequence<string>().DuplicateKeys(x => x, StringComparer.Ordinal);
        }
    }
}
