using FQueueSynchronizer.Etcd;
using NUnit.Framework;

namespace FQueue.Tests.Etcd
{
    [TestFixture]
    public class EtcdCompoundClientFactoryTests
    {
        [TestCase("http", "")]
        [TestCase("http", "/whatever")]
        [TestCase("https", "")]
        [TestCase("https", "/whatever")]
        public void Create(string prefix, string postfix)
        {
            EtcdCompoundClientFactory factory = new EtcdCompoundClientFactory();
            IEtcdCompoundClient client = factory.CreateClient($"{prefix}://127.0.0.1{postfix}");

            Assert.IsNotNull(client);
        }
    }
}