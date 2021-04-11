using System.Globalization;
using AutoFixture;
using FQueue.Logic;
using NUnit.Framework;

namespace FQueue.Tests.Logic
{
    [TestFixture]
    public class CountResultTests
    {
        [Test]
        [Repeat(5)]
        public void DataToStringIsCorrect()
        {
            Fixture fixture = new Fixture();
            int count = fixture.Create<int>();

            CountResult result = new CountResult(count);

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual(count.ToString(CultureInfo.InvariantCulture), result.DataToString());
        }
    }
}