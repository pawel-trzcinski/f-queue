using System.Text;
using log4net;
using Newtonsoft.Json;

namespace FQueue.Settings
{
    [JsonObject(nameof(FQueueConfiguration.Performance))]
    public class PerformanceConfiguration : ConfigurationReporter
    {
#warning TODO - unit tests
        private static readonly ILog _log = LogManager.GetLogger(typeof(PerformanceConfiguration));

        public uint BufferMaxSize { get; }

        // ReSharper disable once InconsistentNaming
        public uint BufferMaxSizeMB { get; }

        [JsonConstructor]
        // ReSharper disable once InconsistentNaming
        public PerformanceConfiguration(uint bufferMaxSize, uint bufferMaxSizeMB)
            : base(nameof(PerformanceConfiguration), INDENT)
        {
            BufferMaxSize = bufferMaxSize;
            BufferMaxSizeMB = bufferMaxSizeMB;

            ValidateConfiguration();
        }

        private void ValidateConfiguration()
        {
            _log.Debug($"Validating {nameof(PerformanceConfiguration)}");
            _log.Info($"{nameof(PerformanceConfiguration)} valid");
        }

        #region IConfigurationReporter implementation

        public void ReportConfiguration(StringBuilder sb)
        {
#warning TODO - unit tests
            Report(sb, nameof(BufferMaxSize), BufferMaxSize);
            Report(sb, nameof(BufferMaxSizeMB), BufferMaxSizeMB);
        }

        #endregion IConfigurationReporter implementation
    }
}