using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FQueue.Watchdog;
using FQueue.Watchdog.Checkers;
using Moq;
using NUnit.Framework;

namespace FQueue.Tests.Watchdog
{
    [TestFixture]
    public class WatchdogThreadTests
    {
        private class WatchdogThreadTester : WatchdogThread
        {
            private readonly Action _startBgTask;
            private readonly Action _stopBgTask;

            public WatchdogThreadTester(Func<IEnumerable<IChecker>> getCheckers, int intervalMs, Action startBgTask, Action stopBgTask)
                : base(getCheckers)
            {
                _checkIntervalMs = intervalMs;
                _startBgTask = startBgTask;
                _stopBgTask = stopBgTask;
            }

            protected override void StartSpecificBackgroundTasks()
            {
                _startBgTask();
            }

            protected override void StopSpecificBackgroundTasks()
            {
                _stopBgTask();
            }
        }

        [Test]
        [Repeat(5)]
        public void ExecutesCheckersAndBackgroundTask()
        {
            object executionObjectsLock = new object();
            List<Guid> executionSteps = new List<Guid>();

            Guid bgStartId = Guid.NewGuid();
            Guid bgStopId = Guid.NewGuid();
            Guid checker1Id = Guid.NewGuid();
            Guid checker2Id = Guid.NewGuid();
            Guid checker3Id = Guid.NewGuid();
            Guid enableActionId = Guid.NewGuid();
            Guid disableActionId = Guid.NewGuid();

            ManualResetEvent wait1 = new ManualResetEvent(false);
            ManualResetEvent wait2 = new ManualResetEvent(false);
            ManualResetEvent wait3 = new ManualResetEvent(false);

            Mock<IChecker> checkerMock1 = new Mock<IChecker>();
            checkerMock1.Setup(p => p.Name).Returns("Checker1");
            checkerMock1
                .Setup(p => p.Check())
                .Callback(() =>
                {
                    lock (executionObjectsLock)
                    {
                        executionSteps.Add(checker1Id);
                    }

                    wait1.Set();
                })
                .Returns(true);

            Mock<IChecker> checkerMock2 = new Mock<IChecker>();
            checkerMock2.Setup(p => p.Name).Returns("Checker2");
            checkerMock2
                .Setup(p => p.Check())
                .Callback(() =>
                {
                    lock (executionObjectsLock)
                    {
                        executionSteps.Add(checker2Id);
                    }

                    wait2.Set();
                })
                .Returns(true);

            Mock<IChecker> checkerMock3 = new Mock<IChecker>();
            checkerMock3.Setup(p => p.Name).Returns("Checker3");
            checkerMock3
                .Setup(p => p.Check())
                .Callback(() =>
                {
                    lock (executionObjectsLock)
                    {
                        executionSteps.Add(checker3Id);
                    }

                    wait3.Set();
                })
                .Returns(true);

            WatchdogThreadTester tester = new WatchdogThreadTester
            (
                () => new[] {checkerMock1.Object, checkerMock2.Object, checkerMock3.Object},
                1000 * 1000,
                () =>
                {
                    lock (executionObjectsLock)
                    {
                        executionSteps.Add(bgStartId);
                    }

                    wait2.Set();
                },
                () =>
                {
                    lock (executionObjectsLock)
                    {
                        executionSteps.Add(bgStopId);
                    }

                    wait2.Set();
                }
            );

            tester.StartChecking
            (
                () =>
                {
                    lock (executionObjectsLock)
                    {
                        executionSteps.Add(enableActionId);
                    }
                },
                () =>
                {
                    lock (executionObjectsLock)
                    {
                        executionSteps.Add(disableActionId);
                    }
                }
            );

            WaitHandle.WaitAll(new[] {wait1, wait2, wait3}.Cast<WaitHandle>().ToArray());

            tester.StopChecking();

            int skip = 0;
            Assert.AreEqual(bgStartId, executionSteps.Skip(skip).First());
            ++skip;

            Assert.AreEqual(1, executionSteps.Skip(skip).Take(3).Count(p => p == checker1Id));
            Assert.AreEqual(1, executionSteps.Skip(skip).Take(3).Count(p => p == checker2Id));
            Assert.AreEqual(1, executionSteps.Skip(skip).Take(3).Count(p => p == checker3Id));
            skip += 3;

            Assert.AreEqual(enableActionId, executionSteps.Skip(skip).First());
            ++skip;

            Assert.IsFalse(executionSteps.Any(p => p == disableActionId));
        }


        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 2)]
        [TestCase(3, 3)]
        [TestCase(6, 1)]
        [TestCase(7, 7)]
        public void CheckerFailureDisablesWatchdog(int checkersCount, int failedCount)
        {
#warning TODO
        }

        [Test]
        public void StopBeforeStartError()
        {
            Mock<IChecker> checkerMock1 = new Mock<IChecker>();
            checkerMock1.Setup(p => p.Name).Returns("Checker1");
            checkerMock1.Setup(p => p.Check()).Returns(true);

            Mock<IChecker> checkerMock2 = new Mock<IChecker>();
            checkerMock2.Setup(p => p.Name).Returns("Checker2");
            checkerMock2.Setup(p => p.Check()).Returns(true);

            Mock<IChecker> checkerMock3 = new Mock<IChecker>();
            checkerMock3.Setup(p => p.Name).Returns("Checker3");
            checkerMock3.Setup(p => p.Check()).Returns(true);

            WatchdogThreadTester tester = new WatchdogThreadTester
            (
                () => new[] {checkerMock1.Object, checkerMock2.Object, checkerMock3.Object},
                1000 * 1000,
                () => { },
                () => { }
            );

            Assert.Throws<InvalidOperationException>(() => tester.StopChecking());
        }
    }
}