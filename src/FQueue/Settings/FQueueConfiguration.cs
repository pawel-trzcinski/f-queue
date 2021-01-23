using System;
using System.Text;
using log4net;
using Newtonsoft.Json;

namespace FQueue.Settings
{
    [Serializable]
    public class FQueueConfiguration : ConfigurationReporter
    {
#warning TODO - unit tests
        private static readonly ILog _log = LogManager.GetLogger(typeof(FQueueConfiguration));

        public RestConfiguration Rest { get; }

        public FilesConfiguration Files { get; }

        public PerformanceConfiguration Performance { get; }


        [JsonConstructor]
        public FQueueConfiguration(RestConfiguration rest, FilesConfiguration files, PerformanceConfiguration performance)
            : base(nameof(FQueueConfiguration), String.Empty)
        {
            Rest = rest;
            Files = files;
            Performance = performance;

            ValidateConfiguration();
        }

        private void ValidateConfiguration()
        {
            _log.Debug($"Validating {nameof(FQueueConfiguration)}");
            _log.Info($"{nameof(FQueueConfiguration)} valid");
        }

        #region IConfigurationReporter implementation

        private void ReportConfiguration(StringBuilder sb)
        {
            Rest.ReportConfiguration(sb);
            Files.ReportConfiguration(sb);
            Performance.ReportConfiguration(sb);
        }

        #endregion IConfigurationReporter implementation

        public string ReportConfiguration()
        {
            StringBuilder sb = new StringBuilder();
            ReportConfiguration(sb);
            return sb.ToString();
        }
    }
}