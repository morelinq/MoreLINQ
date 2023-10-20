#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Leandro F. Vieira (leandromoh). All rights reserved.
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
    public class SequenceTest
    {
        [TestCase(-10,  -4)]
        [TestCase( -1,   5)]
        [TestCase(  1,  10)]
        [TestCase( 30,  55)]
        [TestCase( 27, 172)]
        public void SequenceWithAscendingRange(int start, int stop)
        {
            var result = MoreEnumerable.Sequence(start, stop);
            var expectations = Enumerable.Range(start, stop - start + 1);

            Assert.That(result, Is.EqualTo(expectations));
        }

        [TestCase( -4, -10)]
        [TestCase(  5,  -1)]
        [TestCase( 10,   1)]
        [TestCase( 55,  30)]
        [TestCase(172,  27)]
        public void SequenceWithDescendingRange(int start, int stop)
        {
            var result = MoreEnumerable.Sequence(start, stop);
            var expectations = Enumerable.Range(stop, start - stop + 1).Reverse();

            Assert.That(result, Is.EqualTo(expectations));
        }

        [TestCase(-10,  -4, 2)]
        [TestCase( -1,   5, 3)]
        [TestCase(  1,  10, 1)]
        [TestCase( 30,  55, 4)]
        [TestCase( 27, 172, 9)]
        public void SequenceWithAscendingRangeAscendingStep(int start, int stop, int step)
        {
            var result = MoreEnumerable.Sequence(start, stop, step);
            var expectations = Enumerable.Range(start, stop - start + 1).TakeEvery(step);

            Assert.That(result, Is.EqualTo(expectations));
        }

        [TestCase(-10,  -4, -2)]
        [TestCase( -1,   5, -3)]
        [TestCase(  1,  10, -1)]
        [TestCase( 30,  55, -4)]
        [TestCase( 27, 172, -9)]
        public void SequenceWithAscendingRangeDescendigStep(int start, int stop, int step)
        {
            var result = MoreEnumerable.Sequence(start, stop, step);

            Assert.That(result, Is.Empty);
        }

        [TestCase( -4, -10, 2)]
        [TestCase(  5,  -1, 3)]
        [TestCase( 10,   1, 1)]
        [TestCase( 55,  30, 4)]
        [TestCase(172,  27, 9)]
        public void SequenceWithDescendingRangeAscendingStep(int start, int stop, int step)
        {
            var result = MoreEnumerable.Sequence(start, stop, step);

            Assert.That(result, Is.Empty);
        }

        [TestCase( -4, -10, -2)]
        [TestCase(  5,  -1, -3)]
        [TestCase( 10,   1, -1)]
        [TestCase( 55,  30, -4)]
        [TestCase(172,  27, -9)]
        public void SequenceWithDescendingRangeDescendigStep(int start, int stop, int step)
        {
            var result = MoreEnumerable.Sequence(start, stop, step);
            var expectations = Enumerable.Range(stop, start - stop + 1).Reverse().TakeEvery(Math.Abs(step));

            Assert.That(result, Is.EqualTo(expectations));
        }

        [TestCase(int.MaxValue, int.MaxValue,   -1)]
        [TestCase(int.MaxValue, int.MaxValue,    1)]
        [TestCase(int.MaxValue, int.MaxValue, null)]
        [TestCase(           0,            0,   -1)]
        [TestCase(           0,            0,    1)]
        [TestCase(           0,            0, null)]
        [TestCase(int.MinValue, int.MinValue,   -1)]
        [TestCase(int.MinValue, int.MinValue,    1)]
        [TestCase(int.MinValue, int.MinValue, null)]
        public void SequenceWithStartEqualsStop(int start, int stop, int? step)
        {
            var result = step.HasValue ? MoreEnumerable.Sequence(start, stop, step.Value)
                                       : MoreEnumerable.Sequence(start, stop);

            Assert.That(start, Is.EqualTo(result.Single()));
        }

        [TestCase(int.MaxValue - 1, int.MaxValue,            1,                             2)]
        [TestCase(int.MinValue + 1, int.MinValue,           -1,                             2)]
        [TestCase(               0, int.MaxValue,     10000000, (int.MaxValue / 10000000) + 1)]
        [TestCase(    int.MinValue, int.MaxValue, int.MaxValue,                             3)]
        public void SequenceEdgeCases(int start, int stop, int step, int count)
        {
            var result = MoreEnumerable.Sequence(start, stop, step);

            Assert.That(result.Count(), Is.EqualTo(count));
        }

        [TestCase(           5,           10)]
        [TestCase(int.MaxValue, int.MaxValue)]
        [TestCase(int.MinValue, int.MaxValue)]
        public void SequenceWithStepZero(int start, int stop)
        {
            var result = MoreEnumerable.Sequence(start, stop, 0);

            Assert.That(result.Take(100).All(x => x == start), Is.True);
        }
    }
}
