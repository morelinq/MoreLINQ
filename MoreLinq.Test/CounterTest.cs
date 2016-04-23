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
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test {

    [TestFixture]
    public class CounterTest {

        #region Construct
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CounterConstructNullCollection() {
            var counter = new Counter<String>(null);
        }

        [Test]
        public void CounterCountsInputValues() {
            var counter = new Counter<Int32>(new[] { 0, 1, 1, 1, 2, 2, 3, 3 });
            Assert.AreEqual(0, counter[-1]);
            Assert.AreEqual(1, counter[0]);
            Assert.AreEqual(3, counter[1]);
            Assert.AreEqual(2, counter[2]);
            Assert.AreEqual(2, counter[3]);
            Assert.AreEqual(0, counter[4]);
        }

        private class StringLengthComparer : IEqualityComparer<String> {
            public bool Equals(string x, string y) { return x.Length == y.Length; }
            public int GetHashCode(string obj) { return obj.Length.GetHashCode(); }
        }

        [Test]
        public void CounterWithComparer() {
            //Arrange
            var items = new[] { "apple", "orange", "banana", "lemon", "grape", "lime" };
            //Act
            var counter = new Counter<String>(items, new StringLengthComparer());
            //Assert
            Assert.AreEqual(3, counter["apple"]);
            Assert.AreEqual(3, counter["lemon"]);
            Assert.AreEqual(3, counter["grape"]);

            Assert.AreEqual(2, counter["orange"]);
            Assert.AreEqual(2, counter["banana"]);

            Assert.AreEqual(0, counter["watermelon"]);
        }
        #endregion

        #region Increment/Decrement

        [Test]
        public void CounterIncrementAddsOne() {
            //Arrange
            var items = new[] { "a", "a", "b", "c", "c", "c", null };
            var counter = new Counter<String>(items);
            //Act
            counter.Increment("a");
            //Assert
            Assert.AreEqual(3, counter["a"]);
            Assert.AreEqual(1, counter["b"]);
            Assert.AreEqual(3, counter["c"]);
            Assert.AreEqual(1, counter[null]);
        }

        [Test]
        public void CounterIncrementNullAddsOne() {
            //Arrange
            var items = new[] { "a", "a", "b", "c", "c", "c", null };
            var counter = new Counter<String>(items);
            //Act
            counter.Increment(null);
            //Assert
            Assert.AreEqual(2, counter["a"]);
            Assert.AreEqual(1, counter["b"]);
            Assert.AreEqual(3, counter["c"]);
            Assert.AreEqual(2, counter[null]);
        }

        [Test]
        public void CounterDecrementSubtractsOne() {
            //Arrange
            var items = new[] { "a", "a", "b", "c", "c", "c", null };
            var counter = new Counter<String>(items);
            //Act
            counter.Decrement("a");
            //Assert
            Assert.AreEqual(1, counter["a"]);
            Assert.AreEqual(1, counter["b"]);
            Assert.AreEqual(3, counter["c"]);
            Assert.AreEqual(1, counter[null]);
        }

        [Test]
        public void CounterDecrementNullSubtractsOne() {
            //Arrange
            var items = new[] { "a", "a", "b", "c", "c", "c", null };
            var counter = new Counter<String>(items);
            //Act
            counter.Decrement(null);
            //Assert
            Assert.AreEqual(2, counter["a"]);
            Assert.AreEqual(1, counter["b"]);
            Assert.AreEqual(3, counter["c"]);
            Assert.AreEqual(0, counter[null]);
        }

        [Test]
        public void CounterDecrementStopsAtZero() {
            //Arrange
            var items = new[] { "a", "a", "b", "c", "c", "c", null };
            var counter = new Counter<String>(items);
            //Act
            for (var i = 0; i < 10; i++) {
                counter.Decrement("a");
            }
            //Assert
            Assert.AreEqual(0, counter["a"]);
            Assert.AreEqual(1, counter["b"]);
            Assert.AreEqual(3, counter["c"]);
            Assert.AreEqual(1, counter[null]);
        }

        [Test]
        public void CounterDecrementNullStopsAtZero() {
            //Arrange
            var items = new[] { "a", "a", "b", "c", "c", "c", null };
            var counter = new Counter<String>(items);
            //Act
            for (var i = 0; i < 10; i++) {
                counter.Decrement(null);
            }
            //Assert
            Assert.AreEqual(2, counter["a"]);
            Assert.AreEqual(1, counter["b"]);
            Assert.AreEqual(3, counter["c"]);
            Assert.AreEqual(0, counter[null]);
        }

        [Test]
        public void CounterDecrementReturnsCorrectValue() {
            //Arrange
            var items = new[] { "a", "a", "b", "c", "c", "c", null };
            var counter = new Counter<String>(items);
            //Act
            Assert.AreEqual(true, counter.Decrement("a")); //1 left
            Assert.AreEqual(true, counter.Decrement("a")); //0 left
            Assert.AreEqual(false, counter.Decrement("a")); //Cannot decrement further
        }

        [Test]
        public void CounterDecerementNullReturnsCorrectValue() {
            //Arrange
            var items = new[] { "a", "a", "b", "c", "c", "c", null };
            var counter = new Counter<String>(items);
            //Act
            Assert.AreEqual(true, counter.Decrement(null)); //0 left
            Assert.AreEqual(false, counter.Decrement(null)); //Cannot decrement further
        }

        #endregion

        [Test]
        public void CounterGetEnumerator() {
            //Arrange
            var items = new[] { "a", "a", "b", "c", "c", "c", null };
            var counter = new Counter<String>(items);
            var expected = new[]{
                new KeyValuePair<String, Int32>("a", 2),
                new KeyValuePair<String, Int32>("b", 1),
                new KeyValuePair<String, Int32>("c", 3),
                new KeyValuePair<String, Int32>(null, 1)
            };
            //Act
            var counts = counter.ToArray();
            //Assert
            CollectionAssert.AreEquivalent(expected, counts);
        }

        [Test]
        public void CounterGetEnumeratorDoesNotReturnCountsOfZero() {
            //Arrange
            var items = new[] { "a", "a", "b", "c", "c", "c", null };
            var counter = new Counter<String>(items);
            counter.Decrement("b");

            var expected = new[]{
                new KeyValuePair<String, Int32>("a", 2),
                new KeyValuePair<String, Int32>("c", 3),
                new KeyValuePair<String, Int32>(null, 1)
            };
            //Act
            var counts = counter.ToArray();
            //Assert
            CollectionAssert.AreEquivalent(expected, counts);
        }
    }
}
