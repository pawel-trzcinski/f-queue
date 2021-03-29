using System;
using System.Linq;
using System.Text;
using AutoFixture;
using FQueue.Context;
using FQueue.Exceptions;
using FQueue.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace FQueue.Tests.Models
{
    [TestFixture]
    public class QueueEntryTests
    {
        [Serializable]
        private class TestEntry
        {
            public int Prop1 { get; set; }
            public string Prop2 { get; set; }
            public TestInnerClass Obj1 { get; set; }

            public static TestEntry Create()
            {
                Fixture fixture = new Fixture();
                return new TestEntry
                {
                    Prop1 = fixture.Create<int>(),
                    Prop2 = fixture.Create<string>(),
                    Obj1 = TestInnerClass.Create()
                };
            }
        }

        [Serializable]
        private class TestEntryFull : TestEntry
        {
            public string Tag { get; set; }

            public new static TestEntryFull Create()
            {
                Fixture fixture = new Fixture();
                return new TestEntryFull
                {
                    Tag = fixture.Create<string>(),
                    Prop1 = fixture.Create<int>(),
                    Prop2 = fixture.Create<string>(),
                    Obj1 = TestInnerClass.Create()
                };
            }

            public bool Equals(TestEntryFull other)
            {
                return
                    Tag == other.Tag
                    &&
                    Prop1 == other.Prop1
                    &&
                    Prop2 == other.Prop2
                    &&
                    Obj1.Equals(other.Obj1);
            }
        }

        [Serializable]
        private class TestInnerClass
        {
            public Guid Prop3 { get; set; }
            public string[] Arr1 { get; set; }

            public static TestInnerClass Create()
            {
                Fixture fixture = new Fixture();
                return new TestInnerClass
                {
                    Prop3 = fixture.Create<Guid>(),
                    Arr1 = Enumerable.Repeat(0, new Random().Next(25)).Select(p => fixture.Create<string>()).ToArray()
                };
            }

            public bool Equals(TestInnerClass other)
            {
                return
                    Prop3.Equals(other.Prop3)
                    &&
                    Arr1.SequenceEqual(other.Arr1);
            }
        }

        [Test]
        public void CreateJsonFromBytes()
        {
            TestEntryFull originalEntry = TestEntryFull.Create();

            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(originalEntry));

            QueueEntry queueEntry = QueueEntry.FromBytes(bytes);
            TestEntryFull entryFromBytes = JsonConvert.DeserializeObject<TestEntryFull>(queueEntry.Data);

            Assert.AreEqual(originalEntry.Tag, queueEntry.Tag);
            Assert.IsTrue(originalEntry.Equals(entryFromBytes));
        }

        [Test]
        public void CreateJsonFromRequestString()
        {
            TestEntryFull originalEntry = TestEntryFull.Create();

            string requestString = JsonConvert.SerializeObject(originalEntry);

            QueueEntry requestEntry = QueueEntry.FromRequestString(new QueueContext("x"), requestString, Int32.MaxValue);

            Assert.AreEqual(originalEntry.Tag, requestEntry.Tag);
            Assert.IsTrue(originalEntry.Equals(JsonConvert.DeserializeObject<TestEntryFull>(requestEntry.Data)));
        }

        [Test]
        public void FromBytesThrowsOnNoToken()
        {
            TestEntry entryWithNoTag = TestEntry.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(entryWithNoTag));

            Assert.Throws<ArgumentException>(() => QueueEntry.FromBytes(bytes));
        }

        [Test]
        public void FromStringThrowsOnNoToken()
        {
            TestEntry entryWithNoTag = TestEntry.Create();
            string requestString = JsonConvert.SerializeObject(entryWithNoTag);

            Assert.Throws<ArgumentException>(() => QueueEntry.FromRequestString(new QueueContext("x"), requestString, Int32.MaxValue));
        }

        [Test]
        public void FromStringThrowsOnTooBigFrame()
        {
            TestEntryFull entry = TestEntryFull.Create();
            string requestString = JsonConvert.SerializeObject(entry);
            byte[] bytes = Encoding.UTF8.GetBytes(requestString);

            Assert.Throws<TooBigRequestException>(() => QueueEntry.FromRequestString(new QueueContext("x"), requestString, bytes.Length - 1));
        }
    }
}