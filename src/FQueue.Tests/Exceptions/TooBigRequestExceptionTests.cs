using AutoFixture;
using FQueue.Exceptions;
using NUnit.Framework;

namespace FQueue.Tests.Exceptions
{
    [TestFixture]
    public class TooBigRequestExceptionTests
    {
        [Test]
        [Repeat(5)]
        public void ToStringPrintsDataNeeded()
        {
            Fixture fixture = new Fixture();
            int dataFileMaximumSizeB = fixture.Create<int>();
            int requestBytesCount = fixture.Create<int>();
            int frameSize = fixture.Create<int>();
            string contextMessage = fixture.Create<string>();

            TooBigRequestException exception = new TooBigRequestException(new QueueContextMock(contextMessage), dataFileMaximumSizeB, requestBytesCount, frameSize);
            string exceptionToStringResult = exception.ToString();

            Assert.IsTrue(exceptionToStringResult.Contains(dataFileMaximumSizeB.ToString()));
            Assert.IsTrue(exceptionToStringResult.Contains(requestBytesCount.ToString()));
            Assert.IsTrue(exceptionToStringResult.Contains(frameSize.ToString()));
            Assert.IsTrue(exceptionToStringResult.Contains(contextMessage));
        }
    }
}