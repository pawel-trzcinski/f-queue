using System;
using FQueue.Configuration;
using FQueue.Configuration.Validation;
using NUnit.Framework;

namespace FQueue.Tests.Configuration
{
    [TestFixture]
    public class ThrottlingConfigurationTests
    {
        public static ThrottlingConfiguration CreateConfiguration
        (
            bool enabled = true,
            int concurrentRequestsLimit = 5,
            int queueLimit = 50,
            int queueTimeoutS = 30,
            int maximumServerConnections = 100
        )
        {
            return new ThrottlingConfiguration(enabled, concurrentRequestsLimit, queueLimit, queueTimeoutS, maximumServerConnections);
        }

        [Test]
        public void ValidationOk()
        {
            Assert.DoesNotThrow(() => new ThrottlingConfigurationValidator().Validate(CreateConfiguration()));
        }

        [Test]
        public void Validation_MinimalValues()
        {
            Assert.DoesNotThrow(() => new ThrottlingConfigurationValidator().Validate(CreateConfiguration
            (
                concurrentRequestsLimit: 1,
                queueLimit: 0,
                queueTimeoutS: 5,
                maximumServerConnections: 1
            )));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void ValidationError_ConcurrentRequestsLimit(int concurrentRequestsLimit)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ThrottlingConfigurationValidator().Validate(CreateConfiguration(concurrentRequestsLimit: concurrentRequestsLimit)));
        }

        [TestCase(-2)]
        [TestCase(-1)]
        public void ValidationError_QueueLimit(int queueLimit)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ThrottlingConfigurationValidator().Validate(CreateConfiguration(queueLimit: queueLimit)));
        }

        [TestCase(-2)]
        [TestCase(0)]
        [TestCase(4)]
        public void ValidationError_QueueTimeout(int queueTimeoutS)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ThrottlingConfigurationValidator().Validate(CreateConfiguration(queueTimeoutS: queueTimeoutS)));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void ValidationError_MaximumServerConnections(int maximumServerConnections)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ThrottlingConfigurationValidator().Validate(CreateConfiguration(maximumServerConnections: maximumServerConnections)));
        }

        [TestCase(1, 1, 1)]
        [TestCase(1, 2, 2)]
        [TestCase(2, 1, 2)]
        [TestCase(2, 2, 3)]
        public void ValidationError_ConnectionsMustBeEnoughForQueueAndExecution(int concurrentRequestsLimit, int queueLimit, int maximumServerConnections)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ThrottlingConfigurationValidator().Validate(CreateConfiguration
            (
                concurrentRequestsLimit: concurrentRequestsLimit,
                queueLimit: queueLimit,
                maximumServerConnections: maximumServerConnections
            )));
        }
    }
}