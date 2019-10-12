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
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class TrySingleTest
    {
        [Test]
        public void TrySingleWithEmptySource()
        {
            var arrayWithNone = new int?[0];

            var (cardinality, value) = arrayWithNone.TrySingle("zero", "one", "many", ValueTuple.Create);

            Assert.AreEqual(cardinality, "zero");
            Assert.IsNull(value);
        }

        [Test]
        public void TrySingleWithSingleton()
        {
            var arrayWithOne = new int?[] { 10 };

            var (cardinality, value) = arrayWithOne.TrySingle("zero", "one", "many", ValueTuple.Create);

            Assert.AreEqual(cardinality, "one");
            Assert.AreEqual(value, 10);
        }

        [Test]
        public void TrySingleWithMoreThanOne()
        {
            var arrayWithMultiple = new int?[] { 10, 20 };

            var (cardinality, value) = arrayWithMultiple.TrySingle("zero", "one", "many", ValueTuple.Create);

            Assert.AreEqual(cardinality, "many");
            Assert.IsNull(value);
        }

        [Test]
        public void TrySingleDoesNotConsumeMoreThanTwoElementsFromTheSequence()
        {
            IEnumerable<int> TestSequence()
            {
                yield return 1;
                yield return 2;
                throw new Exception("TrySingle should not have attempted to consume a third element.");
            }

            var (cardinality, value) = TestSequence().TrySingle("zero", "one", "many");
            Assert.AreEqual("many", cardinality);
            Assert.AreEqual(default(int), value);
        }

        [Test]
        public void TrySingleOptimizesForList()
        {
            var list = new BreakingList<int>();

            var (cardinality, value) = list.TrySingle("zero", "one", "many");
            Assert.AreEqual("zero", cardinality);
            Assert.AreEqual(default(int), value);

            list = new BreakingList<int>(new List<int> {1});
            (cardinality, value) = list.TrySingle("zero", "one", "many");
            Assert.AreEqual("one", cardinality);
            Assert.AreEqual(1, value);

            list = new BreakingList<int>(new List<int> {1, 2});
            (cardinality, value) = list.TrySingle("zero", "one", "many");
            Assert.AreEqual("many", cardinality);
            Assert.AreEqual(default(int), value);
        }

        [Test]
        public void TrySingleOptimizesForICollection()
        {
            var coll = new BreakingCollection<int>();

            var (cardinality, value) = coll.TrySingle("zero", "one", "many");
            Assert.AreEqual("zero", cardinality);
            Assert.AreEqual(default(int), value);

            coll = new BreakingCollection<int>(new List<int> {1, 2});

            (cardinality, value) = coll.TrySingle("zero", "one", "many");
            Assert.AreEqual("many", cardinality);
            Assert.AreEqual(default(int), value);
        }
    }
}
