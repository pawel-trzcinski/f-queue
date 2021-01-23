using FQueue.Models;

namespace FQueue.Data
{
    public abstract class DataProtocol : IDataProtocol
    {
        public DataProtocolVersion Version { get; }

        protected DataProtocol(DataProtocolVersion version)
        {
            Version = version;
        }

        /// <summary>
        /// Read QueueEqntry from data stream. This method is thread-safe.
        /// </summary>
        public abstract QueueEntry GetEntry(byte[] data);

        /// <summary>
        /// White QueueEqntry to data stream. This method is thread-safe.
        /// </summary>
        public abstract byte[] GetBytes(QueueEntry entry);
    }
}