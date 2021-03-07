using FQueueSynchronizer.Etcd;

namespace FQueueSynchronizer.Watchdog.Watchers
{
    public class LeaderElectionWatcher : ILeaderElectionWatcher
    {
#warning TODO
        private readonly IEtcdWrapper _etcdWrapper;

        public string Name => nameof(LeaderElectionWatcher);

        public LeaderElectionWatcher(IEtcdWrapper etcdWrapper)
        {
            _etcdWrapper = etcdWrapper;
        }

        public bool Check()
        {
            throw new System.NotImplementedException();
        }
    }
}