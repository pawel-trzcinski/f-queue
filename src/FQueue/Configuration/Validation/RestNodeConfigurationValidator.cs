using log4net;

namespace FQueue.Configuration.Validation
{
    public class RestNodeConfigurationValidator : IRestNodeConfigurationValidator
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(RestConfigurationValidator));

        private readonly IThrottlingConfigurationValidator _throttlingConfigurationValidator;

        public RestNodeConfigurationValidator(IThrottlingConfigurationValidator throttlingConfigurationValidator)
        {
            _throttlingConfigurationValidator = throttlingConfigurationValidator;
        }

        public void Validate(RestNodeConfiguration configuration)
        {
            _log.Debug($"Validating {nameof(RestConfiguration)}");
            _throttlingConfigurationValidator.Validate(configuration.Throttling);
            _log.Info($"{nameof(RestConfiguration)} valid");
        }
    }
}