using FQueue.Watchdog;
using FQueue.Watchdog.Watchers;
using FQueueSynchronizer.Watchdog.Checkers;
using FQueueSynchronizer.Watchdog.Watchers;

namespace FQueueSynchronizer.Watchdog
{
    public class SynchronizerWatchdogThread : WatchdogThread
    {
#warning TODO

        private readonly IEtcdLeaseChecker _etcdLeaseChecker;

        public SynchronizerWatchdogThread(ILeaderElectionWatcher leaderElectionWatcher, IDiskSpaceWatcher diskSpaceWatcher, IEtcdLeaseChecker etcdLeaseChecker) 
            : base(new IWatcher[]{ leaderElectionWatcher, diskSpaceWatcher })
        {
            _etcdLeaseChecker = etcdLeaseChecker;
        }
    }
}