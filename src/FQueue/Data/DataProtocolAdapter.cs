using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Force.Crc32;
using FQueue.Context;
using FQueue.Exceptions;
using FQueue.Models;
using log4net;

namespace FQueue.Data
{
    public class DataProtocolAdapter : IDataProtocolAdapter
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(DataProtocolAdapter));

        // DataFile frame
        // 1B - start - 55
        // 1B - protocol version
        // 4B - int msg length = n - it's int (and not uint) on purpose - byte arrays have length of type int, so this simplifies implementation
        // nB - msg
        // 4B - CRC32 of msg
        // 1B - end - 4B
        private const int FRAME_SIZE_WITHOUT_MESSAGE = 1 + 1 + 4 + 4 + 1;

#warning TODO - unit tests
        public const int START_BYTE = 0x55;
        public const int END_BYTE = 0x4B;

        private readonly IDataProtocolFactory _dataProtocolFactory;

        private static readonly HashSet<int> _supportedProtocols = new HashSet<int>(GetSupportedProtocols());
        private static readonly DataProtocolVersion _dataProtocolVersion = GetNewestProtocolVersion();

        #region init

        public DataProtocolAdapter(IDataProtocolFactory dataProtocolFactory)
        {
            _dataProtocolFactory = dataProtocolFactory;
        }

        private static IEnumerable<int> GetSupportedProtocols()
        {
            return from DataProtocolVersion dataProtocolVersion in Enum.GetValues(typeof(DataProtocolVersion)) select (int) dataProtocolVersion;
        }

        private static DataProtocolVersion GetNewestProtocolVersion()
        {
            var version = (DataProtocolVersion) (from DataProtocolVersion dataProtocolVersion in Enum.GetValues(typeof(DataProtocolVersion)) select (int) dataProtocolVersion).Max();
            _log.Info($"Newest DataProtocolVersion used to write: {version}");
            return version;
        }

        #endregion init

        public static int CalculateFrameSize(int messageSize)
        {
            return FRAME_SIZE_WITHOUT_MESSAGE + messageSize;
        }

        #region ReadEntry

        /// <summary>
        /// Read QueueEqntry from DataFile frame stream. This method is thread-safe.
        /// </summary>
        /// <exception cref="InvalidCrcException">This exception is thrown if crc calculated from stream does nto equal crc written in the file</exception>
        /// <exception cref="UnsupportedProtocolVersionException">This exception is thrown if stream data indicate to not implemented protocol</exception>
        /// <exception cref="InvalidBoundryByteException">This exception is thrown if values of start or end byte have wrong values. This might indicate that while reading, application poiints to wrong place in the stream</exception>
        /// <exception cref="DataFrameException">This exception is thrown if frame is not up to specification</exception>
        public QueueEntry ReadEntry(QueueContext context, Stream inputStream)
        {
#warning TEST
            if (!ReadStartByte(context, inputStream))
            {
                return null;
            }

            DataProtocolVersion dataProtocolVersion = ReadDataProtocolVersion(context, inputStream);
            int messageLength = ReadMessageLength(context, inputStream);
            byte[] messageBuffer = ReadMessageBuffer(context, inputStream, messageLength);
            uint crc = ReadCrc(context, inputStream);
            CheckCrc(context, messageBuffer, crc);
            ReadEndByte(context, inputStream);

            return _dataProtocolFactory.GetProtocol(dataProtocolVersion).GetEntry(messageBuffer);
        }

        private static bool ReadStartByte(QueueContext context, Stream inputStream)
        {
            int startByte = inputStream.ReadByte();
            if (startByte == -1)
            {
                return false;
            }

            if (startByte != START_BYTE)
            {
                throw new InvalidBoundryByteException(context, START_BYTE, (byte) startByte);
            }

            return true;
        }

        private static DataProtocolVersion ReadDataProtocolVersion(QueueContext context, Stream inputStream)
        {
            int protocolVersion = inputStream.ReadByte();
            if (protocolVersion == -1)
            {
                throw new DataFrameException(context, "Unable to read protocol version byte");
            }

            if (!_supportedProtocols.Contains(protocolVersion))
            {
                throw new UnsupportedProtocolVersionException(context, (byte) protocolVersion);
            }

            return (DataProtocolVersion) protocolVersion;
        }

        private static int ReadMessageLength(QueueContext context, Stream inputStream)
        {
            const int BYTES_TO_READ = 4;
            byte[] messageLengthBuffer = new byte[BYTES_TO_READ];
            int readCount = inputStream.Read(messageLengthBuffer, 0, BYTES_TO_READ);
            if (readCount != BYTES_TO_READ)
            {
                throw new DataFrameException(context, $"Unable to read {BYTES_TO_READ} bytes of message length");
            }

            int messageLength = BitConverter.ToInt32(messageLengthBuffer, 0);

            if (messageLength == 0)
            {
                throw new DataFrameException(context, "Message length in frame is 0");
            }

            return messageLength;
        }

        private static byte[] ReadMessageBuffer(QueueContext context, Stream inputStream, int messageLength)
        {
            byte[] messageBuffer = new byte[messageLength];
            int readCount = inputStream.Read(messageBuffer, 0, messageLength);
            if (readCount != messageLength)
            {
                throw new DataFrameException(context, $"Unable to read {messageLength} bytes of message");
            }

            return messageBuffer;
        }

        private static uint ReadCrc(QueueContext context, Stream inputStream)
        {
            byte[] crcBuffer = new byte[4];
            int readCount = inputStream.Read(crcBuffer, 0, 4);
            if (readCount != 4)
            {
                throw new DataFrameException(context, "Unable to read 4 bytes of crc");
            }

            return BitConverter.ToUInt32(crcBuffer, 0);
        }

        private static void CheckCrc(QueueContext context, byte[] messageBuffer, uint crc)
        {
            uint crcCalculated = Crc32Algorithm.Compute(messageBuffer);

            if (crcCalculated != crc)
            {
                throw new InvalidCrcException(context, messageBuffer, crc, crcCalculated);
            }
        }

        private static void ReadEndByte(QueueContext context, Stream inputStream)
        {
            int endByte = inputStream.ReadByte();
            if (endByte == -1)
            {
                throw new DataFrameException(context, "Unable to read end byte");
            }

            if (endByte != END_BYTE)
            {
                throw new InvalidBoundryByteException(context, END_BYTE, (byte) endByte);
            }
        }

        #endregion ReadEntry

        #region WriteEntry

        /// <summary>
        /// White QueueEqntry to DataFile frame stream. This method is thread-safe.
        /// </summary>
        public void WriteEntry(QueueEntry entry, Stream outputStream)
        {
#warning TEST
            WriteStartByte(outputStream);
            WriteDataProtocolVersion(outputStream);
            WriteMessageLength(outputStream, entry.DataBytes.Length);
            WriteMessage(outputStream, entry.DataBytes);
            WriteCrc(outputStream, entry.DataBytes);
            WriteEndByte(outputStream);
        }

        private static void WriteStartByte(Stream outputStream)
        {
            outputStream.WriteByte(START_BYTE);
        }

        private static void WriteDataProtocolVersion(Stream outputStream)
        {
            outputStream.WriteByte((byte) _dataProtocolVersion);
        }

        private static void WriteMessageLength(Stream outputStream, int messageLength)
        {
            const int BYTES_TO_WRITE = 4;
            byte[] messageLengthBuffer = BitConverter.GetBytes(messageLength);
            outputStream.Write(messageLengthBuffer, 0, BYTES_TO_WRITE);
        }

        private static void WriteMessage(Stream outputStream, byte[] message)
        {
            outputStream.Write(message, 0, message.Length);
        }

        private static void WriteCrc(Stream outputStream, byte[] messageBuffer)
        {
            uint crcCalculated = Crc32Algorithm.Compute(messageBuffer);
            const int BYTES_TO_WRITE = 4;
            byte[] crcBuffer = BitConverter.GetBytes(crcCalculated);
            outputStream.Write(crcBuffer, 0, BYTES_TO_WRITE);
        }

        private static void WriteEndByte(Stream outputStream)
        {
            outputStream.WriteByte(END_BYTE);
        }

        #endregion WriteEntry
    }
}