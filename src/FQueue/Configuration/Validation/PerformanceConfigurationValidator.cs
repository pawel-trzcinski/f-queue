using log4net;

namespace FQueue.Configuration.Validation
{
    public class PerformanceConfigurationValidator : IPerformanceConfigurationValidator
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(PerformanceConfigurationValidator));

        public void Validate(PerformanceConfiguration configuration)
        {
            _log.Debug($"Validating {nameof(PerformanceConfiguration)}");
            _log.Info($"{nameof(PerformanceConfiguration)} valid");
        }
    }
}