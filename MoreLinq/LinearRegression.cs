#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2017 Atif Aziz. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    static partial class MoreEnumerable
    {
        #if !NO_VALUE_TUPLES

        /// <summary>
        /// Uses simple linear regression to calculate the slope, y-intercept
        /// and correlation coefficient (as a triplet) of a sequence
        /// two-dimensional (x, y) sample data points.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="xySelector">
        /// Function that projects a data point pair (x, y) from an element in
        /// <paramref name="source"/>.</param>
        /// <typeparam name="T">Type of source sequence elements.</typeparam>
        /// <returns>
        /// A triplet of the calculated slope, intercept and correlation
        /// coefficient, respectively.
        /// </returns>

        public static (double Slope, double Intercept, double Correlation)
            LinearRegression<T>(this IEnumerable<T> source, Func<T, (double X, double Y)> xySelector)
        {
            if (xySelector == null) throw new ArgumentNullException(nameof(xySelector));

            return source.Select(e => xySelector(e))
                         .LinearRegression(e => e.X, e => e.Y, ValueTuple.Create);
        }

        #endif

        /// <summary>
        /// Uses simple linear regression to calculate the slope, y-intercept
        /// and correlation coefficient of a sequence two-dimensional (x, y)
        /// sample data points.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="xSelector">
        /// Function the project the X component of the data pair given an
        /// item from <paramref name="source"/>.</param>
        /// <param name="ySelector">
        /// Function the project the Y component of the data pair given an
        /// item from <paramref name="source"/>.</param>
        /// <param name="resultSelector">
        /// Function that projects the result from the slope, y-intercept
        /// and correlation coefficient, respectively.</param>
        /// <typeparam name="T">Type of source sequence elements.</typeparam>
        /// <typeparam name="TResult">
        /// Type of the value returned by <paramref name="resultSelector"/></typeparam>
        /// <returns>
        /// Result of <paramref name="resultSelector"/>.
        /// </returns>

        public static TResult LinearRegression<T, TResult>(this IEnumerable<T> source,
            Func<T, double> xSelector, Func<T, double> ySelector,
            Func<double, double, double, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (xSelector == null) throw new ArgumentNullException(nameof(xSelector));
            if (ySelector == null) throw new ArgumentNullException(nameof(ySelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            var n          = 0;
            var sumX       = 0.0;
            var sumSqrX    = 0.0;
            var sumY       = 0.0;
            var sumSqrY    = 0.0;
            var sumProduct = 0.0;

            foreach (var item in source)
            {
                var x = xSelector(item);
                var y = ySelector(item);
                n++;
                sumX += x;
                sumY += y;
                sumSqrX += x * x;
                sumSqrY += y * y;
                sumProduct += x * y;
            }

            var sqrSumX = sumX * sumX;
            var sqrSumY = sumY * sumY;

            var t = n * sumProduct - sumX * sumY;
            var u = n * sumSqrX - sqrSumX;
            var m = t / u;
            var b = (sumY - m * sumX) / n;
            var r = t / Math.Sqrt(u * (n * sumSqrY - sqrSumY));

            return resultSelector(m, b, r);
        }
    }
}
