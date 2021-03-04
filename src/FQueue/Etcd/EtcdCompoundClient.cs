using System;
using System.Threading;
using System.Threading.Tasks;
using dotnet_etcd.interfaces;
using Etcdserverpb;

namespace FQueue.Etcd
{
    public class EtcdCompoundClient : IEtcdCompoundClient
    {
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

        public TxnResponse Transaction(TxnRequest request)
        {
            return _etcdClient.Transaction(request);
        }

        public LeaseGrantResponse LeaseGrant(LeaseGrantRequest request)
        {
            return _etcdClient.LeaseGrant(request);
        }

        public Task LeaseKeepAlive(long leaseId, CancellationToken cancellationToken)
        {
            return _etcdClient.LeaseKeepAlive(leaseId, cancellationToken);
        }

        public RangeResponse Get(string key)
        {
            return _etcdClient.Get(key);
        }
    }
}