using System;
using System.Net;
using AutoFixture;
using FQueue.Logic;
using NUnit.Framework;

namespace FQueue.Tests.Logic
{
    [TestFixture]
    public class LogicResultTests
    {
        private class LogicResultTester : LogicResult
        {
            public LogicResultTester(HttpStatusCode status, bool shouldHaveData)
                : base(status, shouldHaveData)
            {
            }

            public override string DataToString()
            {
                return String.Empty;
            }
        }

        [Test]
        [Repeat(5)]
        public void PassedValuesAreCorrectlyAssigned()
        {
            Fixture fixture = new Fixture();
            HttpStatusCode status = fixture.Create<HttpStatusCode>();
            bool shouldHaveData = fixture.Create<bool>();

            LogicResultTester tester = new LogicResultTester(status, shouldHaveData);

            Assert.AreEqual(status, tester.Status);
            Assert.AreEqual(shouldHaveData, tester.ShouldHaveData);
            Assert.IsTrue(status == HttpStatusCode.OK || !tester.IsOk);
        }

        [Test]

        public void PassedOkIsOk()
        {
            LogicResultTester tester = new LogicResultTester(HttpStatusCode.OK, true);

            Assert.AreEqual(HttpStatusCode.OK, tester.Status);
            Assert.IsTrue(tester.IsOk);
        }
    }
}