using Newtonsoft.Json;
using System;
using System.Text;
using log4net;

namespace FQueue.Settings
{
    [JsonObject(nameof(RestConfiguration.Throttling))]
    public class ThrottlingConfiguration : ConfigurationReporter
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ThrottlingConfiguration));

        public int ConcurrentRequestsLimit { get; }
        public int QueueLimit { get; }
        public TimeSpan QueueTimeout { get; }
        public long MaximumServerConnections { get; }

        [JsonConstructor]
        public ThrottlingConfiguration(int concurrentRequestsLimit, int queueLimit, int queueTimeoutS, int maximumServerConnections)
            : base(nameof(ThrottlingConfiguration), INDENT + INDENT)
        {
            ConcurrentRequestsLimit = concurrentRequestsLimit;
            QueueLimit = queueLimit;
            QueueTimeout = TimeSpan.FromSeconds(queueTimeoutS);
            MaximumServerConnections = maximumServerConnections;

            ValidateConfiguration();
        }

        private void ValidateConfiguration()
        {
            _log.Debug($"Validating {nameof(ThrottlingConfiguration)}");

            if (ConcurrentRequestsLimit < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(ConcurrentRequestsLimit), $"{nameof(ConcurrentRequestsLimit)} has to be greater than 0");
            }

            if (QueueLimit < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(QueueLimit), $"{nameof(QueueLimit)} has to be greater or equal to 0");
            }

            if (QueueTimeout.TotalSeconds < 5)
            {
                throw new ArgumentOutOfRangeException(nameof(QueueTimeout), $"{nameof(QueueTimeout)} has to be greater or equal to 5");
            }

            if (MaximumServerConnections < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(MaximumServerConnections), $"{nameof(MaximumServerConnections)} has to be greater than 0");
            }

            if (MaximumServerConnections < ConcurrentRequestsLimit + QueueLimit)
            {
                throw new ArgumentOutOfRangeException(nameof(MaximumServerConnections), $"{nameof(MaximumServerConnections)} has to be greater or equal to the sum of {nameof(ConcurrentRequestsLimit)} and {nameof(QueueLimit)}");
            }

            _log.Info($"{nameof(ThrottlingConfiguration)} valid");
        }

        #region IConfigurationReporter implementation

        public void ReportConfiguration(StringBuilder sb)
        {
            Report(sb, nameof(ConcurrentRequestsLimit), ConcurrentRequestsLimit);
            Report(sb, nameof(QueueLimit), QueueLimit);
            Report(sb, nameof(QueueTimeout), QueueTimeout);
            Report(sb, nameof(MaximumServerConnections), MaximumServerConnections);
        }

        #endregion IConfigurationReporter implementation
    }
}