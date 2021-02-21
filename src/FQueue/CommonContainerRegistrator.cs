using FQueue.Context;
using FQueue.Data;
using FQueue.Data.V01BasicProtocol;
using log4net;
using Microsoft.AspNetCore.Mvc.Controllers;
using FQueue.DateTime;
using FQueue.FileSystem;
using FQueue.FileSystem.VersionFile;
using FQueue.Health;
using FQueue.QueueLockContext;
using FQueue.Rest;
using SimpleInjector;

namespace FQueue
{
    public static class CommonContainerRegistrator
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(CommonContainerRegistrator));

        public static void Register(Container container)
        {
            _log.Info("Registering common");

            container.Options.DefaultScopedLifestyle = ScopedLifestyle.Flowing;

            container.RegisterSingleton<IDateTimeAbstraction, DateTimeAbstraction>();
            container.Register<IFileAbstraction, FileAbstraction>();

            container.Register<IWriteVersionFileCommand, WriteVersionFileCommand>();
            container.RegisterSingleton<ICommandChain, CommandChain>();

            container.RegisterSingleton<IQueueContextFactory, QueueContextFactory>();
            container.RegisterSingleton<ILockContextFactory, LockContextFactory>();

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

            container.RegisterSingleton<IEngine, SynchronizerEngine>();
        }
    }
}