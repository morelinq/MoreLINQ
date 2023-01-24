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
    public class MinByTest
    {
        [Test]
        public void MinByIsLazy()
        {
            _ = new BreakingSequence<int>().MinBy(BreakingFunc.Of<int, int>());
        }

        [Test]
        public void MinByReturnsMinima()
        {
            Assert.That(SampleData.Strings.MinBy(x => x.Length),
                        Is.EqualTo(new[] { "ax", "aa", "ab", "ay", "az" }));
        }

        [Test]
        public void MinByNullComparer()
        {
            Assert.That(SampleData.Strings.MinBy(x => x.Length, null),
                        Is.EqualTo(SampleData.Strings.MinBy(x => x.Length)));
        }

        [Test]
        public void MinByEmptySequence()
        {
            Assert.That(new string[0].MinBy(x => x.Length), Is.Empty);
        }

        [Test]
        public void MinByWithNaturalComparer()
        {
            Assert.That(SampleData.Strings.MinBy(x => x[1]), Is.EqualTo(new[] { "aa" }));
        }

        [Test]
        public void MinByWithComparer()
        {
            Assert.That(SampleData.Strings.MinBy(x => x[1], Comparable<char>.DescendingOrderComparer), Is.EqualTo(new[] { "az" }));
        }

        public class First
        {
            [Test]
            public void ReturnsMinimum()
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                var minima = MoreEnumerable.First(strings.MinBy(s => s.Length));
                Assert.That(minima, Is.EqualTo("ax"));
            }

            [Test]
            public void WithComparerReturnsMinimum()
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                var minima = strings.MinBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
                Assert.That(MoreEnumerable.First(minima), Is.EqualTo("hello"));
            }

            [Test]
            public void WithEmptySourceThrows()
            {
                using var strings = Enumerable.Empty<string>().AsTestingSequence();
                Assert.That(() => MoreEnumerable.First(strings.MinBy(s => s.Length)),
                            Throws.InvalidOperationException);
            }

            [Test]
            public void WithEmptySourceWithComparerThrows()
            {
                using var strings = Enumerable.Empty<string>().AsTestingSequence();
                Assert.That(() => MoreEnumerable.First(strings.MinBy(s => s.Length, Comparable<int>.DescendingOrderComparer)),
                            Throws.InvalidOperationException);
            }
        }

        public class FirstOrDefault
        {
            [Test]
            public void ReturnsMinimum()
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                var minima = strings.MinBy(s => s.Length);
                Assert.That(MoreEnumerable.FirstOrDefault(minima), Is.EqualTo("ax"));
            }

            [Test]
            public void WithComparerReturnsMinimum()
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                var minima = strings.MinBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
                Assert.That(MoreEnumerable.FirstOrDefault(minima), Is.EqualTo("hello"));
            }

            [Test]
            public void WithEmptySourceReturnsDefault()
            {
                using var strings = Enumerable.Empty<string>().AsTestingSequence();
                var minima = strings.MinBy(s => s.Length);
                Assert.That(MoreEnumerable.FirstOrDefault(minima), Is.Null);
            }

            [Test]
            public void WithEmptySourceWithComparerReturnsDefault()
            {
                using var strings = Enumerable.Empty<string>().AsTestingSequence();
                var minima = strings.MinBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
                Assert.That(MoreEnumerable.FirstOrDefault(minima), Is.Null);
            }
        }

        public class Last
        {
            [Test]
            public void ReturnsMinimum()
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                var minima = strings.MinBy(s => s.Length);
                Assert.That(MoreEnumerable.Last(minima), Is.EqualTo("az"));
            }

            [Test]
            public void WithComparerReturnsMinimumPerComparer()
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                var minima = strings.MinBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
                Assert.That(MoreEnumerable.Last(minima), Is.EqualTo("world"));
            }

            [Test]
            public void WithEmptySourceThrows()
            {
                using var strings = Enumerable.Empty<string>().AsTestingSequence();
                Assert.That(() => MoreEnumerable.Last(strings.MinBy(s => s.Length)),
                            Throws.InvalidOperationException);
            }

            [Test]
            public void WithEmptySourceWithComparerThrows()
            {
                using var strings = Enumerable.Empty<string>().AsTestingSequence();
                Assert.That(() => MoreEnumerable.Last(strings.MinBy(s => s.Length, Comparable<int>.DescendingOrderComparer)),
                            Throws.InvalidOperationException);
            }
        }

        public class LastOrDefault
        {
            [Test]
            public void ReturnsMinimum()
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                var minima = strings.MinBy(s => s.Length);
                Assert.That(MoreEnumerable.LastOrDefault(minima), Is.EqualTo("az"));
            }

            [Test]
            public void WithComparerReturnsMinimumPerComparer()
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                var minima = strings.MinBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
                Assert.That(MoreEnumerable.LastOrDefault(minima), Is.EqualTo("world"));
            }

            [Test]
            public void WithEmptySourceReturnsDefault()
            {
                using var strings = Enumerable.Empty<string>().AsTestingSequence();
                var minima = strings.MinBy(s => s.Length);
                Assert.That(MoreEnumerable.LastOrDefault(minima), Is.Null);
            }

            [Test]
            public void WithEmptySourceWithComparerReturnsDefault()
            {
                using var strings = Enumerable.Empty<string>().AsTestingSequence();
                var minima = strings.MinBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
                Assert.That(MoreEnumerable.LastOrDefault(minima), Is.Null);
            }
        }

        public class Take
        {
            [TestCase(0, ExpectedResult = new string[0]                         )]
            [TestCase(1, ExpectedResult = new[] { "ax"                         })]
            [TestCase(2, ExpectedResult = new[] { "ax", "aa"                   })]
            [TestCase(3, ExpectedResult = new[] { "ax", "aa", "ab"             })]
            [TestCase(4, ExpectedResult = new[] { "ax", "aa", "ab", "ay"       })]
            [TestCase(5, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
            [TestCase(6, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
            public string[] ReturnsMinima(int count)
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                return strings.MinBy(s => s.Length).Take(count).ToArray();
            }

            [TestCase(0, ExpectedResult = new string[0]             )]
            [TestCase(1, ExpectedResult = new[] { "hello",         })]
            [TestCase(2, ExpectedResult = new[] { "hello", "world" })]
            [TestCase(3, ExpectedResult = new[] { "hello", "world" })]
            public string[] WithComparerReturnsMinimaPerComparer(int count)
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                return strings.MinBy(s => s.Length, Comparable<int>.DescendingOrderComparer)
                              .Take(count)
                              .ToArray();
            }
        }

        public class TakeLast
        {
            [TestCase(0, ExpectedResult = new string[0]                         )]
            [TestCase(1, ExpectedResult = new[] { "az"                         })]
            [TestCase(2, ExpectedResult = new[] { "ay", "az"                   })]
            [TestCase(3, ExpectedResult = new[] { "ab", "ay", "az"             })]
            [TestCase(4, ExpectedResult = new[] { "aa", "ab", "ay", "az"       })]
            [TestCase(5, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
            [TestCase(6, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
            public string[] ReturnsMinima(int count)
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                return strings.MinBy(s => s.Length).TakeLast(count).ToArray();
            }

            [TestCase(0, ExpectedResult = new string[0]             )]
            [TestCase(1, ExpectedResult = new[] { "world",         })]
            [TestCase(2, ExpectedResult = new[] { "hello", "world" })]
            [TestCase(3, ExpectedResult = new[] { "hello", "world" })]
            public string[] WithComparerReturnsMinimaPerComparer(int count)
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                return strings.MinBy(s => s.Length, Comparable<int>.DescendingOrderComparer)
                              .TakeLast(count)
                              .ToArray();
            }
        }
    }
}
