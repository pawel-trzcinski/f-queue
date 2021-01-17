using FQueueNode;
using NUnit.Framework;

namespace FQueue.Tests
{
    [TestFixture]
    public class ContainerRegistratorTests
    {
        [Test]
        public void ContainerRegistersCorrectly()
        {
            Assert.DoesNotThrow(() => { Assert.IsNotNull(ContainerRegistrator.Register(Program.DEFAULT_CONFIGURATION_FILENAME)); });
        }
    }
}