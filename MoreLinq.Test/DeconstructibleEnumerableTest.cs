#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Atif Aziz. All rights reserved.
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
    public class DeconstructibleEnumerableTest
    {
        [Test]
        public void InitWithNullSource()
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                new DeconstructibleEnumerable<object>(null));

            Assert.That(e.ParamName, Is.EqualTo("source"));
        }

        [Test]
        public void DeconstructDefault()
        {
            var e = Assert.Throws<InvalidOperationException>(() =>
                default(DeconstructibleEnumerable<int>).Deconstruct(out _, out _));
            Assert.That(e.Message, Contains.Substring("short"));
        }

        [Test]
        public void Deconstruct2()
        {
            using (var xs = Enumerable.Range(1, 2).AsTestingSequence())
            {
                var (a, b) = xs.AsDeconstructible();
                Assert.That(a, Is.EqualTo(1));
                Assert.That(b, Is.EqualTo(2));
            }
        }

        [Test]
        public void Deconstruct3()
        {
            using (var xs = Enumerable.Range(1, 3).AsTestingSequence())
            {
                var (a, b, c) = xs.AsDeconstructible();
                Assert.That(a, Is.EqualTo(1));
                Assert.That(b, Is.EqualTo(2));
                Assert.That(c, Is.EqualTo(3));
            }
        }

        [Test]
        public void Deconstruct4()
        {
            using (var xs = Enumerable.Range(1, 4).AsTestingSequence())
            {
                var (a, b, c, d) = xs.AsDeconstructible();
                Assert.That(a, Is.EqualTo(1));
                Assert.That(b, Is.EqualTo(2));
                Assert.That(c, Is.EqualTo(3));
                Assert.That(d, Is.EqualTo(4));
            }
        }

        [Test]
        public void Deconstruct5()
        {
            using (var xs = Enumerable.Range(1, 5).AsTestingSequence())
            {
                var (a, b, c, d, e) = xs.AsDeconstructible();
                Assert.That(a, Is.EqualTo(1));
                Assert.That(b, Is.EqualTo(2));
                Assert.That(c, Is.EqualTo(3));
                Assert.That(d, Is.EqualTo(4));
                Assert.That(e, Is.EqualTo(5));
            }
        }

        [Test]
        public void Deconstruct6()
        {
            using (var xs = Enumerable.Range(1, 6).AsTestingSequence())
            {
                var (a, b, c, d, e, f) = xs.AsDeconstructible();
                Assert.That(a, Is.EqualTo(1));
                Assert.That(b, Is.EqualTo(2));
                Assert.That(c, Is.EqualTo(3));
                Assert.That(d, Is.EqualTo(4));
                Assert.That(e, Is.EqualTo(5));
                Assert.That(f, Is.EqualTo(6));
            }
        }

        [Test]
        public void Deconstruct7()
        {
            using (var xs = Enumerable.Range(1, 7).AsTestingSequence())
            {
                var (a, b, c, d, e, f, g) = xs.AsDeconstructible();
                Assert.That(a, Is.EqualTo(1));
                Assert.That(b, Is.EqualTo(2));
                Assert.That(c, Is.EqualTo(3));
                Assert.That(d, Is.EqualTo(4));
                Assert.That(e, Is.EqualTo(5));
                Assert.That(f, Is.EqualTo(6));
                Assert.That(g, Is.EqualTo(7));
            }
        }

        [Test]
        public void Deconstruct8()
        {
            using (var xs = Enumerable.Range(1, 8).AsTestingSequence())
            {
                var (a, b, c, d, e, f, g, h) = xs.AsDeconstructible();
                Assert.That(a, Is.EqualTo(1));
                Assert.That(b, Is.EqualTo(2));
                Assert.That(c, Is.EqualTo(3));
                Assert.That(d, Is.EqualTo(4));
                Assert.That(e, Is.EqualTo(5));
                Assert.That(f, Is.EqualTo(6));
                Assert.That(g, Is.EqualTo(7));
                Assert.That(h, Is.EqualTo(8));
            }
        }

        [Test]
        public void Deconstruct9()
        {
            using (var xs = Enumerable.Range(1, 9).AsTestingSequence())
            {
                var (a, b, c, d, e, f, g, h, i) = xs.AsDeconstructible();
                Assert.That(a, Is.EqualTo(1));
                Assert.That(b, Is.EqualTo(2));
                Assert.That(c, Is.EqualTo(3));
                Assert.That(d, Is.EqualTo(4));
                Assert.That(e, Is.EqualTo(5));
                Assert.That(f, Is.EqualTo(6));
                Assert.That(g, Is.EqualTo(7));
                Assert.That(h, Is.EqualTo(8));
                Assert.That(i, Is.EqualTo(9));
            }
        }

        [Test]
        public void Deconstruct10()
        {
            using (var xs = Enumerable.Range(1, 10).AsTestingSequence())
            {
                var (a, b, c, d, e, f, g, h, i, j) = xs.AsDeconstructible();
                Assert.That(a, Is.EqualTo(1));
                Assert.That(b, Is.EqualTo(2));
                Assert.That(c, Is.EqualTo(3));
                Assert.That(d, Is.EqualTo(4));
                Assert.That(e, Is.EqualTo(5));
                Assert.That(f, Is.EqualTo(6));
                Assert.That(g, Is.EqualTo(7));
                Assert.That(h, Is.EqualTo(8));
                Assert.That(i, Is.EqualTo(9));
                Assert.That(j, Is.EqualTo(10));
            }
        }

        [Test]
        public void Deconstruct11()
        {
            using (var xs = Enumerable.Range(1, 11).AsTestingSequence())
            {
                var (a, b, c, d, e, f, g, h, i, j, k) = xs.AsDeconstructible();
                Assert.That(a, Is.EqualTo(1));
                Assert.That(b, Is.EqualTo(2));
                Assert.That(c, Is.EqualTo(3));
                Assert.That(d, Is.EqualTo(4));
                Assert.That(e, Is.EqualTo(5));
                Assert.That(f, Is.EqualTo(6));
                Assert.That(g, Is.EqualTo(7));
                Assert.That(h, Is.EqualTo(8));
                Assert.That(i, Is.EqualTo(9));
                Assert.That(j, Is.EqualTo(10));
                Assert.That(k, Is.EqualTo(11));
            }
        }

        [Test]
        public void Deconstruct12()
        {
            using (var xs = Enumerable.Range(1, 12).AsTestingSequence())
            {
                var (a, b, c, d, e, f, g, h, i, j, k, l) = xs.AsDeconstructible();
                Assert.That(a, Is.EqualTo(1));
                Assert.That(b, Is.EqualTo(2));
                Assert.That(c, Is.EqualTo(3));
                Assert.That(d, Is.EqualTo(4));
                Assert.That(e, Is.EqualTo(5));
                Assert.That(f, Is.EqualTo(6));
                Assert.That(g, Is.EqualTo(7));
                Assert.That(h, Is.EqualTo(8));
                Assert.That(i, Is.EqualTo(9));
                Assert.That(j, Is.EqualTo(10));
                Assert.That(k, Is.EqualTo(11));
                Assert.That(l, Is.EqualTo(12));
            }
        }

        [Test]
        public void Deconstruct13()
        {
            using (var xs = Enumerable.Range(1, 13).AsTestingSequence())
            {
                var (a, b, c, d, e, f, g, h, i, j, k, l, m) = xs.AsDeconstructible();
                Assert.That(a, Is.EqualTo(1));
                Assert.That(b, Is.EqualTo(2));
                Assert.That(c, Is.EqualTo(3));
                Assert.That(d, Is.EqualTo(4));
                Assert.That(e, Is.EqualTo(5));
                Assert.That(f, Is.EqualTo(6));
                Assert.That(g, Is.EqualTo(7));
                Assert.That(h, Is.EqualTo(8));
                Assert.That(i, Is.EqualTo(9));
                Assert.That(j, Is.EqualTo(10));
                Assert.That(k, Is.EqualTo(11));
                Assert.That(l, Is.EqualTo(12));
                Assert.That(m, Is.EqualTo(13));
            }
        }

        [Test]
        public void Deconstruct14()
        {
            using (var xs = Enumerable.Range(1, 14).AsTestingSequence())
            {
                var (a, b, c, d, e, f, g, h, i, j, k, l, m, n) = xs.AsDeconstructible();
                Assert.That(a, Is.EqualTo(1));
                Assert.That(b, Is.EqualTo(2));
                Assert.That(c, Is.EqualTo(3));
                Assert.That(d, Is.EqualTo(4));
                Assert.That(e, Is.EqualTo(5));
                Assert.That(f, Is.EqualTo(6));
                Assert.That(g, Is.EqualTo(7));
                Assert.That(h, Is.EqualTo(8));
                Assert.That(i, Is.EqualTo(9));
                Assert.That(j, Is.EqualTo(10));
                Assert.That(k, Is.EqualTo(11));
                Assert.That(l, Is.EqualTo(12));
                Assert.That(m, Is.EqualTo(13));
                Assert.That(n, Is.EqualTo(14));
            }
        }

        [Test]
        public void Deconstruct15()
        {
            using (var xs = Enumerable.Range(1, 15).AsTestingSequence())
            {
                var (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) = xs.AsDeconstructible();
                Assert.That(a, Is.EqualTo(1));
                Assert.That(b, Is.EqualTo(2));
                Assert.That(c, Is.EqualTo(3));
                Assert.That(d, Is.EqualTo(4));
                Assert.That(e, Is.EqualTo(5));
                Assert.That(f, Is.EqualTo(6));
                Assert.That(g, Is.EqualTo(7));
                Assert.That(h, Is.EqualTo(8));
                Assert.That(i, Is.EqualTo(9));
                Assert.That(j, Is.EqualTo(10));
                Assert.That(k, Is.EqualTo(11));
                Assert.That(l, Is.EqualTo(12));
                Assert.That(m, Is.EqualTo(13));
                Assert.That(n, Is.EqualTo(14));
                Assert.That(o, Is.EqualTo(15));
            }
        }

        [Test]
        public void Deconstruct16()
        {
            using (var xs = Enumerable.Range(1, 16).AsTestingSequence())
            {
                var (a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) = xs.AsDeconstructible();
                Assert.That(a, Is.EqualTo(1));
                Assert.That(b, Is.EqualTo(2));
                Assert.That(c, Is.EqualTo(3));
                Assert.That(d, Is.EqualTo(4));
                Assert.That(e, Is.EqualTo(5));
                Assert.That(f, Is.EqualTo(6));
                Assert.That(g, Is.EqualTo(7));
                Assert.That(h, Is.EqualTo(8));
                Assert.That(i, Is.EqualTo(9));
                Assert.That(j, Is.EqualTo(10));
                Assert.That(k, Is.EqualTo(11));
                Assert.That(l, Is.EqualTo(12));
                Assert.That(m, Is.EqualTo(13));
                Assert.That(n, Is.EqualTo(14));
                Assert.That(o, Is.EqualTo(15));
                Assert.That(p, Is.EqualTo(16));
            }
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        [TestCase(16)]
        public void DeconstructWithTooShortSequence(int count)
        {
            using (var xs = Enumerable.Range(1, count - 1).AsTestingSequence())
            {
                var e = Assert.Throws<InvalidOperationException>(() =>
                    Deconstruct(xs, count));
                Assert.That(e.Message, Contains.Substring("short"));
            }
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        [TestCase(15)]
        [TestCase(16)]
        public void DeconstructWithTooLongSequence(int count)
        {
            using (var xs = Enumerable.Range(1, count + 1).AsTestingSequence())
            {
                var e = Assert.Throws<InvalidOperationException>(() =>
                    Deconstruct(xs, count));
                Assert.That(e.Message, Contains.Substring("long"));
            }
        }

        static void Deconstruct<T>(IEnumerable<T> source, int count)
        {
            switch (count)
            {
                case  2: source.AsDeconstructible().Deconstruct(out _, out _); break;
                case  3: source.AsDeconstructible().Deconstruct(out _, out _, out _); break;
                case  4: source.AsDeconstructible().Deconstruct(out _, out _, out _, out _); break;
                case  5: source.AsDeconstructible().Deconstruct(out _, out _, out _, out _, out _); break;
                case  6: source.AsDeconstructible().Deconstruct(out _, out _, out _, out _, out _, out _); break;
                case  7: source.AsDeconstructible().Deconstruct(out _, out _, out _, out _, out _, out _, out _); break;
                case  8: source.AsDeconstructible().Deconstruct(out _, out _, out _, out _, out _, out _, out _, out _); break;
                case  9: source.AsDeconstructible().Deconstruct(out _, out _, out _, out _, out _, out _, out _, out _, out _); break;
                case 10: source.AsDeconstructible().Deconstruct(out _, out _, out _, out _, out _, out _, out _, out _, out _, out _); break;
                case 11: source.AsDeconstructible().Deconstruct(out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _); break;
                case 12: source.AsDeconstructible().Deconstruct(out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _); break;
                case 13: source.AsDeconstructible().Deconstruct(out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _); break;
                case 14: source.AsDeconstructible().Deconstruct(out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _); break;
                case 15: source.AsDeconstructible().Deconstruct(out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _); break;
                case 16: source.AsDeconstructible().Deconstruct(out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _, out _); break;
                default: throw new ArgumentOutOfRangeException(nameof(count), count, null);
            }
        }
    }
}
