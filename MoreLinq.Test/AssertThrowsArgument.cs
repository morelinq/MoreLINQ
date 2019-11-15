#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 George Vovos. All rights reserved.
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

    sealed class AssertThrowsArgument
    {
        [Obsolete("This is redundant with the NullArgumentTest fixture.")]
        public static void NullException(string expectedParamName, TestDelegate code)
        {
            Exception<ArgumentNullException>(expectedParamName, code);
        }

        public static void Exception(string expectedParamName, TestDelegate code)
        {
            Exception<ArgumentException>(expectedParamName, code);
        }

        public static void OutOfRangeException(string expectedParamName, TestDelegate code)
        {
            Exception<ArgumentOutOfRangeException>(expectedParamName, code);
        }

        static void Exception<TActual>(string expectedParamName, TestDelegate code) where TActual : ArgumentException
        {
            var e = Assert.Throws<TActual>(code);

            Assert.That(e.ParamName, Is.EqualTo(expectedParamName));
        }
    }
}
