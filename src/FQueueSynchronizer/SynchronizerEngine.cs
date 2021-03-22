using System.Reflection;
using FQueue;
using FQueue.Configuration;
using FQueue.Health;
using FQueue.Watchdog;
using FQueueSynchronizer.Rest;
using log4net;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace FQueueSynchronizer
{
    /// <summary>
    /// Main engine of the app. It contains all the bad, non-injectable stuff.
    /// </summary>
    public class SynchronizerEngine : Engine
    {
#warning TODO
#warning TODO - unit tests
        private static readonly ILog _log = LogManager.GetLogger(typeof(SynchronizerEngine));

        private readonly IConfigurationReader _configurationReader;
        private readonly IHealthChecker _healthChecker;

        public SynchronizerEngine
        (
            IWatchdogThread watchdogThread,
            IControllerFactory controllerFactory,
            IConfigurationReader configurationReader,

            IHealthChecker healthChecker
        )
            : base(watchdogThread, controllerFactory)
        {
            _configurationReader = configurationReader;
            _healthChecker = healthChecker;
        }

        protected override Assembly GetControllerAssembly()
        {
            return typeof(SynchronizerController).Assembly;
        }

        protected override void StartSpecificLogic()
        {
#warning TODO - zbadaj spójność danych i czy we wszystkich repozytoriach jest to samo (np to co w pliku wersji musi się odnosić do realnej sytuacji na dysku)
#warning TODO - konfigurowalna moc sprawdzania spójności przy starcie (same pliki wersji, struktura katalogów i plików, wsie dane - w kilku wątkach porównujemy pliki)
#warning TODO - startuj wątki i poczekaj aż się wsio odpali

            _log.Info("No specific logic starting");
        }

        protected override void StopSpecificLogic()
        {
            _log.Info("No specific logic stopping");
        }

        protected override RestConfiguration GetRestConfiguration()
        {
            return _configurationReader.Configuration.RestSynchronizer;
        }
    }
}