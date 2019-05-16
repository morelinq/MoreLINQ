#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Atif Aziz. All rights reserved.
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
    using NUnit.Framework.Interfaces;

    [TestFixture]
    public class DeconstructTest
    {
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        [TestCase(16)]
        public void WithNullSource(int width)
        {
            var source = (object[])null;

            var e = Assert.Throws<ArgumentNullException>(() =>
            {
                switch (width)
                {
                    case 2: (_, _, _) = source; break;
                    case 3: (_, _, _, _) = source; break;
                    case 4: (_, _, _, _, _) = source; break;
                    case 5: (_, _, _, _, _, _) = source; break;
                    case 6: (_, _, _, _, _, _, _) = source; break;
                    case 7: (_, _, _, _, _, _, _, _) = source; break;
                    case 8: (_, _, _, _, _, _, _, _, _) = source; break;
                    case 9: (_, _, _, _, _, _, _, _, _, _) = source; break;
                    case 10: (_, _, _, _, _, _, _, _, _, _, _) = source; break;
                    case 11: (_, _, _, _, _, _, _, _, _, _, _, _) = source; break;
                    case 12: (_, _, _, _, _, _, _, _, _, _, _, _, _) = source; break;
                    case 13: (_, _, _, _, _, _, _, _, _, _, _, _, _, _) = source; break;
                    case 14: (_, _, _, _, _, _, _, _, _, _, _, _, _, _, _) = source; break;
                    case 15: (_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _) = source; break;
                    case 16: (_, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _, _) = source; break;
                    default: throw new ArgumentOutOfRangeException(nameof(width), width, null);
                }
            });

            Assert.That(e.ParamName, Is.EqualTo("source"));
        }

        static readonly IEnumerable<ITestCaseData> DeconstructCases =
            from w in Enumerable.Range(2, 16 - 1)
            from c in Enumerable.Range(0, w + 2)
            select new TestCaseData(w, c);

        [TestCaseSource(nameof(DeconstructCases))]
        public void Deconstruct(int width, int count)
        {
            var xs = Enumerable.Range(1, count);
            using (var ts = xs.AsTestingSequence())
            {
                int a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p;
                c = d = e = f = g = h = i = j = k = l = m = n = o = p = 0;
                int rc;

                switch (width)
                {
                    case 2: (a, b, rc) = ts; break;
                    case 3: (a, b, c, rc) = ts; break;
                    case 4: (a, b, c, d, rc) = ts; break;
                    case 5: (a, b, c, d, e, rc) = ts; break;
                    case 6: (a, b, c, d, e, f, rc) = ts; break;
                    case 7: (a, b, c, d, e, f, g, rc) = ts; break;
                    case 8: (a, b, c, d, e, f, g, h, rc) = ts; break;
                    case 9: (a, b, c, d, e, f, g, h, i, rc) = ts; break;
                    case 10: (a, b, c, d, e, f, g, h, i, j, rc) = ts; break;
                    case 11: (a, b, c, d, e, f, g, h, i, j, k, rc) = ts; break;
                    case 12: (a, b, c, d, e, f, g, h, i, j, k, l, rc) = ts; break;
                    case 13: (a, b, c, d, e, f, g, h, i, j, k, l, m, rc) = ts; break;
                    case 14: (a, b, c, d, e, f, g, h, i, j, k, l, m, n, rc) = ts; break;
                    case 15: (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, rc) = ts; break;
                    case 16: (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, rc) = ts; break;
                    default: throw new ArgumentOutOfRangeException(nameof(width), width, null);
                }

                var all = new[] { a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, 0 };

                foreach (var (alpha, exp, act) in
                    from t in
                        all.Zip(xs, (exp, act) => new { Expected = exp, Actual = act })
                           .Take(width)
                           .Index(0)
                    select ((char)('a' + t.Key), t.Value.Expected, t.Value.Actual))
                {
                    Assert.That(act, Is.EqualTo(exp), alpha.ToString());
                }

                all.Skip(count).AssertSequenceEqual(Enumerable.Repeat(0, all.Length - count));

                Assert.That(rc, Is.EqualTo(Math.Min(width, count)));
            }
        }
    }
}
