using FQueue.Configuration;
using FQueue.Configuration.Validation;
using Moq;
using NUnit.Framework;

namespace FQueue.Tests.Configuration
{
    public class RestNodeConfigurationTests
    {
        public static RestNodeConfiguration CreateConfiguration
        (
            ushort hostingPort = 7081
        )
        {
            return new RestNodeConfiguration(hostingPort, ThrottlingConfigurationTests.CreateConfiguration(), true);
        }

        [Test]
        public void ValidationOk()
        {
            Mock<IThrottlingConfigurationValidator> throttlingConfigurationValidatorMock = new Mock<IThrottlingConfigurationValidator>();

            RestNodeConfiguration configuration = CreateConfiguration();

            Assert.DoesNotThrow(() => new RestConfigurationValidator(throttlingConfigurationValidatorMock.Object).Validate(configuration));

            throttlingConfigurationValidatorMock.Verify(validator => validator.Validate(configuration.Throttling), Times.Once);
        }
    }
}