using System.Threading;
using System.Threading.Tasks;

namespace FQueue.Etcd
{
    public interface IEtcdWrapper
    {
        /// <summary>
        /// Create new lease
        /// </summary>
        /// <param name="serverUri">ETCD server uri</param>
        /// <param name="leaseId">Id of the lease you want to create</param>
        /// <param name="ttl">Lease's TimeToLive</param>
        /// <returns><b>true</b> if lease was created</returns>
        bool CreateLease(string serverUri, long leaseId, long ttl);

        /// <summary>
        /// Attempt to become a leader
        /// </summary>
        /// <param name="serverUri">ETCD server uri</param>
        /// <param name="leaseId">Id of the lease <i>leader-election</i> key is created with</param>
        /// <returns><true>true</true> if you became the leader</returns>
        bool LockLeader(string serverUri, long leaseId);

        /// <summary>
        /// Start continous KeepAlive requests.
        /// </summary>
        /// <param name="serverUri">ETCD server uri</param>
        /// <param name="leaseId">Id of the lease</param>
        /// <param name="token">Cancellation token to stop KeepAlive requests</param>
        /// <returns>Task where KeepAlive requests are sent every 1/3 of TTL</returns>
        Task StartKeepAlive(string serverUri, long leaseId, CancellationToken token);

        /// <summary>
        /// Get value from etcd
        /// </summary>
        /// <param name="serverUri">ETCD server uri</param>
        /// <param name="key">Key to take value from</param>
        /// <returns>Value for the key or <b>null</b> if no there is no key or error occured</returns>
        string GetKey(string serverUri, string key);
    }
}