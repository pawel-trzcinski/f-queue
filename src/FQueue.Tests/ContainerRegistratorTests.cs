using FQueueNode;
using FQueueSynchronizer;
using NUnit.Framework;

namespace FQueue.Tests
{
    [TestFixture]
    public class ContainerRegistratorTests
    {
#warning TODO - check if all DataProtocols are registered correctly

        [Test]
        public void NodeContainerRegistersCorrectly()
        {
            Assert.DoesNotThrow(() => { Assert.IsNotNull(NodeContainerRegistrator.Register()); });
        }

        [Test]
        public void SynchronizerContainerRegistersCorrectly()
        {
            Assert.DoesNotThrow(() => { Assert.IsNotNull(SynchronizerContainerRegistrator.Register()); });
        }
    }
}