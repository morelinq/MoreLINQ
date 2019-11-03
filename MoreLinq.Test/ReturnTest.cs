#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Mitch Bodmer. All rights reserved.
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
    using NUnit.Framework;

    class ReturnTest
    {
        [Test]
        public void TestResultingSequenceContainsSingle()
        {
            MoreEnumerable.Return(new object()).AssertCount(1);
        }

        [Test]
        public void TestResultingSequenceContainsItemProvided()
        {
            var item = new object();
            MoreEnumerable.Return(item).Assert(containedItem => containedItem == item);
        }
    }
}
