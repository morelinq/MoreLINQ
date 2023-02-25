#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2019 Atif Aziz. All rights reserved.
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

namespace MoreLinq.Test
{
    using System;
    using NUnit.Framework;
    using Reactive;

    [TestFixture]
    public class SubjectTest
    {
        static IDisposable Subscribe<T>(IObservable<T> subject,
                                        Action<T>? onNext = null,
                                        Action<Exception>? onError = null,
                                        Action? onCompleted = null) =>
            subject.Subscribe(onNext ?? BreakingAction.Of<T>(),
                              onError ?? BreakingAction.Of<Exception>(),
                              onCompleted ?? BreakingAction.WithoutArguments);

        [Test]
        public void SubscribeWithNullObserverThrows()
        {
            var subject = new Subject<int>();
            Assert.That(() => subject.Subscribe(null!),
                        Throws.ArgumentNullException("observer"));
        }

        [Test]
        public void OnNextObservations()
        {
            var a = 0;
            var b = 0;

            var subject = new Subject<int>();

            using (Subscribe(subject, x => a = x))
            using (Subscribe(subject, x => b = x))
                subject.OnNext(42);

            Assert.That(a, Is.EqualTo(42));
            Assert.That(b, Is.EqualTo(42));
        }

        [Test]
        public void OnErrorObservations()
        {
            Exception? error1 = null;
            Exception? error2 = null;

            var subject = new Subject<int>();
            var error = new TestException();

            using (Subscribe(subject, onError: e => error1 = e))
            using (Subscribe(subject, onError: e => error2 = e))
                subject.OnError(error);

            Assert.That(error1, Is.SameAs(error));
            Assert.That(error2, Is.SameAs(error));
        }

        [Test]
        public void OnCompletedObservations()
        {
            var completed1 = false;
            var completed2 = false;

            var subject = new Subject<int>();

            using (Subscribe(subject, onCompleted: () => completed1 = true))
            using (Subscribe(subject, onCompleted: () => completed2 = true))
                subject.OnCompleted();

            Assert.That(completed1, Is.True);
            Assert.That(completed2, Is.True);
        }

        [Test]
        public void SubscriptionDisposal()
        {
            var a = 0;
            var b = 0;

            var subject = new Subject<int>();

            Subscribe(subject).Dispose();
            using var s1 = Subscribe(subject, x => a = x);
            Subscribe(subject).Dispose();
            using var s2 = Subscribe(subject, x => b = x);
            Subscribe(subject).Dispose();

            subject.OnNext(42);

            s1.Dispose();
            s2.Dispose();

            Assert.That(a, Is.EqualTo(42));
            Assert.That(b, Is.EqualTo(42));
        }

        [Test]
        public void SubscriptionReDisposalIsHarmless()
        {
            var subject = new Subject<int>();
            var subscription = Subscribe(subject);

            subscription.Dispose();
            subscription.Dispose();

            subject.OnNext(42);
        }

        [Test]
        public void SubscriptionPostCompletion()
        {
            var completed = false;
            var subject = new Subject<int>();
            subject.OnCompleted();

            using (Subscribe(subject, onCompleted: () => completed = true))
            {
                Assert.That(completed, Is.True);
            }
        }

        [Test]
        public void SubscriptionPostError()
        {
            Exception? observedError = null;
            var subject = new Subject<int>();
            var error = new TestException();
            subject.OnError(error);

            using (Subscribe(subject, onError: e => observedError = e))
            {
                Assert.That(observedError, Is.SameAs(error));
            }
        }

        [Test]
        public void OnNextMutedWhenCompleted()
        {
            var subject = new Subject<int>();
            using (Subscribe(subject, onCompleted: delegate { }))
            {
                subject.OnCompleted();
                subject.OnNext(42);
            }
        }

        [Test]
        public void OnErrorMutedWhenCompleted()
        {
            var subject = new Subject<int>();
            using (Subscribe(subject, onCompleted: delegate { }))
            {
                subject.OnCompleted();
                subject.OnError(new TestException());
            }
        }

        [Test]
        public void OnNextMutedWhenErrored()
        {
            var subject = new Subject<int>();
            using (Subscribe(subject, onError: delegate { }))
            {
                subject.OnError(new TestException());
                subject.OnNext(42);
            }
        }

        [Test]
        public void OnCompleteMutedWhenErrored()
        {
            var subject = new Subject<int>();
            using (Subscribe(subject, onError: delegate { }))
            {
                subject.OnError(new TestException());
                subject.OnCompleted();
            }
        }

        [Test]
        public void CompletesOnce()
        {
            var count = 0;
            var subject = new Subject<int>();
            using (Subscribe(subject, onCompleted: () => count++))
            {
                subject.OnCompleted();
                subject.OnCompleted();
            }

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void ErrorsOnce()
        {
            var count = 0;
            var subject = new Subject<int>();
            using (Subscribe(subject, onError: _ => count++))
            {
                subject.OnError(new TestException());
                subject.OnError(new TestException());
            }

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void SafeToDisposeDuringOnNext()
        {
            var subject = new Subject<int>();
            IDisposable? subscription = null;
            var action = new Action(() =>
            {
                Debug.Assert(subscription is not null);
                subscription.Dispose();
            });
            subscription = subject.Subscribe(_ => action());
            subject.OnNext(42);
            action = BreakingAction.WithoutArguments;
            subject.OnNext(42);
        }
    }
}
