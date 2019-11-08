using NUnit.Framework.Interfaces;

namespace MoreLinq.Test
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class BatchByTests
    {
        public static readonly IEnumerable<ITestCaseData> TestData =
            from d in new[]
            {
                new {
                    s = new[] {"Q1", "Q2", "A1", "A2"},
                    k = new[] {"Q", "A"},
                    r = new[] {
                        new [] {("A", "A1"), ("Q", "Q1")},
                        new [] {("A", "A2"), ("Q", "Q2")}}},
                new {
                    s = new[] {"Q1", "A1", "Q2", "A2"},
                    k = new[] {"Q", "A"},
                    r = new[] {
                        new [] {("A", "A1"), ("Q", "Q1")},
                        new [] {("A", "A2"), ("Q", "Q2")}}},
                new {
                    s = new[] {"A1", "Q1", "Q2", "A2"},
                    k = new[] {"Q", "A"},
                    r = new[] {
                        new [] {("A", "A1"), ("Q", "Q1")},
                        new [] {("A", "A2"), ("Q", "Q2")}}},
                new {
                    s = new[] {"Q1", "A1", "A2", "Q2"},
                    k = new[] {"Q", "A"},
                    r = new[] {
                        new [] {("A", "A1"), ("Q", "Q1")},
                        new [] {("A", "A2"), ("Q", "Q2")}}},
                new {
                    s = new[] {"A1", "Q1", "A2", "Q2"},
                    k = new[] {"Q", "A"},
                    r = new[] {
                        new [] {("A", "A1"), ("Q", "Q1")},
                        new [] {("A", "A2"), ("Q", "Q2")}}},
                new {
                    s = new[] {"A1", "A2", "Q1", "Q2"},
                    k = new[] {"Q", "A"},
                    r = new[] {
                        new [] {("A", "A1"), ("Q", "Q1")},
                        new [] {("A", "A2"), ("Q", "Q2")}}},
                new {
                    s = new[] {"Q1", null, "A1", "#", "Q2", "A2"},
                    k = new[] {"Q", "A"},
                    r = new[] {
                        new [] {("A", "A1"), ("Q", "Q1")},
                        new [] {("A", "A2"), ("Q", "Q2")}}},
            }
            select new TestCaseData(d.s, d.k).Returns(d.r);


        [Test, TestCaseSource(nameof(TestData))]
        public (string Key, string Value)[][] BatchByOnKnownCase(string[] source, string[] acceptedKeys)
        {
            static string KeySelector(string s) => s?.FirstOrDefault().ToString();
            var equalityComparer = EqualityComparer<string>.Default;

            return source.BatchBy(acceptedKeys, KeySelector, equalityComparer)
                .Select(d => d.Select(kvp => (kvp.Key, kvp.Value)).OrderBy(kvp => kvp.Key).ToArray())
                .ToArray();
        }

        [Test]
        public void BatchByDoNotEnumerateSourceOnEmptyAcceptedKeys()
        {
            var source = MoreEnumerable.From<int>(() => throw new TestException());
            static string KeySelector(int i) => $"{i}";
            var acceptedKeys = Enumerable.Empty<string>();
            var equalityComparer = EqualityComparer<string>.Default;

            void Code()
            {
                source.BatchBy(acceptedKeys, KeySelector, equalityComparer);
            }

            Assert.DoesNotThrow(Code);
        }

        [Test]
        public void BatchByDoNotReEnumerateSourceOnMultipleEnumeration()
        {
            var source = Enumerable.Range(0, 5);
            static string KeySelector(int i) => $"{i}";
            var acceptedKeys = TestingSequence.Of("0", "1", "2", "3");
            var equalityComparer = EqualityComparer<string>.Default;

            void Code()
            {
                var enumerable = source.BatchBy(acceptedKeys, KeySelector, equalityComparer);
                enumerable.Consume();
                enumerable.Consume();
            }

            Assert.DoesNotThrow(Code);
        }

        [Test]
        public void BatchByDoNotThrowOnDuplicateAcceptedKeyAtCreation()
        {
            using var source = TestingSequence.Of(0, 1, 2, 3);
            static string KeySelector(int i) => $"{i}";
            var acceptedKeys = TestingSequence.Of("0", "1", "1", "3");
            var equalityComparer = EqualityComparer<string>.Default;

            void Code()
            {
                source.BatchBy(acceptedKeys, KeySelector, equalityComparer);
            }

            Assert.DoesNotThrow(Code);
        }

        [Test]
        public void BatchByDoNotThrowOnNullAcceptedKeyAtCreation()
        {
            using var source = TestingSequence.Of(0, 1, 2, 3);
            static string KeySelector(int i) => $"{i}";
            var acceptedKeys = TestingSequence.Of("0", "1", null, "3");
            var equalityComparer = EqualityComparer<string>.Default;

            void Code()
            {
                source.BatchBy(acceptedKeys, KeySelector, equalityComparer);
            }

            Assert.DoesNotThrow(Code);
        }

        [Test]
        public void BatchByDoNotThrowOnNullKeyFromKeySelector()
        {
            using var source = TestingSequence.Of(0, 1, 2, 3);
            static string KeySelector(int i) => i == 0 ? null : $"{i}";
            var acceptedKeys = TestingSequence.Of("0", "1", "1", "3");
            var equalityComparer = EqualityComparer<string>.Default;

            void Code()
            {
                source.BatchBy(acceptedKeys, KeySelector, equalityComparer);
            }

            Assert.DoesNotThrow(Code);
        }

        [Test]
        public void BatchByDoNotThrowOnUnknownKeyFromKeySelector()
        {
            using var source = TestingSequence.Of(0, 1, 2, 3);
            static string KeySelector(int i) => $"#{i}#";
            var acceptedKeys = TestingSequence.Of("0", "1", "1", "3");
            var equalityComparer = EqualityComparer<string>.Default;

            void Code()
            {
                source.BatchBy(acceptedKeys, KeySelector, equalityComparer);
            }

            Assert.DoesNotThrow(Code);
        }

        [Test]
        public void BatchByIsLazy()
        {
            var source = new BreakingSequence<int>();
            static string KeySelector(int i) => $"{i}";
            var acceptedKeys = TestingSequence.Of("0", "1", "2", "3");
            var equalityComparer = EqualityComparer<string>.Default;

            void Code()
            {
                source.BatchBy(acceptedKeys, KeySelector, equalityComparer);
            }

            Assert.DoesNotThrow(Code);
        }

        [Test]
        public void BatchByThrowOnDuplicateAcceptedKeyOnFirstIteration()
        {
            using var source = TestingSequence.Of(0, 1, 2, 3);
            static string KeySelector(int i) => $"{i}";
            var acceptedKeys = TestingSequence.Of("0", "1", "1", "3");
            var equalityComparer = EqualityComparer<string>.Default;

            void Code()
            {
                source.BatchBy(acceptedKeys, KeySelector, equalityComparer).GetEnumerator().MoveNext();
            }

            Assert.Throws<ArgumentException>(Code);
        }

        [Test]
        public void BatchByThrowOnNullAcceptedKeyOnFirstIteration()
        {
            using var source = TestingSequence.Of(0, 1, 2, 3);
            static string KeySelector(int i) => $"{i}";
            var acceptedKeys = TestingSequence.Of("0", "1", null, "3");
            var equalityComparer = EqualityComparer<string>.Default;

            void Code()
            {
                source.BatchBy(acceptedKeys, KeySelector, equalityComparer).GetEnumerator().MoveNext();
            }

            Assert.Throws<ArgumentNullException>(Code);
        }
    }
}
