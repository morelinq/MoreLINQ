#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Vegard Løkken. All rights reserved.
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
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class IfTest
    {
        [Test]
        [TestCase(true, ExpectedResult = new[] { "FIRST", "SECOND" })]
        [TestCase(false, ExpectedResult = new[] { "first", "second" })]
        public IEnumerable<string> IfConditionThenToUpper(bool condition)
        {
            var sequence = new[] { "first", "second" };
            return sequence.If(condition, s => s.Select(i => i.ToUpper()));
        }

        [Test]
        [TestCase("first", ExpectedResult = new[] { "FIRST", "SECOND" })]
        [TestCase("second", ExpectedResult = new[] { "first", "second" })]
        public IEnumerable<string> IfFirstElementMatchesThenToUpper(string match)
        {
            var sequence = new[] { "first", "second" };
            return sequence.If(s => s.First() == match, s => s.Select(i => i.ToUpper()));
        }
    }
}
