using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        [Test, TestCaseSource("GetTestCases")]
        public void NullCheck(MethodInfo method, object[] arguments, string parameterName)
        {
            var ex = Assert.Throws<TargetInvocationException>(() => method.Invoke(null, arguments));

            var inner = ex.InnerException;
            Assert.That(inner, Is.TypeOf<ArgumentNullException>());

            // TODO enable later
            //var argumentNullException = ((ArgumentNullException)inner);
            //Assert.That(argumentNullException.ParamName, Is.EqualTo(parameterName));
        }

        // TODO: test null allowed
        // TODO: test that both of them test all methods/parameter combinations.

        private IEnumerable<ITestCaseData> GetTestCases()
        {
            var flags = BindingFlags.Public | BindingFlags.Static;
            return typeof (MoreEnumerable).GetMethods(flags).SelectMany(CreateTestCases);
        }

        private IEnumerable<ITestCaseData> CreateTestCases(MethodInfo methodDefinition)
        {
            var method = InstantiateMethod(methodDefinition);
            var parameters = method.GetParameters().ToList();

            return from param in parameters
                where IsReferenceType(param) && !CanBeNull(param)
                let arguments = parameters.Select(p => p == param ? null : CreateInstance(p.ParameterType)).ToArray()
                let testName = GetTestName(methodDefinition, param)
                select (ITestCaseData) new TestCaseData(method, arguments, param.Name).SetName(testName);
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

            return typeParameter;
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
            if (type == typeof (int)) return new int(); // often used as size, range, etc.. avoid ArgumentOutOfRange. 
            if (type.IsValueType || HasDefaultConstructor(type)) return Activator.CreateInstance(type);
            if (type.IsArray) return Array.CreateInstance(type.GetElementType(), 0);
            if (typeof (Delegate).IsAssignableFrom(type)) return CreateDelegateInstance(type);
            if (type == typeof (string)) return "";
            if (type == typeof (Random)) return new Random();
            if (type == typeof (DataTable)) return new DataTable();

            if (type.IsGenericParameter)
            {
                // TODO
                return new object();
            }

            return CreateGenericInterfaceInstance(type);
        }

        private Delegate CreateDelegateInstance(Type type)
        {
            var invoke = type.GetMethod("Invoke");
            var parameters = invoke.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name));
            var body = Expression.Default(invoke.ReturnType); // TODO: Requires >= .NET 4.0
            var lambda = Expression.Lambda(type, body, parameters);

            return null;
            // TODO
            //return lambda.Compile();
        }

        private object CreateGenericInterfaceInstance(Type type)
        {
            Debug.Assert(type.IsGenericType && type.IsInterface);
            var name = type.Name.Substring(1); // Delete first character, i.e. the 'I' in IEnumerable
            var definition = typeof (GenericArgs).GetNestedType(name);
            var instantiation = definition.MakeGenericType(type.GetGenericArguments());

            return Activator.CreateInstance(instantiation);
        }

        static class GenericArgs
        {
            public class Enumerable<T> : IEnumerable<T>
            {
                public IEnumerator<T> GetEnumerator() { throw new NotImplementedException(); }
                IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
            }

            public class OrderedEnumerable<T> : Enumerable<T>, IOrderedEnumerable<T>
            {
                public IOrderedEnumerable<T> CreateOrderedEnumerable<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer, bool descending) { throw new NotImplementedException(); }
            }

            public class Comparer<T> : IComparer<T>
            {
                public int Compare(T x, T y) { throw new NotImplementedException(); }
            }

            public class EqualityComparer<T> : IEqualityComparer<T>
            {
                public bool Equals(T x, T y) { throw new NotImplementedException(); }
                public int GetHashCode(T obj) { throw new NotImplementedException(); }
            }
        }
    }
}