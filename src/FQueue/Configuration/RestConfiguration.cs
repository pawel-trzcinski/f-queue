using Newtonsoft.Json;

namespace FQueue.Configuration
{
    [JsonObject(nameof(FQueueConfiguration.Rest))]
    public class RestConfiguration
    {
        // ReSharper disable once MemberInitializerValueIgnored
        public ushort HostingPort { get; } = 7081; // ASCII 70 - F  81 - Q

        public ThrottlingConfiguration NodeThrottling { get; }
        public ThrottlingConfiguration SynchronizerThrottling { get; }

        [JsonConstructor]
        public RestConfiguration(ushort hostingPort, ThrottlingConfiguration nodeThrottling, ThrottlingConfiguration synchronizerThrottling)
        {
            HostingPort = hostingPort;
            NodeThrottling = nodeThrottling;
            SynchronizerThrottling = synchronizerThrottling;
        }
    }
}