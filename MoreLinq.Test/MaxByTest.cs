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
            new BreakingSequence<int>().MaxBy(x => x);
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
    }
}
