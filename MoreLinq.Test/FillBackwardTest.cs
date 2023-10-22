#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Atif Aziz. All rights reserved.
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

    [TestFixture]
    public class FillBackwardTest
    {
        [Test]
        public void FillBackwardIsLazy()
        {
            _ = new BreakingSequence<object>().FillBackward();
        }

        [Test]
        public void FillBackward()
        {
            int? na = null;
            var input = new[] { na, na, 1, 2, na, na, na, 3, 4, na, na };
            var result = input.FillBackward();
            Assert.That(result, Is.EqualTo(new[] { 1, 1, 1, 2, 3, 3, 3, 3, 4, na, na }));
        }

        [Test]
        public void FillBackwardWithFillSelector()
        {
            var xs = new[] { 0, 0, 1, 2, 0, 0, 0, 3, 4, 0, 0 };

            var result =
                xs.Select(x => new { X = x, Y = x })
                  .FillBackward(e => e.X == 0, (m, nm) => new { m.X, nm.Y });

            Assert.That(result, Is.EqualTo(new[]
            {
                new { X = 0, Y = 1 },
                new { X = 0, Y = 1 },
                new { X = 1, Y = 1 },
                new { X = 2, Y = 2 },
                new { X = 0, Y = 3 },
                new { X = 0, Y = 3 },
                new { X = 0, Y = 3 },
                new { X = 3, Y = 3 },
                new { X = 4, Y = 4 },
                new { X = 0, Y = 0 },
                new { X = 0, Y = 0 },
            }));
        }
    }
}
