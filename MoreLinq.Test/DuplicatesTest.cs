namespace MoreLinq.Test
{
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class DuplicatesTest
    {
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
        public void When_Asking_For_Duplicates_On_Sequence_With_Duplicates_Then_True_Is_Returned()
        {
            var stringArray = new[]
            {
                "FirstElement",
                "DUPLICATED_STRING",
                "DUPLICATED_STRING",
                "ThirdElement"
            };

            var duplicates = stringArray.Duplicates();

            Assert.That(duplicates, Contains.Item("DUPLICATED_STRING"));
        }

        [Test]
        public void When_Asking_For_Duplicates_On_Sequence_With_Duplicates_Then_It_Does_Not_Iterate_Unnecessary_On_Elements()
        {
            var source = MoreEnumerable.From(() => "FirstElement",
                () => "DUPLICATED_STRING",
                () => "DUPLICATED_STRING",
                () => throw new TestException());

            Assert.DoesNotThrow(() => source.Duplicates());
        }

        [Test]
        public void When_Asking_Duplicates_Then_It_Is_Executed_Right_Away()
        {
            _ = Assert.Throws<BreakException>(() => new BreakingSequence<string>().Duplicates().Consume());
        }

        [Test]
        public void When_Asking_For_Duplicates_On_Sequence_With_Custom_Always_True_Comparer_Then_True_Is_Returned()
        {
            var stringArray = new[]
            {
                "FirstElement",
                "SecondElement",
                "ThirdElement"
            };

            var duplicates = stringArray.Duplicates(new DummyStringAlwaysTrueComparer()).ToArray();

            Assert.That(duplicates, Contains.Item(stringArray[1]));
            Assert.That(duplicates, Contains.Item(stringArray[2]));
        }

        [Test]
        public void When_Asking_For_Duplicates_On_None_Duplicates_Sequence_With_Custom_Always_True_Comparer_Then_True_Is_Returned()
        {
            var stringArray = new[]
            {
                "FirstElement",
                "SecondElement",
                "ThirdElement"
            };

            var duplicates = stringArray.Duplicates(new DummyStringAlwaysTrueComparer()).ToArray();

            Assert.That(duplicates, Contains.Item(stringArray[1]));
            Assert.That(duplicates, Contains.Item(stringArray[2]));
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

        sealed class DummyClass
        {
            public string ComparableString { get; }

            public DummyClass(string comparableString) => ComparableString = comparableString;
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
