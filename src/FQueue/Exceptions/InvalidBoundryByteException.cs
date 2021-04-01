using System;
using System.Text;
using FQueue.Context;

namespace FQueue.Exceptions
{
    [Serializable]
    public class InvalidBoundryByteException : DataFrameException
    {
        public byte BoundryByteExpected { get; }
        public byte BoundryByteActual { get; }

        public InvalidBoundryByteException(QueueContext context, byte boundryByteExpected, byte boundryByteActual)
            : base(context, $"Invalid boundry byte found")
        {
            BoundryByteExpected = boundryByteExpected;
            BoundryByteActual = boundryByteActual;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(base.ToString());

            sb.AppendLine($"BoundryByteExpected: {BoundryByteExpected}");
            sb.AppendLine($"BoundryByteActual: {BoundryByteActual}");

            return sb.ToString();
        }
    }
}