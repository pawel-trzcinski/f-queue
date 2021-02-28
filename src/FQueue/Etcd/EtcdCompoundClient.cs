using System;
using System.Threading;
using System.Threading.Tasks;
using dotnet_etcd.interfaces;
using Etcdserverpb;
using Grpc.Core;

namespace FQueue.Etcd
{
    public class EtcdCompoundClient : IEtcdCompoundClient
    {
#warning TODO - unit tests
        private readonly IEtcdClient _etcdClient;
        private readonly IDisposable _disposable;

        public EtcdCompoundClient(IEtcdClient etcdClient, IDisposable disposable)
        {
            _etcdClient = etcdClient;
            _disposable = disposable;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        public TxnResponse Transaction(TxnRequest request, Metadata headers = null, System.DateTime? deadline = null, CancellationToken cancellationToken = default)
        {
            return _etcdClient.Transaction(request, headers, deadline, cancellationToken);
        }

        public LeaseGrantResponse LeaseGrant(LeaseGrantRequest request, Metadata headers = null, System.DateTime? deadline = null, CancellationToken cancellationToken = default)
        {
            return _etcdClient.LeaseGrant(request, headers, deadline, cancellationToken);
        }

        public Task LeaseKeepAlive(long leaseId, CancellationToken cancellationToken)
        {
            return _etcdClient.LeaseKeepAlive(leaseId, cancellationToken);
        }

        public RangeResponse Get(string key, Metadata headers = null, System.DateTime? deadline = null, CancellationToken cancellationToken = default)
        {
            return _etcdClient.Get(key, headers, deadline, cancellationToken);
        }
    }
}