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
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class DistinctByTest
    {
        [Test]
        public void DistinctBy()
        {
            string[] source = { "first", "second", "third", "fourth", "fifth" };
            var distinct = source.DistinctBy(word => word.Length);
            distinct.AssertSequenceEqual("first", "second");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DistinctByNullSequence()
        {
            string[] source = null;
            source.DistinctBy(x => x.Length);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DistinctByNullKeySelector()
        {
            string[] source = {};
            source.DistinctBy((Func<string,string>) null);
        }

        [Test]
        public void DistinctByIsLazy()
        {
            new BreakingSequence<string>().DistinctBy(x => x.Length);
        }

        [Test]
        public void DistinctByWithComparer()
        {
            string[] source = { "first", "FIRST", "second", "second", "third" };
            var distinct = source.DistinctBy(word => word, StringComparer.OrdinalIgnoreCase);
            distinct.AssertSequenceEqual("first", "second", "third");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DistinctByNullSequenceWithComparer()
        {
            string[] source = null;
            source.DistinctBy(x => x, StringComparer.Ordinal);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DistinctByNullKeySelectorWithComparer()
        {
            string[] source = { };
            source.DistinctBy(null, StringComparer.Ordinal);
        }

        [Test]
        public void DistinctByNullComparer()
        {
            string[] source = { "first", "second", "third", "fourth", "fifth" };
            var distinct = source.DistinctBy(word => word.Length, null);
            distinct.AssertSequenceEqual("first", "second");
        }

        [Test]
        public void DistinctByIsLazyWithComparer()
        {
            new BreakingSequence<string>().DistinctBy(x => x, StringComparer.Ordinal);
        }
    }
}
