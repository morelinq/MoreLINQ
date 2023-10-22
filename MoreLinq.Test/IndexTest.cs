#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2012 Atif Aziz. All rights reserved.
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

namespace MoreLinq.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class IndexTest
    {
        [Test]
        public void IndexIsLazy()
        {
            var bs = new BreakingSequence<object>();
            _ = bs.Index();
            _ = bs.Index(0);
        }

        [Test]
        public void IndexSequence()
        {
            const string one = "one";
            const string two = "two";
            const string three = "three";
            var result = new[] { one, two, three }.Index();
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
            var result = new[] { one, two, three }.Index(10);
            result.AssertSequenceEqual(
                KeyValuePair.Create(10, one),
                KeyValuePair.Create(11, two),
                KeyValuePair.Create(12, three));
        }
    }
}
