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

namespace MoreLinq.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class GenerateTest
    {
        [Test]
        public void GenerateTerminatesWhenCheckReturnsFalse()
        {
            var result = MoreEnumerable.Generate(1, n => n + 2).TakeWhile(n => n < 10);

            result.AssertSequenceEqual(1, 3, 5, 7, 9);
        }

        [Test]
        public void GenerateProcessesNonNumerics()
        {
            var result = MoreEnumerable.Generate("", s => s + 'a').TakeWhile(s => s.Length < 5);

            result.AssertSequenceEqual("", "a", "aa", "aaa", "aaaa");
        }

        [Test]
        public void GenerateIsLazy()
        {
            _ = MoreEnumerable.Generate(0, BreakingFunc.Of<int, int>());
        }

        [Test]
        public void GenerateFuncIsNotInvokedUnnecessarily()
        {
            MoreEnumerable.Generate(0, BreakingFunc.Of<int, int>())
                          .Take(1)
                          .Consume();
        }

        [Test]
        public void GenerateByIndexIsLazy()
        {
            _ = MoreEnumerable.GenerateByIndex(BreakingFunc.Of<int, int>());
        }

        [Test]
        public void GenerateByIndex()
        {
            var sequence = MoreEnumerable.GenerateByIndex(x => x.ToInvariantString()).Take(3);
            sequence.AssertSequenceEqual("0", "1", "2");
        }
    }
}
