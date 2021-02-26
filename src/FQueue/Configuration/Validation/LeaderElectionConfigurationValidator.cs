using System;
using log4net;

namespace FQueue.Configuration.Validation
{
    public class LeaderElectionConfigurationValidator : ILeaderElectionConfigurationValidator
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(LeaderElectionConfigurationValidator));

        public void Validate(LeaderElectionConfiguration configuration)
        {
            _log.Debug($"Validating {nameof(RestConfiguration)}");
            if (configuration.LeaseTtlS == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(configuration.LeaseTtlS), "Lease TTL must be a positive number");
            }

            _log.Info($"{nameof(RestConfiguration)} valid");
        }
    }
}