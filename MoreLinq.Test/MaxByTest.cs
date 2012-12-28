#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008-2011 Jonathan Skeet. All rights reserved.
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
    public class MaxByTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MaxByNullSequence()
        {
            ((IEnumerable<string>)null).MaxBy(x => x.Length);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MaxByNullSelector()
        {
            SampleData.Strings.MaxBy<string, int>(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MaxByNullComparer()
        {
            SampleData.Strings.MaxBy(x => x.Length, null);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MaxByEmptySequence()
        {
            new string[0].MaxBy(x => x.Length);
        }

        [Test]
        public void MaxByWithNaturalComparer()
        {
            Assert.AreEqual("az", SampleData.Strings.MaxBy(x => x[1]));
        }

        [Test]
        public void MaxByWithComparer()
        {
            Assert.AreEqual("aa", SampleData.Strings.MaxBy(x => x[1], SampleData.ReverseCharComparer));
        }

        [Test]
        public void MaxByReturnsFirstOfEquals()
        {
            Assert.AreEqual("hello", SampleData.Strings.MaxBy(x => x.Length));
        }
    }
}
