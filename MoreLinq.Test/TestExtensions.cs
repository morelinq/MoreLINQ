#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
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
    using System.Collections.Generic;
    using NUnit.Framework;
    using NUnit.Framework.Constraints;

    public enum SourceKind
    {
        Sequence,
        BreakingList,
        BreakingReadOnlyList,
        BreakingCollection,
        BreakingReadOnlyCollection
    }

    static partial class TestExtensions
    {
        /// <summary>
        /// Just to make our testing easier so we can chain the assertion call.
        /// </summary>

        internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected) =>
            Assert.That(actual, Is.EqualTo(expected));

        /// <summary>
        /// Make testing even easier - a params array makes for readable tests :)
        /// The sequence should be evaluated exactly once.
        /// </summary>

        internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, params T[] expected) =>
            Assert.That(actual, Is.EqualTo(expected));

        internal static void AssertSequence<T>(this IEnumerable<T> actual, params IResolveConstraint[] expectations)
        {
            var i = 0;
            foreach (var item in actual)
            {
                Assert.That(i, Is.LessThan(expectations.Length), "Actual sequence has more items than expected.");
                var expectation = expectations[i];
                Assert.That(item, expectation, "Unexpected element in sequence at index " + i);
                i++;
            }
            Assert.That(i, Is.EqualTo(expectations.Length), "Actual sequence has fewer items than expected.");
        }

        internal static IEnumerable<string> GenerateSplits(this string str, params char[] separators)
        {
            foreach (var split in str.Split(separators))
                yield return split;
        }

        internal static IEnumerable<IEnumerable<T>> ArrangeCollectionTestCases<T>(this IEnumerable<T> input)
        {
            yield return input.ToSourceKind(SourceKind.Sequence);
            yield return input.ToSourceKind(SourceKind.BreakingReadOnlyCollection);
            yield return input.ToSourceKind(SourceKind.BreakingCollection);
        }

        internal static IEnumerable<T> ToSourceKind<T>(this IEnumerable<T> input, SourceKind sourceKind)
        {
            switch (sourceKind)
            {
                case SourceKind.Sequence:
                    return input.Select(x => x);
                case SourceKind.BreakingList:
                    return new BreakingList<T>(input.ToList());
                case SourceKind.BreakingReadOnlyList:
                    return new BreakingReadOnlyList<T>(input.ToList());
                case SourceKind.BreakingCollection:
                    return new BreakingCollection<T>(input.ToList());
                case SourceKind.BreakingReadOnlyCollection:
                    return new BreakingReadOnlyCollection<T>(input.ToList());
                default:
                    throw new ArgumentException(null, nameof(sourceKind));
            }
        }
    }
}
