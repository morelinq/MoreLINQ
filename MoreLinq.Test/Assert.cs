using System;
using NUnit.Framework;

namespace MoreLinq.Test
{
    sealed class Assert : NUnit.Framework.Assert
    {
        [Obsolete("This is redundant with the NullArgumentTest fixture.")]
        public static void ThrowsArgumentNullException(string expectedParamName, TestDelegate code)
        {
            ThrowsArgumentException<ArgumentNullException>(expectedParamName, code);
        }

        public static void ThrowsArgumentException(string expectedParamName, TestDelegate code)
        {
            ThrowsArgumentException<ArgumentException>(expectedParamName, code);
        }

        public static void ThrowsArgumentOutOfRangeException(string expectedParamName, TestDelegate code)
        {
            ThrowsArgumentException<ArgumentOutOfRangeException>(expectedParamName, code);
        }

        static void ThrowsArgumentException<TActual>(string expectedParamName, TestDelegate code) where TActual : ArgumentException
        {
            var e = Throws<TActual>(code);

            That(e.ParamName, Is.EqualTo(expectedParamName));
        }
    }
}
