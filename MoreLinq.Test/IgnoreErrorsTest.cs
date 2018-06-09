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
        public void IgnoreErrorsError1IsLazy()
        {
            new BreakingSequence<int>().IgnoreErrors<int, TestException>();
        }

        [Test]
        public void IgnoreErrorsError1()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => throw new TestException(),
                                             () => throw new TestException(),
                                             () => 2,
                                             () => throw new NullReferenceException(),
                                             () => 3);

            var result = source.IgnoreErrors<int, TestException>();

            Assert.That(result.Take(2), Is.EqualTo(Enumerable.Range(1, 2)));
            Assert.Throws<NullReferenceException>(() => result.ElementAt(2));
        }

       [Test]
        public void IgnoreErrorsError1WithBaseException()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => throw new TestException(),
                                             () => 2,
                                             () => throw new NullReferenceException(),
                                             () => 3,
                                             () => throw new ArgumentException(),
                                             () => 4,
                                             () => throw new TestException(),
                                             () => 5,
                                             () => throw new NullReferenceException(),
                                             () => 6,
                                             () => throw new ArgumentException(),
                                             () => 7,
                                             () => throw new Exception(),
                                             () => 8);

            var result = source.IgnoreErrors<int, Exception>();

            Assert.That(result, Is.EqualTo(Enumerable.Range(1, 8)));
        }

        [Test]
        public void IgnoreErrorsError1InParsing()
        {
            var source = "O,l,2,3,4,S,6,7,B,9".Split(',')
                                              .Select(x => int.Parse(x, CultureInfo.InvariantCulture));
            var result = source.IgnoreErrors<int, FormatException>();

            Assert.That(result, Is.EqualTo(new[] { 2, 3, 4, 6, 7, 9 }));
        }

        [Test]
        public void IgnoreErrorsError2IsLazy()
        {
            new BreakingSequence<int>().IgnoreErrors<int, TestException, NullReferenceException>();
        }

        [Test]
        public void IgnoreErrorsError2()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => throw new TestException(),
                                             () => 2,
                                             () => throw new NullReferenceException(),
                                             () => 3,
                                             () => throw new NullReferenceException(),
                                             () => 4,
                                             () => throw new Exception(),
                                             () => 5);

            var result = source.IgnoreErrors<int, TestException, NullReferenceException>();

            Assert.That(result.Take(4), Is.EqualTo(Enumerable.Range(1, 4)));
            Assert.Throws<Exception>(() => result.ElementAt(4));
        }

        [Test]
        public void IgnoreErrorsError3IsLazy()
        {
            new BreakingSequence<int>().IgnoreErrors<int, TestException, NullReferenceException, ArgumentException>();
        }

        [Test]
        public void IgnoreErrorsError3()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => throw new TestException(),
                                             () => 2,
                                             () => throw new NullReferenceException(),
                                             () => 3,
                                             () => throw new ArgumentException(),
                                             () => 4,
                                             () => throw new TestException(),
                                             () => 5,
                                             () => throw new NullReferenceException(),
                                             () => 6,
                                             () => throw new ArgumentException(),
                                             () => 7,
                                             () => throw new Exception(),
                                             () => 8);

            var result = source.IgnoreErrors<int, TestException, NullReferenceException, ArgumentException>();

            Assert.That(result.Take(7), Is.EqualTo(Enumerable.Range(1, 7)));
            Assert.Throws<Exception>(() => result.ElementAt(7));
        }

        [Test]
        public void IgnoreErrorsError1PredicateIsLazy()
        {
            new BreakingSequence<int>().IgnoreErrors(BreakingFunc.Of<Exception, bool>());
        }

        [Test]
        public void IgnoreErrorsError1Predicate()
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
        public void IgnoreErrorsError1PredicateNoTypeMatch()
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
        public void IgnoreErrorsError1PredicateWithBaseException()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => throw new TestException(),
                                             () => throw new Exception(),
                                             () => 2,
                                             () => throw new ArgumentException(),
                                             () => 3,
                                             () => throw new NullReferenceException(),
                                             () => 4,
                                             () => throw new Exception(),
                                             () => 5);

            var result = source.IgnoreErrors((Exception e) => !(e is NullReferenceException));

            Assert.That(result.Take(3), Is.EqualTo(Enumerable.Range(1, 3)));
            Assert.Throws<NullReferenceException>(() => result.ElementAt(3));
        }

        [Test]
        public void IgnoreErrorsError2PredicateIsCaughtInOrder()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => throw new TestException(),
                                             () => 2,
                                             () => throw new Exception(),
                                             () => 3);

            Func<TestException, bool> testExceptionPredicate = e => true;
            Func<Exception, bool> exceptionPredicate = e => false;

            var result = source.IgnoreErrors(testExceptionPredicate,
                                             exceptionPredicate);

            Assert.That(result.Take(2), Is.EqualTo(Enumerable.Range(1, 2)));
            Assert.Throws<Exception>(() => result.ElementAt(2));

            result = source.IgnoreErrors(exceptionPredicate,
                                         testExceptionPredicate);

            Assert.That(result.Take(1), Is.EqualTo(Enumerable.Range(1, 1)));
            Assert.Throws<TestException>(() => result.ElementAt(1));
        }
    }
}
