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

using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class FillForwardTest
    {
        [Test]
        public void FillForwardWithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source", () => MoreEnumerable.FillForward<object>(null));
        }

        [Test]
        public void FillForwardWithNullPredicate()
        {
            Assert.ThrowsArgumentNullException("predicate", () => new object[0].FillForward(null));
        }

        [Test]
        public void FillForwardWithFillSelectorButNullSequence()
        {
            Assert.ThrowsArgumentNullException("source", () =>
                MoreEnumerable.FillForward<object>(null, _ => false, delegate { return null; }));
        }

        [Test]
        public void FillForwardWithFillSelectorButNullPredicate()
        {
            Assert.ThrowsArgumentNullException("predicate", () =>
                new object[0].FillForward(null, delegate { return null; }));
        }

        [Test]
        public void FillForwardWithNullFillSelector()
        {
            Assert.ThrowsArgumentNullException("fillSelector", () =>
                new object[0].FillForward(_ => false, null));
        }

        [Test]
        public void FillForwardIsLazy()
        {
            new BreakingSequence<object>().FillForward();
        }

        [Test]
        public void FillForward()
        {
            int? na = null;
            var input = new[] { na, na, 1, 2, na, na, na, 3, 4, na, na };
            var result = input.FillForward();
            Assert.That(result, Is.EquivalentTo(new[] { na, na, 1, 2, 2, 2, 2, 3, 4, 4, 4 }));
        }

        [Test]
        public void FillForwardWithFillSelector()
        {
            const string table = @"
                Europe UK          London      123
                -      -           Manchester  234
                -      -           Glasgow     345
                -      Germany     Munich      456
                -      -           Frankfurt   567
                -      -           Stuttgart   678
                Africa Egypt       Cairo       789
                -      -           Alexandria  890
                -      Kenya       Nairobi     901
            ";

            var data =
                from line in table.Split('\n')
                select line.Trim() into line
                where !string.IsNullOrEmpty(line)
                select Regex.Split(line, "\x20+").Fold((cont, ctry, city, val) => new
                {
                    Continent = cont,
                    Country   = ctry,
                    City      = city,
                    Value     = int.Parse(val),
                });

            data = data.FillForward(e => e.Continent == "-", (f, e) => new { f.Continent, e.Country, e.City, e.Value })
                        .FillForward(e => e.Country   == "-", (f, e) => new { e.Continent, f.Country, e.City, e.Value });


            Assert.That(data, Is.EquivalentTo(new[]
            {
                new { Continent = "Europe", Country = "UK",      City = "London",     Value = 123 },
                new { Continent = "Europe", Country = "UK",      City = "Manchester", Value = 234 },
                new { Continent = "Europe", Country = "UK",      City = "Glasgow",    Value = 345 },
                new { Continent = "Europe", Country = "Germany", City = "Munich",     Value = 456 },
                new { Continent = "Europe", Country = "Germany", City = "Frankfurt",  Value = 567 },
                new { Continent = "Europe", Country = "Germany", City = "Stuttgart",  Value = 678 },
                new { Continent = "Africa", Country = "Egypt",   City = "Cairo",      Value = 789 },
                new { Continent = "Africa", Country = "Egypt",   City = "Alexandria", Value = 890 },
                new { Continent = "Africa", Country = "Kenya",   City = "Nairobi",    Value = 901 },
            }));
        }
    }
}