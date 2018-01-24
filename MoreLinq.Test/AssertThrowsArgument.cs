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
