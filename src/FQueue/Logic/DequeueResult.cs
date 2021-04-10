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
#warning TODO
            throw new System.NotImplementedException();
        }
    }
}