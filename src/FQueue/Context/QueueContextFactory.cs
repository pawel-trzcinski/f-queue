using System.Collections.Concurrent;

namespace FQueue.Context
{
    public class QueueContextFactory : IQueueContextFactory
    {
        private readonly ConcurrentDictionary<string, QueueContext> _contextDictionary = new ConcurrentDictionary<string, QueueContext>();

        public QueueContext GetContext(string queueName)
        {
            return _contextDictionary.GetOrAdd(queueName, CreateNewContext);
        }

        private QueueContext CreateNewContext(string queueName)
        {
            return new QueueContext(queueName);
        }
    }
}