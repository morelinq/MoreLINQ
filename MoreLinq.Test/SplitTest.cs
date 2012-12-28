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

using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    using System.Collections.Generic;

    [TestFixture]
    public class SplitTest
    {
        [Test]
        public void SplitWithSeparatorAndResultTransformation()
        {
            var result = MoreEnumerable.Split("the quick brown fox".ToCharArray(), ' ', chars => new string(chars.ToArray()));
            result.AssertSequenceEqual("the", "quick", "brown", "fox");
        }

        [Test]
        public void SplitUptoMaxCount()
        {
            var result = MoreEnumerable.Split("the quick brown fox".ToCharArray(), ' ', 2, chars => new string(chars.ToArray()));
            result.AssertSequenceEqual("the", "quick", "brown fox");
        }

        [Test]
        public void SplitWithSeparatorSelector()
        {
            var result = MoreEnumerable.Split(new int?[] { 1, 2, null, 3, null, 4, 5, 6 }, n => n == null);
            using (var reader = Read(result))
            {
                reader.Read().AssertSequenceEqual(1, 2);
                reader.Read().AssertSequenceEqual(3);
                reader.Read().AssertSequenceEqual(4, 5, 6);
                reader.ReadEnd();
            }
        }

        [Test]
        public void SplitWithSeparatorSelectorUptoMaxCount()
        {
            var result = MoreEnumerable.Split(new int?[] { 1, 2, null, 3, null, 4, 5, 6 }, n => n == null, 1);
            using (var reader = Read(result))
            {
                reader.Read().AssertSequenceEqual(1, 2);
                reader.Read().AssertSequenceEqual(3, null, 4, 5, 6);
                reader.ReadEnd();
            }
        }

        private static SequenceReader<T> Read<T>(IEnumerable<T> source)
        {
            return new SequenceReader<T>(source);
        }
    }
}
