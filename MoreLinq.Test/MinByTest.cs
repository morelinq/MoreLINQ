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

namespace MoreLinq.Test
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class MinByTest
    {
        [Test]
        public void MinByNullComparer()
        {
            Assert.AreEqual("ax", SampleData.Strings.MinBy(x => x.Length, null));
        }

        [Test]
        public void MinByEmptySequence()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new string[0].MinBy(x => x.Length));
        }

        [Test]
        public void MinByWithNaturalComparer()
        {
            Assert.AreEqual("aa", SampleData.Strings.MinBy(x => x[1]));
        }

        [Test]
        public void MinByWithComparer()
        {
            Assert.AreEqual("az", SampleData.Strings.MinBy(x => x[1], SampleData.ReverseCharComparer));
        }

        [Test]
        public void MinByReturnsFirstOfEquals()
        {
            Assert.AreEqual("ax", SampleData.Strings.MinBy(x => x.Length));
        }
    }
}
