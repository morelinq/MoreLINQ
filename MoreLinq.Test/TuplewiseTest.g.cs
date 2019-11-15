#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Phillip Palk. All rights reserved.
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
    public class TuplewiseTest
    {
        private void TuplewiseNWide<TSource, TResult, TFunc>(Func<IEnumerable<TSource>, TFunc, IEnumerable<TResult>> tuplewise, TFunc resultSelector, IEnumerable<TSource> source, params TResult[] fullResult)
        {
            var arity = resultSelector.GetType().GetGenericArguments().Length - 1;

            for (var i = 0; i < fullResult.Length; ++i)
                using (var ts = source.Take(i).AsTestingSequence())
                    Assert.That(tuplewise(ts, resultSelector), Is.EqualTo(fullResult.Take(i - arity + 1)));
        }

        private void TuplewiseNWideInt<TFunc>(Func<IEnumerable<int>, TFunc, IEnumerable<int>> tuplewise, TFunc resultSelector)
        {
            const int rangeLen = 100;
            var arity = resultSelector.GetType().GetGenericArguments().Length - 1;

            TuplewiseNWide(
                tuplewise,
                resultSelector,
                Enumerable.Range(1, rangeLen),
                Enumerable.Range(1, rangeLen - (arity - 1)).Select(x => x * arity + Enumerable.Range(1, arity - 1).Sum()).ToArray()
            );
        }

        private void TuplewiseNWideString<TFunc>(Func<IEnumerable<char>, TFunc, IEnumerable<string>> tuplewise, TFunc resultSelector)
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";
            var arity = resultSelector.GetType().GetGenericArguments().Length - 1;

            TuplewiseNWide(
                tuplewise,
                resultSelector,
                alphabet,
                Enumerable.Range(0, alphabet.Length - (arity - 1)).Select(i => alphabet.Skip(i).Take(arity)).ToArray()
            );
        }

        [Test]
        public void TuplewiseIsLazy()
        {
            new BreakingSequence<object>().Tuplewise(BreakingFunc.Of<object, object,                 int>());
            new BreakingSequence<object>().Tuplewise(BreakingFunc.Of<object, object, object,         int>());
            new BreakingSequence<object>().Tuplewise(BreakingFunc.Of<object, object, object, object, int>());
        }

        [Test]
        public void TuplewiseIntegers()
        {
            TuplewiseNWideInt<Func<int, int,           int>>((source, func) => MoreEnumerable.Tuplewise(source, func), (a, b      ) => a + b        );
            TuplewiseNWideInt<Func<int, int, int,      int>>((source, func) => MoreEnumerable.Tuplewise(source, func), (a, b, c   ) => a + b + c    );
            TuplewiseNWideInt<Func<int, int, int, int, int>>((source, func) => MoreEnumerable.Tuplewise(source, func), (a, b, c, d) => a + b + c + d);
        }

        [Test]
        public void TuplewiseStrings()
        {
            TuplewiseNWideString<Func<char, char,             string>>((source, func) => MoreEnumerable.Tuplewise(source, func), (a, b      ) => string.Join(string.Empty, a, b      ));
            TuplewiseNWideString<Func<char, char, char,       string>>((source, func) => MoreEnumerable.Tuplewise(source, func), (a, b, c   ) => string.Join(string.Empty, a, b, c   ));
            TuplewiseNWideString<Func<char, char, char, char, string>>((source, func) => MoreEnumerable.Tuplewise(source, func), (a, b, c, d) => string.Join(string.Empty, a, b, c, d));
        }
    }
}
