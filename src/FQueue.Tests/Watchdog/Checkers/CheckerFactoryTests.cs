using System;
using FQueue.Watchdog.Checkers;
using NUnit.Framework;
using SimpleInjector;

namespace FQueue.Tests.Watchdog.Checkers
{
    [TestFixture]
    public class CheckerFactoryTests
    {
        private interface ICheckerTester : IChecker
        {
        }

        private class Checker1Tester : IChecker
        {
            public string Name { get; } = Guid.NewGuid().ToString();

            public bool Check()
            {
                return true;
            }
        }

        private class Checker2Tester : IChecker
        {
            public string Name { get; } = Guid.NewGuid().ToString();

            public bool Check()
            {
                return true;
            }
        }

        private class Checker3Tester : ICheckerTester
        {
            public string Name { get; } = Guid.NewGuid().ToString();

            public bool Check()
            {
                return true;
            }
        }

        [Test]
        public void CreateReturnsWantedImplementation()
        {
            Checker1Tester tester1 = new Checker1Tester();
            Checker2Tester tester2 = new Checker2Tester();
            Checker3Tester tester3 = new Checker3Tester();

            Container container = new Container();
            container.Register<Checker1Tester>(() => tester1);
            container.Register<Checker2Tester>(() => tester2);
            container.Register<ICheckerTester>(() => tester3);

            CheckerFactory factory = new CheckerFactory(container);
            Assert.AreEqual(tester1.Name, factory.CreateChecker<Checker1Tester>().Name);
            Assert.AreEqual(tester2.Name, factory.CreateChecker<Checker2Tester>().Name);
            Assert.AreEqual(tester3.Name, factory.CreateChecker<ICheckerTester>().Name);
        }
    }
}