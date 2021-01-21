using System;
using FQueue.Context;

namespace FQueue.Exceptions
{
    [Serializable]
    public class InvalidBoundryByte : FQueueException
    {
        public byte BoundryByteExpected { get; }
        public byte BoundryByteActual { get; }

        public InvalidBoundryByte(QueueContext context, byte boundryByteExpected, byte boundryByteActual)
            :base(context, $"Invalid boundry byte found. Expected: {boundryByteExpected} Actual: {boundryByteActual}")
        {
            BoundryByteExpected = boundryByteExpected;
            BoundryByteActual = boundryByteActual;
        }
    }
}