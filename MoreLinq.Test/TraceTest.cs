#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class TraceTest
    {
        [Test]
        public void TraceSequence()
        {
            var trace = Lines(CaptureTrace(() => "the quick brown fox".Split().Trace().Consume()));
            trace.AssertSequenceEqual("the", "quick", "brown", "fox");
        }

        [Test]
        public void TraceSequenceWithSomeNullElements()
        {
            var trace = Lines(CaptureTrace(() => new int?[] {1, null, 2, null, 3}.Trace().Consume()));
            trace.AssertSequenceEqual("1", string.Empty, "2", string.Empty, "3");
        }

        [Test]
        public void TraceSequenceWithSomeNullReferences()
        {
            var trace = Lines(CaptureTrace(() => new[] { "the", null, "quick", null, "brown", null, "fox" }.Trace().Consume()));

            trace.AssertSequenceEqual("the", string.Empty, "quick", string.Empty, "brown", string.Empty, "fox");
        }

        [Test]
        public void TraceSequenceWithFormatting()
        {
            var trace = Lines(CaptureTrace(delegate
            {
                using (new CurrentThreadCultureScope(CultureInfo.InvariantCulture))
                    new[] { 1234, 5678 }.Trace("{0:N0}").Consume();
            }));

            trace.AssertSequenceEqual("1,234", "5,678");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TraceSequenceWithNullFormatter()
        {
            new object[0].Trace((Func<object, string>)null);
        }

        [Test]
        public void TraceSequenceWithFormatter()
        {
            var trace = Lines(CaptureTrace(delegate
            {
                var formatter = CultureInfo.InvariantCulture;
                new int?[] { 1234, null, 5678 }.Trace(n => n.HasValue 
                                                           ? n.Value.ToString("N0", formatter) 
                                                           : "#NULL")
                                               .Consume();
            }));

            trace.AssertSequenceEqual("1,234", "#NULL", "5,678");
        }

        private static IEnumerable<string> Lines(string str)
        {
            return Lines(string.IsNullOrEmpty(str)
                         ? TextReader.Null
                         : new StringReader(str));
        }

        private static IEnumerable<string> Lines(TextReader reader)
        {
            Debug.Assert(reader != null);
            string line;
            while ((line = reader.ReadLine()) != null)
                yield return line;
        }

        private static string CaptureTrace(Action action)
        {
            var writer = new StringWriter();
            var listener = new TextWriterTraceListener(writer);
            Trace.Listeners.Add(listener);
            try
            {
                action();
                Trace.Flush();
            }
            finally
            {
                Trace.Listeners.Remove(listener);
            }
            return writer.ToString();
        }
    }
}
