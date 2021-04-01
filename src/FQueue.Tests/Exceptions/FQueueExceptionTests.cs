using System;
using FQueue.Exceptions;
using NUnit.Framework;

namespace FQueue.Tests.Exceptions
{
    [TestFixture]
    public class FQueueExceptionTests
    {
        private class FQueueExceptionTester : FQueueException
        {
            public FQueueExceptionTester(QueueContextMock context, string message, Exception innerException = null)
                : base(context, message, innerException)
            {
            }
        }

        [Test]
        public void ToStringPrintsDataNeeded()
        {
            string innerExceptionContextMessage = Guid.NewGuid().ToString();
            string innerExceptionMessage = Guid.NewGuid().ToString();
            string contextMessage = Guid.NewGuid().ToString();
            string message = Guid.NewGuid().ToString();

            FQueueExceptionTester tester = new FQueueExceptionTester(new QueueContextMock(contextMessage), message);
            Assert.AreEqual(message, tester.Message);
            string testerString = tester.ToString();
            Assert.IsTrue(testerString.Contains(message));
            Assert.IsTrue(testerString.Contains(contextMessage));

            FQueueExceptionTester innerException = new FQueueExceptionTester(new QueueContextMock(innerExceptionContextMessage), innerExceptionMessage);
            tester = new FQueueExceptionTester(new QueueContextMock(contextMessage), message, innerException);
            Assert.AreEqual(message, tester.Message);
            testerString = tester.ToString();
            Assert.IsTrue(testerString.Contains(message));
            Assert.IsTrue(testerString.Contains(contextMessage));
            Assert.IsTrue(testerString.Contains(innerExceptionMessage));
            Assert.IsTrue(testerString.Contains(innerExceptionContextMessage));
        }
    }
}