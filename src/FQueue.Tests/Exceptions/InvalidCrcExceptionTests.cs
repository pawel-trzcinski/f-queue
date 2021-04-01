using AutoFixture;
using FQueue.Exceptions;
using NUnit.Framework;

namespace FQueue.Tests.Exceptions
{
    [TestFixture]
    public class InvalidCrcExceptionTests
    {
        [Test]
        [Repeat(5)]
        public void ToStringPrintsDataNeeded()
        {
            Fixture fixture = new Fixture();
            uint storedCrc32 = fixture.Create<uint>();
            uint calculatedCrc32 = fixture.Create<uint>();
            string contextMessage = fixture.Create<string>();

            InvalidCrcException exception = new InvalidCrcException(new QueueContextMock(contextMessage), new byte[0], storedCrc32, calculatedCrc32);
            string exceptionToStringResult = exception.ToString();

            Assert.IsTrue(exceptionToStringResult.Contains(storedCrc32.ToString()));
            Assert.IsTrue(exceptionToStringResult.Contains(calculatedCrc32.ToString()));
            Assert.IsTrue(exceptionToStringResult.Contains(contextMessage));
        }
    }
}