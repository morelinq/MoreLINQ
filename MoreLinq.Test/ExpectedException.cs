namespace MoreLinq.Test
{
    using System;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using NUnit.Framework.Internal;
    using NUnit.Framework.Internal.Commands;

    /// <summary>
    /// For backward compatibility with unit tests still testing exceptional
    /// cases using <see cref="ExpectedExceptionAttribute"/>.
    /// </summary>
    /// <remarks>
    /// This should be removed once unit tests using this attribute have
    /// been migrated to using
    /// <see cref="Assert.Throws{TActual}(NUnit.Framework.TestDelegate,string,object[])"/>
    /// that is the recommended and better way.
    /// </remarks>

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    sealed class ExpectedExceptionAttribute : NUnitAttribute, IWrapTestMethod
    {
        readonly Type _expectedExceptionType;

        public ExpectedExceptionAttribute(Type type)
        {
            _expectedExceptionType = type;
        }

        public TestCommand Wrap(TestCommand command)
        {
            return new ExpectedExceptionCommand(command, _expectedExceptionType);
        }

        class ExpectedExceptionCommand : DelegatingTestCommand
        {
            readonly Type _expectedType;

            public ExpectedExceptionCommand(TestCommand innerCommand, Type expectedType)
                : base(innerCommand)
            {
                _expectedType = expectedType;
            }

            public override TestResult Execute(TestExecutionContext context)
            {
                Type caughtType = null;

                try
                {
                    innerCommand.Execute(context);
                }
                catch (Exception ex)
                {
                    if (ex is NUnitException)
                        ex = ex.InnerException;
                    caughtType = ex.GetType();
                }

                if (caughtType == _expectedType)
                    context.CurrentResult.SetResult(ResultState.Success);
                else if (caughtType != null)
                    context.CurrentResult.SetResult(ResultState.Failure,
                        string.Format("Expected {0} but got {1}", _expectedType.Name, caughtType.Name));
                else
                    context.CurrentResult.SetResult(ResultState.Failure,
                        string.Format("Expected {0} but no exception was thrown", _expectedType.Name));

                return context.CurrentResult;
            }
        }
    }
}