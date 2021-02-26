using FQueue;
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

            //container.RegisterSingleton<IConfigurationReader>(() => new ConfigurationReader(configurationFilename));

            container.RegisterSingleton<IEngine, SynchronizerEngine>();

            _log.Debug("Container verification attempt");
            container.Verify();

            return container;
        }
    }
}