using System;
using System.Text;
using FQueue.Context;
using FQueue.Data;
using FQueue.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FQueue.Models
{
    public class QueueEntry
    {
#warning TODO - unit tests
        public string Tag { get; private set; }
        public string Data { get; private set; }
        public byte[] DataBytes { get; private set; }

        private QueueEntry()
        {
        }

        public static QueueEntry FromRequestString(QueueContext context, string bodyRequestString, int dataFileMaximumSizeB)
        {
            JObject request = JObject.Parse(bodyRequestString);

            if (!request.TryGetValue(nameof(QueueEntry.Tag), StringComparison.OrdinalIgnoreCase, out JToken token))
            {
                throw new ArgumentException($"No {nameof(QueueEntry.Tag)} found in input JSON");
            }

            byte[] dataBytes = Encoding.UTF8.GetBytes(bodyRequestString);
            int frameSize = DataProtocolAdapter.CalculateFrameSize(dataBytes.Length);
            if (frameSize < dataFileMaximumSizeB)
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

        public static QueueEntry FromBytes(byte[] data)
        {
            return JsonConvert.DeserializeObject<QueueEntry>(Encoding.UTF8.GetString(data));
        }
    }
}