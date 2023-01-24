#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2016 Leandro F. Vieira (leandromoh). All rights reserved.
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
    using System.Collections.Generic;

    [TestFixture]
    public class AtMostTest
    {
        [Test]
        public void AtMostWithNegativeCount()
        {
            Assert.That(() => new[] { 1 }.AtMost(-1),
                        Throws.ArgumentOutOfRangeException("count"));
        }

        public static IEnumerable<TestCaseData> AtMostSource =>
            from k in SourceKinds.Sequence.Concat(SourceKinds.Collection)
            from e in new[]
            {
                (Size: 0, Count: 0),
                (Size: 0, Count: 1),
                (Size: 1, Count: 0),
                (Size: 1, Count: 1),
                (Size: 1, Count: 2),
                (Size: 3, Count: 1)
            }
            select new TestCaseData(k, e.Size, e.Count)
                .Returns(e.Size <= e.Count)
                .SetName($"{{m}}({k}[{e.Size}], {e.Count})");

        [TestCaseSource(nameof(AtMostSource))]
        public bool AtMost(SourceKind sourceKind, int sequenceSize, int atMostAssertCount) =>
            Enumerable.Range(0, sequenceSize).ToSourceKind(sourceKind).AtMost(atMostAssertCount);

        [Test]
        public void AtMostDoesNotIterateUnnecessaryElements()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => 2,
                                             () => 3,
                                             () => throw new TestException());
            Assert.That(source.AtMost(2), Is.False);
        }
    }
}
