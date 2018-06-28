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
    using Experimental;
    using NUnit.Framework;
    using System;
    using System.Globalization;

    [TestFixture]
    public class SkipErroneousTest
    {
        [Test]
        public void SkipErroneousError1IsLazy()
        {
            new BreakingSequence<int>().SkipErroneous<int, TestException>();
        }

        [Test]
        public void SkipErroneousError1()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => throw new TestException(),
                                             () => throw new TestException(),
                                             () => 2,
                                             () => throw new NullReferenceException(),
                                             () => 3);

            using (var test = source.AsTestingSequence())
            {
                var result = test.SkipErroneous<int, TestException>()
                                 .Memoize();

                Assert.That(result.Take(2), Is.EqualTo(Enumerable.Range(1, 2)));
                Assert.Throws<NullReferenceException>(() => result.ElementAt(2));
            }
        }

       [Test]
        public void SkipErroneousError1WithBaseException()
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

            using (var test = source.AsTestingSequence())
            {
                var result = test.SkipErroneous<int, Exception>();

                Assert.That(result, Is.EqualTo(Enumerable.Range(1, 8)));
            }
        }

        [Test]
        public void SkipErroneousError1InParsing()
        {
            var source = "O,l,2,3,4,S,6,7,B,9".Split(',')
                                              .Select(x => int.Parse(x, CultureInfo.InvariantCulture));

            using (var test = source.AsTestingSequence())
            {
                var result = test.SkipErroneous<int, FormatException>();

                Assert.That(result, Is.EqualTo(new[] { 2, 3, 4, 6, 7, 9 }));
            }
        }

        [Test]
        public void SkipErroneousError2IsLazy()
        {
            new BreakingSequence<int>().SkipErroneous<int, TestException, NullReferenceException>();
        }

        [Test]
        public void SkipErroneousError2()
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

            using (var test = source.AsTestingSequence())
            {
                var result = test.SkipErroneous<int, TestException, NullReferenceException>()
                                 .Memoize();

                Assert.That(result.Take(4), Is.EqualTo(Enumerable.Range(1, 4)));
                Assert.Throws<Exception>(() => result.ElementAt(4));
            }
        }

        [Test]
        public void SkipErroneousError3IsLazy()
        {
            new BreakingSequence<int>().SkipErroneous<int, TestException, NullReferenceException, ArgumentException>();
        }

        [Test]
        public void SkipErroneousError3()
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

            using (var test = source.AsTestingSequence())
            {
                var result = test.SkipErroneous<int, TestException, NullReferenceException, ArgumentException>()
                                 .Memoize();

                Assert.That(result.Take(7), Is.EqualTo(Enumerable.Range(1, 7)));
                Assert.Throws<Exception>(() => result.ElementAt(7));
            }
        }

        [Test]
        public void SkipErroneousError1PredicateIsLazy()
        {
            new BreakingSequence<int>().SkipErroneous(BreakingFunc.Of<Exception, bool>());
        }

        [Test]
        public void SkipErroneousError1Predicate()
        {
            const string key = "ignore";
            var source = MoreEnumerable.From(() => 1,
                                             () => throw new TestException() { Data = { [key] = true } },
                                             () => 2,
                                             () => throw new TestException() { Data = { [key] = false } },
                                             () => 3);

            using (var test = source.AsTestingSequence())
            {
                var result = test.SkipErroneous((TestException e) => (bool) e.Data[key])
                                 .Memoize();

                Assert.That(result.Take(2), Is.EqualTo(Enumerable.Range(1, 2)));
                Assert.Throws<TestException>(() => result.ElementAt(2));
            }
        }

        [Test]
        public void SkipErroneousError1PredicateNoTypeMatch()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => 2,
                                             () => throw new TestException(),
                                             () => 3);

            using (var test = source.AsTestingSequence())
            {
                var result = test.SkipErroneous((ArgumentException e) => true)
                                 .Memoize();

                Assert.That(result.Take(2), Is.EqualTo(Enumerable.Range(1, 2)));
                Assert.Throws<TestException>(() => result.ElementAt(2));
            }
        }

        [Test]
        public void SkipErroneousError1PredicateWithBaseException()
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

            using (var test = source.AsTestingSequence())
            {
                var result = test.SkipErroneous((Exception e) => !(e is NullReferenceException))
                                 .Memoize();

                Assert.That(result.Take(3), Is.EqualTo(Enumerable.Range(1, 3)));
                Assert.Throws<NullReferenceException>(() => result.ElementAt(3));
            }
        }

        [Test]
        public void SkipErroneousError2PredicateIsCaughtInOrder()
        {
            var source = MoreEnumerable.From(() => 1,
                                             () => throw new TestException(),
                                             () => 2,
                                             () => throw new Exception(),
                                             () => 3);
            var testCount = 0;
            var exceptionCount = 0;

            Func<TestException, bool> testExceptionPredicate = e => { testCount++; return true; };
            Func<Exception, bool>  exceptionPredicate = e => { exceptionCount++; return true; };

            source.SkipErroneous(testExceptionPredicate, exceptionPredicate).Consume();

            Assert.AreEqual(1, testCount);
            Assert.AreEqual(1, exceptionCount);

            testCount = 0;
            exceptionCount = 0;

            source.SkipErroneous(exceptionPredicate, testExceptionPredicate).Consume();

            Assert.AreEqual(0, testCount);
            Assert.AreEqual(2, exceptionCount);
        }
    }
}
