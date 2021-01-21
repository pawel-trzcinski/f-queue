using System.IO;
using FQueue.Models;

namespace FQueue.Data
{
    public abstract class DataProtocol : IDataProtocol
    {
        public const byte START_BYTE = 0x55;
        public const byte END_BYTE = 0x4B;

        public DataProtocolVersion Version { get; }

        // Protokół plikowy v1:
        // 1B - start - 55
        // 1B - wersja protokołu
        // 2B - ushort msg length = n
        // nB - msg - read to end
        // 4B - CRC32 of ("2B - ushort msg length = n" + "nB - msg - read to end")
        // 1B - end - 4B

        protected DataProtocol(DataProtocolVersion version)
        {
            Version = version;
        }

        /// <summary>
        /// Read QueueEqntry from DataFile stream. This method is thread-safe.
        /// </summary>
        public abstract QueueEntry ReadEntry(Stream inputStream);
#warning TODO - czytanie start i end byte; czytanie i sprawdzanie wersji protokołu; czytanie i sprawdzanie crc - przekazywanie do konkretnego protokołu dalej

        /// <summary>
        /// White QueueEqntry to DataFile stream. This method is thread-safe.
        /// </summary>
        public abstract void WriteEntry(QueueEntry entry, Stream outputStream);
    }
}