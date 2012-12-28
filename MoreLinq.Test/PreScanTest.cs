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
    public class PreScanTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PreScanNullSequence()
        {
            MoreEnumerable.PreScan(null, SampleData.Plus, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PreScanNullOperation()
        {
            SampleData.Values.PreScan(null, 0);
        }

        [Test]
        public void PreScanSum()
        {
            var result = SampleData.Values.PreScan(SampleData.Plus, 0);
            var gold = new[] { 0, 1, 3, 6, 10, 15, 21, 28, 36, 45 };
            result.AssertSequenceEqual(gold);
        }

        [Test]
        public void PreScanMul()
        {
            var seq = new[] { 1, 2, 3 };
            var gold = new[] { 1, 1, 2 };
            var result = seq.PreScan(SampleData.Mul, 1);
            result.AssertSequenceEqual(gold);
        }
    }
}
