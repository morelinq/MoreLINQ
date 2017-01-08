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

        private static void ThrowsArgumentException<TActual>(string argumentName, TestDelegate code) where TActual : ArgumentException
        {
            var e = Throws<TActual>(code);

            That(e.ParamName, Is.EqualTo(argumentName));
        }
    }
}
