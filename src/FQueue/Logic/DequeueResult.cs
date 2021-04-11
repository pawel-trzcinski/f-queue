using System.Linq;
using System.Text;
using FQueue.Models;

namespace FQueue.Logic
{
    public class DequeueResult : SuccessResult
    {
        private readonly QueueEntry[] _entries;

        public DequeueResult(QueueEntry[] entries)
        {
            _entries = entries;
        }

        public override string DataToString()
        {
            if (_entries == null || _entries.Length == 0)
            {
                return "[]";
            }

            StringBuilder sb = new StringBuilder(2 + _entries.Length - 1 + _entries.Sum(p => p.Data.Length));
            sb.Append('[');

            QueueEntry first = _entries.First();
            sb.Append(first.Data);

            for (int i = 1; i < _entries.Length; i++)
            {
                sb.Append(',');
                sb.Append(_entries[i].Data);
            }

            sb.Append(']');

            return sb.ToString();
        }
    }
}