using System;
using System.IO;
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

        public const string DEFAULT_DATA_FOLDER = "/fqueue/data";
        public const string DEFAULT_BACKUP_FOLDER = "/fqueue/backup";

        public RestConfiguration Rest { get; }

        public string[] DataFolders { get; }
        public string BackupFolder { get; }

        public PerformanceConfiguration Performance { get; }
    

        [JsonConstructor]
        public FQueueConfiguration(RestConfiguration rest, string[] dataFolders, string backupFolder, PerformanceConfiguration performance)
            : base(nameof(FQueueConfiguration), String.Empty)
        {
            Rest = rest;
            DataFolders = dataFolders == null || dataFolders.Length == 0 ? new[] {DEFAULT_DATA_FOLDER} : dataFolders;
            BackupFolder = (backupFolder ?? DEFAULT_BACKUP_FOLDER).Trim('/');
            Performance = performance;

            ValidateConfiguration();
        }

        private void ValidateConfiguration()
        {
            _log.Debug($"Validating {nameof(FQueueConfiguration)}");

            foreach (string dataFolder in DataFolders)
            {
                string fullDataFolder = Path.GetFullPath(dataFolder);
                if (String.IsNullOrEmpty(fullDataFolder))
                {
                    throw new ArgumentException($"data folder '{dataFolder}' is invalid", nameof(DataFolders));
                }
            }

            string fullBackupFolder = Path.GetFullPath(BackupFolder);
            if (String.IsNullOrEmpty(fullBackupFolder))
            {
                throw new ArgumentException($"backup folder '{BackupFolder}' is invalid", nameof(BackupFolder));
            }

            _log.Info($"{nameof(FQueueConfiguration)} valid");
        }

        #region IConfigurationReporter implementation

        private void ReportConfiguration(StringBuilder sb)
        {
#warning TODO - unit tests
            Report(sb, nameof(DataFolders), DataFolders);
            Report(sb, nameof(BackupFolder), BackupFolder);

            Rest.ReportConfiguration(sb);
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