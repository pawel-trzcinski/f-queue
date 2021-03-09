using log4net;

namespace FQueue.Configuration.Validation
{
    public class RestConfigurationValidator : IRestConfigurationValidator
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(RestConfigurationValidator));

        private readonly IThrottlingConfigurationValidator _throttlingConfigurationValidator;

        public RestConfigurationValidator(IThrottlingConfigurationValidator throttlingConfigurationValidator)
        {
            _throttlingConfigurationValidator = throttlingConfigurationValidator;
        }

        public void Validate(RestConfiguration configuration)
        {
            _log.Debug($"Validating {nameof(RestConfiguration)}");
            _throttlingConfigurationValidator.Validate(configuration.Throttling);
            _log.Info($"{nameof(RestConfiguration)} valid");
        }
    }
}