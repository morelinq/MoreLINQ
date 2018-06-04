#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Leandro F. Vieira (leandromoh). All rights reserved.
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
    using System;

    [TestFixture]
    public class IgnoreErrorsTest
    {
        [Test]
        public void IgnoreErrorsIsLazy()
        {
            new BreakingSequence<int>().IgnoreErrors(BreakingFunc.Of<Exception, bool>());
        }

        [Test]
        public void IgnoreErrors()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => throw new TestException("True"),
                                             () => 2,
                                             () => throw new TestException("False"),
                                             () => 3);

            var result = source.IgnoreErrors((TestException e) => bool.Parse(e.Message));
            var expectations = Enumerable.Range(1, 2);

            Assert.That(result.Take(2), Is.EqualTo(expectations));
            Assert.Throws<TestException>(() => result.ElementAt(2));
        }

        [Test]
        public void IgnoreErrorsNoExceptionTypeMatch()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => 2,
                                             () => throw new TestException(),
                                             () => 3);

            var result = source.IgnoreErrors((ArgumentException e) => true);
            var expectations = Enumerable.Range(1, 2);

            Assert.That(result.Take(2), Is.EqualTo(expectations));
            Assert.Throws<TestException>(() => result.ElementAt(2));
        }

        [Test]
        public void IgnoreErrorsWithBaseException()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => throw new TestException(),
                                             () => 2,
                                             () => throw new Exception(),
                                             () => 3,
                                             () => throw new NullReferenceException(),
                                             () => 4,
                                             () => throw new ArgumentException(),
                                             () => 5);

            var result = source.IgnoreErrors((Exception e) => true);
            var expectations = Enumerable.Range(1, 5);

            Assert.That(result, Is.EqualTo(expectations));
        }

        [Test]
        public void IgnoreErrorsWithParserFunction()
        {
            var source = "O,l,2,3,4,S,6,7,B,9".Split(',').Select(int.Parse);
            var result = source.IgnoreErrors((FormatException ex) => true);
            var expectations = new[] { 2, 3, 4, 6, 7, 9 };

            Assert.That(result, Is.EqualTo(expectations));
        }
    }
}
