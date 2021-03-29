using System;
using System.IO;
using System.Text;
using FQueue.FileSystem;
using NUnit.Framework;

namespace FQueue.Tests.FileSystem
{
    [TestFixture]
    public class FileAbstractionTests
    {
        private static string CreateTestFilename()
        {
            return Path.Combine(TestContext.CurrentContext.WorkDirectory, Guid.NewGuid().ToString("N"));
        }

        [Test]
        public void ReadAllText()
        {
            string testString = Guid.NewGuid().ToString();
            string filename = CreateTestFilename();
            File.WriteAllText(filename, testString, Encoding.UTF8);

            Assert.AreEqual(testString, new FileAbstraction().ReadAllText(filename));

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        [Test]
        public void WriteAllText()
        {
            string testString = Guid.NewGuid().ToString();
            string filename = CreateTestFilename();
            new FileAbstraction().WriteAllText(filename, testString);

            Assert.AreEqual(testString, File.ReadAllText(filename, Encoding.UTF8));

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }
    }
}