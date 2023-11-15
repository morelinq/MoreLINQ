#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2023 Julien Aspirot. All rights reserved.
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
    using NUnit.Framework;

    [TestFixture]
    public class DuplicatesTest
    {
        [Test]
        public void Duplicates_IsLazy()
        {
            _ = new BreakingSequence<object>().Duplicates();
        }

        [Test]
        public void When_Asking_For_Duplicates_On_Sequence_Without_Duplicates_Then_Empty_Sequence_Is_Returned()
        {
            var stringArray = new[]
            {
                "FirstElement",
                "SecondElement",
                "ThirdElement"
            };

            var duplicates = stringArray.Duplicates();

            Assert.That(duplicates, Is.Empty);
        }

        [Test]
        public void When_Asking_For_Duplicates_On_Sequence_With_Duplicates_Then_Duplicates_Are_Returned()
        {
            var stringArray = new[]
            {
                "FirstElement",
                "DUPLICATED_STRING",
                "DUPLICATED_STRING",
                "DUPLICATED_STRING",
                "ThirdElement"
            };

            var duplicates = stringArray.Duplicates().ToArray();

            Assert.That(duplicates, Contains.Item("DUPLICATED_STRING"));
            Assert.That(duplicates.AtMost(1), Is.True);
        }

        [Test]
        public void When_Asking_For_Duplicates_On_Sequence_With_Multiple_Duplicates_Then_Duplicates_Are_Returned()
        {
            var stringArray = new[]
            {
                "FirstElement",
                "DUPLICATED_STRING",
                "DUPLICATED_STRING",
                "DUPLICATED_STRING",
                "ThirdElement",
                "SECOND_DUPLICATED_STRING",
                "SECOND_DUPLICATED_STRING"
            };

            var duplicates = stringArray.Duplicates().ToArray();

            Assert.That(duplicates, Contains.Item("DUPLICATED_STRING"));
            Assert.That(duplicates, Contains.Item("SECOND_DUPLICATED_STRING"));
            Assert.That(duplicates.AtMost(2), Is.True);
        }

        [Test]
        public void When_Asking_For_Duplicates_On_Sequence_With_Custom_Always_True_Comparer_Then_Duplicates_Are_Returned()
        {
            var stringArray = new[]
            {
                "FirstElement",
                "SecondElement",
                "ThirdElement"
            };

            var duplicates = stringArray.Duplicates(new DummyStringAlwaysTrueComparer()).ToArray();

            Assert.That(duplicates.AtMost(1), Is.True);
        }

        [Test]
        public void When_Asking_For_Duplicates_On_Sequence_With_Custom_Always_False_Comparer_Then_Then_Empty_Sequence_Is_Returned()
        {
            var stringArray = new[]
            {
                "FirstElement",
                "SecondElement",
                "ThirdElement"
            };

            var duplicates = stringArray.Duplicates(new DummyStringAlwaysFalseComparer());

            Assert.That(duplicates, Is.Empty);
        }

        [Test]
        public void When_Asking_For_Duplicates_On_Multiple_Duplicates_Sequence_With_Custom_Always_False_Comparer_Then_Then_Empty_Sequence_Is_Returned()
        {
            var stringArray = new[]
            {
                "DUPLICATED_STRING",
                "DUPLICATED_STRING",
                "DUPLICATED_STRING"
            };

            var duplicates = stringArray.Duplicates(new DummyStringAlwaysFalseComparer());

            Assert.That(duplicates, Is.Empty);
        }

        sealed class DummyStringAlwaysTrueComparer : IEqualityComparer<string>
        {
            public bool Equals(string? x, string? y) => true;

            public int GetHashCode(string obj) => 0;
        }

        sealed class DummyStringAlwaysFalseComparer : IEqualityComparer<string>
        {
            public bool Equals(string? x, string? y) => false;

            public int GetHashCode(string obj) => 0;
        }
    }
}
