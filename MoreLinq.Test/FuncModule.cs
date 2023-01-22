#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2020 Atif Aziz. All rights reserved.
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

    // This type is designed to be imported statically.
    //
    // Its members enable replacing explicit instantiations of `Func<...>`,
    // as in:
    //
    //     new Func<string, object, string>((a, b) => a + b)
    //
    // with the shorter version:
    //
    //     Func((string a, object b) => a + b)
    //
    // The `new` is no longer required and the return type can be omitted
    // as it can be inferred through the type of the lambda expression.

    static class FuncModule
    {
        public static Func<T, TResult> Func<T, TResult>(Func<T, TResult> f) => f;
        public static Func<T1, T2, TResult> Func<T1, T2, TResult>(Func<T1, T2, TResult> f) => f;
        public static Func<T1, T2, T3, TResult> Func<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> f) => f;
        public static Func<T1, T2, T3, T4, TResult> Func<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> f) => f;
    }
}
