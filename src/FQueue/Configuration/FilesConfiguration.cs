using System;
using System.Linq;
using Newtonsoft.Json;

namespace FQueue.Configuration
{
    [JsonObject(nameof(FQueueConfiguration.Files))]
    public class FilesConfiguration
    {
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
        {
            string[] dataFoldersFilteres = dataFolders == null ? new string[0] : dataFolders.Where(p => !String.IsNullOrWhiteSpace(p)).Select(p => p.TrimEnd('/')).ToArray();

            DataFolders = (dataFoldersFilteres.Length == 0 ? new[] {DEFAULT_DATA_FOLDER} : dataFoldersFilteres).Select(p => p.TrimEnd('/')).ToArray();
            BackupFolder = (String.IsNullOrWhiteSpace(backupFolder) ? DEFAULT_BACKUP_FOLDER : backupFolder).TrimEnd('/');
            DataFileMaximumSizeMB = dataFileMaximumSizeMB;
            DataFileMaximumSizeB = dataFileMaximumSizeMB * 1024 * 1024;
        }
    }
}