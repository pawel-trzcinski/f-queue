using FQueue.Watchdog.Checkers;
using FQueueSynchronizer.Watchdog;
using FQueueSynchronizer.Watchdog.BackgroundTasks;
using Moq;
using NUnit.Framework;

namespace FQueue.Tests.Watchdog
{
    [TestFixture]
    public class SynchronizerWatchdogThreadTests
    {
        private class SynchronizerWatchdogThreadTester : SynchronizerWatchdogThread
        {
            public SynchronizerWatchdogThreadTester(ICheckerFactory checkerFactory, IEtcdLeaseBackgroundTask etcdLeaseBackgroundTask)
                : base(checkerFactory, etcdLeaseBackgroundTask)
            {
            }

            public void ExecuteFactory()
            {
                _ = _getCheckers();
            }
        }


        [Test]
        public void CorrectCheckersCreated()
        {
            Mock<ICheckerFactory> checkerFactoryMock = new Mock<ICheckerFactory>();
            Mock<IEtcdLeaseBackgroundTask> etcdLeaseBackgroundTaskMock = new Mock<IEtcdLeaseBackgroundTask>();

            SynchronizerWatchdogThreadTester tester = new SynchronizerWatchdogThreadTester(checkerFactoryMock.Object, etcdLeaseBackgroundTaskMock.Object);
            tester.ExecuteFactory();

            etcdLeaseBackgroundTaskMock.Verify(p => p.Start(), Times.Never);
            etcdLeaseBackgroundTaskMock.Verify(p => p.Stop(), Times.Never);
            checkerFactoryMock.Verify(p => p.CreateChecker<IDiskSpaceChecker>(), Times.Once);

            tester.StartChecking(() => { }, () => { });
            etcdLeaseBackgroundTaskMock.Verify(p => p.Start(), Times.Once);
            etcdLeaseBackgroundTaskMock.Verify(p => p.Stop(), Times.Never);

            tester.StopChecking();
            etcdLeaseBackgroundTaskMock.Verify(p => p.Start(), Times.Once);
            etcdLeaseBackgroundTaskMock.Verify(p => p.Stop(), Times.Once);
        }
    }
}