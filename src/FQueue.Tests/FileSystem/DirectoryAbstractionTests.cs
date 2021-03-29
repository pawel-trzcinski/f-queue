using System;
using System.IO;
using FQueue.FileSystem;
using NUnit.Framework;

namespace FQueue.Tests.FileSystem
{
    [TestFixture]
    public class DirectoryAbstractionTests
    {
        private static string CreateTestDirectoryName()
        {
            // the point is that it does not exist
            return Path.Combine(TestContext.CurrentContext.WorkDirectory, Guid.NewGuid().ToString("N"));
        }

        [Test]
        public void Exists()
        {
            string folder = CreateTestDirectoryName();

            Directory.CreateDirectory(folder);

            Assert.IsTrue(new DirectoryAbstraction().Exists(folder));

            if (Directory.Exists(folder))
            {
                Directory.Delete(folder);
            }
        }

        [Test]
        public void DoesNotExist()
        {
            string folder = CreateTestDirectoryName();

            if (Directory.Exists(folder))
            {
                Directory.Delete(folder);
            }

            Assert.IsFalse(new DirectoryAbstraction().Exists(folder));
        }
    }
}