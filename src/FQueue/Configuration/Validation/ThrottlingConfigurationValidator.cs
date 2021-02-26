using System;
using log4net;

namespace FQueue.Configuration.Validation
{
    public class ThrottlingConfigurationValidator : IThrottlingConfigurationValidator
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ThrottlingConfigurationValidator));

        public void Validate(ThrottlingConfiguration configuration)
        {
            _log.Debug($"Validating {nameof(ThrottlingConfiguration)}");

            if (configuration.ConcurrentRequestsLimit < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(configuration.ConcurrentRequestsLimit), $"{nameof(configuration.ConcurrentRequestsLimit)} has to be greater than 0");
            }

            if (configuration.QueueLimit < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(configuration.QueueLimit), $"{nameof(configuration.QueueLimit)} has to be greater or equal to 0");
            }

            if (configuration.QueueTimeout.TotalSeconds < 5)
            {
                throw new ArgumentOutOfRangeException(nameof(configuration.QueueTimeout), $"{nameof(configuration.QueueTimeout)} has to be greater or equal to 5");
            }

            if (configuration.MaximumServerConnections < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(configuration.MaximumServerConnections), $"{nameof(configuration.MaximumServerConnections)} has to be greater than 0");
            }

            if (configuration.MaximumServerConnections < configuration.ConcurrentRequestsLimit + configuration.QueueLimit)
            {
                throw new ArgumentOutOfRangeException(nameof(configuration.MaximumServerConnections), $"{nameof(configuration.MaximumServerConnections)} has to be greater or equal to the sum of {nameof(configuration.ConcurrentRequestsLimit)} and {nameof(configuration.QueueLimit)}");
            }

            _log.Info($"{nameof(ThrottlingConfiguration)} valid");
        }
    }
}