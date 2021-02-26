using System;
using System.IO;
using FQueue.FileSystem;
using log4net;

namespace FQueue.Configuration.Validation
{
    public class FilesConfigurationValidator : IFilesConfigurationValidator
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(PerformanceConfiguration));

        private readonly IDirectoryAbstraction _directoryAbstraction;

        public FilesConfigurationValidator(IDirectoryAbstraction directoryAbstraction)
        {
            _directoryAbstraction = directoryAbstraction;
        }

        public void Validate(FilesConfiguration configuration)
        {
            _log.Debug($"Validating {nameof(FilesConfiguration)}");

            if (configuration.DataFolders == null || configuration.DataFolders.Length == 0)
            {
                // this will never take place
                throw new ArgumentNullException(nameof(configuration.DataFolders), "Data folder are not defined");
            }

            foreach (string dataFolder in configuration.DataFolders)
            {
                string fullDataFolder = Path.GetFullPath(dataFolder);
                if (String.IsNullOrEmpty(fullDataFolder))
                {
                    throw new ArgumentException($"Data folder '{dataFolder}' is invalid", nameof(configuration.DataFolders));
                }

                if (!_directoryAbstraction.Exists(fullDataFolder))
                {
                    throw new ArgumentException($"Data folder '{dataFolder}' does not exist", nameof(configuration.DataFolders));
                }
            }

            string fullBackupFolder = Path.GetFullPath(configuration.BackupFolder);
            if (String.IsNullOrEmpty(fullBackupFolder))
            {
                throw new ArgumentException($"Backup folder '{configuration.BackupFolder}' is invalid", nameof(configuration.BackupFolder));
            }

            if (!_directoryAbstraction.Exists(fullBackupFolder))
            {
                throw new ArgumentException($"Backup folder '{fullBackupFolder}' does not exist", nameof(configuration.BackupFolder));
            }

            if (configuration.DataFileMaximumSizeMB < 10)
            {
                throw new ArgumentOutOfRangeException(nameof(configuration.DataFileMaximumSizeMB), $"Minimum value for {nameof(configuration.DataFileMaximumSizeMB)} is 10");
            }

            if (configuration.DataFileMaximumSizeMB > 256)
            {
                throw new ArgumentOutOfRangeException(nameof(configuration.DataFileMaximumSizeMB), $"Maximum value for {nameof(configuration.DataFileMaximumSizeMB)} is 256");
            }

            _log.Info($"{nameof(FilesConfiguration)} valid");
        }
    }
}