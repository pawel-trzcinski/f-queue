using System;
using System.Text;
using FQueue.Context;
using JetBrains.Annotations;

namespace FQueue.Exceptions
{
    [Serializable]
    public class InvalidCrcException : DataFrameException
    {
        public byte[] DataBytes { get; }

        public uint StoredCrc32 { get; }
        public uint CalculatedCrc32 { get; }

        public InvalidCrcException([NotNull] QueueContext context, byte[] dataBytes, uint storedCrc32, uint calculatedCrc32)
            : base(context, "Crc calculated from stream and saved in file are different")
        {
            DataBytes = dataBytes;
            StoredCrc32 = storedCrc32;
            CalculatedCrc32 = calculatedCrc32;
        }

        public override string ToString()
        {
#warning TODO - unit test
            StringBuilder sb = new StringBuilder(DataBytes.Length);

            sb.AppendLine(base.ToString());

            sb.AppendLine($"StoredCrc32: {StoredCrc32}");
            sb.AppendLine($"CalculatedCrc32: {CalculatedCrc32}");
            sb.AppendLine($"DataBytes: {Convert.ToBase64String(DataBytes)}");

            return sb.ToString();
        }
    }
}