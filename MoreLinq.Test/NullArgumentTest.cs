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
    [TestFixture]
    public class NullArgumentTest
    {
        [Test, TestCaseSource("GetNotNullTestCases")]
        public void NotNull(TestCase testCase)
        {
            var ex = Assert.Throws<ArgumentNullException>(() => testCase.Invoke());
            Assert.That(ex.ParamName, Is.EqualTo(testCase.ParameterName));
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

        private IEnumerable<ITestCaseData> GetNotNullTestCases()
        {
            return GetTestCases(canBeNull: false);
        }

        private IEnumerable<ITestCaseData> GetCanBeNullTestCases()
        {
            return GetTestCases(canBeNull: true);
        }

        private IEnumerable<ITestCaseData> GetTestCases(bool canBeNull)
        {
            var flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;
            return typeof (MoreEnumerable).GetMethods(flags).SelectMany(m => CreateTestCases(m, canBeNull));
        }

        private IEnumerable<ITestCaseData> CreateTestCases(MethodInfo methodDefinition, bool canBeNull)
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

        private string GetTestName(MethodInfo definition, ParameterInfo parameter)
        {
            return string.Format("{0}: '{1}' ({2});\n{3}", definition.Name, parameter.Name, parameter.Position, definition);
        }

        private MethodInfo InstantiateMethod(MethodInfo definition)
        {
            if (!definition.IsGenericMethodDefinition) return definition;

            var typeArguments = definition.GetGenericArguments().Select(InstantiateType).ToArray();
            return definition.MakeGenericMethod(typeArguments);
        }

        private Type InstantiateType(Type typeParameter)
        {
            var constraints = typeParameter.GetGenericParameterConstraints();

            if (constraints.Length == 0) return typeof (int);
            if (constraints.Length == 1) return constraints.Single();

            throw new NotImplementedException("NullArgumentTest.InstantiateType");
        }

        private bool IsReferenceType(ParameterInfo parameter)
        {
            return !parameter.ParameterType.IsValueType; // class or interface
        }

        private bool CanBeNull(ParameterInfo parameter)
        {
            return parameter.GetCustomAttributes(false).Any(a => a.GetType().Name == "CanBeNullAttribute");
        }

        private bool HasDefaultConstructor(Type type)
        {
            return type.GetConstructor(Type.EmptyTypes) != null;
        }

        private object CreateInstance(Type type)
        {
            if (type == typeof (int)) return 7; // often used as size, range, etc. avoid ArgumentOutOfRange for '0'.
            if (type.IsValueType || HasDefaultConstructor(type)) return Activator.CreateInstance(type);
            if (type.IsArray) return Array.CreateInstance(type.GetElementType(), 0);
            if (typeof (Delegate).IsAssignableFrom(type)) return CreateDelegateInstance(type);
            if (type == typeof (string)) return "";

            return CreateGenericInterfaceInstance(type);
        }

        private Delegate CreateDelegateInstance(Type type)
        {
            var invoke = type.GetMethod("Invoke");
            var parameters = invoke.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name));
            var body = Expression.Default(invoke.ReturnType); // requires >= .NET 4.0
            var lambda = Expression.Lambda(type, body, parameters);
            return lambda.Compile();
        }

        private object CreateGenericInterfaceInstance(Type type)
        {
            Debug.Assert(type.IsGenericType && type.IsInterface);
            var name = type.Name.Substring(1); // Delete first character, i.e. the 'I' in IEnumerable
            var definition = typeof (GenericArgs).GetNestedType(name);
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