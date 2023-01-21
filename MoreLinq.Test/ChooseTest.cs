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
    using System.Globalization;
    using NUnit.Framework;

    [TestFixture]
    public class ChooseTest
    {
        [Test]
        public void IsLazy()
        {
            _ = new BreakingSequence<object>().Choose(BreakingFunc.Of<object, (bool, object)>());
        }

        [Test]
        public void WithEmptySource()
        {
            using var xs = Enumerable.Empty<int>().AsTestingSequence();
            Assert.That(xs.Choose(BreakingFunc.Of<int, (bool, int)>()), Is.Empty);
        }

        [Test]
        public void None()
        {
            using var xs = Enumerable.Range(1, 10).AsTestingSequence();
            Assert.That(xs.Choose(_ => (false, 0)), Is.Empty);
        }

        [Test]
        public void ThoseParsable()
        {
            using var xs =
                "O,l,2,3,4,S,6,7,B,9"
                   .Split(',')
                   .Choose(s => (int.TryParse(s, NumberStyles.Integer,
                                              CultureInfo.InvariantCulture,
                                              out var n), n))
                   .AsTestingSequence();

            xs.AssertSequenceEqual(2, 3, 4, 6, 7, 9);
        }

        // A cheap trick to masquerade a tuple as an option

        static class Option
        {
            public static (bool IsSome, T Value) Some<T>(T value) => (true, value);
        }

        static class Option<T>
        {
#pragma warning disable CA1805 // Do not initialize unnecessarily (avoids CS0649)
            public static readonly (bool IsSome, T Value) None = default;
#pragma warning restore CA1805 // Do not initialize unnecessarily
        }

        [Test]
        public void ThoseThatAreIntegers()
        {
            new int?[] { 0, 1, 2, null, 4, null, 6, null, null, 9 }
                .Choose(e => e is { } n ? Option.Some(n) : Option<int>.None)
                .AssertSequenceEqual(0, 1, 2, 4, 6, 9);
        }

        [Test]
        public void ThoseEven()
        {
            Enumerable.Range(1, 10)
                      .Choose(x => x % 2 is 0 ? Option.Some(x) : Option<int>.None)
                      .AssertSequenceEqual(2, 4, 6, 8, 10);
        }
    }
}
