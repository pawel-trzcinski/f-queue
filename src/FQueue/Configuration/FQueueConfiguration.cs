using System;
using Newtonsoft.Json;

namespace FQueue.Configuration
{
    [Serializable]
    public class FQueueConfiguration
    {
        public RestNodeConfiguration RestNode { get; }
        public RestConfiguration RestSynchronizer { get; }

        public FilesConfiguration Files { get; }

        public PerformanceConfiguration Performance { get; }

        public LeaderElectionConfiguration LeaderElection { get; }

        [JsonConstructor]
        public FQueueConfiguration(RestNodeConfiguration restNode, RestConfiguration restSynchronizer, FilesConfiguration files, PerformanceConfiguration performance, LeaderElectionConfiguration leaderElection)
        {
            RestNode = restNode;
            RestSynchronizer = restSynchronizer;
            Files = files;
            Performance = performance;
            LeaderElection = leaderElection;
        }
    }
}