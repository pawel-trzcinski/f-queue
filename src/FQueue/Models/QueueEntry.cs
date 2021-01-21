using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace FQueue.Models
{
#warning TODO - unit tests
    public class QueueEntry
    {
        public string Tag { get; private set; }
        public string Data { get; private set; }
        public byte[] DataBytes { get; private set; }

        private QueueEntry()
        {
        }

        public static QueueEntry FromRequestString(string bodyRequestString)
        {
            JObject request = JObject.Parse(bodyRequestString);

            if (!request.TryGetValue(nameof(QueueEntry.Tag), StringComparison.OrdinalIgnoreCase, out JToken token))
            {
                throw new ArgumentException($"No {nameof(QueueEntry.Tag)} found in input JSON");
            }

            return new QueueEntry
            {
                Data = bodyRequestString,
                Tag = token.Value<string>(),
                DataBytes = Encoding.UTF8.GetBytes(bodyRequestString)
            };
#warning TODO - walidacja ilości bajtów - może być ich tylko 2^32
        }

        public static QueueEntry FromDataFileStream(Stream inputStream)
        {
            long streamStartPosition = inputStream.Position;

            // throws InvalidCrcException
#warning TODO
            return null;
        }

        public void WriteToStream(Stream stream)
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