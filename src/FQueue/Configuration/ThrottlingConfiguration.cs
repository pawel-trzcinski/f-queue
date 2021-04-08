using System;
using Newtonsoft.Json;

namespace FQueue.Configuration
{
    [JsonObject(THROTTLING)]
    public class ThrottlingConfiguration
    {
        public const string THROTTLING = "Throttling";

        public bool Enabled { get; }

        public int ConcurrentRequestsLimit { get; }
        public int QueueLimit { get; }
        public TimeSpan QueueTimeout { get; }
        public long MaximumServerConnections { get; }

        [JsonConstructor]
        public ThrottlingConfiguration(bool enabled, int concurrentRequestsLimit, int queueLimit, int queueTimeoutS, int maximumServerConnections)
        {
            Enabled = enabled;
            ConcurrentRequestsLimit = concurrentRequestsLimit;
            QueueLimit = queueLimit;
            QueueTimeout = TimeSpan.FromSeconds(queueTimeoutS);
            MaximumServerConnections = maximumServerConnections;
        }
    }
}