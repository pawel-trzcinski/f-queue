using System.IO;
using FQueue.Exceptions;
using FQueue.Models;

namespace FQueue.Data
{
    public interface IDataProtocol
    {
        DataProtocolVersion Version { get; }

        /// <summary>
        /// Read QueueEqntry from DataFile stream. This method is thread-safe.
        /// </summary>
        /// <exception cref="InvalidCrcException">This exception is thrown if crc calculated from stream does nto equal crc written in the file</exception>
        /// <exception cref="UnsupportedProtocolVersionException">This exception is thrown if stream data indicate to not implemented protocol</exception>
        /// <exception cref="InvalidBoundryByte">This exception is thrown if values of start or end byte have wrong values. This might indicate that while reading, application poiints to wrong place in the stream</exception>
        QueueEntry ReadEntry(Stream inputStream);

        /// <summary>
        /// White QueueEqntry to DataFile stream. This method is thread-safe.
        /// </summary>
        void WriteEntry(QueueEntry entry, Stream outputStream);
    }
}//