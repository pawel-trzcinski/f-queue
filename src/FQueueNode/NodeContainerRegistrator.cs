using FQueue;
using FQueue.Rest;
using FQueueNode.Rest;
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

            Container container = new Container();
            container.Options.DefaultScopedLifestyle = ScopedLifestyle.Flowing;

            CommonContainerRegistrator.Register(container);

            container.Register<IFQueueController, NodeController>(Lifestyle.Scoped);

            container.RegisterSingleton<IEngine, NodeEngine>();

            _log.Debug("Container verification attempt");
            container.Verify();

            return container;
        }
    }
}