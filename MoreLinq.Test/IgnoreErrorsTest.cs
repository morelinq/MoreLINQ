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
    using System.Globalization;

    [TestFixture]
    public class IgnoreErrorsTest
    {
        [Test]
        public void IgnoreErrorsPredicateIsLazy()
        {
            new BreakingSequence<int>().IgnoreErrors(BreakingFunc.Of<Exception, bool>());
        }

        [Test]
        public void IgnoreErrorsPredicate()
        {
            const string key = "ignore";
            var source = MoreEnumerable.From(() => 1,
                                             () => throw new TestException() { Data = { [key] = true } },
                                             () => 2,
                                             () => throw new TestException() { Data = { [key] = false } },
                                             () => 3);

            var result = source.IgnoreErrors((TestException e) => (bool) e.Data[key]);

            Assert.That(result.Take(2), Is.EqualTo(Enumerable.Range(1, 2)));
            Assert.Throws<TestException>(() => result.ElementAt(2));
        }

        [Test]
        public void IgnoreErrorsPredicateNoExceptionTypeMatch()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => 2,
                                             () => throw new TestException(),
                                             () => 3);

            var result = source.IgnoreErrors((ArgumentException e) => true);

            Assert.That(result.Take(2), Is.EqualTo(Enumerable.Range(1, 2)));
            Assert.Throws<TestException>(() => result.ElementAt(2));
        }

        [Test]
        public void IgnoreErrorsPredicateWithBaseException()
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

            Assert.That(result, Is.EqualTo(Enumerable.Range(1, 5)));
        }

        [Test]
        public void IgnoreErrorsPredicateInParsing()
        {
            var source = "O,l,2,3,4,S,6,7,B,9".Split(',')
                                              .Select(x => int.Parse(x, CultureInfo.InvariantCulture));
            var result = source.IgnoreErrors((FormatException ex) => true);

            Assert.That(result, Is.EqualTo(new[] { 2, 3, 4, 6, 7, 9 }));
        }
    }
}
