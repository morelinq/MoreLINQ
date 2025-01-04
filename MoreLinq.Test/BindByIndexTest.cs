#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Atif Aziz. All rights reserved.
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
    using System.Globalization;
    using System.Text.RegularExpressions;
    using MoreLinq.Extensions;
    using NUnit.Framework;

    public class BindByIndexTest
    {
        [TestCase(new[] { 2, 3 }, ExpectedResult = new[] { "baz", "qux" })]
        [TestCase(new[] { 1, 3 }, ExpectedResult = new[] { "bar", "qux" })]
        [TestCase(new[] { 0, 2, 3 }, ExpectedResult = new[] { "foo", "baz", "qux" })]
        [TestCase(new[] { 0, 1, 1 }, ExpectedResult = new[] { "foo", "bar", "bar" })]
        [TestCase(new[] { 3, 1, 2 }, ExpectedResult = new[] { "qux", "bar", "baz" })]
        [TestCase(new[] { -1, 1, 2, 10 }, ExpectedResult = new[] { "?-1", "bar", "baz", "?10" })]
        public string[] WithoutSpecificLookBackSize(IEnumerable<int> indices)
        {
            using var source = TestingSequence.Of("foo", "bar", "baz", "qux");
            return source.BindByIndex(indices, i => $"?{i.ToInvariantString()}", (s, _) => s)
                         .ToArray();
        }

        [TestCase(0, new[] { 2, 3 }, ExpectedResult = new[] { "baz", "qux" })]
        [TestCase(0, new[] { 1, 3 }, ExpectedResult = new[] { "bar", "qux" })]
        [TestCase(0, new[] { 0, 2, 3 }, ExpectedResult = new[] { "foo", "baz", "qux" })]
        [TestCase(0, new[] { 0, 1, 1 }, ExpectedResult = new[] { "foo", "bar", "bar" })]
        [TestCase(0, new[] { 3, 1, 2 }, ExpectedResult = new[] { "qux", "?1", "?2" })]
        [TestCase(4, new[] { 3, 1, 2 }, ExpectedResult = new[] { "qux", "bar", "baz" })]
        [TestCase(1, new[] { 3, 1, 2 }, ExpectedResult = new[] { "qux", "?1", "baz" })]
        [TestCase(0, new[] { -1, 1, 2, 10 }, ExpectedResult = new[] { "?-1", "bar", "baz", "?10" })]
        public string[] WithSpecificLookBackSize(int lookBackSize, IEnumerable<int> indices)
        {
            using var source = TestingSequence.Of("foo", "bar", "baz", "qux");
            return source.BindByIndex(indices, lookBackSize, i => $"?{i.ToInvariantString()}", (s, _) => s)
                         .ToArray();
        }

        [Test]
        public void ParsingExample()
        {
            const string csv = """
                # Generated using https://mockaroo.com/
                id,first_name,last_name,email,gender,ip_address
                1,Maggee,Hould,mhould0@ft.com,Female,158.221.234.250
                2,Judas,Vedekhov,jvedekhov1@google.co.uk,Male,26.25.8.252
                3,Sharity,Desquesnes,sdesquesnes2@accuweather.com,Female,27.224.140.230
                4,Della,Conant,dconant3@japanpost.jp,Female,229.74.161.94
                5,Sansone,Hardson,shardson4@weather.com,Male,51.154.224.38
                6,Lloyd,Cromley,lcromley5@wikipedia.org,Male,168.145.20.63
                7,Ty,Bamsey,tbamsey6@ca.gov,Male,129.204.46.174
                8,Hurlee,Dumphy,hdumphy7@skyrock.com,Male,95.17.55.115
                9,Andy,Vickarman,avickarman8@qq.com,Male,10.159.118.60
                10,Jerad,Kerley,jkerley9@miitbeian.gov.cn,Male,3.19.136.57
                """;

            // Parse CSV into rows of fields with commented lines, those starting with pound or hash
            // (#), removed.

            var rows =
                from row in Regex.Split(csv.Trim(), "\r?\n")
                select row.Trim() into row
                where row.Length > 0 && row[0] != '#'
                select row.Trim().Split(',');

            // Split header and data rows:

            var (header, data) =
                rows.Index()
                    .Partition(e => e.Key == 0,
                               (hr, dr) => (hr.Single().Value, from e in dr select e.Value));

            // Locate indices of headers:

            int[] bindings = [..from h in new[] { "id", "email", "last_name", "first_name", "foo" }
                                select Array.FindIndex(header, sh => sh == h)];

            // Bind to data using indices:

            string? missing = null;

            var result =
                from row in data
                select row.BindByIndex(bindings, bindings.Length, _ => missing, (f, _) => f)
                          .Fold((id, email, ln, fn, foo) =>
                                    id is null || email is null || ln is null || fn is null
                                    ? null
                                    : new
                                      {
                                          Id        = int.Parse(id, NumberStyles.None, CultureInfo.InvariantCulture),
                                          FirstName = fn,
                                          LastName  = ln,
                                          Email     = email,
                                          Foo       = foo,
                                      });

            result.AssertSequenceEqual(
                new { Id = 1 , FirstName = "Maggee" , LastName = "Hould"     , Email = "mhould0@ft.com"              , Foo = missing },
                new { Id = 2 , FirstName = "Judas"  , LastName = "Vedekhov"  , Email = "jvedekhov1@google.co.uk"     , Foo = missing },
                new { Id = 3 , FirstName = "Sharity", LastName = "Desquesnes", Email = "sdesquesnes2@accuweather.com", Foo = missing },
                new { Id = 4 , FirstName = "Della"  , LastName = "Conant"    , Email = "dconant3@japanpost.jp"       , Foo = missing },
                new { Id = 5 , FirstName = "Sansone", LastName = "Hardson"   , Email = "shardson4@weather.com"       , Foo = missing },
                new { Id = 6 , FirstName = "Lloyd"  , LastName = "Cromley"   , Email = "lcromley5@wikipedia.org"     , Foo = missing },
                new { Id = 7 , FirstName = "Ty"     , LastName = "Bamsey"    , Email = "tbamsey6@ca.gov"             , Foo = missing },
                new { Id = 8 , FirstName = "Hurlee" , LastName = "Dumphy"    , Email = "hdumphy7@skyrock.com"        , Foo = missing },
                new { Id = 9 , FirstName = "Andy"   , LastName = "Vickarman" , Email = "avickarman8@qq.com"          , Foo = missing },
                new { Id = 10, FirstName = "Jerad"  , LastName = "Kerley"    , Email = "jkerley9@miitbeian.gov.cn"   , Foo = missing });
        }
    }
}
