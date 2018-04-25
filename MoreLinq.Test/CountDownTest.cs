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
    using NUnit.Framework;

    [TestFixture]
    public class CountDownTest
    {
        [Test]
        public void IsLazy()
        {
            new BreakingSequence<object>()
                .CountDown(42, BreakingFunc.Of<object, int?, object>());
        }

        public enum SequenceKind
        {
            Sequence,
            List,
            ReadOnlyList,
        }

        static readonly IEnumerable<TestCaseData> Data =
            from e in new[]
            {
                new { Count = -1, CountDown = new int?[] { null, null, null, null, null } },
                new { Count =  0, CountDown = new int?[] { null, null, null, null, null } },
                new { Count =  1, CountDown = new int?[] { null, null, null, null,    0 } },
                new { Count =  2, CountDown = new int?[] { null, null, null,    1,    0 } },
                new { Count =  3, CountDown = new int?[] { null, null,    2,    1,    0 } },
                new { Count =  4, CountDown = new int?[] { null,    3,    2,    1,    0 } },
                new { Count =  5, CountDown = new int?[] {    4,    3,    2,    1,    0 } },
                new { Count =  6, CountDown = new int?[] {    4,    3,    2,    1,    0 } },
                new { Count =  7, CountDown = new int?[] {    4,    3,    2,    1,    0 } },
            }
            let xs = Enumerable.Range(1, 5)
            from ts in new[]
            {
                new { Kind = SequenceKind.Sequence    , Source  = xs.Select(x => x),                  },
                new { Kind = SequenceKind.List        , Source  = xs.ToBreakingList(readOnly: false), },
                new { Kind = SequenceKind.ReadOnlyList, Source  = xs.ToBreakingList(readOnly: true),  },
            }
            select new TestCaseData(ts.Source, ts.Kind, e.Count)
                .Returns(xs.Zip(e.CountDown, ValueTuple.Create))
                .SetName($"{nameof(CountDown)}({{ {xs.First()}..{xs.Last()} }}, {ts.Kind}, {e.Count})");

        [TestCaseSource(nameof(Data))]
        public IEnumerable<(int, int?)>
            CountDown(IEnumerable<int> xs, SequenceKind kind, int count)
        {
            return kind != SequenceKind.Sequence
                 ? xs.CountDown(count, ValueTuple.Create)
                 : _(); IEnumerable<(int, int?)> _()
                 {
                     using (var ts = xs.AsTestingSequence())
                     {
                         foreach (var e in ts.CountDown(count, ValueTuple.Create))
                             yield return e;
                     }
                 }
        }
    }
}
