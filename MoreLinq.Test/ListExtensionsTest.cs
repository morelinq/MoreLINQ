#region License and Terms
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// The MIT License (MIT)
//
// Copyright(c) Microsoft Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class ListExtensionsTest
    {
        [Test]
        public static void TestAddSortedInAscSortedList()
        {
            Assert.IsTrue(Enumerable.Range(1, 4).ToList().AddSorted(5).SequenceEqual(Enumerable.Range(1, 5)));
            Assert.IsTrue(Enumerable.Range(1, 4).ToList().AddSorted(0).SequenceEqual(Enumerable.Range(0, 5)));
            Assert.IsTrue(Enumerable.Range(1, 3).Except(new[] {2}).ToList().AddSorted(2).SequenceEqual(Enumerable.Range(1, 3)));
            Assert.IsTrue(Enumerable.Range(1, 30).Except(new[] {4}).ToList().AddSorted(4).SequenceEqual(Enumerable.Range(1, 30)));
            Assert.IsTrue(Enumerable.Range(1, 30).Except(new[] {12}).ToList().AddSorted(12).SequenceEqual(Enumerable.Range(1, 30)));
            Assert.IsTrue(Enumerable.Range(1, 30).Except(new[] {17}).ToList().AddSorted(17).SequenceEqual(Enumerable.Range(1, 30)));
            Assert.IsTrue(Enumerable.Range(1, 30).Except(new[] {22}).ToList().AddSorted(22).SequenceEqual(Enumerable.Range(1, 30)));
        }

        [Test]
        public static void TestAddSortedInDescSortedList()
        {
            Assert.IsTrue(Enumerable.Range(1, 4).OrderBy(v=>v, OrderByDirection.Descending).ToList().AddSorted(5, OrderByDirection.Descending)
                .SequenceEqual(Enumerable.Range(1, 5).OrderBy(v => v, OrderByDirection.Descending)));
            Assert.IsTrue(Enumerable.Range(1, 4).OrderBy(v => v, OrderByDirection.Descending).ToList().AddSorted(0, OrderByDirection.Descending)
                .SequenceEqual(Enumerable.Range(0, 5).OrderBy(v => v, OrderByDirection.Descending)));
            Assert.IsTrue(Enumerable.Range(1, 3).Except(new[] {2}).OrderBy(v => v, OrderByDirection.Descending).ToList().AddSorted(2, OrderByDirection.Descending)
                .SequenceEqual(Enumerable.Range(1, 3).OrderBy(v => v, OrderByDirection.Descending)));
            Assert.IsTrue(Enumerable.Range(1, 30).Except(new[] {4}).OrderBy(v => v, OrderByDirection.Descending).ToList().AddSorted(4, OrderByDirection.Descending)
                .SequenceEqual(Enumerable.Range(1, 30).OrderBy(v => v, OrderByDirection.Descending)));
            Assert.IsTrue(Enumerable.Range(1, 30).Except(new[] {12}).OrderBy(v => v, OrderByDirection.Descending).ToList().AddSorted(12, OrderByDirection.Descending)
                .SequenceEqual(Enumerable.Range(1, 30).OrderBy(v => v, OrderByDirection.Descending)));
            Assert.IsTrue(Enumerable.Range(1, 30).Except(new[] {17}).OrderBy(v => v, OrderByDirection.Descending).ToList().AddSorted(17, OrderByDirection.Descending)
                .SequenceEqual(Enumerable.Range(1, 30).OrderBy(v => v, OrderByDirection.Descending)));
            Assert.IsTrue(Enumerable.Range(1, 30).Except(new[] {22}).OrderBy(v => v, OrderByDirection.Descending).ToList().AddSorted(22, OrderByDirection.Descending)
                .SequenceEqual(Enumerable.Range(1, 30).OrderBy(v => v, OrderByDirection.Descending)));
        }
    }
}
