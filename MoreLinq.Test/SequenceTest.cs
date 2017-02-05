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
using System.Linq;

namespace MoreLinq.Test
{
    [TestFixture]
    public class SequenceTest
    {
        [TestCase(1, 10)]
        [TestCase(30, 55)]
        [TestCase(27, 172)]
        public void SequenceAscendingTest(int start, int stop)
        {
            Assert.That(MoreEnumerable.Sequence(start, stop), Is.EqualTo(Enumerable.Range(start, stop - start + 1)));
        }

        [TestCase(10, 1)]
        [TestCase(55, 30)]
        [TestCase(172, 27)]
        public void SequenceDescendingTest(int start, int stop)
        {
            Assert.That(MoreEnumerable.Sequence(start, stop), Is.EqualTo(Enumerable.Range(stop, start - stop + 1).Reverse()));
        }


        [TestCase(1, 10, 1)]
        [TestCase(30, 55, 4)]
        [TestCase(27, 172, 9)]
        public void SequenceAscendingWithAscendingStepTest(int start, int stop, int step)
        {
            Assert.That(MoreEnumerable.Sequence(start, stop, step), Is.EqualTo(Enumerable.Range(start, stop - start + 1).TakeEvery(step)));
        }

        [TestCase(1, 10, -1)]
        [TestCase(30, 55, -4)]
        [TestCase(27, 172, -9)]
        public void SequenceAscendingWithDescendigStepTest(int start, int stop, int step)
        {
            Assert.That(MoreEnumerable.Sequence(start, stop, step), Is.EqualTo(Enumerable.Empty<int>()));
        }

        [TestCase(10, 1, 1)]
        [TestCase(55, 30, 4)]
        [TestCase(172, 27, 9)]
        public void SequenceDescendingWithAscendingStepTest(int start, int stop, int step)
        {
            Assert.That(MoreEnumerable.Sequence(start, stop, step), Is.EqualTo(Enumerable.Empty<int>()));
        }

        [TestCase(10, 1, -1)]
        [TestCase(55, 30, -4)]
        [TestCase(172, 27, -9)]
        public void SequenceDescendingWithDescendigStepTest(int start, int stop, int step)
        {
            Assert.That(MoreEnumerable.Sequence(start, stop, step), Is.EqualTo(Enumerable.Range(stop, start - stop + 1).Reverse().TakeEvery(Math.Abs(step))));
        }
    }
}
