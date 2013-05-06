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
using NUnit.Framework;

namespace MoreLinq.Test
{
    [TestFixture]
    public class AcquireTest
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AcquireNullSequence()
        {
            MoreEnumerable.Acquire<IDisposable>(null);
        }

        [Test]
        public void AcquireAll()
        {
            Disposable a = null;
            Disposable b = null;
            Disposable c = null;
           
            var allocators = Futures(() => a = new Disposable(), 
                                     () => b = new Disposable(),
                                     () => c = new Disposable());

            var disposables = allocators.Acquire();
            
            Assert.That(disposables.Length, Is.EqualTo(3));
            
            foreach (var disposable in disposables.Zip(new[] { a, b, c }, (act, exp) => new { Actual = act, Expected = exp }))
            {
                Assert.That(disposable.Actual, Is.SameAs(disposable.Expected));
                Assert.That(disposable.Actual.Disposed, Is.False);
            }
        }

        [Test]
        public void AcquireSome()
        {
            Disposable a = null;
            Disposable b = null;
            Disposable c = null;
            
            var allocators = Futures(() => a = new Disposable(), 
                                     () => b = new Disposable(),
                                     () => { throw new ApplicationException(); },
                                     () => c = new Disposable());
            
            try
            {
                allocators.Acquire();
                Assert.Fail();
            }
            catch (ApplicationException)
            {
                Assert.That(a, Is.Not.Null);
                Assert.That(a.Disposed, Is.True);
                Assert.That(b, Is.Not.Null);
                Assert.That(b.Disposed, Is.True);
                Assert.That(c, Is.Null);
            }
        }

        static IEnumerable<T> Futures<T>(params Func<T>[] allocators)
            where T : IDisposable
        {
            foreach (var allocator in allocators)
                yield return allocator();
        }

        class Disposable : IDisposable
        {
            public bool Disposed { get; private set; }    
            public void Dispose() { Disposed = true; }
        }
    }
}