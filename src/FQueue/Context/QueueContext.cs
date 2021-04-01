using System;

namespace FQueue.Context
{
    [Serializable]
    public class QueueContext
    {
        public string QueueName { get; }

        public QueueContext(string queueName)
        {
            QueueName = queueName;
        }

        public override string ToString()
        {
            return $"QueueName: {QueueName}";
        }
    }
}