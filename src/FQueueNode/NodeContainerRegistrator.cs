using FQueue;
using FQueue.Rest;
using FQueue.Watchdog;
using FQueueNode.Rest;
using FQueueNode.Watchdog;
using log4net;
using SimpleInjector;

namespace FQueueNode
{
    public static class NodeContainerRegistrator
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(NodeContainerRegistrator));

        public static Container Register()
        {
            _log.Info("Registering FE container");

            Container container = CommonContainerRegistrator.Register();

            container.Register<IFQueueController, NodeController>(Lifestyle.Scoped);

            container.RegisterSingleton<IWatchdogThread, NodeWatchdogThread>();

            container.RegisterSingleton<IEngine, NodeEngine>();

            _log.Debug("Container verification attempt");
            container.Verify();

            return container;
        }
    }
}