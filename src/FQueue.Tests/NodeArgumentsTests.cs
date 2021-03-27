using System;
using FQueueNode;
using NUnit.Framework;

namespace FQueue.Tests
{
    [TestFixture]
    public class NodeArgumentsTests
    {
        [Test]
        public void SynchronizerEndpointIsConfigurationUri()
        {
            NodeArguments nodeArguments = new NodeArguments
            {
                SynchronizerEndpoint = Guid.NewGuid().ToString()
            };
            Assert.AreEqual(nodeArguments.SynchronizerEndpoint, nodeArguments.ConfigurationUri);
        }
    }
}