#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2018 Leandro F. Vieira (leandromoh). All rights reserved.
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

    /// <summary>
    /// Actions which throw NotImplementedException if they're ever called.
    /// </summary>
    static class BreakingAction
    {
        internal static Action WithoutArguments =>
            () => throw new NotImplementedException();

        internal static Action<T> Of<T>() =>
            _ => throw new NotImplementedException();

        internal static Action<T1, T2> Of<T1, T2>() =>
            (_, _) => throw new NotImplementedException();

        internal static Action<T1, T2, T3> Of<T1, T2, T3>() =>
            (_, _, _) => throw new NotImplementedException();

        internal static Action<T1, T2, T3, T4> Of<T1, T2, T3, T4>() =>
            (_, _, _, _) => throw new NotImplementedException();
    }
}
