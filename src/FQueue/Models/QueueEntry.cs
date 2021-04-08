using System;
using System.Text;
using FQueue.Context;
using FQueue.Data;
using FQueue.Exceptions;
using Newtonsoft.Json.Linq;

namespace FQueue.Models
{
    [Serializable]
    public class QueueEntry : TagObject
    {
        public string Data { get; private set; }
        public byte[] DataBytes { get; private set; }

        private QueueEntry()
        {
        }

        /// <summary>
        /// Create queue entry from REST body request. Size check made.
        /// </summary>
        public static QueueEntry FromRequestString(QueueContext context, string bodyRequestString, int dataFileMaximumSizeB)
        {
            JObject request = JObject.Parse(bodyRequestString);

            if (!request.TryGetValue(nameof(QueueEntry.Tag), StringComparison.OrdinalIgnoreCase, out JToken token))
            {
                throw new ArgumentException($"No {nameof(QueueEntry.Tag)} found in input JSON");
            }

            byte[] dataBytes = Encoding.UTF8.GetBytes(bodyRequestString);
            int frameSize = DataProtocolAdapter.CalculateFrameSize(dataBytes.Length);
            if (frameSize > dataFileMaximumSizeB)
            {
                throw new TooBigRequestException(context, dataFileMaximumSizeB, dataBytes.Length, frameSize);
            }

            return new QueueEntry
            {
                Data = bodyRequestString,
                Tag = token.Value<string>(),
                DataBytes = Encoding.UTF8.GetBytes(bodyRequestString)
            };
        }

        /// <summary>
        /// Read QueueEntry from data file. No size check needed
        /// </summary>
        public static QueueEntry FromBytes(byte[] dataBytes)
        {
            string data = Encoding.UTF8.GetString(dataBytes);

            JObject entryObject = JObject.Parse(data);
            if (!entryObject.TryGetValue(nameof(QueueEntry.Tag), StringComparison.OrdinalIgnoreCase, out JToken token))
            {
                throw new ArgumentException($"No {nameof(QueueEntry.Tag)} found in input bytes");
            }

            return new QueueEntry
            {
                Data = data,
                Tag = token.Value<string>(),
                DataBytes = dataBytes
            };
        }
    }
}