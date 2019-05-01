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
            new BreakingSequence<int>().MaxBy(BreakingFunc.Of<int, int>());
        }

        [Test]
        public void MaxByReturnsMaxima()
        {
            Assert.AreEqual(new[] { "hello", "world" },
                            SampleData.Strings.MaxBy(x => x.Length));
        }

        [Test]
        public void MaxByNullComparer()
        {
            Assert.AreEqual(SampleData.Strings.MaxBy(x => x.Length),
                            SampleData.Strings.MaxBy(x => x.Length, null));
        }

        [Test]
        public void MaxByEmptySequence()
        {
            Assert.That(new string[0].MaxBy(x => x.Length), Is.Empty);
        }

        [Test]
        public void MaxByWithNaturalComparer()
        {
            Assert.AreEqual(new[] { "az" }, SampleData.Strings.MaxBy(x => x[1]));
        }

        [Test]
        public void MaxByWithComparer()
        {
            Assert.AreEqual(new[] { "aa" }, SampleData.Strings.MaxBy(x => x[1], SampleData.ReverseCharComparer));
        }

        [TestCase(0, ExpectedResult = new string[0]             )]
        [TestCase(1, ExpectedResult = new[] { "hello"          })]
        [TestCase(2, ExpectedResult = new[] { "hello", "world" })]
        [TestCase(3, ExpectedResult = new[] { "hello", "world" })]
        public string[] MaxByTakeReturnsMaxima(int count)
        {
            using (var strings = SampleData.Strings.AsTestingSequence())
                return strings.MaxBy(s => s.Length).Take(count).ToArray();
        }

        [TestCase(0, ExpectedResult = new string[0]             )]
        [TestCase(1, ExpectedResult = new[] { "world"          })]
        [TestCase(2, ExpectedResult = new[] { "hello", "world" })]
        [TestCase(3, ExpectedResult = new[] { "hello", "world" })]
        public string[] MaxByTakeLastReturnsMaxima(int count)
        {
            using (var strings = SampleData.Strings.AsTestingSequence())
                return strings.MaxBy(s => s.Length).TakeLast(count).ToArray();
        }

        [TestCase(0, 0, ExpectedResult = new string[0]                         )]
        [TestCase(3, 1, ExpectedResult = new[] { "aa"                         })]
        [TestCase(1, 0, ExpectedResult = new[] { "ax"                         })]
        [TestCase(2, 0, ExpectedResult = new[] { "ax", "aa"                   })]
        [TestCase(3, 0, ExpectedResult = new[] { "ax", "aa", "ab"             })]
        [TestCase(4, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay"       })]
        [TestCase(5, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
        [TestCase(6, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
        public string[] MaxByTakeWithComparerReturnsMaxima(int count, int index)
        {
            using (var strings = SampleData.Strings.AsTestingSequence())
                return strings.MaxBy(s => s[index], SampleData.ReverseCharComparer)
                              .Take(count)
                              .ToArray();
        }

        [TestCase(0, 0, ExpectedResult = new string[0]                         )]
        [TestCase(3, 1, ExpectedResult = new[] { "aa"                         })]
        [TestCase(1, 0, ExpectedResult = new[] { "az"                         })]
        [TestCase(2, 0, ExpectedResult = new[] { "ay", "az"                   })]
        [TestCase(3, 0, ExpectedResult = new[] { "ab", "ay", "az"             })]
        [TestCase(4, 0, ExpectedResult = new[] { "aa", "ab", "ay", "az"       })]
        [TestCase(5, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
        [TestCase(6, 0, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
        public string[] MaxByTakeLastWithComparerReturnsMaxima(int count, int index)
        {
            using (var strings = SampleData.Strings.AsTestingSequence())
                return strings.MaxBy(s => s[index], SampleData.ReverseCharComparer)
                              .TakeLast(count)
                              .ToArray();
        }
    }
}
