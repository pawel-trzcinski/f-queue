using System;
using System.Text;
using FQueue.Context;
using JetBrains.Annotations;

namespace FQueue.Exceptions
{
    [Serializable]
    public class TooBigRequestException : DataFrameException
    {
        public int DataFileMaximumSizeB { get; }
        public int RequestBytesCount { get; }
        public int FrameSize { get; }

        public TooBigRequestException([NotNull] QueueContext context, int dataFileMaximumSizeB, int requestBytesCount, int frameSize)
            : base(context, "Maximum size of frame exceeded")
        {
            DataFileMaximumSizeB = dataFileMaximumSizeB;
            RequestBytesCount = requestBytesCount;
            FrameSize = frameSize;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(base.ToString());

            sb.AppendLine($"DataFileMaximumSizeB: {DataFileMaximumSizeB}");
            sb.AppendLine($"RequestBytesCount: {RequestBytesCount}");
            sb.AppendLine($"FrameSize: {FrameSize}");

            return sb.ToString();
        }
    }
}