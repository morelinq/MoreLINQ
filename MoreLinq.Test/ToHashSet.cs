#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Atif Aziz. All rights reserved.
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

    using static MoreLinq.Extensions.ToHashSetExtension;

    [TestFixture]
    public class ToHashSetTest
    {
        [Test]
        public void ToHashSetBasic()
        {
            var hs = Enumerable.Range(1, 10)
                .ToHashSet();

            Assert.That(hs.Count, Is.EqualTo(10));
        }

        [Test]
        public void ToHashSetComparer()
        {
            var hs = Enumerable.Range(1, 10)
               .ToHashSet(EqualityComparer.Create<int>((x, y) => x == y));

            Assert.That(hs.Count, Is.EqualTo(10));
        }
    }
}

namespace Linq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MoreLinq;

    public static partial class BuildTest
    {
        public static void ToHashSetCanBuildWithSystemLinq()
        {
            new int[0].ToHashSet();
        }
    }
}
