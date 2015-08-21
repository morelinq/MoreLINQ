using System;
using NUnit.Framework;

namespace MoreLinq.Test
{
    static class AssertUtil
    {
        public static void Throws<TException>(Action action) 
            where TException : Exception
        {
            try
            {
                action();
            }
            catch( Exception ex )
            {
                // we caught the expected exception, all is well
                if( ex is TException )
                    return;

                // we caught some other type of exception, this is a problem
                var message = string.Format("Expected {0} but caught {1} instead.",
                                            typeof (TException).Name, ex.GetType().Name);
                throw new AssertionException(message, ex);
            }
        }
    }
}