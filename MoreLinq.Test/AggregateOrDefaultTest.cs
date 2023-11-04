namespace MoreLinq.Test
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class AggregateOrDefaultTest
    {
        [Test]
        public void AggregateOrDefaultWhenCollectionIsNull()
        {
            string[]? collection = null;

            var e1 = Assert.Throws<ArgumentNullException>(
                () => collection!.AggregateOrDefault((curr, next) => curr + next));
            var e2 = Assert.Throws<ArgumentNullException>(
                () => collection!.AggregateOrDefault((curr, next) => curr + next, "<null>"));

            Assert.That(e1?.ParamName, Is.EqualTo("source"));
            Assert.That(e2?.ParamName, Is.EqualTo("source"));
        }

        [Test]
        public void AggregateOrDefaultWhenFuncIsNull()
        {
            var collection = System.Linq.Enumerable.Empty<string>().ToList();

            var e1 = Assert.Throws<ArgumentNullException>(
                () => collection.AggregateOrDefault(null!));
            var e2 = Assert.Throws<ArgumentNullException>(
                () => collection.AggregateOrDefault(null!, "<null>"));

            Assert.That(e1?.ParamName, Is.EqualTo("func"));
            Assert.That(e2?.ParamName, Is.EqualTo("func"));
        }

        [Test]
        public void AggregateOrDefaultWhenCollectionIsEmpty()
        {
            var collection = System.Linq.Enumerable.Empty<string>().ToList();
            var result1 = collection.AggregateOrDefault((curr, next) => curr + next);
            var result2 = collection.AggregateOrDefault((curr, next) => curr + next, "<null>");

            Assert.That(result1, Is.Null);
            Assert.That(result2, Is.EqualTo("<null>"));
        }

        [Test]
        public void AggregateOrDefaultWhenCollectionHasOneElement()
        {
            var collection = new[] { "john" };
            var result1 = collection.AggregateOrDefault((curr, next) => curr + next);
            var result2 = collection.AggregateOrDefault((curr, next) => curr + next, "<null>");

            Assert.That(result1, Is.EqualTo("john"));
            Assert.That(result2, Is.EqualTo("john"));
        }

        [Test]
        public void AggregateOrDefaultWhenCollectionHasManyElements()
        {
            var collection = new[] { "john", "jane", "alex" };
            var result1 = collection.AggregateOrDefault((curr, next) => curr + next);
            var result2 = collection.AggregateOrDefault((curr, next) => curr + next, "<null>");

            Assert.That(result1, Is.EqualTo("johnjanealex"));
            Assert.That(result2, Is.EqualTo("johnjanealex"));
        }
    }
}
