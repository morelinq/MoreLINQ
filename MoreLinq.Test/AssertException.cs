using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MoreLinq.Test
{
    public class AssertException : Assert
    {
        public static void ThrowsArgumentNull(string argumentName, TestDelegate code)
        {
            ThrowsArgumentException<ArgumentNullException>(argumentName, code);
        }

        public static void ThrowsArgument(string argumentName, TestDelegate code)
        {
            ThrowsArgumentException<ArgumentException>(argumentName, code);
        }

        public static void ThrowsOutOfRange(string argumentName, TestDelegate code)
        {
            ThrowsArgumentException<ArgumentOutOfRangeException>(argumentName, code);
        }

        public static void ThrowsInvalidOperation(TestDelegate code)
        {
            Throws<InvalidOperationException>(code);
        }

        public static void ThrowsSequence(TestDelegate code)
        {
            Throws<SequenceException>(code);
        }

        public static void ThrowsException(TestDelegate code)
        {
            Throws<Exception>(code);
        }

        private static void ThrowsArgumentException<TActual>(string argumentName, TestDelegate code) where TActual : ArgumentException
        {
            var e = Throws<TActual>(code);

            That(e.ParamName, Is.EqualTo(argumentName));
        }
    }
}
