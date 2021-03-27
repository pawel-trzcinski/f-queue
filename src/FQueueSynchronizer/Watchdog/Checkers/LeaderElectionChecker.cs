using System;
using FQueue.Configuration;
using FQueueSynchronizer.Etcd;
using FQueueSynchronizer.Watchdog.BackgroundTasks;
using log4net;

namespace FQueueSynchronizer.Watchdog.Checkers
{
    public class LeaderElectionChecker : ILeaderElectionChecker
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(LeaderElectionChecker));

        private readonly IEtcdWrapper _etcdWrapper;
        private readonly IEtcdLeaseBackgroundTask _etcdLeaseBackgroundTask;
        private readonly IServerUri _serverUri;

        public string Name => nameof(LeaderElectionChecker);

        public LeaderElectionChecker(IEtcdWrapper etcdWrapper, IEtcdLeaseBackgroundTask etcdLeaseBackgroundTask, IServerUri serverUri)
        {
            _etcdWrapper = etcdWrapper;
            _etcdLeaseBackgroundTask = etcdLeaseBackgroundTask;
            _serverUri = serverUri;
        }

        public bool Check()
        {
            try
            {
                return _etcdWrapper.LockLeader(_serverUri.Uri, _etcdLeaseBackgroundTask.LeaseId);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return false;
            }
        }
    }
}