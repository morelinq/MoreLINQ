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
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class ScanTest
    {
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ScanEmpty()
        {
            (new int[] { }).Scan(SampleData.Plus).Count();
        }

        [Test]
        public void ScanSum()
        {
            var result = SampleData.Values.Scan(SampleData.Plus);
            var gold = SampleData.Values.PreScan(SampleData.Plus, 0).Zip(SampleData.Values, SampleData.Plus);
            result.AssertSequenceEqual(gold);
        }

        [Test]
        public void ScanIsLazy()
        {
            new BreakingSequence<object>().Scan<object>(delegate { throw new NotImplementedException(); });
        }
    }
}
