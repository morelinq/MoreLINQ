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

using System;
using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class FoldTest
    {
        [Test]
        public void Fold1WithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source",() =>
                MoreEnumerable.Fold<object, object>(null, a => null));
        }

        [Test]
        public void Fold2WithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source", () =>
                MoreEnumerable.Fold<object, object>(null, (a, b) => null));
        }

        [Test]
        public void Fold3WithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source", () =>
                MoreEnumerable.Fold<object, object>(null, (a, b, c) => null));
        }
        
        [Test]
        public void Fold4WithNullSequence()
        {
            Assert.ThrowsArgumentNullException("source", () =>
                MoreEnumerable.Fold<object, object>(null, (a, b, c, d) => null));
        }

        [Test]
        public void Fold1WithNullFolder()
        {
            Assert.ThrowsArgumentNullException("folder",() =>
                new object[0].Fold((Func<object, object>)null));
        }

        [Test]
        public void Fold2WithNullFolder()
        {
            Assert.ThrowsArgumentNullException("folder",() =>
                new object[0].Fold((Func<object, object, object>)null));
        }
        
        [Test]
        public void Fold3WithNullFolder()
        {
            Assert.ThrowsArgumentNullException("folder", () =>
                new object[0].Fold((Func<object, object, object, object>)null));
        }
        
        [Test]
        public void Fold4WithNullFolder()
        {
            Assert.ThrowsArgumentNullException("folder", () =>
                new object[0].Fold((Func<object, object, object, object, object>)null));
        }

        [Test]
        public void FoldWithTooFewItems()
        {
            Assert.Throws<Exception>(() =>
                Enumerable.Range(1, 3).Fold((a, b, c, d) => a + b + c + d));
        }

        [Test]
        public void FoldWithEmptySequence()
        {
            Assert.Throws<Exception>(() =>
                Enumerable.Empty<int>().Fold(a => a));
        }

        [Test]
        public void FoldWithTooManyItems()
        {
            Assert.Throws<Exception>(() =>
                Enumerable.Range(1, 3).Fold((a, b) => a + b));
        }

        [Test]
        public void Fold()
        {
            Assert.AreEqual(1,  Enumerable.Range(1, 1).Fold(a => a), "fold 1");
            Assert.AreEqual(2,  Enumerable.Range(1, 2).Fold((a, b) => a * b), "fold 2");
            Assert.AreEqual(6,  Enumerable.Range(1, 3).Fold((a, b, c) => a * b * c), "fold 3");
            Assert.AreEqual(24, Enumerable.Range(1, 4).Fold((a, b, c, d) => a * b * c * d), "fold 4");
        }
    }
}