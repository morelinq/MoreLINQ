
namespace MoreLinq.Test
{
    internal static class Combinatorics
    {
        public static double Factorial(int n)
        {
            var fac = 1.0d;
            while (n > 0)
                fac *= n--;
            return fac;
        }

        public static double Binomial(int n, int k)
        {
            return Factorial(n) / (Factorial(n - k) * Factorial(k));
        }
    }
}