using AutoFixture;
using FQueue.Exceptions;
using NUnit.Framework;

namespace FQueue.Tests.Exceptions
{
    [TestFixture]
    public class InvalidBoundryByteExceptionTests
    {
        [Test]
        [Repeat(5)]
        public void ToStringPrintsDataNeeded()
        {
            Fixture fixture = new Fixture();
            byte boundryByteExpected = fixture.Create<byte>();
            byte boundryByteActual = fixture.Create<byte>();
            string contextMessage = fixture.Create<string>();

            InvalidBoundryByteException exception = new InvalidBoundryByteException(new QueueContextMock(contextMessage), boundryByteExpected, boundryByteActual);
            string exceptionToStringResult = exception.ToString();

            Assert.IsTrue(exceptionToStringResult.Contains(boundryByteExpected.ToString()));
            Assert.IsTrue(exceptionToStringResult.Contains(boundryByteActual.ToString()));
            Assert.IsTrue(exceptionToStringResult.Contains(contextMessage));
        }
    }
}