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
    public class FillBackwardTest
    {
        [Test]
        public void FillBackwardWithNullSequence()
        {
            var e = Assert.Throws<ArgumentNullException>(() => MoreEnumerable.FillBackward<object>(null));
            Assert.That(e.ParamName, Is.EqualTo("source"));
        }

        [Test]
        public void FillBackwardWithNullPredicate()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new object[0].FillBackward(null));
            Assert.That(e.ParamName, Is.EqualTo("predicate"));
        }

        [Test]
        public void FillBackwardIsLazy()
        {
            new BreakingSequence<object>().FillBackward();
        }

        [Test]
        public void FillBackward()
        {
            int? na = null;
            var input = new[] { na, na, 1, 2, na, na, na, 3, 4, na, na };
            var result = input.FillBackward();
            Assert.That(result, Is.EquivalentTo(new[] { 1, 1, 1, 2, 3, 3, 3, 3, 4, na, na }));
        }
    }
}