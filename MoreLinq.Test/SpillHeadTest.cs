#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2020 Atif Aziz. All rights reserved.
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using NUnit.Framework;
    using static FuncModule;

    [TestFixture]
    public class SpillHeadTest
    {
        [Test]
        public void RepeatHeadElementWithRest()
        {
            using var ts = Enumerable.Range(5, 6).AsTestingSequence();
            var result = ts.SpillHead();

            Assert.That(result, Is.EqualTo(new[]
            {
                (5, 6), (5, 7), (5, 8), (5, 9), (5, 10)
            }));
        }

        [Test]
        public void HeadElementOnly()
        {
            using var ts = new[] { "head" }.AsTestingSequence();
            var result = ts.SpillHead();
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void RepeatHeadElementsWithRest()
        {
            using var ts = Enumerable.Range(5, 6).AsTestingSequence();
            var result = ts.SpillHead(2, hs => hs, (h, d) => (h[0], h[1], d));

            Assert.That(result, Is.EqualTo(new[]
            {
                (5, 6, 7), (5, 6, 8), (5, 6, 9), (5, 6, 10)
            }));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void InsufficientElementsPerHeadCount(int count)
        {
            using var ts = Enumerable.Repeat("head", count).AsTestingSequence();
            var result = ts.SpillHead(3,
                                      BreakingFunc.Of<List<string>, object>(),
                                      BreakingFunc.Of<object, string, object>());

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void PredicatedHeads()
        {
            var heads = new[] { "head1", "head2", "head3" };
            var data = new[] { "foo", "bar", "baz" };
            using var ts = heads.Concat(data).AsTestingSequence();
            var result =  ts.SpillHead(h => Regex.IsMatch(h, "^head[0-9]$"),
                                       hs => string.Join("|", hs),
                                       (h, e) => new { Head = h, Data = e });

            Assert.That(result, Is.EqualTo(new[]
            {
                new { Head = "head1|head2|head3", Data = "foo" },
                new { Head = "head1|head2|head3", Data = "bar" },
                new { Head = "head1|head2|head3", Data = "baz" },
            }));
        }

        [Test]
        public void CustomAccumulation()
        {
            var heads = new[] { "head1", "head2", "head3" };
            var data = new[] { "foo", "bar", "baz" };
            using var ts = heads.Concat(data).AsTestingSequence();
            var result =  ts.SpillHead(h => Regex.IsMatch(h, "^head[0-9]$"),
                                       Enumerable.Empty<string>(),
                                       MoreEnumerable.Return,
                                       (hs, h) => hs.Append(h),
                                       hs => string.Join("|", hs),
                                       (h, e) => new { Head = h, Data = e });

            Assert.That(result, Is.EqualTo(new[]
            {
                new { Head = "head1|head2|head3", Data = "foo" },
                new { Head = "head1|head2|head3", Data = "bar" },
                new { Head = "head1|head2|head3", Data = "baz" },
            }));
        }

        [Test]
        public void NoneSatisfyHeadPredicate()
        {
            using var words = new[] { "foo", "bar", "baz" }.AsTestingSequence();
            var result = words.SpillHead(e => e == "head",
                                         hs => hs.Count,
                                         (hc, e) => new { HeadCount = 0, Data = e });

            Assert.That(result, Is.EqualTo(new[]
            {
                new { HeadCount = 0, Data = "foo" },
                new { HeadCount = 0, Data = "bar" },
                new { HeadCount = 0, Data = "baz" },
            }));
        }

        [Test]
        public void Csv()
        {
            const string csv = @"
                a,c,b
                1,3,2
                4,6,5
                7,9,8";

            var rows =
                from line in Regex.Split(csv.Trim(), @"\r?\n")
                select line.Split(',').Select(f => f.Trim()).ToArray();

            using var ts = rows.AsTestingSequence();
            var result =
                    ts.SpillHead(
                          h => MoreEnumerable.Return(h.Index()
                                                      .ToDictionary(e => e.Value, e => e.Key))
                                             .SelectMany(d => new[] { "a", "b", "c" },
                                                         (d, n) => d[n])
                                             .Select(i => Func((string[] s) => s[i]))
                                             .ToArray(),
                          (bs, r) => bs.Select(b => int.Parse(b(r), CultureInfo.InvariantCulture))
                                       .Fold((a, b, c) => new { A = a, B = b, C = c }));

            Assert.That(result, Is.EqualTo(new[]
            {
                new { A = 1, B = 2, C = 3 },
                new { A = 4, B = 5, C = 6 },
                new { A = 7, B = 8, C = 9 },
            }));
        }

        [Test]
        public void CsvWithColumnsInCommentLines()
        {
            const string csv = @"
                ; a = Column A
                ; b = Column B
                ; c = Column C
                1,2,3
                4,5,6
                7,8,9";

            using var ts = Regex.Split(csv.Trim(), @"\r?\n")
                                .Select(line => line.Trim())
                                .AsTestingSequence();

            var result =
                from e in
                    ts.SpillHead(h => Regex.Match(h, @"^;\s*(\w+)") is var m & m.Success ? (true, m.Groups[1].Value) : default,
                                 h => MoreEnumerable.Return(h.Index()
                                                             .ToDictionary(e => e.Value, e => e.Key))
                                                    .SelectMany(d => new[] { "a", "b", "c" },
                                                                (d, n) => d[n])
                                                    .Select(i => Func((string[] s) => s[i]))
                                                    .ToArray(),
                                 (bs, r) => new { Bindings = bs, Fields = r.Split(',') })
                select e.Bindings
                        .Select(b => int.Parse(b(e.Fields), CultureInfo.InvariantCulture))
                        .Fold((a, b, c) => new { A = a, B = b, C = c });

            Assert.That(result, Is.EqualTo(new[]
            {
                new { A = 1, B = 2, C = 3 },
                new { A = 4, B = 5, C = 6 },
                new { A = 7, B = 8, C = 9 },
            }));
        }
    }
}
