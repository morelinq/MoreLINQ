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
        [ExpectedException(typeof(ArgumentNullException))]
        public void Fold1WithNullSequence()
        {
            MoreEnumerable.Fold<object, object>(null, a => null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Fold2WithNullSequence()
        {
            MoreEnumerable.Fold<object, object>(null, (a, b) => null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Fold3WithNullSequence()
        {
            MoreEnumerable.Fold<object, object>(null, (a, b, c) => null);
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Fold4WithNullSequence()
        {
            MoreEnumerable.Fold<object, object>(null, (a, b, c, d) => null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Fold1WithNullFolder()
        {
            new object[0].Fold((Func<object, object>) null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Fold2WithNullFolder()
        {
            new object[0].Fold((Func<object, object, object>) null);
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Fold3WithNullFolder()
        {
            new object[0].Fold((Func<object, object, object, object>) null);
        }
        
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Fold4WithNullFolder()
        {
            new object[0].Fold((Func<object, object, object, object, object>) null);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void FoldWithTooFewItems()
        {
            Enumerable.Range(1, 3).Fold((a, b, c, d) => a + b + c + d);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void FoldWithTooManyItems()
        {
            Enumerable.Range(1, 3).Fold((a, b) => a + b);
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