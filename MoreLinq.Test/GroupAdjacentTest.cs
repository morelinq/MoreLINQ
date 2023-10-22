#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2012 Atif Aziz. All rights reserved.
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
    using static MoreLinq.Extensions.AppendExtension;

    [TestFixture]
    public class GroupAdjacentTest
    {
        [Test]
        public void GroupAdjacentIsLazy()
        {
            var bs = new BreakingSequence<object>();
            var bf = BreakingFunc.Of<object, int>();
            var bfo = BreakingFunc.Of<object, object>();
            var bfg = BreakingFunc.Of<int, IEnumerable<object>, IEnumerable<object>>();

            _ = bs.GroupAdjacent(bf);
            _ = bs.GroupAdjacent(bf, bfo);
            _ = bs.GroupAdjacent(bf, bfo, EqualityComparer<int>.Default);
            _ = bs.GroupAdjacent(bf, EqualityComparer<int>.Default);
            _ = bs.GroupAdjacent(bf, bfg);
            _ = bs.GroupAdjacent(bf, bfg, EqualityComparer<int>.Default);
        }

        [Test]
        public void GroupAdjacentSourceSequence()
        {
            const string one = "one";
            const string two = "two";
            const string three = "three";
            const string four = "four";
            const string five = "five";
            const string six = "six";
            const string seven = "seven";
            const string eight = "eight";
            const string nine = "nine";
            const string ten = "ten";

            var source = new[] { one, two, three, four, five, six, seven, eight, nine, ten };

            var groupings = source.GroupAdjacent(s => s.Length);

            using var reader = groupings.Read();
            AssertGrouping(reader, 3, one, two);
            AssertGrouping(reader, 5, three);
            AssertGrouping(reader, 4, four, five);
            AssertGrouping(reader, 3, six);
            AssertGrouping(reader, 5, seven, eight);
            AssertGrouping(reader, 4, nine);
            AssertGrouping(reader, 3, ten);
            reader.ReadEnd();
        }

        [Test]
        public void GroupAdjacentSourceSequenceComparer()
        {
            var source = new[] { "foo", "FOO", "Foo", "bar", "BAR", "Bar" };

            var groupings = source.GroupAdjacent(s => s, StringComparer.OrdinalIgnoreCase);

            using var reader = groupings.Read();
            AssertGrouping(reader, "foo", "foo", "FOO", "Foo");
            AssertGrouping(reader, "bar", "bar", "BAR", "Bar");
            reader.ReadEnd();
        }

        [Test]
        public void GroupAdjacentSourceSequenceElementSelector()
        {
            var source = new[]
            {
                new { Month = 1, Value = 123 },
                new { Month = 1, Value = 456 },
                new { Month = 1, Value = 789 },
                new { Month = 2, Value = 987 },
                new { Month = 2, Value = 654 },
                new { Month = 2, Value = 321 },
                new { Month = 3, Value = 789 },
                new { Month = 3, Value = 456 },
                new { Month = 3, Value = 123 },
                new { Month = 1, Value = 123 },
                new { Month = 1, Value = 456 },
                new { Month = 1, Value = 781 },
            };

            var groupings = source.GroupAdjacent(e => e.Month, e => e.Value * 2);

            using var reader = groupings.Read();
            AssertGrouping(reader, 1, 123 * 2, 456 * 2, 789 * 2);
            AssertGrouping(reader, 2, 987 * 2, 654 * 2, 321 * 2);
            AssertGrouping(reader, 3, 789 * 2, 456 * 2, 123 * 2);
            AssertGrouping(reader, 1, 123 * 2, 456 * 2, 781 * 2);
            reader.ReadEnd();
        }

        [Test]
        public void GroupAdjacentSourceSequenceElementSelectorComparer()
        {
            var source = new[]
            {
                new { Month = "jan", Value = 123 },
                new { Month = "Jan", Value = 456 },
                new { Month = "JAN", Value = 789 },
                new { Month = "feb", Value = 987 },
                new { Month = "Feb", Value = 654 },
                new { Month = "FEB", Value = 321 },
                new { Month = "mar", Value = 789 },
                new { Month = "Mar", Value = 456 },
                new { Month = "MAR", Value = 123 },
                new { Month = "jan", Value = 123 },
                new { Month = "Jan", Value = 456 },
                new { Month = "JAN", Value = 781 },
            };

            var groupings = source.GroupAdjacent(e => e.Month, e => e.Value * 2, StringComparer.OrdinalIgnoreCase);

            using var reader = groupings.Read();
            AssertGrouping(reader, "jan", 123 * 2, 456 * 2, 789 * 2);
            AssertGrouping(reader, "feb", 987 * 2, 654 * 2, 321 * 2);
            AssertGrouping(reader, "mar", 789 * 2, 456 * 2, 123 * 2);
            AssertGrouping(reader, "jan", 123 * 2, 456 * 2, 781 * 2);
            reader.ReadEnd();
        }

        [Test]
        public void GroupAdjacentSourceSequenceResultSelector()
        {
            var source = new[]
            {
                new { Month = 1, Value = 123 },
                new { Month = 1, Value = 456 },
                new { Month = 1, Value = 789 },
                new { Month = 2, Value = 987 },
                new { Month = 2, Value = 654 },
                new { Month = 2, Value = 321 },
                new { Month = 3, Value = 789 },
                new { Month = 3, Value = 456 },
                new { Month = 3, Value = 123 },
                new { Month = 1, Value = 123 },
                new { Month = 1, Value = 456 },
                new { Month = 1, Value = 781 },
            };

            var groupings = source.GroupAdjacent(e => e.Month, (_, group) => group.Sum(v => v.Value));

            using var reader = groupings.Read();
            AssertResult(reader, 123 + 456 + 789);
            AssertResult(reader, 987 + 654 + 321);
            AssertResult(reader, 789 + 456 + 123);
            AssertResult(reader, 123 + 456 + 781);
            reader.ReadEnd();
        }

        [Test]
        public void GroupAdjacentSourceSequenceResultSelectorComparer()
        {
            var source = new[]
            {
                new { Month = "jan", Value = 123 },
                new { Month = "Jan", Value = 456 },
                new { Month = "JAN", Value = 789 },
                new { Month = "feb", Value = 987 },
                new { Month = "Feb", Value = 654 },
                new { Month = "FEB", Value = 321 },
                new { Month = "mar", Value = 789 },
                new { Month = "Mar", Value = 456 },
                new { Month = "MAR", Value = 123 },
                new { Month = "jan", Value = 123 },
                new { Month = "Jan", Value = 456 },
                new { Month = "JAN", Value = 781 },
            };

            var groupings = source.GroupAdjacent(e => e.Month, (_, group) => group.Sum(v => v.Value), StringComparer.OrdinalIgnoreCase);

            using var reader = groupings.Read();
            AssertResult(reader, 123 + 456 + 789);
            AssertResult(reader, 987 + 654 + 321);
            AssertResult(reader, 789 + 456 + 123);
            AssertResult(reader, 123 + 456 + 781);
            reader.ReadEnd();
        }

        [Test]
        public void GroupAdjacentSourceSequenceWithSomeNullKeys()
        {
            var groupings =
                Enumerable.Range(1, 5)
                          .SelectMany(x => Enumerable.Repeat((int?)x, x).Append(null))
                          .GroupAdjacent(x => x);

            int?[] aNull = { null };

            using var reader = groupings.Read();
            AssertGrouping(reader, 1, 1);
            AssertGrouping(reader, null, aNull);
            AssertGrouping(reader, 2, 2, 2);
            AssertGrouping(reader, null, aNull);
            AssertGrouping(reader, 3, 3, 3, 3);
            AssertGrouping(reader, null, aNull);
            AssertGrouping(reader, 4, 4, 4, 4, 4);
            AssertGrouping(reader, null, aNull);
            AssertGrouping(reader, 5, 5, 5, 5, 5, 5);
            AssertGrouping(reader, null, aNull);
            reader.ReadEnd();
        }

        static void AssertGrouping<TKey, TElement>(SequenceReader<System.Linq.IGrouping<TKey, TElement>> reader,
            TKey key, params TElement[] elements)
        {
            var grouping = reader.Read();
            Assert.That(grouping, Is.Not.Null);
            Assert.That(grouping.Key, Is.EqualTo(key));
            grouping.AssertSequenceEqual(elements);
        }

        static void AssertResult<TElement>(SequenceReader<TElement> reader, TElement element)
        {
            var result = reader.Read();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(element));
        }
    }
}
