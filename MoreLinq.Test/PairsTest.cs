#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2012 Atif Aziz. All rights reserved.
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

    [TestFixture]
    public class PairsTest
    {
        [Test]
        public void PairsNullArgument()
        {
            #pragma warning disable 8600, 8604 // need null enumeration for test
            IEnumerable<int> source = null;
            Assert.Throws<ArgumentNullException>(() => source.Pairs());
            #pragma warning restore
        }

        [TestCase("[]", "[]")]
        [TestCase("[A]", "[]")]
        [TestCase("[A,B]", "[[A,B]]")]
        [TestCase("[A,A]", "[[A,A]]")]
        [TestCase("[A,B,C]", "[[A,B],[A,C],[B,C]]")]
        [TestCase("[A,B,C,D]", "[[A,B],[A,C],[A,D],[B,C],[B,D],[C,D]]")]
        [TestCase("[A,B,A,D]")]
        public void PairwiseWithSequenceShorterThanTwo(in string input, in string expected)
        {
            var source = ParseStringArray(input);
            var expectedPairs = ParseNestedStringArray(expected).Select(strs => (strs.First(), strs.Last()));
            var actualPairs = source.Pairs();

            actualPairs.AssertSequenceEquivalent(expectedPairs);
        }

        private static IEnumerable<IEnumerable<string>> ParseNestedStringArray(in string str)
        {
            return ParseStringArray(str).Select(strs => ParseStringArray(strs));
        }        

        private static IEnumerable<string> ParseStringArray(in string str)
        {
            return str.Substring(1, str.Length - 2).Split(',');
        }
    }
}
