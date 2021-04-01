using AutoFixture;
using FQueue.Exceptions;
using NUnit.Framework;

namespace FQueue.Tests.Exceptions
{
    [TestFixture]
    public class UnsupportedProtocolVersionExceptionTests
    {
        [Test]
        [Repeat(5)]
        public void ToStringPrintsDataNeeded()
        {
            Fixture fixture = new Fixture();
            byte protocolVersion = fixture.Create<byte>();
            string contextMessage = fixture.Create<string>();

            UnsupportedProtocolVersionException exception = new UnsupportedProtocolVersionException(new QueueContextMock(contextMessage), protocolVersion);
            string exceptionToStringResult = exception.ToString();

            Assert.IsTrue(exceptionToStringResult.Contains(protocolVersion.ToString()));
            Assert.IsTrue(exceptionToStringResult.Contains(contextMessage));
        }
    }
}