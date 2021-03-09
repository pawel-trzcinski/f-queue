using FQueue;
using FQueue.Configuration;
using FQueue.Watchdog;
using log4net;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace FQueueNode
{
    public class NodeEngine : Engine
    {
#warning TODO
#warning TODO - unit tests
        private static readonly ILog _log = LogManager.GetLogger(typeof(NodeEngine));

        private readonly IConfigurationReader _configurationReader;

        public NodeEngine(IWatchdogThread watchdogThread, IControllerFactory controllerFactory, IConfigurationReader configurationReader)
            : base(watchdogThread, controllerFactory)
        {
            _configurationReader = configurationReader;
        }

        protected override void StartSpecificLogic()
        {
            _log.Info("No specific logic starting");
        }

        protected override void StopSpecificLogic()
        {
            _log.Info("No specific logic stopping");
        }

        protected override RestConfiguration GetRestConfiguration()
        {
            return _configurationReader.Configuration.RestNode;
        }
    }
}