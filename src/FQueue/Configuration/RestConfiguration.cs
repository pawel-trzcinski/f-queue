using Newtonsoft.Json;

namespace FQueue.Configuration
{
    public class RestConfiguration
    {
        // ReSharper disable once MemberInitializerValueIgnored
        public ushort HostingPort { get; } = 7081; // ASCII 70 - F  81 - Q

        public ThrottlingConfiguration Throttling { get; }

        [JsonConstructor]
        public RestConfiguration(ushort hostingPort, ThrottlingConfiguration throttling)
        {
            HostingPort = hostingPort;
            Throttling = throttling;
        }
    }
}