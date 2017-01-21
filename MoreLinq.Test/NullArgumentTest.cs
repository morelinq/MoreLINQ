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
        [Test, TestCaseSource("GetNotNullTestCases")]
        public void NotNull(TestCase testCase)
        {
            Assert.ThrowsArgumentNullException(testCase.ParameterName, 
                () => testCase.Invoke());
        }

        [Test, TestCaseSource("GetCanBeNullTestCases")]
        public void CanBeNull(TestCase testCase)
        {
            Assert.DoesNotThrow(() => testCase.Invoke());
        }

        public class TestCase
        {
            public MethodInfo Method { get; private set; }
            public object[] Arguments { get; private set; }
            public string ParameterName { get; private set; }

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

        private static IEnumerable<ITestCaseData> GetNotNullTestCases()
        {
            return GetTestCases(canBeNull: false);
        }

        private static IEnumerable<ITestCaseData> GetCanBeNullTestCases()
        {
            return GetTestCases(canBeNull: true);
        }

        private static IEnumerable<ITestCaseData> GetTestCases(bool canBeNull)
        {
            var flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;
            return typeof (MoreEnumerable).GetMethods(flags).SelectMany(m => CreateTestCases(m, canBeNull));
        }

        private static IEnumerable<ITestCaseData> CreateTestCases(MethodInfo methodDefinition, bool canBeNull)
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

        private static string GetTestName(MethodInfo definition, ParameterInfo parameter)
        {
            return string.Format("{0}: '{1}' ({2});\n{3}", definition.Name, parameter.Name, parameter.Position, definition);
        }

        private static MethodInfo InstantiateMethod(MethodInfo definition)
        {
            if (!definition.IsGenericMethodDefinition) return definition;

            var typeArguments = definition.GetGenericArguments().Select(t => InstantiateType(t.GetTypeInfo())).ToArray();
            return definition.MakeGenericMethod(typeArguments);
        }

        private static Type InstantiateType(TypeInfo typeParameter)
        {
            var constraints = typeParameter.GetGenericParameterConstraints();

            if (constraints.Length == 0) return typeof (int);
            if (constraints.Length == 1) return constraints.Single();

            throw new NotImplementedException("NullArgumentTest.InstantiateType");
        }

        private static bool IsReferenceType(ParameterInfo parameter)
        {
            return !parameter.ParameterType.GetTypeInfo().IsValueType; // class or interface
        }

        private static bool CanBeNull(ParameterInfo parameter)
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

        private static object CreateInstance(Type type)
        {
            if (type == typeof (int)) return 7; // int is used as size/length/range etc. avoid ArgumentOutOfRange for '0'.
            if (type == typeof (string)) return "";
            if (type == typeof(IEnumerable<int>)) return new[] { 1, 2, 3 }; // Provide non-empty sequence for MinBy/MaxBy.
            if (type.IsArray) return Array.CreateInstance(type.GetElementType(), 0);
            if (type.GetTypeInfo().IsValueType || HasDefaultConstructor(type)) return Activator.CreateInstance(type);
            if (typeof(Delegate).IsAssignableFrom(type)) return CreateDelegateInstance(type);

            return CreateGenericInterfaceInstance(type.GetTypeInfo());
        }

        private static bool HasDefaultConstructor(Type type)
        {
            return type.GetConstructor(Type.EmptyTypes) != null;
        }

        private static Delegate CreateDelegateInstance(Type type)
        {
            var invoke = type.GetMethod("Invoke");
            var parameters = invoke.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name));
            var body = Expression.Default(invoke.ReturnType); // requires >= .NET 4.0
            var lambda = Expression.Lambda(type, body, parameters);
            return lambda.Compile();
        }

        private static object CreateGenericInterfaceInstance(TypeInfo type)
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
            private class Enumerator<T> : IEnumerator<T>
            {
                public bool MoveNext() { return false; }
                public T Current { get; private set; }
                object IEnumerator.Current { get { return Current; } }
                public void Reset() { }
                public void Dispose() { }
            }

            public class Enumerable<T> : IEnumerable<T>
            {
                public IEnumerator<T> GetEnumerator() { return new Enumerator<T>(); }
                IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
            }

            public class OrderedEnumerable<T> : Enumerable<T>, IOrderedEnumerable<T>
            {
                public IOrderedEnumerable<T> CreateOrderedEnumerable<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer, bool descending)
                {
                    if (keySelector == null) throw new ArgumentNullException("keySelector");
                    return this;
                }
            }

            public class Comparer<T> : IComparer<T>
            {
                public int Compare(T x, T y) { return -1; }
            }

            public class EqualityComparer<T> : IEqualityComparer<T>
            {
                public bool Equals(T x, T y) { return false; }
                public int GetHashCode(T obj) { return 0; }
            }
        }
    }
}