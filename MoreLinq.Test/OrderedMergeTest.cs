#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Sergiy Zinovyev. All rights reserved.
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

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    /// <summary>
    /// Verify the behavior of the OrderBy/ThenBy operators
    /// </summary>
    [TestFixture]
    public class OrderedMergeTests
    {
        /// <summary>
        /// Verify that OrderedMerge produces correct collection
        /// </summary>
        [Test]
        public void TestOrderedMergeForTwoCollections()
        {
            var arr1 = Enumerable.Range(1, 5).ToList();
            var arr2 = Enumerable.Range(2, 5).ToList();

            var l1 = arr1.OrderedMerge(arr2).ToList();
            var r1 = arr1.Union(arr2).OrderBy(v => v).ToList();

            var r2 = new List<int>();
            r2.AddRange(arr1);
            r2.AddRange(arr2);
            r2 = r2.Distinct().OrderBy(v => v).ToList();
            
            // ensure both operations produce identical results
            Assert.IsTrue(l1.SequenceEqual(r1));
            Assert.IsTrue(l1.SequenceEqual(r2));
        }
    }
}
