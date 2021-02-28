using System;
using System.Threading;
using System.Threading.Tasks;
using Etcdserverpb;
using Google.Protobuf;
using log4net;

namespace FQueue.Etcd
{
    public class EtcdWrapper : IEtcdWrapper
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(EtcdWrapper));
#warning TODO - unit tests
        public const string KEY_LEADER_ELECTION = "leader-election";

        private readonly IEtcdCompoundClientFactory _etcdCompoundClientFactory;

        public EtcdWrapper(IEtcdCompoundClientFactory etcdCompoundClientFactory)
        {
            _etcdCompoundClientFactory = etcdCompoundClientFactory;
        }

        public bool CreateLease(string serverUri, long leaseId, long ttl)
        {
            using (IEtcdCompoundClient client = _etcdCompoundClientFactory.CreateClient(serverUri))
            {
                _log.Debug($"Creating lease {leaseId} wit ttl {ttl}");

                try
                {
                    var leaseGrantResponse = client.LeaseGrant(new LeaseGrantRequest {TTL = ttl, ID = leaseId});
                    if (!String.IsNullOrEmpty(leaseGrantResponse.Error))
                    {
                        _log.Error($"Lease id {leaseId}, ttl {ttl}, error creating: {leaseGrantResponse}");
                        return false;
                    }

                    _log.Info($"Lease {leaseId} created with ttl {ttl}");
                    _log.Debug(leaseGrantResponse);
                    return true;
                }
                catch (Exception ex)
                {
                    _log.Error($"Lease id {leaseId}, ttl {ttl}, exception creating: {ex}");
                    return false;
                }
            }
        }

        public bool LockLeader(string serverUri, long leaseId)
        {
            try
            {
                using (IEtcdCompoundClient client = _etcdCompoundClientFactory.CreateClient(serverUri))
                {
                    var transactionRequest = new TxnRequest();
                    transactionRequest.Compare.Add
                    (
                        new Compare
                        {
                            Key = ByteString.CopyFromUtf8(KEY_LEADER_ELECTION),
                            Target = Compare.Types.CompareTarget.Create,
                            Result = Compare.Types.CompareResult.Greater,
                            CreateRevision = 0
                        }
                    );

                    transactionRequest.Failure.Add(new RequestOp
                    {
                        RequestPut = new PutRequest
                        {
                            Key = ByteString.CopyFromUtf8(KEY_LEADER_ELECTION),
                            Value = ByteString.CopyFromUtf8(leaseId.ToString()),
                            Lease = leaseId
                        }
                    });

                    _ = client.Transaction(transactionRequest);

                    string keyValueInDatabase = GetKey(client, KEY_LEADER_ELECTION);
                    if (String.IsNullOrEmpty(keyValueInDatabase))
                    {
                        return false;
                    }

                    return Convert.ToInt64(keyValueInDatabase) == leaseId;
                }
            }
            catch (Exception ex)
            {
                _log.Error($"Leader locking fo lease {leaseId} exception: {ex}");
                return false;
            }
        }

        public Task StartKeepAlive(string serverUri, long leaseId, CancellationToken token)
        {

            try
            {
                using (IEtcdCompoundClient client = _etcdCompoundClientFactory.CreateClient(serverUri))
                {
                    return client.LeaseKeepAlive(leaseId, token);
                }
            }
            catch (Exception ex)
            {
                _log.Error($"KeepAlive starting failure: {ex}");
                return null;
            }
        }

        public string GetKey(string serverUri, string key)
        {
            try
            {
                using (IEtcdCompoundClient client = _etcdCompoundClientFactory.CreateClient(serverUri))
                {
                    return GetKey(client, key);
                }
            }
            catch (Exception ex)
            {
                _log.Error($"Get {key} failure: {ex}");
                return null;
            }
        }

        private string GetKey(IEtcdCompoundClient client, string key)
        {
            RangeResponse response = client.Get(key);
            if (response.Count == 0 || response.Kvs.Count == 0)
            {
                return null;
            }

            return response.Kvs[0].Value.ToStringUtf8();
        }
    }
}