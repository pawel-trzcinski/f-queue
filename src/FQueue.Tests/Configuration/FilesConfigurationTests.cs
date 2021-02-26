using System;
using System.Linq;
using FQueue.Configuration;
using FQueue.Configuration.Validation;
using FQueue.FileSystem;
using Moq;
using NUnit.Framework;

namespace FQueue.Tests.Configuration
{
    [TestFixture]
    public class FilesConfigurationTests
    {
        public static FilesConfiguration CreateConfiguration
        (
            string backupFolder = "/backup",
            int dataFileMaximumSizeMB = 250
        )
        {
            return CreateConfiguration
            (
                new[] {"/data1", "/data2"},
                backupFolder,
                dataFileMaximumSizeMB
            );
        }

        private static FilesConfiguration CreateConfiguration
        (
            string[] dataFolders,
            string backupFolder = "/backup",
            int dataFileMaximumSizeMB = 250
        )
        {
            return new FilesConfiguration(dataFolders, backupFolder, dataFileMaximumSizeMB);
        }

        [Test]
        public void ValidationOk()
        {
            Mock<IDirectoryAbstraction> directoryAbstractionMock = new Mock<IDirectoryAbstraction>();
            directoryAbstractionMock.Setup(p => p.Exists(It.IsAny<string>())).Returns(true);

            Assert.DoesNotThrow(() => new FilesConfigurationValidator(directoryAbstractionMock.Object).Validate(CreateConfiguration()));
        }

        [TestCase(10)]
        [TestCase(20)]
        [TestCase(50)]
        [TestCase(100)]
        [TestCase(200)]
        [TestCase(256)]
        public void MBTransferedToBytes(int megabytes)
        {
            FilesConfiguration configuration = CreateConfiguration(dataFileMaximumSizeMB: megabytes);
            Assert.IsNotNull(configuration);
            Assert.AreEqual(1024 * 1024 * megabytes, configuration.DataFileMaximumSizeB);
        }

        [Test]
        public void EmptyDataFoldersOverriden()
        {
            FilesConfiguration configuration = CreateConfiguration(dataFolders: null);
            Assert.IsNotNull(configuration);
            Assert.AreEqual(FilesConfiguration.DEFAULT_DATA_FOLDER, configuration.DataFolders.Single());

            configuration = CreateConfiguration(dataFolders: new string[0]);
            Assert.IsNotNull(configuration);
            Assert.AreEqual(FilesConfiguration.DEFAULT_DATA_FOLDER, configuration.DataFolders.Single());

            configuration = CreateConfiguration(dataFolders: new[] {String.Empty, " ", null});
            Assert.IsNotNull(configuration);
            Assert.AreEqual(FilesConfiguration.DEFAULT_DATA_FOLDER, configuration.DataFolders.Single());
        }

        [Test]
        public void DataFoldersNormalized()
        {
            FilesConfiguration configuration = CreateConfiguration(dataFolders: new[] {"/data1/", "/data2/", "/data3"});
            Assert.IsNotNull(configuration);
            Assert.AreEqual("/data1", configuration.DataFolders.Skip(0).First());
            Assert.AreEqual("/data2", configuration.DataFolders.Skip(1).First());
            Assert.AreEqual("/data3", configuration.DataFolders.Skip(2).First());
        }

        [Test]
        public void EmptyBackupFolderOverriden()
        {
            FilesConfiguration configuration = CreateConfiguration(backupFolder: null);
            Assert.IsNotNull(configuration);
            Assert.AreEqual(FilesConfiguration.DEFAULT_BACKUP_FOLDER, configuration.BackupFolder);

            configuration = CreateConfiguration(backupFolder: String.Empty);
            Assert.IsNotNull(configuration);
            Assert.AreEqual(FilesConfiguration.DEFAULT_BACKUP_FOLDER, configuration.BackupFolder);

            configuration = CreateConfiguration(backupFolder: " ");
            Assert.IsNotNull(configuration);
            Assert.AreEqual(FilesConfiguration.DEFAULT_BACKUP_FOLDER, configuration.BackupFolder);
        }

        [Test]
        public void BackupFolderNormalized()
        {
            FilesConfiguration configuration = CreateConfiguration(backupFolder: "/data0/");
            Assert.IsNotNull(configuration);
            Assert.AreEqual("/data0", configuration.BackupFolder);
        }

        [Test]
        public void ValidationError_DataFolderInvalid1()
        {
            Mock<IDirectoryAbstraction> directoryAbstractionMock = new Mock<IDirectoryAbstraction>();
            directoryAbstractionMock.Setup(p => p.Exists(@"xx:\qq")).Returns(false);
            directoryAbstractionMock.Setup(p => p.Exists("/data2")).Returns(true);

            Assert.Throws<ArgumentException>(() => new FilesConfigurationValidator(directoryAbstractionMock.Object).Validate(CreateConfiguration(new[] {@"xx:\qq", "/data2"})));
        }

        [Test]
        public void ValidationError_DataFolderInvalid2()
        {
            Mock<IDirectoryAbstraction> directoryAbstractionMock = new Mock<IDirectoryAbstraction>();
            directoryAbstractionMock.Setup(p => p.Exists("/data1")).Returns(true);
            directoryAbstractionMock.Setup(p => p.Exists("%#&^@#DFH")).Returns(false);

            Assert.Throws<ArgumentException>(() => new FilesConfigurationValidator(directoryAbstractionMock.Object).Validate(CreateConfiguration(new[] {"/data1", "%#&^@#DFH"})));
        }

        [TestCase(0)]
        [TestCase(9)]
        [TestCase(257)]
        [TestCase(300)]
        [TestCase(512)]
        [TestCase(1024)]
        public void ValidationError_DataFileMaximumSize(int megabytes)
        {
            Mock<IDirectoryAbstraction> directoryAbstractionMock = new Mock<IDirectoryAbstraction>();
            directoryAbstractionMock.Setup(p => p.Exists(It.IsAny<string>())).Returns(true);

            Assert.Throws<ArgumentOutOfRangeException>(() => new FilesConfigurationValidator(directoryAbstractionMock.Object).Validate(CreateConfiguration(dataFileMaximumSizeMB: megabytes)));
        }
    }
}