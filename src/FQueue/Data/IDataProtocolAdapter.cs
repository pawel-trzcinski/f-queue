using System.IO;
using FQueue.Context;
using FQueue.Exceptions;
using FQueue.Models;

namespace FQueue.Data
{
    public interface IDataProtocolAdapter
    {
        /// <summary>
        /// Read QueueEqntry from DataFile frame stream. This method is thread-safe.
        /// </summary>
        /// <exception cref="InvalidCrcException">This exception is thrown if crc calculated from stream does nto equal crc written in the file</exception>
        /// <exception cref="UnsupportedProtocolVersionException">This exception is thrown if stream data indicate to not implemented protocol</exception>
        /// <exception cref="InvalidBoundryByteException">This exception is thrown if values of start or end byte have wrong values. This might indicate that while reading, application poiints to wrong place in the stream</exception>
        /// <exception cref="DataFrameException">This exception is thrown if frame is not up to specification</exception>
        QueueEntry ReadEntry(QueueContext context, Stream inputStream);

        /// <summary>
        /// White QueueEqntry to DataFile frame stream. This method is thread-safe.
        /// </summary>
        void WriteEntry(QueueEntry entry, Stream outputStream);
    }
}