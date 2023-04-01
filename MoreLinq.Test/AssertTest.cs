#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2013 Atif Aziz. All rights reserved.
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
    using NUnit.Framework;

    [TestFixture]
    public class AssertTest
    {
        [Test]
        public void AssertIsLazy()
        {
            _ = new BreakingSequence<object>().Assert(BreakingFunc.Of<object, bool>());
            _ = new BreakingSequence<object>().Assert(BreakingFunc.Of<object, bool>(), BreakingFunc.Of<object, Exception>());
        }

        [Test]
        public void AssertSequenceWithValidAllElements()
        {
            var source = new[] { 2, 4, 6, 8 };
            source.Assert(n => n % 2 == 0).AssertSequenceEqual(source);
        }

        [Test]
        public void AssertSequenceWithValidSomeInvalidElements()
        {
            var source = new[] { 2, 4, 6, 7, 8, 9 };
            Assert.That(() => source.Assert(n => n % 2 == 0).Consume(),
                        Throws.InvalidOperationException);
        }

        [Test]
        public void AssertSequenceWithInvalidElementsAndCustomErrorReturningNull()
        {
            var source = new[] { 2, 4, 6, 7, 8, 9 };
            Assert.That(() => source.Assert(n => n % 2 == 0, _ => null!).Consume(),
                        Throws.InvalidOperationException);
        }

        [Test]
        public void AssertSequenceWithInvalidElementsAndCustomError()
        {
            var source = new[] { 2, 4, 6, 7, 8, 9 };
            Assert.That(() =>
                source.Assert(n => n % 2 == 0, n => new ValueException(n)).Consume(),
                Throws.TypeOf<ValueException>()
                      .With.Property(nameof(ValueException.Value)).EqualTo(7));
        }

        sealed class ValueException : Exception
        {
            public object Value { get; }
            public ValueException(object value) => Value = value;
        }
    }
}
