using FQueue.Configuration;
using FQueue.Watchdog;
using Microsoft.AspNetCore.Mvc.Controllers;
using NUnit.Framework;

namespace FQueue.Tests
{
    [TestFixture]
    public class EngineTests
    {
        private class EngineTester : Engine
        {
#warning TODO

            public EngineTester(IWatchdogThread watchdogThread, IControllerFactory controllerFactory) 
                : base(watchdogThread, controllerFactory)
            {
            }

            protected override void StartSpecificLogic()
            {
                throw new System.NotImplementedException();
            }

            protected override void StopSpecificLogic()
            {
                throw new System.NotImplementedException();
            }

            protected override RestConfiguration GetRestConfiguration()
            {
                throw new System.NotImplementedException();
            }
        }

#warning TODO
    }
}