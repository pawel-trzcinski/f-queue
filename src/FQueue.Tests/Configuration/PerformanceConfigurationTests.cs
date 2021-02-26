using FQueue.Configuration;
using FQueue.Configuration.Validation;
using NUnit.Framework;

namespace FQueue.Tests.Configuration
{
    [TestFixture]
    public class PerformanceConfigurationTests
    {
        public static PerformanceConfiguration CreateConfiguration
        (
            uint bufferMaxSize = 0,
            uint bufferMaxSizeMB = 1
        )
        {
            return new PerformanceConfiguration(bufferMaxSize, bufferMaxSizeMB);
        }

        [Test]
        public void ValidationOk()
        {
            Assert.DoesNotThrow(() => new PerformanceConfigurationValidator().Validate(CreateConfiguration()));
        }
    }
}