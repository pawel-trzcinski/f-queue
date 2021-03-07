using FQueue;
using FQueue.Watchdog;
using FQueueSynchronizer.Etcd;
using FQueueSynchronizer.Watchdog;
using FQueueSynchronizer.Watchdog.Checkers;
using FQueueSynchronizer.Watchdog.Watchers;
using log4net;
using SimpleInjector;

namespace FQueueSynchronizer
{
    public static class SynchronizerContainerRegistrator
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(SynchronizerContainerRegistrator));

        public static Container Register()
        {
            _log.Info("Registering BE container");

            Container container = new Container();
            container.Options.DefaultScopedLifestyle = ScopedLifestyle.Flowing;

            CommonContainerRegistrator.Register(container);

            container.Register<IEtcdCompoundClientFactory, EtcdCompoundClientFactory>();
            container.Register<IEtcdWrapper, EtcdWrapper>();

            container.Register<ILeaderElectionWatcher, LeaderElectionWatcher>();
            container.RegisterSingleton<EtcdLeaseChecker, EtcdLeaseChecker>();
            container.RegisterSingleton<IWatchdogThread, SynchronizerWatchdogThread>();

            _log.Debug("Container verification attempt");
            container.Verify();

            return container;
        }
    }
}