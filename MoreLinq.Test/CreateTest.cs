#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Atif Aziz. All rights reserved.
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

using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class CreateTest
    {
        [Test]
        public void CreateIsLazy()
        {
            Assert.That(MoreEnumerable.Create(BreakingFunc.Of<IEnumerator<object>>()), Is.Not.Null);
        }

        [Test]
        public void CreateYieldsEnumeratorElements()
        {
            var xs = Enumerable.Range(1, 10);
            var result = MoreEnumerable.Create(() => xs.GetEnumerator());
            Assert.That(result, Is.EquivalentTo(xs));
        }

        [Test]
        public void CreateDisposesEnumerator()
        {
            var item = new object();
            using (var items = Enumerable.Repeat(item, 1).AsTestingSequence())
            {
                var r = MoreEnumerable.Create(() => items.GetEnumerator()).Read();
                Assert.That(r.Read(), Is.EqualTo(item));
                r.ReadEnd();
            }
        }
    }
}
