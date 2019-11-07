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

using System.Threading;

namespace MoreLinq.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;

    [TestFixture]
    public class MonkeyTests
    {
        [Test, TestCaseSource(nameof(GetMonkeyTests))]
        public void MonkeyTest(Action testCase) =>
            testCase();


        static IEnumerable<ITestCaseData> GetMonkeyTests() =>
            GetTestCases(testCaseFactory: (method, args, paramName) => () =>
            {
                Exception e = null;

                try
                {
                    var thread = new Thread(() =>
                    {
                        var r = method.Invoke(new object(), args) as IEnumerable;
                        var enumerator = r?.GetEnumerator();
                        if (enumerator != null)
                            while (enumerator.MoveNext())
                            {
                            }
                    });
                    thread.Start();
                    Thread.Sleep(100);
                    if (thread.IsAlive)
                    {
                        //thread.Abort();
                        Assert.Inconclusive("Task too long");
                    }
                }
                catch (TargetInvocationException tie)
                {
                    e = tie.InnerException;
                }
            });

        static IEnumerable<ITestCaseData> GetTestCases(Func<MethodInfo, object[], string, Action> testCaseFactory) =>
            from m in typeof (MoreEnumerable).GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            select CreateTestCases(m, testCaseFactory);

        static ITestCaseData CreateTestCases(MethodInfo methodDefinition, Func<MethodInfo, object[], string, Action> testCaseFactory)
        {
            try
            {
                var method = InstantiateMethod(methodDefinition);

                var parameters = method.GetParameters().ToList();
                var arguments = parameters.Select(p => CreateInstance(p.ParameterType)).ToArray();
                var testCase = testCaseFactory(method, arguments, "~");
                var testName = GetTestName(methodDefinition, parameters.FirstOrDefault());
                return new TestCaseData(testCase).SetName(testName);
            }
            catch(Exception)
            {
                return null;
            }
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

        static object CreateInstance(Type type)
        {
            if (type == typeof (int)) return 1; // int is used as size/length/range etc. avoid ArgumentOutOfRange for '0'.
            if (type == typeof (string)) return "MONKEY";
            if (type == typeof(TaskScheduler)) return TaskScheduler.Default;
            if (type == typeof(IEnumerable<int>)) return TestingSequence.Of(1, 2, 3); // Provide non-empty sequence for MinBy/MaxBy.
            if (type.IsArray) return Array.CreateInstance(type.GetElementType(), 3);
            if (type.GetTypeInfo().IsValueType || HasDefaultConstructor(type)) return Activator.CreateInstance(type);
            if (typeof(Delegate).IsAssignableFrom(type)) return CreateDelegateInstance(type);

            var typeInfo = type.GetTypeInfo();

            return typeInfo.IsGenericType
                    ? CreateGenericInterfaceInstance(typeInfo)
                    : EmptyEnumerable.Instance;
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

        static class EmptyEnumerable
        {
            public static readonly IEnumerable Instance = new Enumerable();

            sealed class Enumerable : IEnumerable
            {
                public IEnumerator GetEnumerator() => new Enumerator();

                sealed class Enumerator : IEnumerator
                {
                    public bool MoveNext() => false;
                    object IEnumerator.Current => throw new InvalidOperationException();
                    public void Reset() { }
                }
            }
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

            public class OrderedEnumerable<T> : Enumerable<T>, System.Linq.IOrderedEnumerable<T>
            {
                public System.Linq.IOrderedEnumerable<T> CreateOrderedEnumerable<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer, bool descending)
                {
                    if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
                    return this;
                }
            }

            #if !NO_ASYNC

            public class AwaitQuery<T> : Enumerable<T>,
                                         Experimental.IAwaitQuery<T>
            {
                public Experimental.AwaitQueryOptions Options => Experimental.AwaitQueryOptions.Default;
                public Experimental.IAwaitQuery<T> WithOptions(Experimental.AwaitQueryOptions options) => this;
            }

            #endif

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
