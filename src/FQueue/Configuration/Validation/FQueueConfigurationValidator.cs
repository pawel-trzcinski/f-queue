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
        private readonly IRestNodeConfigurationValidator _restNodeConfigurationValidator;

        public FQueueConfigurationValidator
            (
            ILeaderElectionConfigurationValidator leaderElectionConfigurationValidator,
            IFilesConfigurationValidator filesConfigurationValidator, 
            IPerformanceConfigurationValidator performanceConfigurationValidator,
            IRestConfigurationValidator restConfigurationValidator,
            IRestNodeConfigurationValidator restNodeConfigurationValidator
            )
        {
            _leaderElectionConfigurationValidator = leaderElectionConfigurationValidator;
            _filesConfigurationValidator = filesConfigurationValidator;
            _performanceConfigurationValidator = performanceConfigurationValidator;
            _restConfigurationValidator = restConfigurationValidator;
            _restNodeConfigurationValidator = restNodeConfigurationValidator;
        }

        public void Validate(FQueueConfiguration configuration)
        {
            _log.Debug($"Validating {nameof(FQueueConfiguration)}");
            _leaderElectionConfigurationValidator.Validate(configuration.LeaderElection);
            _filesConfigurationValidator.Validate(configuration.Files);
            _performanceConfigurationValidator.Validate(configuration.Performance);
            _restNodeConfigurationValidator.Validate(configuration.RestNode);
            _restConfigurationValidator.Validate(configuration.RestSynchronizer);
            _log.Info($"{nameof(FQueueConfiguration)} valid");
        }
    }
}