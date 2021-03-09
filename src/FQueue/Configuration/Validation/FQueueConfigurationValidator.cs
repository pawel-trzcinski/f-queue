using log4net;

namespace FQueue.Configuration.Validation
{
    public class FQueueConfigurationValidator : IFQueueConfigurationValidator
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(FQueueConfigurationValidator));

        private readonly ILeaderElectionConfigurationValidator _leaderElectionConfigurationValidator;
        private readonly IFilesConfigurationValidator _filesConfigurationValidator;
        private readonly IPerformanceConfigurationValidator _performanceConfigurationValidator;
        private readonly IRestConfigurationValidator _restConfigurationValidator;

        public FQueueConfigurationValidator
            (
            ILeaderElectionConfigurationValidator leaderElectionConfigurationValidator,
            IFilesConfigurationValidator filesConfigurationValidator, 
            IPerformanceConfigurationValidator performanceConfigurationValidator,
            IRestConfigurationValidator restConfigurationValidator
            )
        {
            _leaderElectionConfigurationValidator = leaderElectionConfigurationValidator;
            _filesConfigurationValidator = filesConfigurationValidator;
            _performanceConfigurationValidator = performanceConfigurationValidator;
            _restConfigurationValidator = restConfigurationValidator;
        }

        public void Validate(FQueueConfiguration configuration)
        {
            _log.Debug($"Validating {nameof(FQueueConfiguration)}");
            _leaderElectionConfigurationValidator.Validate(configuration.LeaderElection);
            _filesConfigurationValidator.Validate(configuration.Files);
            _performanceConfigurationValidator.Validate(configuration.Performance);
            _restConfigurationValidator.Validate(configuration.RestNode);
            _restConfigurationValidator.Validate(configuration.RestSynchronizer);
            _log.Info($"{nameof(FQueueConfiguration)} valid");
        }
    }
}