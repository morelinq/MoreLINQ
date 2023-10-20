#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2022 Atif Aziz. All rights reserved.
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
    using NUnit.Framework.Constraints;

    static class Throws
    {
        public static ThrowsNothingConstraint Nothing => NUnit.Framework.Throws.Nothing;
        public static ExactTypeConstraint InvalidOperationException => NUnit.Framework.Throws.InvalidOperationException;
        public static ExactTypeConstraint ObjectDisposedException => TypeOf<ObjectDisposedException>();
        public static ExactTypeConstraint BreakException => TypeOf<BreakException>();

        public static InstanceOfTypeConstraint InstanceOf<T>()
            where T : Exception =>
            NUnit.Framework.Throws.InstanceOf<T>();

        public static ExactTypeConstraint TypeOf<T>()
            where T : Exception =>
            NUnit.Framework.Throws.TypeOf<T>();

        public static EqualConstraint ArgumentException(string expectedParamName) =>
            NUnit.Framework.Throws.ArgumentException.With.ParamName().EqualTo(expectedParamName);

        public static EqualConstraint ArgumentNullException(string expectedParamName) =>
            NUnit.Framework.Throws.ArgumentNullException.With.ParamName().EqualTo(expectedParamName);

        public static ExactTypeConstraint ArgumentOutOfRangeException() =>
            TypeOf<ArgumentOutOfRangeException>();

        public static EqualConstraint ArgumentOutOfRangeException(string expectedParamName) =>
            ArgumentOutOfRangeException().With.ParamName().EqualTo(expectedParamName);

        static ResolvableConstraintExpression ParamName(this ConstraintExpression constraint) =>
            constraint.Property(nameof(System.ArgumentException.ParamName));
    }
}
