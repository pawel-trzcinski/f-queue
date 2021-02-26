using System;
using FQueue.Configuration;
using FQueue.Configuration.Validation;
using NUnit.Framework;

namespace FQueue.Tests.Configuration
{
    [TestFixture]
    public class LeaderElectionConfigurationTests
    {
        public static LeaderElectionConfiguration CreateConfiguration
        (
            ushort leaseTtlS = 5
        )
        {
            return new LeaderElectionConfiguration(leaseTtlS);
        }

        [Test]
        public void ValidationOk()
        {
            Assert.DoesNotThrow(() => new LeaderElectionConfigurationValidator().Validate(CreateConfiguration()));
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(15)]
        [TestCase(20)]
        [TestCase(30)]
        public void ValidationOk_TypicalTtl(int ttl)
        {
            Assert.DoesNotThrow(() => new LeaderElectionConfigurationValidator().Validate(CreateConfiguration((ushort) ttl)));
        }

        [Test]
        public void ValidationError_TtlZero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LeaderElectionConfigurationValidator().Validate(CreateConfiguration(0)));
        }
    }
}