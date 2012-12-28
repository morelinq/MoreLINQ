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
    public class TakeLastTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TakeLastNullSource()
        {
            MoreEnumerable.TakeLast<object>(null, 0);
        }

        [Test]
        public void TakeLast()
        {
            var result = new[]{ 12, 34, 56, 78, 910, 1112 }.TakeLast(3);
            result.AssertSequenceEqual(78, 910, 1112);
        }

        [Test]
        public void TakeLastOnSequenceShortOfCount()
        {
            var result = new[] { 12, 34, 56 }.TakeLast(5);
            result.AssertSequenceEqual(12, 34, 56);
        }

        [Test]
        public void TakeLastWithNegativeCount()
        {
            var result = new[] { 12, 34, 56 }.TakeLast(-2);
            Assert.IsFalse(result.GetEnumerator().MoveNext());
        }

        [Test]
        public void TakeLastIsLazy()
        {
            new BreakingSequence<object>().TakeLast(1);
        }
    }
}
