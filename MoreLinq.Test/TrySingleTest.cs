#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 James Webster. All rights reserved.
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
    public class TrySingleTest
    {
        [Test]
        public void TrySingleWithNone()
        {
            var arrayWithNone = new int?[0];

            var result = arrayWithNone.TrySingle("zero", "one", "many", ((cardinality, value) => (cardinality, value)));
            Assert.AreEqual(result.cardinality, "zero");
            Assert.IsNull(result.value);
        }

        [Test]
        public void TrySingleWithSingleton()
        {
            var arrayWithOne = new int?[] { 10 };

            var (cardinality, value) = arrayWithOne.TrySingle("zero", "one", "many", (c, v) => (c, v));
            Assert.AreEqual(cardinality, "one");
            Assert.AreEqual(value, 10);
        }

        [Test]
        public void TrySingleWithMoreThanOne()
        {
            var arrayWithMultiple = new int?[] {10, 20};

            var (cardinality, value) = arrayWithMultiple.TrySingle("zero", "one", "many", (c, v) => (c, v));

            Assert.AreEqual(cardinality, "many");
            Assert.IsNull(value);
        }
    }
}
