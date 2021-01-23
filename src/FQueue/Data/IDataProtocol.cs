using FQueue.Models;

namespace FQueue.Data
{
    public interface IDataProtocol
    {
        DataProtocolVersion Version { get; }

        /// <summary>
        /// Create queue entry object from data bytes. This method is thread-safe.
        /// </summary>
        QueueEntry GetEntry(byte[] data);

        /// <summary>
        /// White QueueEqntry to DataFile stream. This method is thread-safe.
        /// </summary>
        byte[] GetBytes(QueueEntry entry);
    }
}//