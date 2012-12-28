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
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class ConsumeTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConsumeWithNullSource()
        {
            MoreEnumerable.Consume<int>(null);
        }

        [Test]
        public void ConsumeReallyConsumes()
        {
            var counter = 0;
            var sequence = LinqEnumerable.Range(0, 10).Pipe(x => counter++);
            sequence.Consume();
            Assert.AreEqual(10, counter);
        }
    }
}
