using Newtonsoft.Json;

namespace FQueue.Configuration
{
    [JsonObject(nameof(FQueueConfiguration.Performance))]
    public class PerformanceConfiguration
    {
        public uint BufferMaxSize { get; }

        // ReSharper disable once InconsistentNaming
        public uint BufferMaxSizeMB { get; }

        [JsonConstructor]
        // ReSharper disable once InconsistentNaming
        public PerformanceConfiguration(uint bufferMaxSize, uint bufferMaxSizeMB)
        {
            BufferMaxSize = bufferMaxSize;
            BufferMaxSizeMB = bufferMaxSizeMB;
        }
    }
}