using FQueue.Configuration;
using FQueue.Configuration.Validation;
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
using FQueue.Watchdog.Watchers;
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

            container.RegisterSingleton<IServerUri, ServerUri>();

            container.RegisterSingleton<IDateTimeAbstraction, DateTimeAbstraction>();
            container.Register<IFileAbstraction, FileAbstraction>();
            container.Register<IDirectoryAbstraction, DirectoryAbstraction>();

            container.Register<ILeaderElectionConfigurationValidator, LeaderElectionConfigurationValidator>();
            container.Register<IFilesConfigurationValidator, FilesConfigurationValidator>();
            container.Register<IPerformanceConfigurationValidator, PerformanceConfigurationValidator>();
            container.Register<IThrottlingConfigurationValidator, ThrottlingConfigurationValidator>();
            container.Register<IRestConfigurationValidator, RestConfigurationValidator>();
            container.Register<IFQueueConfigurationValidator, FQueueConfigurationValidator>();

            container.RegisterSingleton<IDiskSpaceWatcher, DiskSpaceWatcher>();

            container.Register<IWriteVersionFileCommand, WriteVersionFileCommand>();
            container.RegisterSingleton<ICommandChain, CommandChain>();

            container.RegisterSingleton<IQueueContextFactory, QueueContextFactory>();
            container.RegisterSingleton<ILockContextFactory, LockContextFactory>();

            container.RegisterSingleton<IDataProtocolFactory, DataProtocolFactory>();
            container.Collection.Register
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
        }
    }
}