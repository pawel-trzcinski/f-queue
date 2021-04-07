using FQueue.Configuration;
using FQueue.Configuration.Validation;
using Moq;
using NUnit.Framework;

namespace FQueue.Tests.Configuration
{
    [TestFixture]
    public class FQueueConfigurationTests
    {
        public static FQueueConfiguration CreateConfiguration()
        {
            return new FQueueConfiguration
            (
                RestNodeConfigurationTests.CreateConfiguration(),
                RestConfigurationTests.CreateConfiguration(),
                FilesConfigurationTests.CreateConfiguration(),
                PerformanceConfigurationTests.CreateConfiguration(),
                LeaderElectionConfigurationTests.CreateConfiguration()
            );
        }

        [Test]
        public void ValidationOk()
        {
            Mock<IRestConfigurationValidator> restConfigurationValidatorMock = new Mock<IRestConfigurationValidator>();
            Mock<IRestNodeConfigurationValidator> restNodeConfigurationValidatorMock = new Mock<IRestNodeConfigurationValidator>();
            Mock<IFilesConfigurationValidator> filesConfigurationValidatorMock = new Mock<IFilesConfigurationValidator>();
            Mock<IPerformanceConfigurationValidator> performanceConfigurationValidatorMock = new Mock<IPerformanceConfigurationValidator>();
            Mock<ILeaderElectionConfigurationValidator> leaderElectionConfigurationValidatorMock = new Mock<ILeaderElectionConfigurationValidator>();

            FQueueConfiguration configuration = CreateConfiguration();

            Assert.DoesNotThrow(() => new FQueueConfigurationValidator
            (
                leaderElectionConfigurationValidatorMock.Object,
                filesConfigurationValidatorMock.Object,
                performanceConfigurationValidatorMock.Object,
                restConfigurationValidatorMock.Object, 
                restNodeConfigurationValidatorMock.Object
            ).Validate(configuration));

            restNodeConfigurationValidatorMock.Verify(validator => validator.Validate(configuration.RestNode), Times.Once);
            restConfigurationValidatorMock.Verify(validator => validator.Validate(configuration.RestSynchronizer), Times.Once);
            filesConfigurationValidatorMock.Verify(validator => validator.Validate(configuration.Files), Times.Once);
            performanceConfigurationValidatorMock.Verify(validator => validator.Validate(configuration.Performance), Times.Once);
            leaderElectionConfigurationValidatorMock.Verify(validator => validator.Validate(configuration.LeaderElection), Times.Once);
        }
    }
}