using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Etcdserverpb;
using FQueueSynchronizer.Etcd;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Moq;
using Mvccpb;
using NUnit.Framework;

namespace FQueue.Tests.Etcd
{
    [TestFixture]
    public class EtcdWrapperTests
    {
        #region CreateLease

        [Test]
        public void CreateLease_Error()
        {
            var fixture = new Fixture();
            string error = fixture.Create<string>();
            string serverUri = fixture.Create<string>();
            long leaseId = fixture.Create<long>();
            long ttl = fixture.Create<long>();

            Mock<IEtcdCompoundClient> clientMock = new Mock<IEtcdCompoundClient>();
            clientMock.Setup(p => p.LeaseGrant(It.IsAny<LeaseGrantRequest>()))
                .Callback<LeaseGrantRequest>(lgr =>
                {
                    Assert.IsNotNull(lgr);
                    Assert.AreEqual(leaseId, lgr.ID);
                    Assert.AreEqual(ttl, lgr.TTL);
                })
                .Returns(new LeaseGrantResponse {Error = error});

            Mock<IEtcdCompoundClientFactory> etcdCompoundClientFactoryMock = new Mock<IEtcdCompoundClientFactory>();
            etcdCompoundClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(clientMock.Object);

            bool result = new EtcdWrapper(etcdCompoundClientFactoryMock.Object).CreateLease(serverUri, leaseId, ttl);

            Assert.IsFalse(result);
            clientMock.Verify(p => p.LeaseGrant(It.IsAny<LeaseGrantRequest>()), Times.Once);

            etcdCompoundClientFactoryMock.Verify(p => p.CreateClient(serverUri), Times.Once);
        }

        [Test]
        public void CreateLease_Exception()
        {
            var fixture = new Fixture();
            string serverUri = fixture.Create<string>();
            long leaseId = fixture.Create<long>();
            long ttl = fixture.Create<long>();

            Mock<IEtcdCompoundClient> clientMock = new Mock<IEtcdCompoundClient>();
            clientMock.Setup(p => p.LeaseGrant(It.IsAny<LeaseGrantRequest>()))
                .Callback<LeaseGrantRequest>(lgr =>
                {
                    Assert.IsNotNull(lgr);
                    Assert.AreEqual(leaseId, lgr.ID);
                    Assert.AreEqual(ttl, lgr.TTL);

                    throw new Exception("Whatever");
                });

            Mock<IEtcdCompoundClientFactory> etcdCompoundClientFactoryMock = new Mock<IEtcdCompoundClientFactory>();
            etcdCompoundClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(clientMock.Object);

            bool result = new EtcdWrapper(etcdCompoundClientFactoryMock.Object).CreateLease(serverUri, leaseId, ttl);

            Assert.IsFalse(result);
            clientMock.Verify(p => p.LeaseGrant(It.IsAny<LeaseGrantRequest>()), Times.Once);

            etcdCompoundClientFactoryMock.Verify(p => p.CreateClient(serverUri), Times.Once);
        }

        [Test]
        public void CreateLease()
        {
            var fixture = new Fixture();
            string serverUri = fixture.Create<string>();
            long leaseId = fixture.Create<long>();
            long ttl = fixture.Create<long>();

            Mock<IEtcdCompoundClient> clientMock = new Mock<IEtcdCompoundClient>();
            clientMock.Setup(p => p.LeaseGrant(It.IsAny<LeaseGrantRequest>()
                ))
                .Callback<LeaseGrantRequest>(lgr =>
                {
                    Assert.IsNotNull(lgr);
                    Assert.AreEqual(leaseId, lgr.ID);
                    Assert.AreEqual(ttl, lgr.TTL);
                })
                .Returns(new LeaseGrantResponse {Error = String.Empty});

            Mock<IEtcdCompoundClientFactory> etcdCompoundClientFactoryMock = new Mock<IEtcdCompoundClientFactory>();
            etcdCompoundClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(clientMock.Object);

            bool result = new EtcdWrapper(etcdCompoundClientFactoryMock.Object).CreateLease(serverUri, leaseId, ttl);

            Assert.IsTrue(result);
            clientMock.Verify(p => p.LeaseGrant(It.IsAny<LeaseGrantRequest>()), Times.Once);

            etcdCompoundClientFactoryMock.Verify(p => p.CreateClient(serverUri), Times.Once);
        }

        #endregion CreateLease

        #region LockLeader

        [TestCase(1, null, false)]
        [TestCase(1, 2, false)]
        [TestCase(1, 1, true)]
        public void LockLeader(long expectedLeaseId, long? returnedLeaseId, bool expectedResult)
        {
            var fixture = new Fixture();
            string serverUri = fixture.Create<string>();

            Mock<IEtcdCompoundClient> clientMock = new Mock<IEtcdCompoundClient>();
            clientMock
                .Setup(p => p.Transaction(It.IsAny<TxnRequest>()))
                .Callback<TxnRequest>
                (
                    r =>
                    {
                        Assert.AreEqual(1, r.Compare.Count);
                        Compare compare = r.Compare[0];

                        Assert.AreEqual(EtcdWrapper.KEY_LEADER_ELECTION, compare.Key.ToStringUtf8());
                        Assert.AreEqual(Compare.Types.CompareTarget.Create, compare.Target);
                        Assert.AreEqual(Compare.Types.CompareResult.Greater, compare.Result);
                        Assert.Zero(compare.CreateRevision);

                        Assert.AreEqual(1, r.Failure.Count);
                        RequestOp requestOp = r.Failure[0];
                        PutRequest putRequest = requestOp.RequestPut;
                        Assert.IsNotNull(putRequest);

                        Assert.AreEqual(EtcdWrapper.KEY_LEADER_ELECTION, putRequest.Key.ToStringUtf8());
                        Assert.AreEqual(expectedLeaseId.ToString(), putRequest.Value.ToStringUtf8());
                        Assert.AreEqual(expectedLeaseId, putRequest.Lease);
                    }
                );
            clientMock.Setup(p => p.Get(EtcdWrapper.KEY_LEADER_ELECTION)).Returns
            (
                returnedLeaseId.HasValue
                    ? new RangeResponse
                    {
                        Count = 1,
                        Kvs = {new RepeatedField<KeyValue> {new KeyValue {Value = ByteString.CopyFromUtf8(returnedLeaseId.ToString())}}}
                    }
                    : new RangeResponse()
            );

            Mock<IEtcdCompoundClientFactory> etcdCompoundClientFactoryMock = new Mock<IEtcdCompoundClientFactory>();
            etcdCompoundClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(clientMock.Object);

            EtcdWrapper wrapper = new EtcdWrapper(etcdCompoundClientFactoryMock.Object);

            bool isLeader = wrapper.LockLeader(serverUri, expectedLeaseId);

            etcdCompoundClientFactoryMock.Verify(p => p.CreateClient(serverUri), Times.Once);
            Assert.AreEqual(expectedResult, isLeader);
            clientMock.Verify(p => p.Transaction(It.IsAny<TxnRequest>()), Times.Once);
            clientMock.Verify(p => p.Get(EtcdWrapper.KEY_LEADER_ELECTION), Times.Once);
        }

        [Test]
        public void LockLeader_TransactionException()
        {
            var fixture = new Fixture();
            string serverUri = fixture.Create<string>();
            long leaseId = fixture.Create<long>();

            Mock<IEtcdCompoundClient> clientMock = new Mock<IEtcdCompoundClient>();
            clientMock
                .Setup(p => p.Transaction(It.IsAny<TxnRequest>()))
                .Callback<TxnRequest>(r => throw new Exception("xxx"));
            clientMock.Setup(p => p.Get(EtcdWrapper.KEY_LEADER_ELECTION)).Returns
            (
                new RangeResponse
                {
                    Count = 1,
                    Kvs = {new RepeatedField<KeyValue> {new KeyValue {Value = ByteString.CopyFromUtf8("A")}}}
                }
            );

            Mock<IEtcdCompoundClientFactory> etcdCompoundClientFactoryMock = new Mock<IEtcdCompoundClientFactory>();
            etcdCompoundClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(clientMock.Object);

            Assert.IsFalse(new EtcdWrapper(etcdCompoundClientFactoryMock.Object).LockLeader(serverUri, leaseId));
        }

        [Test]
        public void LockLeader_GetException()
        {
            var fixture = new Fixture();
            string serverUri = fixture.Create<string>();
            long leaseId = fixture.Create<long>();

            Mock<IEtcdCompoundClient> clientMock = new Mock<IEtcdCompoundClient>();
            clientMock.Setup(p => p.Get(EtcdWrapper.KEY_LEADER_ELECTION))
                .Callback<string>(r => throw new Exception("xxx"));

            Mock<IEtcdCompoundClientFactory> etcdCompoundClientFactoryMock = new Mock<IEtcdCompoundClientFactory>();
            etcdCompoundClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(clientMock.Object);

            Assert.IsFalse(new EtcdWrapper(etcdCompoundClientFactoryMock.Object).LockLeader(serverUri, leaseId));
        }

        #endregion LockLeader

        #region StartKeepAlive

        [Test]
        public void StartKeepAliveCreatesTask()
        {
            var fixture = new Fixture();
            string serverUri = fixture.Create<string>();
            long leaseId = fixture.Create<long>();
            CancellationToken cancellationToken = fixture.Create<CancellationToken>();
            Task task = fixture.Create<Task>();

            Mock<IEtcdCompoundClient> clientMock = new Mock<IEtcdCompoundClient>();
            clientMock.Setup(p => p.LeaseKeepAlive(It.IsAny<long>(), It.IsAny<CancellationToken>())).Returns(task);

            Mock<IEtcdCompoundClientFactory> etcdCompoundClientFactoryMock = new Mock<IEtcdCompoundClientFactory>();
            etcdCompoundClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(clientMock.Object);

            EtcdWrapper wrapper = new EtcdWrapper(etcdCompoundClientFactoryMock.Object);

            Task returnedTask = wrapper.StartKeepAlive(serverUri, leaseId, cancellationToken);

            etcdCompoundClientFactoryMock.Verify(p => p.CreateClient(serverUri), Times.Once);
            Assert.AreSame(task, returnedTask);
            clientMock.Verify(p => p.LeaseKeepAlive(leaseId, cancellationToken), Times.Once);
        }

        [Test]
        public void StartKeepAliveException()
        {
            var fixture = new Fixture();
            string error = fixture.Create<string>();
            string serverUri = fixture.Create<string>();
            long leaseId = fixture.Create<long>();
            CancellationToken cancellationToken = fixture.Create<CancellationToken>();

            Mock<IEtcdCompoundClient> clientMock = new Mock<IEtcdCompoundClient>();
            clientMock
                .Setup(p => p.LeaseKeepAlive(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .Callback<long, CancellationToken>((l, c) => throw new IOException(error));

            Mock<IEtcdCompoundClientFactory> etcdCompoundClientFactoryMock = new Mock<IEtcdCompoundClientFactory>();
            etcdCompoundClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(clientMock.Object);

            EtcdWrapper wrapper = new EtcdWrapper(etcdCompoundClientFactoryMock.Object);

            Task returnedTask = wrapper.StartKeepAlive(serverUri, leaseId, cancellationToken);

            etcdCompoundClientFactoryMock.Verify(p => p.CreateClient(serverUri), Times.Once);
            Assert.IsNull(returnedTask);
            clientMock.Verify(p => p.LeaseKeepAlive(leaseId, cancellationToken), Times.Once);
        }

        #endregion StartKeepAlive

        #region GetKey

        [Test]
        public void GetKey_ReturnsValue()
        {
            var fixture = new Fixture();
            string serverUri = fixture.Create<string>();
            string key = fixture.Create<string>();
            string value1 = fixture.Create<string>();
            string value2 = fixture.Create<string>();

            Mock<IEtcdCompoundClient> clientMock = new Mock<IEtcdCompoundClient>();
            clientMock.Setup(p => p.Get(It.IsAny<string>())).Returns
            (
                new RangeResponse
                {
                    Count = 2,
                    Kvs =
                    {
                        new RepeatedField<KeyValue> {new KeyValue {Value = ByteString.CopyFromUtf8(value1)}},
                        new RepeatedField<KeyValue> {new KeyValue {Value = ByteString.CopyFromUtf8(value2)}}
                    }
                }
            );

            Mock<IEtcdCompoundClientFactory> etcdCompoundClientFactoryMock = new Mock<IEtcdCompoundClientFactory>();
            etcdCompoundClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(clientMock.Object);

            EtcdWrapper wrapper = new EtcdWrapper(etcdCompoundClientFactoryMock.Object);

            string value = wrapper.GetKey(serverUri, key);

            etcdCompoundClientFactoryMock.Verify(p => p.CreateClient(serverUri), Times.Once);
            Assert.AreEqual(value1, value);
            clientMock.Verify(p => p.Get(key), Times.Once);
        }

        [Test]
        public void GetKey_NoValue()
        {
            var fixture = new Fixture();
            string serverUri = fixture.Create<string>();
            string key = fixture.Create<string>();

            Mock<IEtcdCompoundClient> clientMock = new Mock<IEtcdCompoundClient>();
            clientMock.Setup(p => p.Get(It.IsAny<string>())).Returns(new RangeResponse());

            Mock<IEtcdCompoundClientFactory> etcdCompoundClientFactoryMock = new Mock<IEtcdCompoundClientFactory>();
            etcdCompoundClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(clientMock.Object);

            EtcdWrapper wrapper = new EtcdWrapper(etcdCompoundClientFactoryMock.Object);

            string value = wrapper.GetKey(serverUri, key);

            etcdCompoundClientFactoryMock.Verify(p => p.CreateClient(serverUri), Times.Once);
            Assert.IsNull(value);
            clientMock.Verify(p => p.Get(key), Times.Once);
        }

        [Test]
        public void GetKey_Exception()
        {
            var fixture = new Fixture();
            string serverUri = fixture.Create<string>();
            string key = fixture.Create<string>();

            Mock<IEtcdCompoundClient> clientMock = new Mock<IEtcdCompoundClient>();
            clientMock.Setup(p => p.Get(It.IsAny<string>())).Callback<string>(s => throw new InvalidCastException("whatever"));

            Mock<IEtcdCompoundClientFactory> etcdCompoundClientFactoryMock = new Mock<IEtcdCompoundClientFactory>();
            etcdCompoundClientFactoryMock.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(clientMock.Object);

            EtcdWrapper wrapper = new EtcdWrapper(etcdCompoundClientFactoryMock.Object);

            string value = wrapper.GetKey(serverUri, key);

            etcdCompoundClientFactoryMock.Verify(p => p.CreateClient(serverUri), Times.Once);
            Assert.IsNull(value);
            clientMock.Verify(p => p.Get(key), Times.Once);
        }

        #endregion GetKey
    }
}