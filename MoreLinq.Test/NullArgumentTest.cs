#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2015 Julian Lettner. All rights reserved.
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using StackTrace = System.Diagnostics.StackTrace;

    [TestFixture]
    public class NullArgumentTest
    {
        [Test, TestCaseSource(nameof(GetNotNullTestCases))]
        public void NotNull(Action testCase) =>
            testCase();

        [Test, TestCaseSource(nameof(GetCanBeNullTestCases))]
        public void CanBeNull(Action testCase) =>
            testCase();

        static IEnumerable<ITestCaseData> GetNotNullTestCases() =>
            GetTestCases(canBeNull: false, testCaseFactory: (method, args, paramName) => () =>
            {
                Exception? e = null;

                try
                {
                    _ = method.Invoke(null, args);
                }
                catch (TargetInvocationException tie)
                {
                    e = tie.InnerException;
                }

                Assert.That(e, Is.Not.Null, $"No exception was thrown when {nameof(ArgumentNullException)} was expected.");
                Assert.That(e, Is.InstanceOf<ArgumentNullException>().With.Property(nameof(ArgumentNullException.ParamName)).EqualTo(paramName));
                Debug.Assert(e is not null);
                var stackTrace = new StackTrace(e, false);
                var stackFrame = stackTrace.GetFrames().First();
                var actualType = stackFrame.GetMethod()?.DeclaringType;
                Assert.That(actualType, Is.SameAs(typeof(MoreEnumerable)));
            });

        static IEnumerable<ITestCaseData> GetCanBeNullTestCases() =>
            GetTestCases(canBeNull: true, testCaseFactory: (method, args, _) => () => method.Invoke(null, args));

        static IEnumerable<ITestCaseData> GetTestCases(bool canBeNull, Func<MethodInfo, object?[], string, Action> testCaseFactory) =>
            from m in typeof(MoreEnumerable).GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            from t in CreateTestCases(m, canBeNull, testCaseFactory)
            select t;

        static IEnumerable<ITestCaseData> CreateTestCases(MethodInfo methodDefinition, bool canBeNull, Func<MethodInfo, object?[], string, Action> testCaseFactory)
        {
            var method = InstantiateMethod(methodDefinition);
            var parameters = method.GetParameters().ToList();

            return from param in parameters
                   where IsReferenceType(param) && CanBeNull(param) == canBeNull
                   let arguments = parameters.Select(p => p == param ? null : CreateInstance(p.ParameterType)).ToArray()
                   let testCase = testCaseFactory(method, arguments,
#pragma warning disable CA2201 // Do not raise reserved exception types
                                                  param.Name ?? throw new NullReferenceException())
#pragma warning restore CA2201 // Do not raise reserved exception types
                   let testName = GetTestName(methodDefinition, param)
                   select (ITestCaseData)new TestCaseData(testCase).SetName(testName);
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

            return constraints.Length switch
            {
                0 => typeof(int),
                1 => constraints.Single(),
                _ => throw new NotImplementedException("NullArgumentTest.InstantiateType")
            };
        }

        static bool IsReferenceType(ParameterInfo parameter) =>
            !parameter.ParameterType.GetTypeInfo().IsValueType;

        static bool CanBeNull(ParameterInfo parameter)
        {
            var nullableTypes =
                from t in new[] { typeof(IEqualityComparer<>), typeof(IComparer<>) }
                select t.GetTypeInfo();

            var nullableParameters = new[]
            {
                nameof(MoreEnumerable.Assert) + ".errorSelector",
                nameof(MoreEnumerable.From) + ".function",
                nameof(MoreEnumerable.From) + ".function1",
                nameof(MoreEnumerable.From) + ".function2",
                nameof(MoreEnumerable.From) + ".function3",
                nameof(MoreEnumerable.ToDataTable) + ".expressions",
                nameof(MoreEnumerable.Trace) + ".format"
            };

            var type = parameter.ParameterType.GetTypeInfo();
            type = type.IsGenericType ? type.GetGenericTypeDefinition().GetTypeInfo() : type;
            var param = parameter.Member.Name + "." + parameter.Name;

            return nullableTypes.Contains(type) || nullableParameters.Contains(param);
        }

        static object CreateInstance(Type type)
        {
            if (type == typeof(int)) return 7; // int is used as size/length/range etc. avoid ArgumentOutOfRange for '0'.
            if (type == typeof(string)) return "";
            if (type == typeof(TaskScheduler)) return TaskScheduler.Default;
            if (type == typeof(IEnumerable<int>)) return new[] { 1, 2, 3 }; // Provide non-empty sequence for MinBy/MaxBy.
            if (typeof(Delegate).IsAssignableFrom(type)) return CreateDelegateInstance(type);

            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                Debug.Assert(elementType is not null);
                return Array.CreateInstance(elementType, 0);
            }

            if (type.GetTypeInfo().IsValueType || HasDefaultConstructor(type))
            {
                var instance = Activator.CreateInstance(type);
                Debug.Assert(instance is not null);
                return instance;
            }

            return type.GetTypeInfo() is { IsGenericType: true } typeInfo
                 ? CreateGenericInterfaceInstance(typeInfo)
                 : EmptyEnumerable.Instance;
        }

        static bool HasDefaultConstructor(Type type) =>
            type.GetConstructor(Type.EmptyTypes) != null;

        static Delegate CreateDelegateInstance(Type type)
        {
            var invoke = type.GetMethod("Invoke");
            Debug.Assert(invoke is not null);
            var parameters = invoke.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name));
            var body = Expression.Default(invoke.ReturnType); // requires >= .NET 4.0
            var lambda = Expression.Lambda(type, body, parameters);
            return lambda.Compile();
        }

        static object CreateGenericInterfaceInstance(TypeInfo type)
        {
            Debug.Assert(type.IsGenericType && type.IsInterface);
            var name = type.Name[1..]; // Delete first character, i.e. the 'I' in IEnumerable
            var definition = typeof(GenericArgs).GetTypeInfo().GetNestedType(name);
            Debug.Assert(definition is not null);
            var instance = Activator.CreateInstance(definition.MakeGenericType(type.GetGenericArguments()));
            Debug.Assert(instance is not null);
            return instance;
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
            sealed class Enumerator<T> : IEnumerator<T?>
            {
                public bool MoveNext() => false;
                public T? Current { get; private set; }
                object? IEnumerator.Current => Current;
                public void Reset() { }
                public void Dispose() { }
            }

            public class Enumerable<T> : IEnumerable<T?>
            {
                public IEnumerator<T?> GetEnumerator() => new Enumerator<T?>();
                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            }

#pragma warning disable CA1812 // Avoid uninstantiated internal classes

            public sealed class OrderedEnumerable<T> : Enumerable<T>, System.Linq.IOrderedEnumerable<T?>
            {
                public System.Linq.IOrderedEnumerable<T?> CreateOrderedEnumerable<TKey>(Func<T, TKey> keySelector, IComparer<TKey>? comparer, bool descending)
                {
                    if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
                    return this;
                }
            }

            public sealed class AwaitQuery<T> : Enumerable<T?>,
                                                Experimental.IAwaitQuery<T?>
            {
                public Experimental.AwaitQueryOptions Options => Experimental.AwaitQueryOptions.Default;
                public Experimental.IAwaitQuery<T?> WithOptions(Experimental.AwaitQueryOptions options) => this;
            }

            public sealed class Comparer<T> : IComparer<T>
            {
                public int Compare(T? x, T? y) => -1;
            }

            public sealed class EqualityComparer<T> : IEqualityComparer<T>
            {
                public bool Equals(T? x, T? y) => false;
                public int GetHashCode(T obj) => 0;
            }

#pragma warning restore CA1812 // Avoid uninstantiated internal classes
        }
    }
}
