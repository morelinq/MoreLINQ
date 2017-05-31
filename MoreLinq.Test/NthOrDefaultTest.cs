#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Leandro F. Vieira (leandromoh). All rights reserved.
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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using LinqEnumerable = System.Linq.Enumerable;

namespace MoreLinq.Test
{
    [TestFixture]
    public class NthOrDefaultTest
    {
        // NthOrDefault Tests

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NthOrDefaultWithNullSequence()
        {
            (null as IEnumerable<int>).NthOrDefault(1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NthOrDefaultWithNthZero()
        {
            Enumerable.Range(1, 10).NthOrDefault(0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NthOrDefaultWithNthNegative()
        {
            Enumerable.Range(1, 10).NthOrDefault(-1);
        }

        [Test]
        public void NthOrDefaultSimpleTest()
        {
            var numbers = Enumerable.Range(1, 100);

            for (int i = 1; i < 101; i++)
            {
                Assert.IsTrue(numbers.NthOrDefault(i) == i);
            }

            Assert.IsTrue(numbers.NthOrDefault(101) == default(int));
        }

        // NthOrDefault using predicate Tests

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NthOrDefaultUsingPredicateWithNullSequence()
        {
            (null as IEnumerable<int>).NthOrDefault(1, x => x % 2 == 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NthOrDefaultUsingPredicateWithNullPredicate()
        {
            Enumerable.Range(1, 10).NthOrDefault(1, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NthOrDefaultUsingPredicateWithNthZero()
        {
            Enumerable.Range(1, 10).NthOrDefault(0, x => x % 2 == 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NthOrDefaultUsingPredicateWithNthNegative()
        {
            Enumerable.Range(1, 10).NthOrDefault(-1, x => x % 2 == 0);
        }

        [Test]
        public void NthOrDefaultUsingPredicateSimpleTest()
        {
            var numbers = Enumerable.Range(1, 100);
            var evens = numbers.Where(x => x % 2 == 0).ToArray();

            for (int i = 1; i < 51; i++)
            {
                Assert.IsTrue(numbers.NthOrDefault(i, x => x % 2 == 0) == evens[i - 1]);
            }

            Assert.IsTrue(numbers.NthOrDefault(51, x => x % 2 == 0) == default(int));
        }

        // Nth Tests

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NthWithNullSequence()
        {
            (null as IEnumerable<int>).Nth(1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NthWithNthZero()
        {
            Enumerable.Range(1, 10).Nth(0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NthWithNthNegative()
        {
            Enumerable.Range(1, 10).Nth(-1);
        }

        [Test]
        public void NthSimpleTest()
        {
            var numbers = Enumerable.Range(1, 100);

            for (int i = 1; i < 101; i++)
            {
                Assert.IsTrue(numbers.Nth(i) == i);
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NthReturningExceptionSimpleTest()
        {
            Enumerable.Range(1, 100).Nth(101);
        }

        // Nth using predicate Tests

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NthUsingPredicateWithNullSequence()
        {
            (null as IEnumerable<int>).Nth(1, x => x % 2 == 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NthUsingPredicateWithNullPredicate()
        {
            Enumerable.Range(1, 10).Nth(1, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NthUsingPredicateWithNthZero()
        {
            Enumerable.Range(1, 10).Nth(0, x => x % 2 == 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NthUsingPredicateWithNthNegative()
        {
            Enumerable.Range(1, 10).Nth(-1, x => x % 2 == 0);
        }

        [Test]
        public void NthUsingPredicateSimpleTest()
        {
            var numbers = Enumerable.Range(1, 100);
            var evens = numbers.Where(x => x % 2 == 0).ToArray();

            for (int i = 1; i < 51; i++)
            {
                Assert.IsTrue(numbers.Nth(i, x => x % 2 == 0) == evens[i - 1]);
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NthUsingPredicateReturningExceptionSimpleTest()
        {
            Enumerable.Range(1, 100).Nth(51, x => x % 2 == 0);
        }
    }
}
