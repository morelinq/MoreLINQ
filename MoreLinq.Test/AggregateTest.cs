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
    using System.Globalization;
    using NUnit.Framework;
    using Experimental;
    using System.Reactive.Linq;

    [TestFixture]
    public class AggregateTest
    {
        // TODO add more tests

        [Test]
        public void Seven()
        {
            var result =
                Enumerable
                    .Range(1, 10)
                    .Shuffle()
                    .Select(n => new { Num = n, Str = n.ToString(CultureInfo.InvariantCulture) })
                    .Aggregate(
                        s => s.Sum(e => e.Num),
                        s => s.Select(e => e.Num).Where(n => n % 2 == 0).Sum(),
                        s => s.Count(),
                        s => s.Min(e => e.Num),
                        s => s.Max(e => e.Num),
                        s => s.Select(e => e.Str.Length).Distinct().ToArray(),
                        s => s.ToArray(),
                        (sum, esum, count, min, max, lengths, items) => new
                        {
                            Sum           = sum,
                            EvenSum       = esum,
                            Count         = count,
                            Average       = (double)sum / count,
                            Min           = min,
                            Max           = max,
                            UniqueLengths = lengths,
                            Items         = items,
                        }
                    );

            Assert.That(result.Sum, Is.EqualTo(55));
            Assert.That(result.EvenSum, Is.EqualTo(30));
            Assert.That(result.Count, Is.EqualTo(10));
            Assert.That(result.Average, Is.EqualTo(5.5));
            Assert.That(result.Min, Is.EqualTo(1));
            Assert.That(result.Max, Is.EqualTo(10));
            result.UniqueLengths.AssertSequenceEqual(1, 2);
            result.Items
                  .OrderBy(e => e.Num)
                  .AssertSequenceEqual(new { Num =  1, Str =  "1" },
                                       new { Num =  2, Str =  "2" },
                                       new { Num =  3, Str =  "3" },
                                       new { Num =  4, Str =  "4" },
                                       new { Num =  5, Str =  "5" },
                                       new { Num =  6, Str =  "6" },
                                       new { Num =  7, Str =  "7" },
                                       new { Num =  8, Str =  "8" },
                                       new { Num =  9, Str =  "9" },
                                       new { Num = 10, Str = "10" });
        }
    }
}
