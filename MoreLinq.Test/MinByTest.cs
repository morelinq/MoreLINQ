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
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class MinByTest
    {
        [Test]
        public void MinByIsLazy()
        {
            new BreakingSequence<int>().MinBy(BreakingFunc.Of<int, int>());
        }

        [Test]
        public void MinByReturnsMinima()
        {
            Assert.AreEqual(new[] { "ax", "aa", "ab", "ay", "az" },
                            SampleData.Strings.MinBy(x => x.Length));
        }

        [Test]
        public void MinByNullComparer()
        {
            Assert.AreEqual(SampleData.Strings.MinBy(x => x.Length),
                            SampleData.Strings.MinBy(x => x.Length, null));
        }

        [Test]
        public void MinByEmptySequence()
        {
            Assert.That(new string[0].MinBy(x => x.Length), Is.Empty);
        }

        [Test]
        public void MinByWithNaturalComparer()
        {
            Assert.AreEqual(new[] { "aa" }, SampleData.Strings.MinBy(x => x[1]));
        }

        [Test]
        public void MinByWithComparer()
        {
            Assert.AreEqual(new[] { "az" }, SampleData.Strings.MinBy(x => x[1], SampleData.ReverseCharComparer));
        }

        [TestCase(0, ExpectedResult = new string[0]                         )]
        [TestCase(1, ExpectedResult = new[] { "ax"                         })]
        [TestCase(2, ExpectedResult = new[] { "ax", "aa"                   })]
        [TestCase(3, ExpectedResult = new[] { "ax", "aa", "ab"             })]
        [TestCase(4, ExpectedResult = new[] { "ax", "aa", "ab", "ay"       })]
        [TestCase(5, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
        [TestCase(6, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
        public string[] MinByTakeReturnsMinima(int count)
        {
            using (var strings = SampleData.Strings.AsTestingSequence())
                return strings.MinBy(s => s.Length).Take(count).ToArray();
        }

        [TestCase(0, ExpectedResult = new string[0]                         )]
        [TestCase(1, ExpectedResult = new[] { "az"                         })]
        [TestCase(2, ExpectedResult = new[] { "ay", "az"                   })]
        [TestCase(3, ExpectedResult = new[] { "ab", "ay", "az"             })]
        [TestCase(4, ExpectedResult = new[] { "aa", "ab", "ay", "az"       })]
        [TestCase(5, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
        [TestCase(6, ExpectedResult = new[] { "ax", "aa", "ab", "ay", "az" })]
        public string[] MinByTakeLastReturnsMinima(int count)
        {
            using (var strings = SampleData.Strings.AsTestingSequence())
                return strings.MinBy(s => s.Length).TakeLast(count).ToArray();
        }

        [TestCase(0, ExpectedResult = new string[0]             )]
        [TestCase(1, ExpectedResult = new[] { "hello",         })]
        [TestCase(2, ExpectedResult = new[] { "hello", "world" })]
        [TestCase(3, ExpectedResult = new[] { "hello", "world" })]
        public string[] MinByTakeWithComparerReturnsMinima(int count)
        {
            using (var strings = SampleData.Strings.AsTestingSequence())
                return strings.MinBy(s => s.Length, Comparer<int>.Create((x, y) => -Math.Sign(x.CompareTo(y))))
                              .Take(count)
                              .ToArray();
        }

        [TestCase(0, ExpectedResult = new string[0]             )]
        [TestCase(1, ExpectedResult = new[] { "world",         })]
        [TestCase(2, ExpectedResult = new[] { "hello", "world" })]
        [TestCase(3, ExpectedResult = new[] { "hello", "world" })]
        public string[] MinByTakeLastWithComparerReturnsMinima(int count)
        {
            using (var strings = SampleData.Strings.AsTestingSequence())
                return strings.MinBy(s => s.Length, Comparer<int>.Create((x, y) => -Math.Sign(x.CompareTo(y))))
                              .TakeLast(count)
                              .ToArray();
        }
    }
}
