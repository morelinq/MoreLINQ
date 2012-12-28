#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008-2011 Jonathan Skeet. All rights reserved.
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MoreLinq.Test
{
    internal static class TestExtensions
    {
        /// <summary>
        /// Just to make our testing easier, all ourselves to use the real SequenceEquals
        /// call from LINQ to Obects.
        /// </summary>
        internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            Assert.IsTrue(actual.SequenceEqual(expected));
        }

        /// <summary>
        /// Make testing even easier - a params array makes for readable tests :)
        /// The sequence is evaluated exactly once.
        /// </summary>
        internal static void AssertSequenceEqual<T>(this IEnumerable<T> actual, params T[] expected)
        {
            // Working with a copy means we can look over it more than once.
            // We're safe to do that with the array anyway.
            var copy = actual.ToList();
            var result = copy.SequenceEqual(expected);
            // Looks nicer than Assert.IsTrue or Assert.That, unfortunately.
            if (!result)
            {
                Assert.Fail("Expected: " +
                    ",".InsertBetween(expected.Select(x => Convert.ToString(x))) + "; was: " +
                    ",".InsertBetween(copy.Select(x => Convert.ToString(x))));
            }
        }

        internal static string InsertBetween(this string delimiter, IEnumerable<string> items)
        {
            var builder = new StringBuilder();
            foreach (var item in items)
            {
                if (builder.Length != 0)
                {
                    builder.Append(delimiter);
                }
                builder.Append(item);
            }
            return builder.ToString();
        }

        internal static IEnumerable<string> GenerateSplits(this string str, params char[] separators)
        {
            foreach (var split in str.Split(separators))
                yield return split;
        }

    }
}
