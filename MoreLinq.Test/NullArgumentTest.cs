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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;

namespace MoreLinq.Test
{
    using NUnit.Framework.Interfaces;

    [TestFixture]
    public class NullArgumentTest
    {
        [Test, TestCaseSource(nameof(GetNotNullTestCases))]
        public void NotNull(TestCase testCase)
        {
            Assert.ThrowsArgumentNullException(testCase.ParameterName,
                () => testCase.Invoke());
        }

        [Test, TestCaseSource(nameof(GetCanBeNullTestCases))]
        public void CanBeNull(TestCase testCase)
        {
            Assert.DoesNotThrow(() => testCase.Invoke());
        }

        public class TestCase
        {
            public MethodInfo Method { get; }
            public object[] Arguments { get; }
            public string ParameterName { get; }

            public TestCase(MethodInfo method, object[] arguments, string parameterName)
            {
                Method = method;
                Arguments = arguments;
                ParameterName = parameterName;
            }

            public object Invoke()
            {
                try
                {
                    return Method.Invoke(null, Arguments);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }
        }

        static IEnumerable<ITestCaseData> GetNotNullTestCases() =>
            GetTestCases(canBeNull: false);

        static IEnumerable<ITestCaseData> GetCanBeNullTestCases() =>
            GetTestCases(canBeNull: true);

        static IEnumerable<ITestCaseData> GetTestCases(bool canBeNull) =>
            from m in typeof (MoreEnumerable).GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            from t in CreateTestCases(m, canBeNull)
            select t;

        static IEnumerable<ITestCaseData> CreateTestCases(MethodInfo methodDefinition, bool canBeNull)
        {
            var method = InstantiateMethod(methodDefinition);
            var parameters = method.GetParameters().ToList();

            return from param in parameters
                where IsReferenceType(param) && CanBeNull(param) == canBeNull
                let arguments = parameters.Select(p => p == param ? null : CreateInstance(p.ParameterType)).ToArray()
                let testCase = new TestCase(method, arguments, param.Name)
                let testName = GetTestName(methodDefinition, param)
                select (ITestCaseData) new TestCaseData(testCase).SetName(testName);
        }

        static string GetTestName(MethodInfo definition, ParameterInfo parameter) =>
            $"{definition.Name}: '{parameter.Name}' ({parameter.Position});\n{definition}";

        static MethodInfo InstantiateMethod(MethodInfo definition)
        {
            if (!definition.IsGenericMethodDefinition) return definition;

            var typeArguments = definition.GetGenericArguments().Select(t => InstantiateType(t.GetTypeInfo())).ToArray();
            return definition.MakeGenericMethod(typeArguments);
        }

        static Type InstantiateType(TypeInfo typeParameter)
        {
            var constraints = typeParameter.GetGenericParameterConstraints();

            if (constraints.Length == 0) return typeof (int);
            if (constraints.Length == 1) return constraints.Single();

            throw new NotImplementedException("NullArgumentTest.InstantiateType");
        }

        static bool IsReferenceType(ParameterInfo parameter) =>
            !parameter.ParameterType.GetTypeInfo().IsValueType;

        static bool CanBeNull(ParameterInfo parameter)
        {
            var nullableTypes =
                from t in new[] { typeof (IEqualityComparer<>), typeof (IComparer<>) }
                select t.GetTypeInfo();
            var nullableParameters = new[] { "Assert.errorSelector", "ToDataTable.expressions", "ToDelimitedString.delimiter", "Trace.format" };

            var type = parameter.ParameterType.GetTypeInfo();
            type = type.IsGenericType ? type.GetGenericTypeDefinition().GetTypeInfo() : type;
            var param = parameter.Member.Name + "." + parameter.Name;

            return nullableTypes.Contains(type) || nullableParameters.Contains(param);
        }

        static object CreateInstance(Type type)
        {
            if (type == typeof (int)) return 7; // int is used as size/length/range etc. avoid ArgumentOutOfRange for '0'.
            if (type == typeof (string)) return "";
            if (type == typeof(IEnumerable<int>)) return new[] { 1, 2, 3 }; // Provide non-empty sequence for MinBy/MaxBy.
            if (type.IsArray) return Array.CreateInstance(type.GetElementType(), 0);
            if (type.GetTypeInfo().IsValueType || HasDefaultConstructor(type)) return Activator.CreateInstance(type);
            if (typeof(Delegate).IsAssignableFrom(type)) return CreateDelegateInstance(type);

            return CreateGenericInterfaceInstance(type.GetTypeInfo());
        }

        static bool HasDefaultConstructor(Type type) =>
            type.GetConstructor(Type.EmptyTypes) != null;

        static Delegate CreateDelegateInstance(Type type)
        {
            var invoke = type.GetMethod("Invoke");
            var parameters = invoke.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name));
            var body = Expression.Default(invoke.ReturnType); // requires >= .NET 4.0
            var lambda = Expression.Lambda(type, body, parameters);
            return lambda.Compile();
        }

        static object CreateGenericInterfaceInstance(TypeInfo type)
        {
            Debug.Assert(type.IsGenericType && type.IsInterface);
            var name = type.Name.Substring(1); // Delete first character, i.e. the 'I' in IEnumerable
            var definition = typeof (GenericArgs).GetTypeInfo().GetNestedType(name);
            var instantiation = definition.MakeGenericType(type.GetGenericArguments());
            return Activator.CreateInstance(instantiation);
        }

        // ReSharper disable UnusedMember.Local, UnusedAutoPropertyAccessor.Local
        static class GenericArgs
        {
            class Enumerator<T> : IEnumerator<T>
            {
                public bool MoveNext() => false;
                public T Current { get; private set; }
                object IEnumerator.Current => Current;
                public void Reset() { }
                public void Dispose() { }
            }

            public class Enumerable<T> : IEnumerable<T>
            {
                public IEnumerator<T> GetEnumerator() => new Enumerator<T>();
                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            }

            public class OrderedEnumerable<T> : Enumerable<T>, IOrderedEnumerable<T>
            {
                public IOrderedEnumerable<T> CreateOrderedEnumerable<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer, bool descending)
                {
                    if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
                    return this;
                }
            }

            public class Comparer<T> : IComparer<T>
            {
                public int Compare(T x, T y) => -1;
            }

            public class EqualityComparer<T> : IEqualityComparer<T>
            {
                public bool Equals(T x, T y) => false;
                public int GetHashCode(T obj) => 0;
            }
        }
    }
}