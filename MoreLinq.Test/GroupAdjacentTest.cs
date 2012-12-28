#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008-2011 Jonathan Skeet. All rights reserved.
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

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace MoreLinq.Test
{
    [TestFixture]
    public class GroupAdjacentTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GroupAdjacentNullSource()
        {
            MoreEnumerable.GroupAdjacent<object, object>(null, delegate { return 0; });
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GroupAdjacentNullKeySelector()
        {
            MoreEnumerable.GroupAdjacent<object, object>(new object[0], null);
        }

        [Test]
        public void GroupAdjacentIsLazy()
        {
            MoreEnumerable.GroupAdjacent(new BreakingSequence<object>(), delegate { return 0; });
            MoreEnumerable.GroupAdjacent(new BreakingSequence<object>(), delegate { return 0; }, o => o);
            MoreEnumerable.GroupAdjacent(new BreakingSequence<object>(), delegate { return 0; }, o => o, EqualityComparer<int>.Default);
            MoreEnumerable.GroupAdjacent(new BreakingSequence<object>(), delegate { return 0; }, EqualityComparer<int>.Default);
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
            var groupings = MoreEnumerable.GroupAdjacent(source, s => s.Length);
            
            using (var reader = groupings.Read())
            {
                AssertGrouping(reader, 3, one, two);
                AssertGrouping(reader, 5, three);
                AssertGrouping(reader, 4, four, five);
                AssertGrouping(reader, 3, six);
                AssertGrouping(reader, 5, seven, eight);
                AssertGrouping(reader, 4, nine);
                AssertGrouping(reader, 3, ten);
                reader.ReadEnd();
            }
        }

        [Test]
        public void GroupAdjacentSourceSequenceComparer()
        {
            var source = new[] { "foo", "FOO", "Foo", "bar", "BAR", "Bar" };
            var groupings = MoreEnumerable.GroupAdjacent(source, s => s, StringComparer.OrdinalIgnoreCase);

            using (var reader = groupings.Read())
            {
                AssertGrouping(reader, "foo", "foo", "FOO", "Foo");
                AssertGrouping(reader, "bar", "bar", "BAR", "Bar");
                reader.ReadEnd();
            }
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
            };

            var groupings = MoreEnumerable.GroupAdjacent(source, e => e.Month, e => e.Value * 2);

            using (var reader = groupings.Read())
            {
                AssertGrouping(reader, 1, 123 * 2, 456 * 2, 789 * 2);
                AssertGrouping(reader, 2, 987 * 2, 654 * 2, 321 * 2);
                AssertGrouping(reader, 3, 789 * 2, 456 * 2, 123 * 2);
                reader.ReadEnd();
            }
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
            };

            var groupings = MoreEnumerable.GroupAdjacent(source, e => e.Month, e => e.Value * 2, StringComparer.OrdinalIgnoreCase);

            using (var reader = groupings.Read())
            {
                AssertGrouping(reader, "jan", 123 * 2, 456 * 2, 789 * 2);
                AssertGrouping(reader, "feb", 987 * 2, 654 * 2, 321 * 2);
                AssertGrouping(reader, "mar", 789 * 2, 456 * 2, 123 * 2);
                reader.ReadEnd();
            }
        }

        static void AssertGrouping<TKey, TElement>(SequenceReader<IGrouping<TKey, TElement>> reader, 
            TKey key, params TElement[] elements)
        {
            var grouping = reader.Read();
            Assert.That(grouping, Is.Not.Null);
            Assert.That(grouping.Key, Is.EqualTo(key));
            grouping.AssertSequenceEqual(elements);
        }
    }
}
