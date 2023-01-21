#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Felipe Sateler. All rights reserved.
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
    using System;
    using NUnit.Framework;

    public class FromTest
    {
        [Test]
        public void TestFromIsLazy()
        {
            var breakingFunc = BreakingFunc.Of<int>();
            _ = MoreEnumerable.From(breakingFunc);
            _ = MoreEnumerable.From(breakingFunc, breakingFunc);
            _ = MoreEnumerable.From(breakingFunc, breakingFunc, breakingFunc);
            _ = MoreEnumerable.From(breakingFunc, breakingFunc, breakingFunc, breakingFunc);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void TestFromInvokesMethods(int numArgs)
        {
            int F1() => -2;
            int F2() => 4;
            int F3() => int.MaxValue;
            int F4() => int.MinValue;

            switch (numArgs)
            {
                case 1: MoreEnumerable.From(F1).AssertSequenceEqual(F1()); break;
                case 2: MoreEnumerable.From(F1, F2).AssertSequenceEqual(F1(), F2()); break;
                case 3: MoreEnumerable.From(F1, F2, F3).AssertSequenceEqual(F1(), F2(), F3()); break;
                case 4: MoreEnumerable.From(F1, F2, F3, F4).AssertSequenceEqual(F1(), F2(), F3(), F4()); break;
                default: throw new ArgumentOutOfRangeException(nameof(numArgs));
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void TestFromInvokesMethodsMultipleTimes(int numArgs)
        {
            var evals = new[] { 0, 0, 0, 0 };
            int F1() { evals[0]++; return -2; }
            int F2() { evals[1]++; return -2; }
            int F3() { evals[2]++; return -2; }
            int F4() { evals[3]++; return -2; }

            var results = numArgs switch
            {
                1 => MoreEnumerable.From(F1),
                2 => MoreEnumerable.From(F1, F2),
                3 => MoreEnumerable.From(F1, F2, F3),
                4 => MoreEnumerable.From(F1, F2, F3, F4),
                _ => throw new ArgumentOutOfRangeException(nameof(numArgs))
            };

            results.Consume();
            results.Consume();
            results.Consume();

            // numArgs functions were evaluated...
            evals.Take(numArgs).AssertSequenceEqual(Enumerable.Repeat(3, numArgs));
            // safety check: we didn't evaluate functions past numArgs
            evals.Skip(numArgs).AssertSequenceEqual(Enumerable.Repeat(0, evals.Length - numArgs));
        }
    }
}
