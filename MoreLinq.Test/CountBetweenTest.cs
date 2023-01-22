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
    public class CountBetweenTest
    {
        [Test]
        public void CountBetweenWithNegativeMin()
        {
            Assert.That(() => new[] { 1 }.CountBetween(-1, 0),
                        Throws.ArgumentOutOfRangeException("min"));
        }

        [Test]
        public void CountBetweenWithNegativeMax()
        {
            Assert.That(() => new[] { 1 }.CountBetween(0, -1),
                        Throws.ArgumentOutOfRangeException("max"));
        }

        [Test]
        public void CountBetweenWithMaxLesserThanMin()
        {
            Assert.That(() => new[] { 1 }.CountBetween(1, 0),
                        Throws.ArgumentOutOfRangeException("max"));
        }

        static IEnumerable<TestCaseData> CountBetweenSource =>
            from args in new[]
            {
                (Count: 1, Min: 1, Max: 1),
                (Count: 1, Min: 2, Max: 4),
                (Count: 2, Min: 2, Max: 4),
                (Count: 3, Min: 2, Max: 4),
                (Count: 4, Min: 2, Max: 4),
                (Count: 5, Min: 2, Max: 4),
            }
            from type in SourceKinds.Sequence.Concat(SourceKinds.Collection)
            select new TestCaseData(type, args.Count, args.Min, args.Max)
                .Returns(args.Count >= args.Min && args.Count <= args.Max)
                .SetName($"{{m}}({type}[{args.Count}], {args.Min}, {args.Max})");

        [TestCaseSource(nameof(CountBetweenSource))]
        public bool CountBetween(SourceKind sourceKind, int count, int min, int max)
        {
            return Enumerable.Range(0, count).ToSourceKind(sourceKind).CountBetween(min, max);
        }

        [Test]
        public void CountBetweenDoesNotIterateUnnecessaryElements()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => 2,
                                             () => 3,
                                             () => 4,
                                             () => throw new TestException());
            Assert.That(source.CountBetween(2, 3), Is.False);
        }
    }
}
