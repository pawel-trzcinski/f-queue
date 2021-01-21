namespace FQueue.Context
{
    public interface IQueueContextFactory
    {
        QueueContext GetContext(string queueName);
    }
}