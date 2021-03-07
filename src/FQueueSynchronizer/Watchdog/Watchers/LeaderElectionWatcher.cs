using System;
using FQueue.Configuration;
using FQueueSynchronizer.Etcd;
using FQueueSynchronizer.Watchdog.Checkers;
using log4net;

namespace FQueueSynchronizer.Watchdog.Watchers
{
    public class LeaderElectionWatcher : ILeaderElectionWatcher
    {
#warning TODO - unit tests
        private static readonly ILog _log = LogManager.GetLogger(typeof(LeaderElectionWatcher));

        private readonly IEtcdWrapper _etcdWrapper;
        private readonly IEtcdLeaseChecker _etcdLeaseChecker;
        private readonly IServerUri _serverUri;

        public string Name => nameof(LeaderElectionWatcher);

        public LeaderElectionWatcher(IEtcdWrapper etcdWrapper, IEtcdLeaseChecker etcdLeaseChecker, IServerUri serverUri)
        {
            _etcdWrapper = etcdWrapper;
            _etcdLeaseChecker = etcdLeaseChecker;
            _serverUri = serverUri;
        }

        public bool Check()
        {
            try
            {
                return _etcdWrapper.LockLeader(_serverUri.Uri, _etcdLeaseChecker.LeaseId);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return false;
            }
        }
    }
}