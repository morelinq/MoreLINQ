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

namespace MoreLinq.Test
{
    [TestFixture]
    public class EquiZipTest
    {
        private static Tuple<TFirst, TSecond> Tuple<TFirst, TSecond>(TFirst a, TSecond b)
        {
            return new Tuple<TFirst, TSecond>(a, b);
        }

        [Test]
        public void BothSequencesDisposedWithUnequalLengthsAndLongerFirst()
        {
            using (var longer = TestingSequence.Of(1, 2, 3))
            using (var shorter = TestingSequence.Of(1, 2))
            {
                // Yes, this will throw... but then we should still have disposed both sequences
                try
                {
                    longer.EquiZip(shorter, (x, y) => x + y).Consume();
                }
                catch (InvalidOperationException)
                {
                    // Expected
                }
            }
        }

        [Test]
        public void BothSequencesDisposedWithUnequalLengthsAndShorterFirst()
        {
            using (var longer = TestingSequence.Of(1, 2, 3))
            using (var shorter = TestingSequence.Of(1, 2))
            {
                // Yes, this will throw... but then we should still have disposed both sequences
                try
                {
                    shorter.EquiZip(longer, (x, y) => x + y).Consume();
                }
                catch (InvalidOperationException)
                {
                    // Expected
                }
            }
        }

        [Test]
        public void ZipWithEqualLengthSequencesFailStrategy()
        {
            var zipped = new[] { 1, 2, 3 }.EquiZip(new[] { 4, 5, 6 }, Tuple);
            Assert.That(zipped, Is.Not.Null);
            zipped.AssertSequenceEqual(Tuple(1, 4), Tuple(2, 5), Tuple(3, 6));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ZipWithFirstSequenceShorterThanSecondFailStrategy()
        {
            var zipped = new[] { 1, 2 }.EquiZip(new[] { 4, 5, 6 }, Tuple);
            Assert.That(zipped, Is.Not.Null);
            zipped.Consume();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ZipWithFirstSequnceLongerThanSecondFailStrategy()
        {
            var zipped = new[] { 1, 2, 3 }.EquiZip(new[] { 4, 5 }, Tuple);
            Assert.That(zipped, Is.Not.Null);
            zipped.Consume();
            zipped.AssertSequenceEqual(Tuple(1, 4), Tuple(2, 5));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ZipWithNullFirstSequence()
        {
            MoreEnumerable.EquiZip(null, new[] { 4, 5, 6 }, BreakingFunc.Of<int, int, int>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ZipWithNullSecondSequence()
        {
            new[] { 1, 2, 3 }.EquiZip(null, BreakingFunc.Of<int, int, int>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ZipWithNullResultSelector()
        {
            new[] { 1, 2, 3 }.EquiZip<int, int, int>(new[] { 4, 5, 6 }, null);
        }

        [Test]
        public void ZipIsLazy()
        {
            var bs = new BreakingSequence<int>();
            bs.EquiZip<int, int, int>(bs, delegate { throw new NotImplementedException(); });
        }
    }
}
