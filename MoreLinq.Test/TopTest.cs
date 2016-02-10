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
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class TopTests
    {
        [Test]
        public void TopWithNullSequence()
        {
            Assert.AreEqual("source", Assert.Throws<ArgumentNullException>(() => MoreEnumerable.Top<object>(null, 0)).ParamName);
            Assert.AreEqual("source", Assert.Throws<ArgumentNullException>(() => MoreEnumerable.Top(null, 0, Comparer<object>.Default)).ParamName);
        }

        [Test]
        public void Top()
        {
            var top = Enumerable.Range(1, 10)
                                .Reverse()
                                .Concat(0)
                                .Top(5);

            top.AssertSequenceEqual(Enumerable.Range(0, 5));
        }

        [Test]
        public void TopWithDuplicates()
        {
            var top = Enumerable.Range(1, 10)
                                .Reverse()
                                .Concat(Enumerable.Repeat(3, 3))
                                .Top(5);

            top.AssertSequenceEqual(1, 2, 3, 3, 3);
        }

        [Test]
        public void TopWithComparer()
        {
            var alphabet = Enumerable.Range(0, 26)
                                     .Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
                                     .ToArray();

            var top = alphabet.Top(5, StringComparer.Ordinal);

            top.Select(s => s[0]).AssertSequenceEqual('A', 'C', 'E', 'G', 'I');
        }

        [Test]
        public void TopIsLazy()
        {
            new BreakingSequence<object>().Top(1);
        }
    }
}