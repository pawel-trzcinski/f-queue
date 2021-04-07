using Newtonsoft.Json;

namespace FQueue.Configuration
{
    public class RestNodeConfiguration : RestConfiguration
    {
        public bool InternalQueueOperationReturnsError { get; }

        [JsonConstructor]
        public RestNodeConfiguration(ushort hostingPort, ThrottlingConfiguration throttling, bool internalQueueOperationReturnsError)
            : base(hostingPort, throttling)
        {
            InternalQueueOperationReturnsError = internalQueueOperationReturnsError;
        }
    }
}