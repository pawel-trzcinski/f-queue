using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using FQueue.Configuration;
using FQueue.Context;
using FQueue.Tests.Configuration;
using FQueueNode.Logic;
using Moq;
using NUnit.Framework;

namespace FQueue.Tests.Logic
{
    [TestFixture]
    public class RestExecutorTests
    {
        private class RestExecutorLockTester : RestExecutor
        {
            private readonly Action _actionToExecute;
            private readonly string _queueName;

            public RestExecutorLockTester(Action actionToExecute, string queueName)
                : base(new Mock<IConfigurationReader>().Object, new Mock<IQueueContextFactory>().Object, new Mock<INodeQueueManager>().Object)
            {
                _actionToExecute = actionToExecute;
                _queueName = queueName;
            }

            protected override T LockLocallyWithContext<T>(string queueName, Func<QueueContext, T> action)
            {
                Assert.AreEqual(_queueName, queueName);
                _actionToExecute();
                return default;
            }
        }

        [Test]
        public void AllExecutorInterfaceActionsLockOnName()
        {
            Type iType = typeof(IRestExecutor);

            MethodInfo[] methods = iType.GetMethods();

            bool[] executionCheck = new bool[methods.Length];

            for (int i = 0; i < methods.Length; i++)
            {
                int iConst = i;
                MethodInfo method = methods[i];
                string queueName = Guid.NewGuid().ToString();

                RestExecutor executor = new RestExecutorLockTester(() => { executionCheck[iConst] = true; }, queueName);

                ParameterInfo[] parameters = method.GetParameters();
                Assert.IsTrue(parameters.Any(p => p.Name.Equals("queueName") && p.ParameterType == typeof(string)));

                object[] arguments = new object[parameters.Length];
                for (int j = 0; j < arguments.Length; j++)
                {
                    ParameterInfo pi = parameters[j];

                    if (pi.Name.Equals("queueName"))
                    {
                        arguments[j] = queueName;
                    }
                    else
                    {
                        if (pi.ParameterType.IsValueType)
                        {
                            arguments[j] = Activator.CreateInstance(pi.ParameterType);
                        }
                        else
                        {
                            arguments[j] = null;
                        }
                    }
                }

                method.Invoke(executor, BindingFlags.Public | BindingFlags.InvokeMethod, null, arguments, CultureInfo.InvariantCulture);
            }

            Assert.AreEqual(executionCheck.Length, executionCheck.Count(p => p));
        }

        [Test]
        public void LocalLockSynchronizesThreads()
        {
            QueueContextFactory qeueContextFactory = new QueueContextFactory();
            FQueueConfiguration configuration = FQueueConfigurationTests.CreateConfiguration();

            const int NUMBER_OF_THREADS = 50;
            Random random = new Random();
            const int ONE_WAIT_MAX = 100;

            ManualResetEvent threadStartGuard = new ManualResetEvent(false);
            WaitHandle[] endIndicators = new WaitHandle[NUMBER_OF_THREADS];

            string queueName = Guid.NewGuid().ToString();
            List<Thread> threads = new List<Thread>(NUMBER_OF_THREADS);
            ConcurrentQueue<int> executionTimes = new ConcurrentQueue<int>();
            int index = 0;

            for (int i = 0; i < NUMBER_OF_THREADS; i++)
            {
                int startSleep = random.Next(5, ONE_WAIT_MAX);
                int endSleep = random.Next(5, ONE_WAIT_MAX);
                ManualResetEvent endIndicator = new ManualResetEvent(false);
                endIndicators[i] = endIndicator;

                Thread thread = new Thread(delegate()
                {
                    Mock<IConfigurationReader> configurationReaderMock = new Mock<IConfigurationReader>();
                    configurationReaderMock.Setup(p => p.Configuration).Returns(configuration);

                    Mock<INodeQueueManager> nodeQueueManagerMock = new Mock<INodeQueueManager>();
                    nodeQueueManagerMock
                        .Setup(p => p.Count(It.IsAny<QueueContext>()))
                        .Callback<QueueContext>(context =>
                        {
                            executionTimes.Enqueue(Interlocked.Increment(ref index));
                            Thread.Sleep(startSleep);
                            executionTimes.Enqueue(Interlocked.Increment(ref index));
                            Thread.Sleep(endSleep);
                            executionTimes.Enqueue(Interlocked.Increment(ref index));
                        });

                    RestExecutor executor = new RestExecutor(configurationReaderMock.Object, qeueContextFactory, nodeQueueManagerMock.Object);

                    // wait for all threads to start
                    threadStartGuard.WaitOne();

                    executor.Count(queueName);

                    // notify that this thread ended
                    endIndicator.Set();
                });

                threads.Add(thread);
                thread.Start();
            }

            // fire all Dequeue methods
            threadStartGuard.Set();

            // wait for all dequeue inner actions to complete
            WaitHandle.WaitAll(endIndicators, TimeSpan.FromMilliseconds(2 * (ONE_WAIT_MAX + 5)) + TimeSpan.FromSeconds(5));

            for (int i = 0; i < NUMBER_OF_THREADS; i++)
            {
                if (!threads[i].Join(TimeSpan.FromSeconds(5)))
                {
                    Assert.Fail($"Unable to join thread {i}");
                }
            }

            Assert.AreEqual(3 * NUMBER_OF_THREADS, executionTimes.Count);
            Assert.IsTrue(executionTimes.SequenceEqual(executionTimes.OrderBy(p => p)));
        }

        [Test]
        public void DequeueExecutesManager()
        {
#warning TODO
        }

        [Test]
        public void CountExecutesManager()
        {
#warning TODO
        }

        [Test]
        public void PeekExecutesManager()
        {
#warning TODO
        }

        [Test]
        public void PeekTagExecutesManager()
        {
#warning TODO
        }

        [Test]
        public void BackupExecutesManager()
        {
#warning TODO
#warning TODO - backup creates default filename if not provided
        }

        [Test]
        public void EnqueueEmptyString()
        {
#warning TODO - enqueue - empty is ok
        }

        [Test]
        public void EnqueueObject()
        {
#warning TODO
        }

        [Test]
        public void EnqueueObjectThrowsOnNoTag()
        {
#warning TODO
        }

        [Test]
        public void EnqueueArray()
        {
#warning TODO
        }

        [Test]
        public void EnqueueArrayThrowsOnNoTagInAnyObject()
        {
#warning TODO
        }
    }
}