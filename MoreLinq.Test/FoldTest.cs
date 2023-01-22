#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2013 Atif Aziz. All rights reserved.
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
    using NUnit.Framework;

    [TestFixture]
    public class FoldTest
    {
        [Test]
        public void FoldWithTooFewItems()
        {
            Assert.That(() => Enumerable.Range(1, 3).Fold(BreakingFunc.Of<int, int, int, int, int>()),
                        Throws.TypeOf<InvalidOperationException>()
                              .And.Message.EqualTo("Sequence contains too few elements when exactly 4 were expected."));
        }

        [Test]
        public void FoldWithEmptySequence()
        {
            Assert.That(() => Enumerable.Empty<int>().Fold(BreakingFunc.Of<int, int>()),
                        Throws.TypeOf<InvalidOperationException>()
                              .And.Message.EqualTo("Sequence contains too few elements when exactly 1 was expected."));
        }

        [Test]
        public void FoldWithTooManyItems()
        {
            Assert.That(() => Enumerable.Range(1, 3).Fold(BreakingFunc.Of<int, int, int>()),
                        Throws.TypeOf<InvalidOperationException>()
                              .And.Message.EqualTo("Sequence contains too many elements when exactly 2 were expected."));
        }

        [Test]
        public void Fold()
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";

            using (var ts = alphabet.Take( 1).AsTestingSequence()) Assert.That(ts.Fold(a                                                => string.Join(string.Empty, a                                             )), Is.EqualTo("a"               ), "fold 1" );
            using (var ts = alphabet.Take( 2).AsTestingSequence()) Assert.That(ts.Fold((a, b                                          ) => string.Join(string.Empty, a, b                                          )), Is.EqualTo("ab"              ), "fold 2" );
            using (var ts = alphabet.Take( 3).AsTestingSequence()) Assert.That(ts.Fold((a, b, c                                       ) => string.Join(string.Empty, a, b, c                                       )), Is.EqualTo("abc"             ), "fold 3" );
            using (var ts = alphabet.Take( 4).AsTestingSequence()) Assert.That(ts.Fold((a, b, c, d                                    ) => string.Join(string.Empty, a, b, c, d                                    )), Is.EqualTo("abcd"            ), "fold 4" );
            using (var ts = alphabet.Take( 5).AsTestingSequence()) Assert.That(ts.Fold((a, b, c, d, e                                 ) => string.Join(string.Empty, a, b, c, d, e                                 )), Is.EqualTo("abcde"           ), "fold 5" );
            using (var ts = alphabet.Take( 6).AsTestingSequence()) Assert.That(ts.Fold((a, b, c, d, e, f                              ) => string.Join(string.Empty, a, b, c, d, e, f                              )), Is.EqualTo("abcdef"          ), "fold 6" );
            using (var ts = alphabet.Take( 7).AsTestingSequence()) Assert.That(ts.Fold((a, b, c, d, e, f, g                           ) => string.Join(string.Empty, a, b, c, d, e, f, g                           )), Is.EqualTo("abcdefg"         ), "fold 7" );
            using (var ts = alphabet.Take( 8).AsTestingSequence()) Assert.That(ts.Fold((a, b, c, d, e, f, g, h                        ) => string.Join(string.Empty, a, b, c, d, e, f, g, h                        )), Is.EqualTo("abcdefgh"        ), "fold 8" );
            using (var ts = alphabet.Take( 9).AsTestingSequence()) Assert.That(ts.Fold((a, b, c, d, e, f, g, h, i                     ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i                     )), Is.EqualTo("abcdefghi"       ), "fold 9" );
            using (var ts = alphabet.Take(10).AsTestingSequence()) Assert.That(ts.Fold((a, b, c, d, e, f, g, h, i, j                  ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j                  )), Is.EqualTo("abcdefghij"      ), "fold 10");
            using (var ts = alphabet.Take(11).AsTestingSequence()) Assert.That(ts.Fold((a, b, c, d, e, f, g, h, i, j, k               ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k               )), Is.EqualTo("abcdefghijk"     ), "fold 11");
            using (var ts = alphabet.Take(12).AsTestingSequence()) Assert.That(ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l            ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l            )), Is.EqualTo("abcdefghijkl"    ), "fold 12");
            using (var ts = alphabet.Take(13).AsTestingSequence()) Assert.That(ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m         ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m         )), Is.EqualTo("abcdefghijklm"   ), "fold 13");
            using (var ts = alphabet.Take(14).AsTestingSequence()) Assert.That(ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n      ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n      )), Is.EqualTo("abcdefghijklmn"  ), "fold 14");
            using (var ts = alphabet.Take(15).AsTestingSequence()) Assert.That(ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o   ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n, o   )), Is.EqualTo("abcdefghijklmno" ), "fold 15");
            using (var ts = alphabet.Take(16).AsTestingSequence()) Assert.That(ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p)), Is.EqualTo("abcdefghijklmnop"), "fold 16");
        }
    }
}
