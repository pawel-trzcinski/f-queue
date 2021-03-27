using AutoFixture;
using FQueue.Configuration;
using FQueueSynchronizer.Etcd;
using FQueueSynchronizer.Watchdog.BackgroundTasks;
using FQueueSynchronizer.Watchdog.Checkers;
using Moq;
using NUnit.Framework;

namespace FQueue.Tests.Watchdog.Checkers
{
    [TestFixture]
    public class LeaderElectionCheckerTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void ReturnWrappersLeaderElectionLock(bool wrapperValue)
        {
            Fixture fixture = new Fixture();
            long leaseId = fixture.Create<long>();
            string serverUri = fixture.Create<string>();

            Mock<IEtcdWrapper> etcdWrapperMock = new Mock<IEtcdWrapper>();
            etcdWrapperMock.Setup(p => p.LockLeader(It.IsAny<string>(), It.IsAny<long>())).Returns(wrapperValue);

            Mock<IEtcdLeaseBackgroundTask> etcdLeaseBackgroundTaskMock = new Mock<IEtcdLeaseBackgroundTask>();
            etcdLeaseBackgroundTaskMock.Setup(p => p.LeaseId).Returns(leaseId);

            Mock<IServerUri> serverUriMock = new Mock<IServerUri>();
            serverUriMock.Setup(p => p.Uri).Returns(serverUri);

            LeaderElectionChecker checker = new LeaderElectionChecker(etcdWrapperMock.Object, etcdLeaseBackgroundTaskMock.Object, serverUriMock.Object);

            Assert.IsNotNull(checker.Name);
            Assert.AreEqual(wrapperValue, checker.Check());
            etcdWrapperMock.Verify(p => p.LockLeader(serverUri, leaseId), Times.Once);
        }
    }
}