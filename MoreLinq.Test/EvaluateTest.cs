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

    public class EvaluateTest
    {
        [Test]
        public void TestEvaluateIsLazy()
        {
            _ = new BreakingSequence<Func<int>>().Evaluate();
        }

        [Test]
        public void TestEvaluateInvokesMethods()
        {
            var factories = new Func<int>[]
            {
                () => -2,
                () => 4,
                () => int.MaxValue,
                () => int.MinValue,
            };
            var results = factories.Evaluate();

            results.AssertSequenceEqual(-2, 4, int.MaxValue, int.MinValue);
        }

        [Test]
        public void TestEvaluateInvokesMethodsMultipleTimes()
        {
            var evals = 0;
            var factories = new Func<int>[]
            {
                () => { evals++; return -2; },
            };
            var results = factories.Evaluate();

            results.Consume();
            results.Consume();
            results.Consume();

            Assert.That(evals, Is.EqualTo(3));
        }
    }
}
