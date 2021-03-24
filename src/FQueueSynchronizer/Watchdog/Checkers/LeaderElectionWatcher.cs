using System;
using FQueue.Configuration;
using FQueueSynchronizer.Etcd;
using FQueueSynchronizer.Watchdog.BackgroundTasks;
using log4net;

namespace FQueueSynchronizer.Watchdog.Checkers
{
    public class LeaderElectionWatcher : ILeaderElectionWatcher
    {
#warning TODO - unit tests
        private static readonly ILog _log = LogManager.GetLogger(typeof(LeaderElectionWatcher));

        private readonly IEtcdWrapper _etcdWrapper;
        private readonly IEtcdLeaseBackgroundTask _etcdLeaseBackgroundTask;
        private readonly IServerUri _serverUri;

        public string Name => nameof(LeaderElectionWatcher);

        public LeaderElectionWatcher(IEtcdWrapper etcdWrapper, IEtcdLeaseBackgroundTask etcdLeaseBackgroundTask, IServerUri serverUri)
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