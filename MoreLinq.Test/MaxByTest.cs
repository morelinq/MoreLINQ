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
    public class MaxByTest
    {
        [Test]
        public void MaxByIsLazy()
        {
            _ = new BreakingSequence<int>().MaxBy(BreakingFunc.Of<int, int>());
        }

        [Test]
        public void MaxByReturnsMaxima()
        {
            Assert.That(SampleData.Strings.MaxBy(x => x.Length),
                        Is.EqualTo(new[] { "hello", "world" }));
        }

        [Test]
        public void MaxByNullComparer()
        {
            Assert.That(SampleData.Strings.MaxBy(x => x.Length, null),
                        Is.EqualTo(SampleData.Strings.MaxBy(x => x.Length)));
        }

        [Test]
        public void MaxByEmptySequence()
        {
            Assert.That(new string[0].MaxBy(x => x.Length), Is.Empty);
        }

        [Test]
        public void MaxByWithNaturalComparer()
        {
            Assert.That(SampleData.Strings.MaxBy(x => x[1]), Is.EqualTo(new[] { "az" }));
        }

        [Test]
        public void MaxByWithComparer()
        {
            Assert.That(SampleData.Strings.MaxBy(x => x[1], Comparable<char>.DescendingOrderComparer), Is.EqualTo(new[] { "aa" }));
        }

        public class First
        {
            [Test]
            public void ReturnsMaximum()
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                var maxima = strings.MaxBy(s => s.Length);
                Assert.That(MoreEnumerable.First(maxima), Is.EqualTo("hello"));
            }

            [Test]
            public void WithComparerReturnsMaximum()
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                var maxima = strings.MaxBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
                Assert.That(MoreEnumerable.First(maxima), Is.EqualTo("ax"));
            }

            [Test]
            public void WithEmptySourceThrows()
            {
                using var strings = Enumerable.Empty<string>().AsTestingSequence();
                Assert.That(() =>
                    MoreEnumerable.First(strings.MaxBy(s => s.Length)),
                    Throws.InvalidOperationException);
            }

            [Test]
            public void WithEmptySourceWithComparerThrows()
            {
                using var strings = Enumerable.Empty<string>().AsTestingSequence();
                Assert.That(() =>
                    MoreEnumerable.First(strings.MaxBy(s => s.Length, Comparable<int>.DescendingOrderComparer)),
                    Throws.InvalidOperationException);
            }
        }

        public class FirstOrDefault
        {
            [Test]
            public void ReturnsMaximum()
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                var maxima = strings.MaxBy(s => s.Length);
                Assert.That(MoreEnumerable.FirstOrDefault(maxima), Is.EqualTo("hello"));
            }

            [Test]
            public void WithComparerReturnsMaximum()
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                var maxima = strings.MaxBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
                Assert.That(MoreEnumerable.FirstOrDefault(maxima), Is.EqualTo("ax"));
            }

            [Test]
            public void WithEmptySourceReturnsDefault()
            {
                using var strings = Enumerable.Empty<string>().AsTestingSequence();
                var maxima = strings.MaxBy(s => s.Length);
                Assert.That(MoreEnumerable.FirstOrDefault(maxima), Is.Null);
            }

            [Test]
            public void WithEmptySourceWithComparerReturnsDefault()
            {
                using var strings = Enumerable.Empty<string>().AsTestingSequence();
                var maxima = strings.MaxBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
                Assert.That(MoreEnumerable.FirstOrDefault(maxima), Is.Null);
            }
        }

        public class Last
        {
            [Test]
            public void ReturnsMaximum()
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                var maxima = strings.MaxBy(s => s.Length);
                Assert.That(MoreEnumerable.Last(maxima), Is.EqualTo("world"));
            }

            [Test]
            public void WithComparerReturnsMaximumPerComparer()
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                var maxima = strings.MaxBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
                Assert.That(MoreEnumerable.Last(maxima), Is.EqualTo("az"));
            }

            [Test]
            public void WithEmptySourceThrows()
            {
                using var strings = Enumerable.Empty<string>().AsTestingSequence();
                Assert.That(() =>
                    MoreEnumerable.Last(strings.MaxBy(s => s.Length)),
                    Throws.InvalidOperationException);
            }

            [Test]
            public void WithEmptySourceWithComparerThrows()
            {
                using var strings = Enumerable.Empty<string>().AsTestingSequence();
                Assert.That(() =>
                    MoreEnumerable.Last(strings.MaxBy(s => s.Length, Comparable<int>.DescendingOrderComparer)),
                    Throws.InvalidOperationException);
            }
        }

        public class LastOrDefault
        {
            [Test]
            public void ReturnsMaximum()
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                var maxima = strings.MaxBy(s => s.Length);
                Assert.That(MoreEnumerable.LastOrDefault(maxima), Is.EqualTo("world"));
            }

            [Test]
            public void WithComparerReturnsMaximumPerComparer()
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                var maxima = strings.MaxBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
                Assert.That(MoreEnumerable.LastOrDefault(maxima), Is.EqualTo("az"));
            }

            [Test]
            public void WithEmptySourceReturnsDefault()
            {
                using var strings = Enumerable.Empty<string>().AsTestingSequence();
                var maxima = strings.MaxBy(s => s.Length);
                Assert.That(MoreEnumerable.LastOrDefault(maxima), Is.Null);
            }

            [Test]
            public void WithEmptySourceWithComparerReturnsDefault()
            {
                using var strings = Enumerable.Empty<string>().AsTestingSequence();
                var maxima = strings.MaxBy(s => s.Length, Comparable<int>.DescendingOrderComparer);
                Assert.That(MoreEnumerable.LastOrDefault(maxima), Is.Null);
            }
        }

        public class Take
        {
            [TestCase(0, ExpectedResult = new string[0]             )]
            [TestCase(1, ExpectedResult = new[] { "hello"          })]
            [TestCase(2, ExpectedResult = new[] { "hello", "world" })]
            [TestCase(3, ExpectedResult = new[] { "hello", "world" })]
            public string[] ReturnsMaxima(int count)
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                return strings.MaxBy(s => s.Length).Take(count).ToArray();
            }

            [TestCase(0, 0, ExpectedResult = new string[0]                         )]
            [TestCase(3, 1, ExpectedResult = new[] { "aa"                         })]
            [TestCase(1, 0, ExpectedResult = new[] { "ax"                         })]
            [TestCase(2, 0, ExpectedResult = new[] { "ax", "aa"                   })]
            [TestCase(3, 0, ExpectedResult = new[] { "ax", "aa", "ab"             })]
            [TestCase(4, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay"       })]
            [TestCase(5, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
            [TestCase(6, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
            public string[] WithComparerReturnsMaximaPerComparer(int count, int index)
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                return strings.MaxBy(s => s[index], Comparable<char>.DescendingOrderComparer)
                              .Take(count)
                              .ToArray();
            }
        }

        public class TakeLast
        {
            [TestCase(0, ExpectedResult = new string[0]             )]
            [TestCase(1, ExpectedResult = new[] { "world"          })]
            [TestCase(2, ExpectedResult = new[] { "hello", "world" })]
            [TestCase(3, ExpectedResult = new[] { "hello", "world" })]
            public string[] TakeLastReturnsMaxima(int count)
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                return strings.MaxBy(s => s.Length).TakeLast(count).ToArray();
            }

            [TestCase(0, 0, ExpectedResult = new string[0]                         )]
            [TestCase(3, 1, ExpectedResult = new[] { "aa"                         })]
            [TestCase(1, 0, ExpectedResult = new[] { "az"                         })]
            [TestCase(2, 0, ExpectedResult = new[] { "ay", "az"                   })]
            [TestCase(3, 0, ExpectedResult = new[] { "ab", "ay", "az"             })]
            [TestCase(4, 0, ExpectedResult = new[] { "aa", "ab", "ay", "az"       })]
            [TestCase(5, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
            [TestCase(6, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
            public string[] WithComparerReturnsMaximaPerComparer(int count, int index)
            {
                using var strings = SampleData.Strings.AsTestingSequence();
                return strings.MaxBy(s => s[index], Comparable<char>.DescendingOrderComparer)
                              .TakeLast(count)
                              .ToArray();
            }
        }
    }
}
