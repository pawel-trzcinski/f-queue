using System;
using System.IO;

namespace FQueue.Context
{
    /// <summary>
    /// Object that is used to locally lock execution and keep information on queue. This is a singleton.
    /// </summary>
    [Serializable]
    public class QueueContext
    {
        public string QueueName { get; }

        public QueueContext(string queueName)
        {
            QueueName = queueName;
        }

        public override string ToString()
        {
            return $"QueueName: {QueueName}";
        }

        public string GetDefaultBackupFile(string backupFolder)
        {
            return Path.Combine(backupFolder, $"{QueueName}.bak");
        }
    }
}