using System;
using System.Net;
using System.Reflection;
using FQueue.Configuration;
using FQueue.Rest;
using FQueue.Watchdog;
using Microsoft.AspNetCore.Mvc.Controllers;
using Moq;
using NUnit.Framework;
using SimpleInjector;

namespace FQueue.Tests
{
    [TestFixture]
    public class EngineTests
    {
        private class EngineTester : Engine
        {
            public int StartSpecificLogicCount { get; private set; }
            public int StopSpecificLogicCount { get; private set; }

            private readonly RestConfiguration _restConfiguration;

            public EngineTester(IWatchdogThread watchdogThread, IControllerFactory controllerFactory, RestConfiguration restConfiguration)
                : base(watchdogThread, controllerFactory)
            {
                _restConfiguration = restConfiguration;
            }

            protected override Assembly GetControllerAssembly()
            {
                return typeof(EngineTests).Assembly;
            }

            protected override void StartSpecificLogic()
            {
                ++StartSpecificLogicCount;
            }

            protected override void StopSpecificLogic()
            {
                ++StopSpecificLogicCount;
            }

            protected override RestConfiguration GetRestConfiguration()
            {
                return _restConfiguration;
            }
        }

        [Test]
        public void EngineLogicAndHostingStarted()
        {
            Mock<IWatchdogThread> watchdogThreadMock = new Mock<IWatchdogThread>();

            bool startExecuted = false;
            watchdogThreadMock.Setup(p => p.StartChecking(It.IsAny<Action>(), It.IsAny<Action>())).Callback<Action, Action>
            (
                (startAction, stopAction) =>
                {
                    startAction(); // force watchdog to start engine logic immediatelly
                    startExecuted = true;
                }
            );
            watchdogThreadMock.Setup(p => p.StopChecking()).Callback(() => Assert.IsTrue(startExecuted));

            string controllerValueString = Guid.NewGuid().ToString().ToLowerInvariant();
            const ushort HOST_PORT = 9873;

            Container container = new Container();
            container.Options.DefaultScopedLifestyle = ScopedLifestyle.Flowing;
            container.Register<IFQueueController>(() => new TestController(controllerValueString), Lifestyle.Scoped);

            RestConfiguration restConfiguration = new RestConfiguration(HOST_PORT, new ThrottlingConfiguration(5, 5, 10, 15));

            EngineTester tester = new EngineTester(watchdogThreadMock.Object, new ControllerFactory(container), restConfiguration);

            tester.Start();

            Assert.AreEqual(1, tester.StartSpecificLogicCount);
            Assert.AreEqual(0, tester.StopSpecificLogicCount);

            WebClient client = new WebClient();
            Assert.AreEqual(controllerValueString, client.DownloadString($"http://127.0.0.1:{HOST_PORT}/{Engine.FQUEUE}/testC/testM"));

            tester.Stop();

            Assert.AreEqual(1, tester.StartSpecificLogicCount);
            Assert.AreEqual(0, tester.StopSpecificLogicCount); // because watchdog did not stop engine logic
        }
    }
}