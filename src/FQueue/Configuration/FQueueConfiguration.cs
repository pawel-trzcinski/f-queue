using System;
using Newtonsoft.Json;

namespace FQueue.Configuration
{
    [Serializable]
    public class FQueueConfiguration
    {
        public RestConfiguration Rest { get; }

        public FilesConfiguration Files { get; }

        public PerformanceConfiguration Performance { get; }

        public LeaderElectionConfiguration LeaderElection { get; }

        [JsonConstructor]
        public FQueueConfiguration(RestConfiguration rest, FilesConfiguration files, PerformanceConfiguration performance, LeaderElectionConfiguration leaderElection)
        {
            Rest = rest;
            Files = files;
            Performance = performance;
            LeaderElection = leaderElection;
        }
    }
}