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
    using System.Collections.Generic;
    using NUnit.Framework;

    class FromTest
    {
        [Test]
        public void TestFromIsLazy()
        {
            var breakingFunc = BreakingFunc.Of<int>();
            MoreEnumerable.From(breakingFunc);
            MoreEnumerable.From(breakingFunc, breakingFunc);
            MoreEnumerable.From(breakingFunc, breakingFunc, breakingFunc);
            MoreEnumerable.From(breakingFunc, breakingFunc, breakingFunc, breakingFunc);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void TestFromInvokesMethods(int numArgs)
        {
            var factories = new Func<int>[]
            {
                () => -2,
                () => 4,
                () => int.MaxValue,
                () => int.MinValue,
            };
            IEnumerable<int> results;
            int[] expectedResults;
            switch (numArgs)
            {
                case 1:
                    results = MoreEnumerable.From(factories[0]);
                    expectedResults = new[] { factories[0]() };
                    break;
                case 2:
                    results = MoreEnumerable.From(factories[0], factories[1]);
                    expectedResults = new[] { factories[0](), factories[1]() };
                    break;
                case 3:
                    results = MoreEnumerable.From(factories[0], factories[1], factories[2]);
                    expectedResults = new[] { factories[0](), factories[1](), factories[2]() };
                    break;
                case 4:
                    results = MoreEnumerable.From(factories[0], factories[1], factories[2], factories[3]);
                    expectedResults = new[] { factories[0](), factories[1](), factories[2](), factories[3]() };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(numArgs));
            }

            results.AssertSequenceEqual(expectedResults);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void TestFromInvokesMethodsMultipleTimes(int numArgs)
        {
            var evals = new int[] { 0, 0, 0, 0 };
            var factories = new Func<int>[]
            {
                () => { evals[0]++; return -2; },
                () => { evals[1]++; return -2; },
                () => { evals[2]++; return -2; },
                () => { evals[3]++; return -2; },
            };
            IEnumerable<int> results;
            switch (numArgs)
            {
                case 1:
                    results = MoreEnumerable.From(factories[0]);
                    break;
                case 2:
                    results = MoreEnumerable.From(factories[0], factories[1]);
                    break;
                case 3:
                    results = MoreEnumerable.From(factories[0], factories[1], factories[2]);
                    break;
                case 4:
                    results = MoreEnumerable.From(factories[0], factories[1], factories[2], factories[3]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(numArgs));
            }

            results.Consume();
            results.Consume();
            results.Consume();

            for (int i = 0; i < numArgs; i++)
            {
                Assert.That(evals[i], Is.EqualTo(3));
            }
            for (int i = numArgs; i < 4; i++)
            {
                Assert.That(evals[i], Is.EqualTo(0));
            }
        }
    }
}
