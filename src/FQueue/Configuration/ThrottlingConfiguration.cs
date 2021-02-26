using System;
using Newtonsoft.Json;

namespace FQueue.Configuration
{
    [JsonObject(THROTTLING)]
    public class ThrottlingConfiguration
    {
        public const string THROTTLING = "Throttling";

        public int ConcurrentRequestsLimit { get; }
        public int QueueLimit { get; }
        public TimeSpan QueueTimeout { get; }
        public long MaximumServerConnections { get; }

        [JsonConstructor]
        public ThrottlingConfiguration(int concurrentRequestsLimit, int queueLimit, int queueTimeoutS, int maximumServerConnections)
        {
            ConcurrentRequestsLimit = concurrentRequestsLimit;
            QueueLimit = queueLimit;
            QueueTimeout = TimeSpan.FromSeconds(queueTimeoutS);
            MaximumServerConnections = maximumServerConnections;
        }
    }
}