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
    public class PairwiseTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PairwiseNullSource()
        {
            MoreEnumerable.Pairwise<object, object>(null, delegate { return 0; });
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PairwiseNullResultSelector()
        {
            new object[0].Pairwise<object, object>(null);
        }

        [Test]
        public void PairwiseIsLazy()
        {
            new BreakingSequence<object>().Pairwise(delegate { return 0; });
        }

        [Test]
        public void PairwiseWideSourceSequence()
        {
            var result = new[] { 123, 456, 789 }.Pairwise((a, b) => new { A = a, B = b, });
            result.AssertSequenceEqual(new { A = 123, B = 456, },
                                       new { A = 456, B = 789, });
        }
    }
}
