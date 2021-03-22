using FQueue.Watchdog;
using FQueue.Watchdog.Watchers;
using FQueueSynchronizer.Watchdog.Checkers;
using FQueueSynchronizer.Watchdog.Watchers;

namespace FQueueSynchronizer.Watchdog
{
    public class SynchronizerWatchdogThread : WatchdogThread
    {
#warning TODO - unit tests

        private readonly IEtcdLeaseChecker _etcdLeaseChecker;

        public SynchronizerWatchdogThread(IWatcherFactory watcherFactory, IEtcdLeaseChecker etcdLeaseChecker)
            : base(() => new IWatcher[] {watcherFactory.CreateWatcher<ILeaderElectionWatcher>(), watcherFactory.CreateWatcher<IDiskSpaceWatcher>()})
        {
            _etcdLeaseChecker = etcdLeaseChecker;
        }

        protected override void StartSpecificCheckers()
        {
            _etcdLeaseChecker.StartChecking();
        }

        protected override void StopSpecificCheckers()
        {
            _etcdLeaseChecker.StopChecking();
        }
    }
}