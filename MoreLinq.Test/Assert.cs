using System;
using NUnit.Framework;

namespace MoreLinq.Test
{
    internal sealed class Assert : NUnit.Framework.Assert
    {
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

        private static void ThrowsArgumentException<TActual>(string expectedParamName, TestDelegate code) where TActual : ArgumentException
        {
            var e = Throws<TActual>(code);

            That(e.ParamName, Is.EqualTo(expectedParamName));
        }
    }
}
