﻿using FQueue.Watchdog;
using FQueue.Watchdog.Checkers;
using FQueueSynchronizer.Watchdog.BackgroundTasks;
using FQueueSynchronizer.Watchdog.Checkers;

namespace FQueueSynchronizer.Watchdog
{
    public class SynchronizerWatchdogThread : WatchdogThread
    {
#warning TODO - unit tests

        private readonly IEtcdLeaseBackgroundTask _etcdLeaseBackgroundTask;

        public SynchronizerWatchdogThread(ICheckerFactory checkerFactory, IEtcdLeaseBackgroundTask etcdLeaseBackgroundTask)
            : base(() => new IChecker[] {checkerFactory.CreateChecker<ILeaderElectionWatcher>(), checkerFactory.CreateChecker<IDiskSpaceChecker>()})
        {
            _etcdLeaseBackgroundTask = etcdLeaseBackgroundTask;
        }

        protected override void StartSpecificBackgroundTasks()
        {
            _etcdLeaseBackgroundTask.Start();
        }

        protected override void StopSpecificBackgroundTasks()
        {
            _etcdLeaseBackgroundTask.Stop();
        }
    }
}