using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MoreLinq.Test
{
    public class AssertThrows : Assert
    {
        public static void ArgumentNull(string argumentName, TestDelegate code)
        {
            ThrowsArgumentException<ArgumentNullException>(argumentName, code);
        }

        public static void Argument(string argumentName, TestDelegate code)
        {
            ThrowsArgumentException<ArgumentException>(argumentName, code);
        }

        public static void OutOfRange(string argumentName, TestDelegate code)
        {
            ThrowsArgumentException<ArgumentOutOfRangeException>(argumentName, code);
        }

        public static void InvalidOperation(TestDelegate code)
        {
            Throws<InvalidOperationException>(code);
        }

        public static void Sequence(TestDelegate code)
        {
            Throws<SequenceException>(code);
        }

        public static void Exception(TestDelegate code)
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
