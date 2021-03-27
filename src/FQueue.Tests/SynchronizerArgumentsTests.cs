using System;
using FQueueSynchronizer;
using NUnit.Framework;

namespace FQueue.Tests
{
    [TestFixture]
    public class SynchronizerArgumentsTests
    {
        [Test]
        public void EtcdEndpointIsConfigurationUri()
        {
            SynchronizerArguments nodeArguments = new SynchronizerArguments
            {
                EtcdEndpoint = Guid.NewGuid().ToString()
            };
            Assert.AreEqual(nodeArguments.EtcdEndpoint, nodeArguments.ConfigurationUri);
        }
    }
}