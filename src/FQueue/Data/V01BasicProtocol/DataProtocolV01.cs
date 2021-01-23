using FQueue.Models;

namespace FQueue.Data.V01BasicProtocol
{
    public class DataProtocolV01 : DataProtocol
    {
#warning TODO - unit tests

        public DataProtocolV01()
            : base(DataProtocolVersion.V01BasicProtocol)
        {
        }

        public override QueueEntry GetEntry(byte[] data)
        {
            return QueueEntry.FromBytes(data);
        }

        public override byte[] GetBytes(QueueEntry entry)
        {
            return entry.DataBytes;
        }
    }
}