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

using System.Linq;
using NUnit.Framework;

namespace MoreLinq.Test
{
    using System;

    [TestFixture]
    public class LinearRegressionTest
    {
        [TestCase(new double[0], new double[0], double.NaN, double.NaN, double.NaN, double.NaN)]

        // Source: https://en.wikipedia.org/wiki/Simple_linear_regression#Numerical_example

        [TestCase(new[] {  1.47,  1.50,  1.52,  1.55,  1.57,  1.60,  1.63,  1.65,  1.68,  1.70,  1.73,  1.75,  1.78,  1.80,  1.83 },
                  new[] { 52.21, 53.12, 54.48, 55.84, 57.20, 58.57, 59.93, 61.29, 63.11, 64.47, 66.28, 68.10, 69.92, 72.19, 74.46 },
                  /* m */ 61.27218654, /* b */ -39.06195592, /* r */ 0.994583794,
                  1e-8)]

        public void LinearRegressionTestCase(double[] xs, double[] ys,
                                             double expectedSlope,
                                             double expectedIntercept,
                                             double expectedCorrelation,
                                             double tolerance)
        {

            var sample = xs.Zip(ys, (x, y) => new { X = x, Y = y });

            var overloads = new[]
            {
                sample.LinearRegression(e => e.X, e => e.Y, ValueTuple.Create),
                sample.LinearRegression(e => (e.X, e.Y)),
            };

            foreach (var t in from t in overloads.Index(1)
                              select new { Overload = t.Key, Result = t.Value })
            {
                (var m, var b, var r) = t.Result;

                Assert.That(m, Is.EqualTo(expectedSlope      ).Within(tolerance), $"Slope (overload #{t.Overload})");
                Assert.That(b, Is.EqualTo(expectedIntercept  ).Within(tolerance), $"Y-Intercept (overload #{t.Overload})");
                Assert.That(r, Is.EqualTo(expectedCorrelation).Within(tolerance), $"Correlation coefficient (overload #{t.Overload})");
            }
        }
    }
}
