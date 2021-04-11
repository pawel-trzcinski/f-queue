using System;
using FQueue.Context;
using NUnit.Framework;

namespace FQueue.Tests.Context
{
    [TestFixture]
    public class QueueContextTests
    {
        [Test]
        public void BackupFileNameCreated()
        {
            string queueName = Guid.NewGuid().ToString();
            string backupFolder = $"/{Guid.NewGuid()}";

            QueueContext context = new QueueContext(queueName);

            string backupFile = context.GetDefaultBackupFile(backupFolder);

            Assert.IsTrue(backupFile.Contains(queueName));
            Assert.IsTrue(backupFile.Contains(backupFolder));
        }
    }
}