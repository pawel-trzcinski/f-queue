using FQueue;
using FQueue.Rest;
using FQueue.Watchdog;
using FQueueSynchronizer.Etcd;
using FQueueSynchronizer.Rest;
using FQueueSynchronizer.Watchdog;
using FQueueSynchronizer.Watchdog.BackgroundTasks;
using FQueueSynchronizer.Watchdog.Checkers;
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

            Container container = CommonContainerRegistrator.Register();

            container.RegisterSingleton<IEtcdCompoundClientFactory, EtcdCompoundClientFactory>();
            container.RegisterSingleton<IEtcdWrapper, EtcdWrapper>(); // singleton because this wrapper is using IEtcdCompoundClientFactory every time

            container.Register<ILeaderElectionWatcher, LeaderElectionWatcher>();
            container.RegisterSingleton<IEtcdLeaseBackgroundTask, EtcdLeaseBackgroundTask>();
            container.RegisterSingleton<IWatchdogThread, SynchronizerWatchdogThread>();

            container.Register<IFQueueController, SynchronizerController>(Lifestyle.Scoped);

            container.RegisterSingleton<IEngine, SynchronizerEngine>();

            _log.Debug("Container verification attempt");
            container.Verify();

            return container;
        }
    }
}