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
        public void FoldWithTooFewItems()
        {
            Assert.Throws<InvalidOperationException>(() =>
                Enumerable.Range(1, 3).Fold((a, b, c, d) => a + b + c + d));
        }

        [Test]
        public void FoldWithEmptySequence()
        {
            Assert.Throws<InvalidOperationException>(() =>
                Enumerable.Empty<int>().Fold(a => a));
        }

        [Test]
        public void FoldWithTooManyItems()
        {
            Assert.Throws<InvalidOperationException>(() =>
                Enumerable.Range(1, 3).Fold((a, b) => a + b));
        }

        [Test]
        public void Fold()
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";

            Assert.AreEqual("a"               , alphabet.Take( 1).Fold(a                                                => string.Join(string.Empty, a                                             )), "fold 1");
            Assert.AreEqual("ab"              , alphabet.Take( 2).Fold((a, b                                          ) => string.Join(string.Empty, a, b                                          )), "fold 2");
            Assert.AreEqual("abc"             , alphabet.Take( 3).Fold((a, b, c                                       ) => string.Join(string.Empty, a, b, c                                       )), "fold 3");
            Assert.AreEqual("abcd"            , alphabet.Take( 4).Fold((a, b, c, d                                    ) => string.Join(string.Empty, a, b, c, d                                    )), "fold 4");
            Assert.AreEqual("abcde"           , alphabet.Take( 5).Fold((a, b, c, d, e                                 ) => string.Join(string.Empty, a, b, c, d, e                                 )), "fold 5");
            Assert.AreEqual("abcdef"          , alphabet.Take( 6).Fold((a, b, c, d, e, f                              ) => string.Join(string.Empty, a, b, c, d, e, f                              )), "fold 6");
            Assert.AreEqual("abcdefg"         , alphabet.Take( 7).Fold((a, b, c, d, e, f, g                           ) => string.Join(string.Empty, a, b, c, d, e, f, g                           )), "fold 7");
            Assert.AreEqual("abcdefgh"        , alphabet.Take( 8).Fold((a, b, c, d, e, f, g, h                        ) => string.Join(string.Empty, a, b, c, d, e, f, g, h                        )), "fold 8");
            Assert.AreEqual("abcdefghi"       , alphabet.Take( 9).Fold((a, b, c, d, e, f, g, h, i                     ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i                     )), "fold 9");
            Assert.AreEqual("abcdefghij"      , alphabet.Take(10).Fold((a, b, c, d, e, f, g, h, i, j                  ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j                  )), "fold 10");
            Assert.AreEqual("abcdefghijk"     , alphabet.Take(11).Fold((a, b, c, d, e, f, g, h, i, j, k               ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k               )), "fold 11");
            Assert.AreEqual("abcdefghijkl"    , alphabet.Take(12).Fold((a, b, c, d, e, f, g, h, i, j, k, l            ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l            )), "fold 12");
            Assert.AreEqual("abcdefghijklm"   , alphabet.Take(13).Fold((a, b, c, d, e, f, g, h, i, j, k, l, m         ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m         )), "fold 13");
            Assert.AreEqual("abcdefghijklmn"  , alphabet.Take(14).Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n      ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n      )), "fold 14");
            Assert.AreEqual("abcdefghijklmno" , alphabet.Take(15).Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o   ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n, o   )), "fold 15");
            Assert.AreEqual("abcdefghijklmnop", alphabet.Take(16).Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p)), "fold 16");
        }
    }
}