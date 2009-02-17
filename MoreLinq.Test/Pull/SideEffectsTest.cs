using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq.Pull;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using System.Diagnostics;

namespace MoreLinq.Test.Pull
{
    using System.Globalization;
    using System.IO;

    [TestFixture]
    public class SideEffectsTest
    {
        #region ForEach
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ForEachNullSequence()
        {
            SideEffects.ForEach<int>(null, x => { throw new InvalidOperationException(); });
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ForEachNullAction()
        {
            SideEffects.ForEach(new[] { 1, 2, 3 }, null);
        }

        [Test]
        public void ForEachWithSequence()
        {
            List<int> results = new List<int>();
            SideEffects.ForEach(new[] { 1, 2, 3 }, results.Add);
            results.AssertSequenceEqual(1, 2, 3);
        }
        #endregion

        #region Pipe
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeNullSequence()
        {
            SideEffects.Pipe<int>(null, x => { throw new InvalidOperationException(); });
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PipeNullAction()
        {
            SideEffects.Pipe(new[] { 1, 2, 3 }, null);
        }

        [Test]
        public void PipeWithSequence()
        {
            List<int> results = new List<int>();
            var returned = SideEffects.Pipe(new[] { 1, 2, 3 }, results.Add);
            // Lazy - nothing has executed yet
            Assert.That(results, Is.Empty);
            returned.AssertSequenceEqual(1, 2, 3);
            // Now it has...
            results.AssertSequenceEqual(1, 2, 3);
        }

        [Test]
        public void PipeIsLazy()
        {
            new BreakingSequence<int>().Pipe(x => { });
        }

        [Test]
        public void PipeActionOccursBeforeYield()
        {
            var source = new[] { new StringBuilder(), new StringBuilder() };
            // The action will occur "in" the pipe, so by the time Where gets it, the
            // sequence will be empty.
            Assert.That(source.Pipe(sb => sb.Append("x"))
                              .Where(x => x.Length==0), 
                        Is.Empty);
        }
        #endregion

        #region Trace

        [Test]
        public void TraceSequence()
        {
            var trace = Lines(CaptureTrace(delegate
            {
                SideEffects.Trace("the quick brown fox".Split()).Exhaust(); 
            }));
            trace.AssertSequenceEqual("the", "quick", "brown", "fox");
        }

        [Test]
        public void TraceSequenceWithSomeNullElements()
        {
            var trace = Lines(CaptureTrace(delegate
            {
                SideEffects.Trace(new int?[] { 1, null, 2, null, 3 }).Exhaust();
            }));
            trace.AssertSequenceEqual("1", string.Empty, "2", string.Empty, "3");
        }

        [Test]
        public void TraceSequenceWithSomeNullReferences()
        {
            var trace = Lines(CaptureTrace(delegate
            {
                SideEffects.Trace(new[] { "the", null, "quick", null, "brown", null, "fox" }).Exhaust();
            }));

            trace.AssertSequenceEqual("the", string.Empty, "quick", string.Empty, "brown", string.Empty, "fox");
        }

        [Test]
        public void TraceSequenceWithFormatting()
        {
            var trace = Lines(CaptureTrace(delegate
            {
                using (new CurrentThreadCultureScope(CultureInfo.InvariantCulture))
                    SideEffects.Trace(new[] { 1234, 5678 }, "{0:N0}").Exhaust();
            }));

            trace.AssertSequenceEqual("1,234", "5,678");
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

        #endregion
    }
}
