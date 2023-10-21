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
    using System.Collections.Generic;
    using System.Collections;
    using NUnit.Framework;

    [TestFixture]
    public class FlattenTest
    {
        // Flatten(this IEnumerable source)

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

        [Test]
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

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void FlattenIsLazy()
        {
            _ = new BreakingSequence<int>().Flatten();
        }

        // Flatten(this IEnumerable source, Func<IEnumerable, bool> predicate)

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

            var result = source.Flatten(obj => obj is not IEnumerable<bool>);

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

            Assert.That(result, Is.EqualTo(expectations));
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

            Assert.That(result, Is.EqualTo(source));
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

            var expectations = new object[]
            {
                1,
                2,
                'b',
                'a',
                'r',
                3,
                4,
                5,
                6
            };

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void FlattenPredicateIsLazy()
        {
            _ = new BreakingSequence<int>().Flatten(BreakingFunc.Of<object, bool>());
        }

        [Test]
        public void FlattenFullIteratedDisposesInnerSequences()
        {
            var expectations = new object[]
            {
                4,
                5,
                6,
                true,
                false,
                7,
            };

            using var inner1 = TestingSequence.Of(4, 5);
            using var inner2 = TestingSequence.Of(true, false);
            using var inner3 = TestingSequence.Of<object>(6, inner2, 7);
            using var source = TestingSequence.Of<object>(inner1, inner3);

            Assert.That(source.Flatten(), Is.EqualTo(expectations));
        }

        [Test]
        public void FlattenInterruptedIterationDisposesInnerSequences()
        {
            using var inner1 = TestingSequence.Of(4, 5);
            using var inner2 = MoreEnumerable.From(() => true,
                                                   () => false,
                                                   () => throw new TestException())
                                             .AsTestingSequence();
            using var inner3 = TestingSequence.Of<object>(6, inner2, 7);
            using var source = TestingSequence.Of<object>(inner1, inner3);

            Assert.That(() => source.Flatten().Consume(),
                        Throws.TypeOf<TestException>());
        }

        [Test]
        public void FlattenEvaluatesInnerSequencesLazily()
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
                        MoreEnumerable.From
                        (
                            () => 10,
                            () => throw new TestException(),
                            () => 12
                        ),
                        13, 14, 15,
                    },
                    16, 17,
                },
                18, 19, 20,
            };

            var result = source.Flatten().Cast<int>();
            var expectations = Enumerable.Range(1, 10);

            Assert.That(result.Take(10), Is.EqualTo(expectations));

            Assert.That(() => source.Flatten().ElementAt(11),
                        Throws.TypeOf<TestException>());
        }

        // Flatten(this IEnumerable source, Func<object, IEnumerable> selector)

        [Test]
        public void FlattenSelectorIsLazy()
        {
            _ = new BreakingSequence<int>().Flatten(BreakingFunc.Of<object, IEnumerable>());
        }

        [Test]
        public void FlattenSelector()
        {
            var source = new[]
            {
                new Series
                {
                    Name = "series1",
                    Attributes = new[]
                    {
                        new Attribute { Values = new[] { 1, 2 } },
                        new Attribute { Values = new[] { 3, 4 } },
                    }
                },
                new Series
                {
                    Name = "series2",
                    Attributes = new[]
                    {
                        new Attribute { Values = new[] { 5, 6 } },
                    }
                }
            };

            var result = source.Flatten(obj => obj switch
            {
                string => null,
                IEnumerable inner => inner,
                Series s => new object[] { s.Name, s.Attributes },
                Attribute a => a.Values,
                _ => null
            });

            var expectations = new object[] { "series1", 1, 2, 3, 4, "series2", 5, 6 };

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void FlattenSelectorFilteringOnlyIntegers()
        {
            var source = new object[]
            {
                true,
                false,
                1,
                "bar",
                new object[]
                {
                    2,
                    new[]
                    {
                        3,
                    },
                },
                'c',
                4,
            };

            var result = source.Flatten(obj => obj switch
            {
                int => null,
                IEnumerable inner => inner,
                _ => Enumerable.Empty<object>()
            });

            var expectations = new object[] { 1, 2, 3, 4 };

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void FlattenSelectorWithTree()
        {
            var source = new Tree<int>
            (
                new Tree<int>
                (
                    new Tree<int>(1),
                    2,
                    new Tree<int>(3)
                ),
                4,
                new Tree<int>
                (
                    new Tree<int>(5),
                    6,
                    new Tree<int>(7)
                )
            );

            var result = new[] { source }.Flatten(obj => obj switch
            {
                int => null,
                Tree<int> tree => new object?[] { tree.Left, tree.Value, tree.Right },
                IEnumerable inner => inner,
                _ => Enumerable.Empty<object>()
            });

            var expectations = Enumerable.Range(1, 7);

            Assert.That(result, Is.EqualTo(expectations));
        }

        sealed class Series
        {
            public required string Name;
            public required Attribute[] Attributes;
        }

        sealed class Attribute
        {
            public required int[] Values;
        }

        sealed class Tree<T>
        {
            public readonly T Value;
            public readonly Tree<T>? Left;
            public readonly Tree<T>? Right;

            public Tree(T value) : this(null, value, null) { }
            public Tree(Tree<T>? left, T value, Tree<T>? right)
            {
                Left = left;
                Value = value;
                Right = right;
            }
        }
    }
}
