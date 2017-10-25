#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Leandro F. Vieira (leandromoh). All rights reserved.
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
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class FlattenTest
    {
        [Test]
        public void Flatten()
        {
            var source = new object[]
            {
                1,
                2,
                new object[]
                {
                    3,
                    new object[]
                    {
                        4,
                        "foo"
                    },
                    5,
                    true,
                },
                "bar",
                6,
                new[]
                {
                    7,
                    8,
                    9,
                    10
                },
            };

            var result = source.Flatten();

            var expectations = new object[]
            {
                1,
                2,
                3,
                4,
                "foo",
                5,
                true,
                "bar",
                6,
                7,
                8,
                9,
                10
            };

            Assert.That(result, Is.EqualTo(expectations));
        }


        public void FlattenCast()
        {
            var source = new object[]
            {
                1, 2, 3, 4, 5,
                new object[]
                {
                    6, 7,
                    new object[]
                    {
                        8, 9,
                        new object[]
                        {
                            10, 11, 12,
                        },
                        13, 14, 15,
                    },
                    16, 17,
                },
                18, 19, 20,
            };

            var result = source.Flatten().Cast<int>();
            var expectations = Enumerable.Range(1, 20);

            Assert.That(result, Is.EquivalentTo(expectations));
        }

        [Test]
        public void FlattenIsLazy()
        {
            new BreakingSequence<int>().Flatten();
        }

        [Test]
        public void FlattenPredicate()
        {
            var source = new object[]
            {
                1,
                2,
                3,
                "bar",
                new object[]
                {
                    4,
                    new[]
                    {
                        true, false
                    },
                    5,
                },
                6,
                7,
            };

            var result = source.Flatten(obj => !(obj is IEnumerable<bool>));

            var expectations = new object[]
            {
                1,
                2,
                3,
                'b',
                'a',
                'r',
                4,
                new[]
                {
                    true,
                    false
                },
                5,
                6,
                7,
            };

            Assert.That(result, Is.EquivalentTo(expectations));
        }

        [Test]
        public void FlattenPredicateAlwaysFalse()
        {
            var source = new object[]
            {
                1,
                2,
                3,
                "bar",
                new[]
                {
                    true,
                    false,
                },
                6
            };

            var result = source.Flatten(_ => false);

            result.AssertSequenceEqual(source);
        }

        [Test]
        public void FlattenPredicateAlwaysTrue()
        {
            var source = new object[]
            {
                1,
                2,
                "bar",
                3,
                new[]
                {
                    4,
                    5,
                },
                6
            };

            var result = source.Flatten(_ => true);

            result.AssertSequenceEqual(1, 2, 'b', 'a', 'r', 3, 4, 5, 6);
        }

        [Test]
        public void FlattenPredicateIsLazy()
        {
            new BreakingSequence<int>().Flatten(BreakingFunc.Of<object, bool>());
        }

        [Test]
        public void FlattenFullIteratedDisposesInnerSequences()
        {
            var inner1 = TestingSequence.Of(4, 5);
            var inner2 = TestingSequence.Of(true, false);
            var inner3 = TestingSequence.Of<object>(6, inner2, 7);

            var source = new object[]
            {
                inner1,
                inner3,
            };

            var expectations = new object[]
            {
                4, 
                5,
                6, 
                true, 
                false, 
                7,
            };

            using (var test = source.AsTestingSequence())
            {
                Assert.That(test.Flatten(), Is.EquivalentTo(expectations));
            }
 
            inner1.Dispose();
            inner2.Dispose();
            inner3.Dispose();
        }

        [Test]
        public void FlattenInterruptedIterationDisposesInnerSequences()
        {
            var inner1 = TestingSequence.Of(4, 5);

            var inner2 = new[] { true, false }
                               .Select<bool, bool>(x => throw new InvalidOperationException())
                               .AsTestingSequence();
            
            var inner3 = TestingSequence.Of<object>(6, inner2, 7);

            var source = new object[]
            {
                inner1,
                inner3,
            };

            using (var test = source.AsTestingSequence())
            {
                test.Flatten().Consume();
            }
 
            inner1.Dispose();
            inner2.Dispose();
            inner3.Dispose();
        }
    }
}
