using AutoFixture;
using FQueue.Configuration;
using FQueue.Tests.Configuration;
using FQueue.Watchdog;
using FQueueNode;
using Microsoft.AspNetCore.Mvc.Controllers;
using Moq;
using NUnit.Framework;

namespace FQueue.Tests
{
    [TestFixture]
    public class NodeEngineTests
    {
        private class NodeEngineTester : NodeEngine
        {
            public NodeEngineTester()
                : this(new Mock<IConfigurationReader>().Object)
            {
            }

            public NodeEngineTester(IConfigurationReader configurationReader)
                : base(new Mock<IWatchdogThread>().Object, new Mock<IControllerFactory>().Object, configurationReader)
            {
            }

            public ushort GetRestPort()
            {
                return GetRestConfiguration().HostingPort;
            }

            public string GetAssemblyName()
            {
                return GetControllerAssembly().FullName;
            }
        }

        [Test]
        public void CorrectAssemblyReturned()
        {
            Assert.AreEqual(typeof(NodeEngine).Assembly.FullName, new NodeEngineTester().GetAssemblyName());
        }

        [Test]
        [Repeat(5)]
        public void RestConfigurationForNodeReturned()
        {
            Fixture fixture = new Fixture();
            ushort port = fixture.Create<ushort>();

            FQueueConfiguration configuration = new FQueueConfiguration
            (
                new RestConfiguration(port, ThrottlingConfigurationTests.CreateConfiguration()),
                RestConfigurationTests.CreateConfiguration(),
                FilesConfigurationTests.CreateConfiguration(),
                PerformanceConfigurationTests.CreateConfiguration(),
                LeaderElectionConfigurationTests.CreateConfiguration()
            );

            Mock<IConfigurationReader> configurationReaderMock = new Mock<IConfigurationReader>();
            configurationReaderMock.Setup(p => p.Configuration).Returns(configuration);

            NodeEngineTester tester = new NodeEngineTester(configurationReaderMock.Object);
            Assert.AreEqual(port, tester.GetRestPort());
        }
    }
}