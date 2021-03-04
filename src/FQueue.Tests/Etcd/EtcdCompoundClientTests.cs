using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using dotnet_etcd.interfaces;
using Etcdserverpb;
using FQueue.Etcd;
using Moq;
using NUnit.Framework;

namespace FQueue.Tests.Etcd
{
    [TestFixture]
    public class EtcdCompoundClientTests
    {
        [Test]
        public void Dispose()
        {
            Mock<IEtcdClient> etcdClientMock = new Mock<IEtcdClient>();
            Mock<IDisposable> disposableMock = new Mock<IDisposable>();

            EtcdCompoundClient client = new EtcdCompoundClient(etcdClientMock.Object, disposableMock.Object);

            client.Dispose();

            disposableMock.Verify(p => p.Dispose(), Times.Once);
            etcdClientMock.Verify(p => p.Dispose(), Times.Never);
        }

        [Test]
        public void Transaction()
        {
            Mock<IEtcdClient> etcdClientMock = new Mock<IEtcdClient>();
            Mock<IDisposable> disposableMock = new Mock<IDisposable>();

            EtcdCompoundClient client = new EtcdCompoundClient(etcdClientMock.Object, disposableMock.Object);

            Fixture fixture = new Fixture();

            TxnRequest txnRequest = fixture.Create<TxnRequest>();
            TxnResponse response = fixture.Create<TxnResponse>();

            etcdClientMock.Setup(p => p.Transaction(txnRequest, null, null, CancellationToken.None)).Returns(response);

            TxnResponse actualResponse = client.Transaction(txnRequest);
            etcdClientMock.Verify(p => p.Transaction(txnRequest, null, null, CancellationToken.None), Times.Once);
            Assert.AreSame(response, actualResponse);
        }

        [Test]
        public void LeaseGrant()
        {
            Mock<IEtcdClient> etcdClientMock = new Mock<IEtcdClient>();
            Mock<IDisposable> disposableMock = new Mock<IDisposable>();

            EtcdCompoundClient client = new EtcdCompoundClient(etcdClientMock.Object, disposableMock.Object);

            Fixture fixture = new Fixture();

            LeaseGrantRequest request = fixture.Create<LeaseGrantRequest>();
            LeaseGrantResponse response = fixture.Create<LeaseGrantResponse>();

            etcdClientMock.Setup(p => p.LeaseGrant(request, null, null, CancellationToken.None)).Returns(response);

            LeaseGrantResponse actualResponse = client.LeaseGrant(request);
            etcdClientMock.Verify(p => p.LeaseGrant(request, null, null, CancellationToken.None), Times.Once);
            Assert.AreSame(response, actualResponse);
        }

        [Test]
        public void LeaseKeepAlive()
        {
            Mock<IEtcdClient> etcdClientMock = new Mock<IEtcdClient>();
            Mock<IDisposable> disposableMock = new Mock<IDisposable>();

            EtcdCompoundClient client = new EtcdCompoundClient(etcdClientMock.Object, disposableMock.Object);

            Fixture fixture = new Fixture();

            long leaseId = fixture.Create<long>();
            CancellationToken cancellationToken = fixture.Create<CancellationToken>();
            Task task = fixture.Create<Task>();

            etcdClientMock.Setup(p => p.LeaseKeepAlive(leaseId, cancellationToken)).Returns(task);

            Task actualTask = client.LeaseKeepAlive(leaseId, cancellationToken);
            etcdClientMock.Verify(p => p.LeaseKeepAlive(leaseId, cancellationToken), Times.Once);
            Assert.AreSame(task, actualTask);
        }

        [Test]
        public void Get()
        {
            Mock<IEtcdClient> etcdClientMock = new Mock<IEtcdClient>();
            Mock<IDisposable> disposableMock = new Mock<IDisposable>();

            EtcdCompoundClient client = new EtcdCompoundClient(etcdClientMock.Object, disposableMock.Object);

            Fixture fixture = new Fixture();

            string key = fixture.Create<string>();
            RangeResponse response = fixture.Create<RangeResponse>();

            etcdClientMock.Setup(p => p.Get(key, null, null, CancellationToken.None)).Returns(response);

            RangeResponse actualResponse = client.Get(key);
            etcdClientMock.Verify(p => p.Get(key, null, null, CancellationToken.None), Times.Once);
            Assert.AreSame(response, actualResponse);
        }
    }
}