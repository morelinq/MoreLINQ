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

    public partial class TuplewiseTest
    {
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
            TuplewiseNWideInt<Func<int, int,           int>>(MoreEnumerable.Tuplewise, (a, b      ) => a + b        );
            TuplewiseNWideInt<Func<int, int, int,      int>>(MoreEnumerable.Tuplewise, (a, b, c   ) => a + b + c    );
            TuplewiseNWideInt<Func<int, int, int, int, int>>(MoreEnumerable.Tuplewise, (a, b, c, d) => a + b + c + d);
        }

        [Test]
        public void TuplewiseStrings()
        {
            TuplewiseNWideString<Func<char, char,             string>>(MoreEnumerable.Tuplewise, (a, b      ) => string.Join(string.Empty, a, b      ));
            TuplewiseNWideString<Func<char, char, char,       string>>(MoreEnumerable.Tuplewise, (a, b, c   ) => string.Join(string.Empty, a, b, c   ));
            TuplewiseNWideString<Func<char, char, char, char, string>>(MoreEnumerable.Tuplewise, (a, b, c, d) => string.Join(string.Empty, a, b, c, d));
        }
    }
}
