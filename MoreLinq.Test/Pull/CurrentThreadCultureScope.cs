using System.Globalization;
using System.Threading;

namespace MoreLinq.Test.Pull
{
    internal sealed class CurrentThreadCultureScope : Scope<CultureInfo>
    {
        public CurrentThreadCultureScope(CultureInfo @new) : 
            base(Thread.CurrentThread.CurrentCulture)
        {
            Install(@new);
        }

        protected override void Restore(CultureInfo old)
        {
            Install(old);
        }

        private static void Install(CultureInfo value)
        {
            Thread.CurrentThread.CurrentCulture = value;
        }
    }
}