using Newtonsoft.Json;

namespace FQueue.Configuration
{
    [JsonObject(nameof(FQueueConfiguration.LeaderElection))]
    public class LeaderElectionConfiguration
    {
        // ReSharper disable once MemberInitializerValueIgnored
        public ushort LeaseTtlS { get; } = 5;

        [JsonConstructor]
        public LeaderElectionConfiguration(ushort leaseTtlS)
        {
            LeaseTtlS = leaseTtlS;
        }
    }
}