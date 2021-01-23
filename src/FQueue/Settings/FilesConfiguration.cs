using System;
using System.IO;
using System.Text;
using log4net;
using Newtonsoft.Json;

namespace FQueue.Settings
{
    [JsonObject(nameof(FQueueConfiguration.Files))]
    public class FilesConfiguration : ConfigurationReporter
    {
#warning TODO - unit tests
        private static readonly ILog _log = LogManager.GetLogger(typeof(PerformanceConfiguration));

        public const string DEFAULT_DATA_FOLDER = "/fqueue/data";
        public const string DEFAULT_BACKUP_FOLDER = "/fqueue/backup";

        public string[] DataFolders { get; }
        public string BackupFolder { get; }

        // ReSharper disable once InconsistentNaming
        public int DataFileMaximumSizeMB { get; }
        public int DataFileMaximumSizeB { get; }

        [JsonConstructor]
        // ReSharper disable once InconsistentNaming
        public FilesConfiguration(string[] dataFolders, string backupFolder, int dataFileMaximumSizeMB)
            : base(nameof(PerformanceConfiguration), INDENT)
        {
            DataFolders = dataFolders == null || dataFolders.Length == 0 ? new[] { DEFAULT_DATA_FOLDER } : dataFolders;
            BackupFolder = (backupFolder ?? DEFAULT_BACKUP_FOLDER).Trim('/');
            DataFileMaximumSizeMB = dataFileMaximumSizeMB;
            DataFileMaximumSizeB = dataFileMaximumSizeMB * 1024 * 1024;

            ValidateConfiguration();
        }

        private void ValidateConfiguration()
        {
            _log.Debug($"Validating {nameof(FilesConfiguration)}");

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

            if(DataFileMaximumSizeMB < 10)
            {
                throw new ArgumentOutOfRangeException(nameof(DataFileMaximumSizeMB), $"Minimum value for {nameof(DataFileMaximumSizeMB)} is 10");
            }

            if (DataFileMaximumSizeMB > 256)
            {
                throw new ArgumentOutOfRangeException(nameof(DataFileMaximumSizeMB), $"Maximum value for {nameof(DataFileMaximumSizeMB)} is 256");
            }

            _log.Info($"{nameof(FilesConfiguration)} valid");
        }

        #region IConfigurationReporter implementation

        public void ReportConfiguration(StringBuilder sb)
        {
#warning TODO - unit tests
            Report(sb, nameof(DataFolders), DataFolders);
            Report(sb, nameof(BackupFolder), BackupFolder);
            Report(sb, nameof(DataFileMaximumSizeMB), DataFileMaximumSizeMB);
        }

        #endregion IConfigurationReporter implementation
    }
}