using FQueue.Configuration;
using NUnit.Framework;

namespace FQueue.Tests
{
    [TestFixture]
    public class CommandLineArgumentsTests
    {
        private class CommandLineArgumentsTester : CommandLineArguments
        {
            public void SetConfigurationUri(string configurationUri)
            {
                ConfigurationUri = configurationUri;
            }
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("ftp://lala")]
        [TestCase("/lala")]
        [TestCase(@"c:\pawel.txt")]
        public void ValidationThrowsOnBadUri(string uri)
        {
            CommandLineArgumentsTester tester = new CommandLineArgumentsTester();
            tester.SetConfigurationUri(uri);
            Assert.Catch(() => tester.Validate());
        }

        [TestCase("http://wp.pl")]
        [TestCase("http://127.0.0.1:81/papa")]
        [TestCase("https://mbank.pl/qwerty")]
        [TestCase("https://onet.pl:50666")]
        [TestCase("http://wp.pl/")]
        [TestCase("http://127.0.0.1:81/papa/")]
        [TestCase("https://mbank.pl/qwerty/")]
        [TestCase("https://onet.pl:50666/")]
        public void ValidationDoesNotThrowOnGoodUri(string uri)
        {
            CommandLineArgumentsTester tester = new CommandLineArgumentsTester();
            tester.SetConfigurationUri(uri);
            Assert.DoesNotThrow(() => tester.Validate());
        }
    }
}