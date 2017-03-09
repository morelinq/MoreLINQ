using NUnit.Common;
using NUnitLite;
using System;
using System.Reflection;

namespace MoreLinq.Test
{
    static class Program
    {
        static int Main(string[] args)
        {
            return new AutoRun(typeof(Program).GetTypeInfo().Assembly)
                .Execute(args, new ExtendedTextWrapper(Console.Out), Console.In);
        }
    }
}
