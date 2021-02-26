﻿using FQueue.Configuration;
using FQueue.Configuration.Validation;
using Moq;
using NUnit.Framework;

namespace FQueue.Tests.Configuration
{
    [TestFixture]
    public class FQueueConfigurationTests
    {
        private static FQueueConfiguration CreateConfiguration()
        {
            return new FQueueConfiguration
            (
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
            Mock<IFilesConfigurationValidator> filesConfigurationValidatorMock = new Mock<IFilesConfigurationValidator>();
            Mock<IPerformanceConfigurationValidator> performanceConfigurationValidatorMock = new Mock<IPerformanceConfigurationValidator>();
            Mock<ILeaderElectionConfigurationValidator> leaderElectionConfigurationValidatorMock = new Mock<ILeaderElectionConfigurationValidator>();

            FQueueConfiguration configuration = CreateConfiguration();

            Assert.DoesNotThrow(() => new FQueueConfigurationValidator
            (
                leaderElectionConfigurationValidatorMock.Object,
                filesConfigurationValidatorMock.Object,
                performanceConfigurationValidatorMock.Object,
                restConfigurationValidatorMock.Object
            ).Validate(configuration));

            restConfigurationValidatorMock.Verify(validator => validator.Validate(configuration.Rest), Times.Once);
            filesConfigurationValidatorMock.Verify(validator => validator.Validate(configuration.Files), Times.Once);
            performanceConfigurationValidatorMock.Verify(validator => validator.Validate(configuration.Performance), Times.Once);
            leaderElectionConfigurationValidatorMock.Verify(validator => validator.Validate(configuration.LeaderElection), Times.Once);
        }
    }
}