using System;
using System.Threading;
using System.Threading.Tasks;
using Etcdserverpb;
using Grpc.Core;

namespace FQueue.Etcd
{
    public interface IEtcdCompoundClient : IDisposable
    {
        TxnResponse Transaction(
            TxnRequest request,
            Metadata headers = null,
            System.DateTime? deadline = null,
            CancellationToken cancellationToken = default);

        LeaseGrantResponse LeaseGrant(
            LeaseGrantRequest request,
            Metadata headers = null,
            System.DateTime? deadline = null,
            CancellationToken cancellationToken = default);

        Task LeaseKeepAlive(long leaseId, CancellationToken cancellationToken);

        RangeResponse Get(
            string key,
            Metadata headers = null,
            System.DateTime? deadline = null,
            CancellationToken cancellationToken = default);
    }
}