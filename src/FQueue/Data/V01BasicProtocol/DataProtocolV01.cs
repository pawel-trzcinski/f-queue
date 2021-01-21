using System.IO;
using FQueue.Models;

namespace FQueue.Data.V01BasicProtocol
{
    public class DataProtocolV01 : DataProtocol
    {
#warning TODO
#warning TODO - unit tests

#warning TODO - to też stestować
        public DataProtocolV01()
            : base(DataProtocolVersion.V01BasicProtocol)
        {
        }

        public override QueueEntry ReadEntry(Stream inputStream)
        {
            long streamStartPosition = inputStream.Position;

            // throws InvalidCrcException
            // throws UnsupportedProtocolVersionException
            
#warning TODO
            return null;
        }

        public override void WriteEntry(QueueEntry entry, Stream outputStream)
        {
#warning TODO
            // Protokół plikowy:
            // 1B - start - 55
            // 1B - wersja protokołu
            // 2B - ushort msg length = n
            // nB - msg - read to end
            // 4B - CRC32 of msg
            // 1B - end - 4B
        }


    }
}