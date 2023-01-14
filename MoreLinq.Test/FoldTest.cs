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
                        Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void FoldWithEmptySequence()
        {
            Assert.That(() => Enumerable.Empty<int>().Fold(BreakingFunc.Of<int, int>()),
                        Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void FoldWithTooManyItems()
        {
            Assert.That(() => Enumerable.Range(1, 3).Fold(BreakingFunc.Of<int, int, int>()),
                        Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Fold()
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";

            Assert.That(alphabet.Take( 1).UsingTestingSequence(ts => ts.Fold(a                                                => string.Join(string.Empty, a                                             ))), Is.EqualTo("a"               ), "fold 1" );
            Assert.That(alphabet.Take( 2).UsingTestingSequence(ts => ts.Fold((a, b                                          ) => string.Join(string.Empty, a, b                                          ))), Is.EqualTo("ab"              ), "fold 2" );
            Assert.That(alphabet.Take( 3).UsingTestingSequence(ts => ts.Fold((a, b, c                                       ) => string.Join(string.Empty, a, b, c                                       ))), Is.EqualTo("abc"             ), "fold 3" );
            Assert.That(alphabet.Take( 4).UsingTestingSequence(ts => ts.Fold((a, b, c, d                                    ) => string.Join(string.Empty, a, b, c, d                                    ))), Is.EqualTo("abcd"            ), "fold 4" );
            Assert.That(alphabet.Take( 5).UsingTestingSequence(ts => ts.Fold((a, b, c, d, e                                 ) => string.Join(string.Empty, a, b, c, d, e                                 ))), Is.EqualTo("abcde"           ), "fold 5" );
            Assert.That(alphabet.Take( 6).UsingTestingSequence(ts => ts.Fold((a, b, c, d, e, f                              ) => string.Join(string.Empty, a, b, c, d, e, f                              ))), Is.EqualTo("abcdef"          ), "fold 6" );
            Assert.That(alphabet.Take( 7).UsingTestingSequence(ts => ts.Fold((a, b, c, d, e, f, g                           ) => string.Join(string.Empty, a, b, c, d, e, f, g                           ))), Is.EqualTo("abcdefg"         ), "fold 7" );
            Assert.That(alphabet.Take( 8).UsingTestingSequence(ts => ts.Fold((a, b, c, d, e, f, g, h                        ) => string.Join(string.Empty, a, b, c, d, e, f, g, h                        ))), Is.EqualTo("abcdefgh"        ), "fold 8" );
            Assert.That(alphabet.Take( 9).UsingTestingSequence(ts => ts.Fold((a, b, c, d, e, f, g, h, i                     ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i                     ))), Is.EqualTo("abcdefghi"       ), "fold 9" );
            Assert.That(alphabet.Take(10).UsingTestingSequence(ts => ts.Fold((a, b, c, d, e, f, g, h, i, j                  ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j                  ))), Is.EqualTo("abcdefghij"      ), "fold 10");
            Assert.That(alphabet.Take(11).UsingTestingSequence(ts => ts.Fold((a, b, c, d, e, f, g, h, i, j, k               ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k               ))), Is.EqualTo("abcdefghijk"     ), "fold 11");
            Assert.That(alphabet.Take(12).UsingTestingSequence(ts => ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l            ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l            ))), Is.EqualTo("abcdefghijkl"    ), "fold 12");
            Assert.That(alphabet.Take(13).UsingTestingSequence(ts => ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m         ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m         ))), Is.EqualTo("abcdefghijklm"   ), "fold 13");
            Assert.That(alphabet.Take(14).UsingTestingSequence(ts => ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n      ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n      ))), Is.EqualTo("abcdefghijklmn"  ), "fold 14");
            Assert.That(alphabet.Take(15).UsingTestingSequence(ts => ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o   ) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n, o   ))), Is.EqualTo("abcdefghijklmno" ), "fold 15");
            Assert.That(alphabet.Take(16).UsingTestingSequence(ts => ts.Fold((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) => string.Join(string.Empty, a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p))), Is.EqualTo("abcdefghijklmnop"), "fold 16");
        }
    }
}
