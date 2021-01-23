using FQueue.Context;
using FQueue.Data;
using FQueue.Data.V01BasicProtocol;
using log4net;
using Microsoft.AspNetCore.Mvc.Controllers;
using FQueue.DateTime;
using FQueue.Health;
using FQueue.Rest;
using FQueue.Settings;
using SimpleInjector;

namespace FQueue
{
    public static class ContainerRegistrator
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ContainerRegistrator));

        public static Container Register(string configurationFilename)
        {
            _log.Info("Registering container");

            Container container = new Container();

            container.Options.DefaultScopedLifestyle = ScopedLifestyle.Flowing;

            container.RegisterSingleton<IDateTimeService, DateTimeService>();

            container.RegisterSingleton<IConfigurationReader>(() => new ConfigurationReader(configurationFilename));

            container.RegisterSingleton<IQueueContextFactory, QueueContextFactory>();
            
            container.RegisterSingleton<IDataProtocolFactory, DataProtocolFactory>();
            container.Register
            (
                typeof(IDataProtocol),
                new[]
                {
                    typeof(DataProtocolV01)
                }
            );
            container.RegisterSingleton<IDataProtocolAdapter, DataProtocolAdapter>();
            
            container.RegisterSingleton<IHealthChecker, HealthChecker>();

            container.Register<IFQueueController, FQueueController>(Lifestyle.Scoped);
            container.RegisterSingleton<IControllerFactory, ControllerFactory>();

            container.RegisterSingleton<IEngine, Engine>();

            _log.Debug("Container verification attempt");
            container.Verify();

            return container;
        }
    }
}