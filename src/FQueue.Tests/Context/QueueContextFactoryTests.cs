using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AutoFixture;
using FQueue.Context;
using NUnit.Framework;

namespace FQueue.Tests.Context
{
    [TestFixture]
    public class QueueContextFactoryTests
    {
        [Test]
        [Repeat(5)]
        public void CreatesContextForAnyString()
        {
            Fixture fixture = new Fixture();
            string name = fixture.Create<string>();

            QueueContextFactory factory = new QueueContextFactory();

            Assert.IsNotNull(factory.GetContext(name));
        }

        [Test]
        public void ContextsAreSingletons()
        {
            Fixture fixture = new Fixture();
            string name1 = fixture.Create<string>();
            string name2 = fixture.Create<string>();

            QueueContextFactory factory = new QueueContextFactory();

            QueueContext context1 = factory.GetContext(name1);
            QueueContext context2 = factory.GetContext(name2);
            QueueContext context3 = factory.GetContext(name2);
            QueueContext context4 = factory.GetContext(name1);
            QueueContext context5 = factory.GetContext(name1);

            Assert.IsNotNull(context1);
            Assert.IsNotNull(context2);
            Assert.IsNotNull(context3);
            Assert.IsNotNull(context4);
            Assert.IsNotNull(context5);

            Assert.AreSame(context1, context4);
            Assert.AreSame(context1, context5);
            Assert.AreSame(context2, context3);
        }

        [TestCase(2, 1)]
        [TestCase(3, 1)]
        [TestCase(5, 5)]
        [TestCase(5, 2)]
        [TestCase(10, 5)]
        [TestCase(50, 5)]
        [TestCase(64, 1)]
        [TestCase(64, 2)]
        [TestCase(64, 10)]
        [TestCase(64, 50)]
        [TestCase(64, 64)]
        public void GettingContextIsThreadSafe(int numberOfThreads, int numberOfNames)
        {
            string[] names = Enumerable.Repeat(String.Empty, numberOfNames).Select(p => Guid.NewGuid().ToString("N")).ToArray();
            ManualResetEvent[] threadStartNotifyHandles = Enumerable.Repeat(String.Empty, numberOfThreads).Select(p => new ManualResetEvent(false)).ToArray();
            ManualResetEvent startWaitHandle = new ManualResetEvent(false);

            List<Thread> tasks = new List<Thread>();

            QueueContextFactory factory = new QueueContextFactory();

            ThreadPool.GetMaxThreads(out int workerThreads, out int completionPortThreads);
            ThreadPool.SetMaxThreads(workerThreads < numberOfThreads ? numberOfThreads : workerThreads, completionPortThreads < numberOfThreads ? numberOfThreads : completionPortThreads);


            for (int i = 0; i < numberOfThreads; i++)
            {
                int i1 = i;
                tasks.Add(new Thread
                    (
                        delegate()
                        {
                            int index = i1 % numberOfNames;
                            string name = names[index];

                            threadStartNotifyHandles[i1].Set();

                            if (!startWaitHandle.WaitOne(TimeSpan.FromMinutes(1)))
                            {
                                throw new InvalidOperationException("Start did not come");
                            }

                            Assert.IsNotNull(factory.GetContext(name));
                        }
                    )
                );
            }

            for (int i = 0; i < numberOfThreads; i++)
            {
                tasks[i].Start();
            }

            // Wait for all threads to start
            WaitHandle.WaitAll(threadStartNotifyHandles.Cast<WaitHandle>().ToArray());

            // execute context getting
            startWaitHandle.Set();

            for (int i = 0; i < numberOfThreads; i++)
            {
                tasks[i].Join(TimeSpan.FromSeconds(15));
            }
        }
    }
}