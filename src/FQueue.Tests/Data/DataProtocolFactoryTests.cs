using System;
using System.Linq;
using FQueue.Data;
using Moq;
using NUnit.Framework;
using SimpleInjector;

namespace FQueue.Tests.Data
{
    [TestFixture]
    public class DataProtocolFactoryTests
    {
        [Test]
        public void CreatesProtocolOfGivenVersion()
        {
            DataProtocolVersion[] versions = Enum.GetValues(typeof(DataProtocolVersion)).Cast<DataProtocolVersion>().Where(p => p != DataProtocolVersion.None).ToArray();

            Container container = new Container();
            container.Collection.Register(versions.Select(p =>
            {
                Mock<IDataProtocol> dp = new Mock<IDataProtocol>();
                dp.Setup(k => k.Version).Returns(p);
                return dp.Object;
            }));

            DataProtocolFactory factory = new DataProtocolFactory(container);

            foreach (DataProtocolVersion version in versions)
            {
                Assert.AreEqual(version, factory.GetProtocol(version).Version);
            }
        }

        [Test]
        public void UnregisteredVersionThrows()
        {
            DataProtocolVersion[] versions = Enum.GetValues(typeof(DataProtocolVersion)).Cast<DataProtocolVersion>().Where(p => p != DataProtocolVersion.None).ToArray();

            Container container = new Container();
            container.Collection.Register(versions.Select(p =>
            {
                Mock<IDataProtocol> dp = new Mock<IDataProtocol>();
                dp.Setup(k => k.Version).Returns(p);
                return dp.Object;
            }));

            DataProtocolFactory factory = new DataProtocolFactory(container);

            Assert.Throws<InvalidOperationException>(() => factory.GetProtocol(DataProtocolVersion.None));
        }
    }
}