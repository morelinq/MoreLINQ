#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008-2011 Jonathan Skeet. All rights reserved.
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
            MoreEnumerable.Pairwise<object, object>(new object[0], null);
        }

        [Test]
        public void PairwiseIsLazy()
        {
            MoreEnumerable.Pairwise(new BreakingSequence<object>(), delegate { return 0; });
        }

        [Test]
        public void PairwiseWideSourceSequence()
        {
            var result = MoreEnumerable.Pairwise(new[] { 123, 456, 789 }, (a, b) => new { A = a, B = b, });
            result.AssertSequenceEqual(new { A = 123, B = 456, },
                                       new { A = 456, B = 789, });
        }
    }
}
