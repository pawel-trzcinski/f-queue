using System;
using System.Linq;
using FQueue.Context;
using FQueue.Logic;
using FQueue.Models;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace FQueue.Tests.Logic
{
    [TestFixture]
    public class DequeueResultTests
    {
        [Test]
        public void NullArrayReturnsBrackets()
        {
            DequeueResult result = new DequeueResult(null);

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual("[]", result.DataToString());
        }

        [Test]
        public void EmptyArrayReturnsBrackets()
        {
            DequeueResult result = new DequeueResult(new QueueEntry[0]);

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual("[]", result.DataToString());
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        public void EntriesJoinedInJsonArray(int entriesCount)
        {
            Guid[] entriesIdentifiers = Enumerable.Repeat(0, entriesCount).Select(p => Guid.NewGuid()).ToArray();
            string[] entries = entriesIdentifiers.Select(p => $"{{\"Tag\":\"tag\", \"D\":\"{p}\"}}").ToArray();

            DequeueResult result = new DequeueResult(entries.Select(p => QueueEntry.FromRequestString(new QueueContext("WHATEVER"), p, 1000)).ToArray());

            Assert.IsTrue(result.IsOk);

            string resultString = result.DataToString();

            Assert.AreEqual('[', resultString.First());
            Assert.AreEqual(']', resultString.Last());

            Assert.IsTrue(entriesIdentifiers.SequenceEqual(JArray.Parse(resultString).Select(p => Guid.Parse(p["D"].ToString()))));
        }
    }
}