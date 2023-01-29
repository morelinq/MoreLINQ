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

namespace MoreLinq.Test
{
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;

    [TestFixture]
    public class PipeTest
    {
        [Test]
        public void PipeWithSequence()
        {
            var results = new List<int>();
            var returned = new[] { 1, 2, 3 }.Pipe(results.Add);
            // Lazy - nothing has executed yet
            Assert.That(results, Is.Empty);
            returned.AssertSequenceEqual(1, 2, 3);
            // Now it has...
            results.AssertSequenceEqual(1, 2, 3);
        }

        [Test]
        public void PipeIsLazy()
        {
            _ = new BreakingSequence<int>().Pipe(BreakingAction.Of<int>());
        }

        [Test]
        public void PipeActionOccursBeforeYield()
        {
            var source = new[] { new StringBuilder(), new StringBuilder() };
            // The action will occur "in" the pipe, so by the time Where gets it, the
            // sequence will be empty.
            Assert.That(source.Pipe(sb => sb.Append('x'))
                              .Where(x => x.Length == 0),
                        Is.Empty);
        }
    }
}
