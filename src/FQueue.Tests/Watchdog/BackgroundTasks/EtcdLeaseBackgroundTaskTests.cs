using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FQueue.Configuration;
using FQueue.Tests.Configuration;
using FQueueSynchronizer.Etcd;
using FQueueSynchronizer.Watchdog.BackgroundTasks;
using Moq;
using NUnit.Framework;

namespace FQueue.Tests.Watchdog.BackgroundTasks
{
    [TestFixture]
    public class EtcdLeaseBackgroundTaskTests
    {
        private static readonly Random _random = new Random();

        [Test]
        [Repeat(5)]
        public void CreateAndCancelKeepaliveTask()
        {
            string serverUri = $"http://{_random.Next(256)}.{_random.Next(256)}.{_random.Next(256)}.{_random.Next(256)}:{_random.Next(65000)}";
            ushort leaseTtl = Convert.ToUInt16(_random.Next(65000));
            long? leaseId = null;
            CancellationToken? token = null;
            Task keepAliveTask = Task.Run(() => { Trace.WriteLine("x"); });

            FQueueConfiguration configuration = new FQueueConfiguration
            (
                RestNodeConfigurationTests.CreateConfiguration(),
                RestConfigurationTests.CreateConfiguration(),
                FilesConfigurationTests.CreateConfiguration(),
                PerformanceConfigurationTests.CreateConfiguration(),
                new LeaderElectionConfiguration(leaseTtl)
            );

            Mock<IEtcdWrapper> etcdWrapperMock = new Mock<IEtcdWrapper>();
            etcdWrapperMock
                .Setup(p => p.CreateLease(serverUri, It.IsAny<long>(), It.IsAny<long>()))
                .Callback<string, long, long>((uri, id, ttl) => { leaseId = id; })
                .Returns(true);
            etcdWrapperMock
                .Setup(p => p.StartKeepAlive(serverUri, It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .Callback<string, long, CancellationToken>((uri, id, ct) => { token = ct; })
                .Returns(keepAliveTask);

            Mock<IServerUri> serverUriMock = new Mock<IServerUri>();
            serverUriMock.Setup(p => p.Uri).Returns(serverUri);

            Mock<IConfigurationReader> configurationReaderMock = new Mock<IConfigurationReader>();
            configurationReaderMock.Setup(p => p.Configuration).Returns(configuration);

            EtcdLeaseBackgroundTask backgroundTask = new EtcdLeaseBackgroundTask(etcdWrapperMock.Object, serverUriMock.Object, configurationReaderMock.Object);

            backgroundTask.Start();

            Assert.IsNotNull(token);
            Assert.IsNotNull(leaseId);

            etcdWrapperMock.Verify(p => p.CreateLease(serverUri, It.IsAny<long>(), Convert.ToInt64(leaseTtl)), Times.Once);
            etcdWrapperMock.Verify(p => p.StartKeepAlive(serverUri, leaseId.Value, token.Value), Times.Once);
            serverUriMock.Verify(p => p.Uri, Times.AtLeastOnce);

            backgroundTask.Stop();

            Assert.IsTrue(token.Value.IsCancellationRequested);
        }

        [Test]
        public void CreateLeaseFailed()
        {
            string serverUri = $"http://{_random.Next(256)}.{_random.Next(256)}.{_random.Next(256)}.{_random.Next(256)}:{_random.Next(65000)}";
            ushort leaseTtl = Convert.ToUInt16(_random.Next(65000));
            Task keepAliveTask = Task.Run(() => { Trace.WriteLine("x"); });

            FQueueConfiguration configuration = new FQueueConfiguration
            (
                RestNodeConfigurationTests.CreateConfiguration(),
                RestConfigurationTests.CreateConfiguration(),
                FilesConfigurationTests.CreateConfiguration(),
                PerformanceConfigurationTests.CreateConfiguration(),
                new LeaderElectionConfiguration(leaseTtl)
            );

            Mock<IEtcdWrapper> etcdWrapperMock = new Mock<IEtcdWrapper>();
            etcdWrapperMock.Setup(p => p.CreateLease(serverUri, It.IsAny<long>(), It.IsAny<long>())).Returns(false);
            etcdWrapperMock.Setup(p => p.StartKeepAlive(serverUri, It.IsAny<long>(), It.IsAny<CancellationToken>())).Returns(keepAliveTask);

            Mock<IServerUri> serverUriMock = new Mock<IServerUri>();
            serverUriMock.Setup(p => p.Uri).Returns(serverUri);

            Mock<IConfigurationReader> configurationReaderMock = new Mock<IConfigurationReader>();
            configurationReaderMock.Setup(p => p.Configuration).Returns(configuration);

            EtcdLeaseBackgroundTask backgroundTask = new EtcdLeaseBackgroundTask(etcdWrapperMock.Object, serverUriMock.Object, configurationReaderMock.Object);

            Assert.Throws<IOException>(() => backgroundTask.Start());

            etcdWrapperMock.Verify(p => p.CreateLease(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<long>()), Times.Once);
            etcdWrapperMock.Verify(p => p.StartKeepAlive(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);

            Assert.Throws<InvalidOperationException>(() => backgroundTask.Stop());
        }

        [Test]
        public void StopExecutedBeforeStart()
        {
            string serverUri = $"http://{_random.Next(256)}.{_random.Next(256)}.{_random.Next(256)}.{_random.Next(256)}:{_random.Next(65000)}";
            Task keepAliveTask = Task.Run(() => { Trace.WriteLine("x"); });

            FQueueConfiguration configuration = FQueueConfigurationTests.CreateConfiguration();

            Mock<IEtcdWrapper> etcdWrapperMock = new Mock<IEtcdWrapper>();
            etcdWrapperMock.Setup(p => p.CreateLease(serverUri, It.IsAny<long>(), It.IsAny<long>())).Returns(true);
            etcdWrapperMock.Setup(p => p.StartKeepAlive(serverUri, It.IsAny<long>(), It.IsAny<CancellationToken>())).Returns(keepAliveTask);

            Mock<IServerUri> serverUriMock = new Mock<IServerUri>();
            serverUriMock.Setup(p => p.Uri).Returns(serverUri);

            Mock<IConfigurationReader> configurationReaderMock = new Mock<IConfigurationReader>();
            configurationReaderMock.Setup(p => p.Configuration).Returns(configuration);

            EtcdLeaseBackgroundTask backgroundTask = new EtcdLeaseBackgroundTask(etcdWrapperMock.Object, serverUriMock.Object, configurationReaderMock.Object);

            Assert.Throws<InvalidOperationException>(() => backgroundTask.Stop());
        }
    }
}