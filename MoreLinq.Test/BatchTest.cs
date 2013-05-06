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

namespace MoreLinq.Test
{
    [TestFixture]
    public class BatchTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BatchNullSequence()
        {
            MoreEnumerable.Batch<object>(null, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchZeroSize()
        {
            MoreEnumerable.Batch(new object[0], 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BatchNegativeSize()
        {
            MoreEnumerable.Batch(new object[0], -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BatcWithhNullResultSelector()
        {
            MoreEnumerable.Batch<object, object>(new object[0], 1, null);
        }

        [Test]
        public void BatchEvenlyDivisibleSequence()
        {
            var result = MoreEnumerable.Batch(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 3);
            using (var reader = result.Read())
            {
                reader.Read().AssertSequenceEqual(1, 2, 3);
                reader.Read().AssertSequenceEqual(4, 5, 6);
                reader.Read().AssertSequenceEqual(7, 8, 9);
                reader.ReadEnd();
            }
        }

        [Test]
        public void BatchUnevenlyDivisbleSequence()
        {
            var result = MoreEnumerable.Batch(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 4);
            using (var reader = result.Read())
            {
                reader.Read().AssertSequenceEqual(1, 2, 3, 4);
                reader.Read().AssertSequenceEqual(5, 6, 7, 8);
                reader.Read().AssertSequenceEqual(9);
                reader.ReadEnd();
            }
        }

        [Test]
        public void BatchSequenceTransformingResult()
        {
            var result = MoreEnumerable.Batch(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 4, batch => batch.Sum());
            result.AssertSequenceEqual(10, 26, 9);
        }

        [Test]
        public void BatchSequenceYieldsBatches()
        {
            var result = MoreEnumerable.Batch(new[] { 1, 2, 3 }, 2);
            using (var reader = result.Read())
            {
                Assert.That(reader.Read(), Is.Not.InstanceOf(typeof(ICollection<int>)));
                Assert.That(reader.Read(), Is.Not.InstanceOf(typeof(ICollection<int>)));
                reader.ReadEnd();
            }
        }

        [Test]
        public void BatchIsLazy()
        {
            MoreEnumerable.Batch(new BreakingSequence<object>(), 1);
        }
    }
}
