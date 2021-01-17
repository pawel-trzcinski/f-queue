using System.Text;
using log4net;
using Newtonsoft.Json;

namespace FQueue.Settings
{
    [JsonObject(nameof(FQueueConfiguration.Rest))]
    public class RestConfiguration : ConfigurationReporter
    {
#warning TODO - unit tests
        private static readonly ILog _log = LogManager.GetLogger(typeof(RestConfiguration));

        // ReSharper disable once MemberInitializerValueIgnored
        public ushort HostingPort { get; } = 7081; // ASCII 70 - F  81 - Q

        public ThrottlingConfiguration Throttling { get; }

        [JsonConstructor]
        public RestConfiguration(ushort hostingPort, string hostingUri, ThrottlingConfiguration throttling)
            : base(nameof(RestConfiguration), INDENT)
        {
            HostingPort = hostingPort;
            Throttling = throttling;

            ValidateConfiguration();
        }

        private void ValidateConfiguration()
        {
            _log.Debug($"Validating {nameof(RestConfiguration)}");
            _log.Info($"{nameof(RestConfiguration)} valid");
        }

        #region IConfigurationReporter implementation

        public void ReportConfiguration(StringBuilder sb)
        {
            Report(sb, nameof(HostingPort), HostingPort);

            Throttling.ReportConfiguration(sb);
        }

        #endregion IConfigurationReporter implementation
    }
}