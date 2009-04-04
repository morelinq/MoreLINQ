namespace MoreLinq.Test.Pull
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using MoreLinq.Pull;
    using NUnit.Framework;

    partial class EnumerableTest
    {
        [Test, Category("SideEffects")]
        public void TraceSequence()
        {
            var trace = Lines(CaptureTrace(delegate
            {
                Enumerable.Trace("the quick brown fox".Split()).Consume();
            }));
            trace.AssertSequenceEqual("the", "quick", "brown", "fox");
        }

        [Test, Category("SideEffects")]
        public void TraceSequenceWithSomeNullElements()
        {
            var trace = Lines(CaptureTrace(delegate
            {
                Enumerable.Trace(new int?[] { 1, null, 2, null, 3 }).Consume();
            }));
            trace.AssertSequenceEqual("1", string.Empty, "2", string.Empty, "3");
        }

        [Test, Category("SideEffects")]
        public void TraceSequenceWithSomeNullReferences()
        {
            var trace = Lines(CaptureTrace(delegate
            {
                Enumerable.Trace(new[] { "the", null, "quick", null, "brown", null, "fox" }).Consume();
            }));

            trace.AssertSequenceEqual("the", string.Empty, "quick", string.Empty, "brown", string.Empty, "fox");
        }

        [Test, Category("SideEffects")]
        public void TraceSequenceWithFormatting()
        {
            var trace = Lines(CaptureTrace(delegate
            {
                using (new CurrentThreadCultureScope(CultureInfo.InvariantCulture))
                    Enumerable.Trace(new[] { 1234, 5678 }, "{0:N0}").Consume();
            }));

            trace.AssertSequenceEqual("1,234", "5,678");
        }

        [Test, Category("SideEffects")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TraceSequenceWithNullFormatter()
        {
            Enumerable.Trace(new object[0], (Func<object, string>)null);
        }

        [Test, Category("SideEffects")]
        public void TraceSequenceWithFormatter()
        {
            var trace = Lines(CaptureTrace(delegate
            {
                var formatter = System.Globalization.CultureInfo.InvariantCulture;
                Enumerable.Trace(new int?[] { 1234, null, 5678 },
                    n => n.HasValue ? n.Value.ToString("N0", formatter) : "#NULL").Consume();
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
