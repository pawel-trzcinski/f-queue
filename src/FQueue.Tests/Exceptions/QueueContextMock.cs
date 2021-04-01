using System;
using FQueue.Context;

namespace FQueue.Tests.Exceptions
{
    public class QueueContextMock : QueueContext
    {
        private readonly string _toStringResult;

        public QueueContextMock(string toStringResult)
            : base(Guid.NewGuid().ToString())
        {
            _toStringResult = toStringResult;
        }

        public override string ToString()
        {
            return _toStringResult;
        }
    }
}