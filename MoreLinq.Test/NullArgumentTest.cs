using System;
using System.Collections.Generic;
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
                let arguments = parameters.Select(p => p == param ? null : GetArgument(p.ParameterType)).ToArray()
                let testName = GetTestName(methodDefinition, param)
                select (ITestCaseData) new TestCaseData(method, arguments, param.Name).SetName(testName);
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

        private string GetTestName(MethodInfo definition, ParameterInfo parameter)
        {
            return string.Format("{0}: '{1}' ({2});\n{3}", definition.Name, parameter.Name, parameter.Position, definition);
        }

        private object GetArgument(Type type)
        {
            if (type.IsValueType) return Activator.CreateInstance(type);
            if (typeof (Delegate).IsAssignableFrom(type)) return CreateDelegate(type);
            if (type == typeof(string)) return "";
            if (type == typeof(Random)) return new Random();
            

            // TODO var typeArg = type.GetGenericArguments().Single();
            if (IsGeneric(type, typeof (IEnumerable<>))) return Array.CreateInstance(type.GetGenericArguments().Single(), 0);
            if (IsGeneric(type, typeof (IComparer<>))) return Create(typeof (Args.Comparer<>), type.GetGenericArguments().Single());
            if (IsGeneric(type, typeof (IEqualityComparer<>))) return Create(typeof (Args.EqualityComparer<>), type.GetGenericArguments().Single());

            return new object();
        }

        private bool IsGeneric(Type instantiation, Type definition)
        {
            return instantiation.IsGenericType && instantiation.GetGenericTypeDefinition() == definition;
        }

        private Delegate CreateDelegate(Type delegateType)
        {
            var invoke = delegateType.GetMethod("Invoke");
            var parameters = invoke.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name));
            var body = Expression.Default(invoke.ReturnType); // TODO: Requires >= .NET 4.0
            var lambda = Expression.Lambda(delegateType, body, parameters);

            return lambda.Compile();
        }

        private object Create(Type definition, Type typeArgument)
        {
            var type = definition.MakeGenericType(typeArgument);
            return Activator.CreateInstance(type);
        }

        static class Args
        {
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