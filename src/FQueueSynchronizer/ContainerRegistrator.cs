using FQueue;
using log4net;
using FQueue.Settings;
using SimpleInjector;

namespace FQueueSynchronizer
{
    public static class ContainerRegistrator
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ContainerRegistrator));

        public static Container Register(string configurationFilename)
        {
            _log.Info("Registering BE container");

            Container container = new Container();
            container.Options.DefaultScopedLifestyle = ScopedLifestyle.Flowing;

            CommonContainerRegistrator.Register(container);
            
            container.RegisterSingleton<IConfigurationReader>(() => new ConfigurationReader(configurationFilename));
            
            _log.Debug("Container verification attempt");
            container.Verify();

            return container;
        }
    }
}