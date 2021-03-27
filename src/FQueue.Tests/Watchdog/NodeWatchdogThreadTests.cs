using FQueue.Watchdog.Checkers;
using FQueueNode.Watchdog;
using Moq;
using NUnit.Framework;

namespace FQueue.Tests.Watchdog
{
    [TestFixture]
    public class NodeWatchdogThreadTests
    {
        private class NodeWatchdogThreadTester : NodeWatchdogThread
        {
            public NodeWatchdogThreadTester(ICheckerFactory checkerFactory)
                : base(checkerFactory)
            {
            }

            public void ExecuteFactory()
            {
                _ = _getCheckers();
            }
        }


        [Test]
        public void CorrectCheckersCreated()
        {
            Mock<ICheckerFactory> checkerFactoryMock = new Mock<ICheckerFactory>();

            new NodeWatchdogThreadTester(checkerFactoryMock.Object).ExecuteFactory();

            checkerFactoryMock.Verify(p => p.CreateChecker<IDiskSpaceChecker>(), Times.Once);
        }
    }
}