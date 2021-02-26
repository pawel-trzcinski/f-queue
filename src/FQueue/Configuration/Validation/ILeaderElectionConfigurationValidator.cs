namespace FQueue.Configuration.Validation
{
    public interface ILeaderElectionConfigurationValidator
    {
        void Validate(LeaderElectionConfiguration configuration);
    }
}