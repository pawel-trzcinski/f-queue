using System;
using System.Threading;
using System.Threading.Tasks;
using Etcdserverpb;

namespace FQueueSynchronizer.Etcd
{
    public interface IEtcdCompoundClient : IDisposable
    {
        TxnResponse Transaction(TxnRequest request);

        LeaseGrantResponse LeaseGrant(LeaseGrantRequest request);

        Task LeaseKeepAlive(long leaseId, CancellationToken cancellationToken);

        RangeResponse Get(string key);
    }
}