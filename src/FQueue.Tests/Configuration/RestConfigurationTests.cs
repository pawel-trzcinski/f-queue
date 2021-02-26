using FQueue.Configuration;
using FQueue.Configuration.Validation;
using Moq;
using NUnit.Framework;

namespace FQueue.Tests.Configuration
{
    [TestFixture]
    public class RestConfigurationTests
    {
        public static RestConfiguration CreateConfiguration
        (
            ushort hostingPort = 7081
        )
        {
            return new RestConfiguration(hostingPort, ThrottlingConfigurationTests.CreateConfiguration(), ThrottlingConfigurationTests.CreateConfiguration());
        }

        [Test]
        public void ValidationOk()
        {
            Mock<IThrottlingConfigurationValidator> throttlingConfigurationValidatorMock = new Mock<IThrottlingConfigurationValidator>();

            RestConfiguration configuration = CreateConfiguration();

            Assert.DoesNotThrow(() => new RestConfigurationValidator(throttlingConfigurationValidatorMock.Object).Validate(configuration));

            throttlingConfigurationValidatorMock.Verify(validator => validator.Validate(configuration.NodeThrottling), Times.Once);
            throttlingConfigurationValidatorMock.Verify(validator => validator.Validate(configuration.SynchronizerThrottling), Times.Once);
        }
    }
}