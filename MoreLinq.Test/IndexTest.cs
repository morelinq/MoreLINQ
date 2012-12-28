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
    public class IndexTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IndexNullSequence()
        {
            MoreEnumerable.Index<object>(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IndexNullSequenceStartIndex()
        {
            MoreEnumerable.Index<object>(null, 0);
        }

        [Test]
        public void IndexIsLazy()
        {
            MoreEnumerable.Index(new BreakingSequence<object>());
            MoreEnumerable.Index(new BreakingSequence<object>(), 0);
        }

        [Test]
        public void IndexSequence()
        {
            const string one = "one";
            const string two = "two";
            const string three = "three";
            var result = MoreEnumerable.Index(new[] { one, two, three });
            result.AssertSequenceEqual(
                KeyValuePair.Create(0, one),
                KeyValuePair.Create(1, two),
                KeyValuePair.Create(2, three));
        }

        [Test]
        public void IndexSequenceStartIndex()
        {
            const string one = "one";
            const string two = "two";
            const string three = "three";
            var result = MoreEnumerable.Index(new[] { one, two, three }, 10);
            result.AssertSequenceEqual(
                KeyValuePair.Create(10, one),
                KeyValuePair.Create(11, two),
                KeyValuePair.Create(12, three));
        }
    }
}
